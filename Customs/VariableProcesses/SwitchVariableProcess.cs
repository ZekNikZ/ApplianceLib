using ApplianceLib.Api;
using Kitchen;
using KitchenLib.Utils;
using KitchenMods;
using Unity.Entities;

namespace ApplianceLib.Customs
{
    [UpdateBefore(typeof(ItemTransferGroup))]
    internal class SwitchVariableProcessContainer : ItemInteractionSystem, IModSystem
    {
        private CVariableProcessContainer VariableProcessContainer;
        private CAppliance Appliance;

        protected override InteractionType RequiredType => InteractionType.Act;

        protected override void Initialise()
        {
            Mod.LogInfo("Initialized VariableProcessContainerSwitcher system");
        }

        protected override bool IsPossible(ref InteractionData data)
        {
            return Require(data.Target, out VariableProcessContainer) && Require(data.Target, out Appliance);
        }

        protected override void Perform(ref InteractionData data)
        {
            if (Require<CItemHolder>(data.Target, out var holder) && holder.HeldItem == default)
            {
                var gdo = GDOUtils.GetCustomGameDataObject(Appliance.ID);
                if (gdo is not null and IVariableProcessAppliance)
                {
                    VariableProcessContainer.Current = (VariableProcessContainer.Current + 1) % ((IVariableProcessAppliance)gdo).VariableApplianceProcesses.Count;
                }
            }

            SetComponent(data.Target, VariableProcessContainer);
        }
    }
}
