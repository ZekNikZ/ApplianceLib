using ApplianceLib.Api;
using HarmonyLib;
using Kitchen;
using Unity.Entities;

namespace ApplianceLib.Customs
{
    [HarmonyPatch(typeof(GrabItems), "AttemptGrabFromProvider")]
    public class ConveyerGrabPatch
    {
        static bool Prefix(ref Entity target, ref EntityContext ctx, ref Entity e, ref CConveyPushItems grab, ref bool __result)
        {
            if (!ctx.Require<CFlexibleContainer>(target, out var container))
                return true;

            if (!ctx.Has<CIsInactive>(target) && ctx.Has<CLockedWhileDuration>(target))
                return true;

            var itemCount = container.Items.Count;
            if (itemCount <= 0 || container.Maximum <= 0)
                return true;

            var currentItem = container.Items.GetItem(itemCount - 1);
            var currentComponents = container.Items[itemCount - 1];
            if (grab.GrabSpecificType && grab.SpecificType != 0 && (grab.SpecificType != currentItem || !currentComponents.IsEquivalent(grab.SpecificComponents)))
                return true;

            container.Items.Remove(itemCount - 1);
            ctx.Set(target, container);

            Entity held_item = ctx.CreateItemGroup(currentItem, currentComponents);
            ctx.UpdateHolder(held_item, e);
            grab.Progress = 0f;
            grab.State = CConveyPushItems.ConveyState.Grab;
            __result = true;
            return false;
        }
    }
}
