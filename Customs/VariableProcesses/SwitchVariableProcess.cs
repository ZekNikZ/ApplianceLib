using Kitchen;
using KitchenData;
using KitchenMods;
using System;
using Unity.Collections;
using Unity.Entities;

namespace ApplianceLib.Customs
{
    [UpdateBefore(typeof(ItemTransferGroup))]
    public class SwitchVariableProcessContainer : ItemInteractionSystem, IModSystem
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
            var target = Tuple.Create(data.Target.Index, data.Target.Version);

            if (Require<CItemHolder>(data.Target, out var holder) && holder.HeldItem == default)
            {
                VariableProcessContainer.Current = (VariableProcessContainer.Current + 1) % VariableProcessContainer.Max;
            }

            SetComponent(data.Target, VariableProcessContainer);
        }
    }

    [UpdateInGroup(typeof(CreationGroup))]
    public class AddCustomApplianceProperties : GenericSystemBase, IModSystem
    {
        private EntityQuery Appliances;

        protected override void Initialise()
        {
            base.Initialise();

            Appliances = GetEntityQuery(new QueryHelper().All(typeof(CCreateAppliance)));
        }

        protected override void OnUpdate()
        {
            using var appliances = Appliances.ToEntityArray(Allocator.TempJob);
            foreach (var appliance in appliances)
            {
                int applianceId = EntityManager.GetComponentData<CCreateAppliance>(appliance).ID;
                if (GameData.Main.TryGet(applianceId, out Appliance gdo))
                {
                    foreach (var prop in gdo.Properties)
                    {
                        if (prop is CVariableProcessContainer container)
                        {
                            EntityManager.AddComponentData(appliance, container);
                            EntityManager.RemoveComponent<CItemProvider>(appliance);
                        }
                    }
                }
            }
        }
    }
}
