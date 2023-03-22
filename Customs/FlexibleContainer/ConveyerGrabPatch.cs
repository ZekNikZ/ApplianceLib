using ApplianceLib.Api;
using HarmonyLib;
using Kitchen;
using Unity.Entities;

namespace ApplianceLib.Customs
{
    [HarmonyPatch(typeof(GrabItems), "AttemptGrabFromProvider")]
    internal class ConveyerGrabPatch
    {
        static bool Prefix(ref Entity target, ref EntityContext ctx, ref Entity e, ref CConveyPushItems grab, ref bool __result)
        {
            // Check if there is a flexible container on the target
            if (!ctx.Require<CFlexibleContainer>(target, out var container))
                return true;

            // Ensure it is not locked
            if (!ctx.Has<CIsInactive>(target) && ctx.Has<CLockedWhileDuration>(target))
                return true;

            // Ensure the container is not empty
            var itemCount = container.Items.Count;
            if (itemCount <= 0 || container.Maximum <= 0)
                return true;

            // Find the item to remove, if possible
            var shouldRemove = true;
            var removeIndex = itemCount - 1;
            if (grab.GrabSpecificType)
            {
                shouldRemove = false;
                for (int i = itemCount - 1; i >= 0; i++)
                {
                    var item = container.Items.GetItem(i);
                    var components = container.Items[i];
                    if (grab.SpecificType == item && components.IsEquivalent(grab.SpecificComponents))
                    {
                        shouldRemove = true;
                        removeIndex = i;
                    }
                }
            }
            if (!shouldRemove)
                return true;

            // Remove the specified item
            var removedItem = container.Items.GetItem(removeIndex);
            var removedComponents = container.Items[removeIndex];
            container.Items.Remove(removeIndex);
            ctx.Set(target, container);

            // Update the conveyor state
            Entity held_item = ctx.CreateItemGroup(removedItem, removedComponents);
            ctx.UpdateHolder(held_item, e);
            grab.Progress = 0f;
            grab.State = CConveyPushItems.ConveyState.Grab;
            __result = true;
            return false;
        }
    }
}
