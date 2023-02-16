using ApplianceLib.Api;
using Kitchen;
using KitchenMods;
using Unity.Entities;

namespace ApplianceLib.Customs
{
    [UpdateBefore(typeof(ItemTransferGroup))]
    internal class SwitchVariableProcessContainer : ItemInteractionSystem, IModSystem
    {
        private CVariableProcessContainer VariableProcessContainer;

        protected override InteractionType RequiredType => InteractionType.Act;

        protected override void Initialise()
        {
            Mod.LogInfo("Initialized VariableProcessContainerSwitcher system");
        }

        protected override bool IsPossible(ref InteractionData data)
        {
            return Require(data.Target, out VariableProcessContainer);
        }

        protected override void Perform(ref InteractionData data)
        {
            if (Require<CItemHolder>(data.Target, out var holder) && holder.HeldItem == default)
            {
                VariableProcessContainer.Current = (VariableProcessContainer.Current + 1) % VariableProcessContainer.Max;
            }

            SetComponent(data.Target, VariableProcessContainer);
        }
    }
}
