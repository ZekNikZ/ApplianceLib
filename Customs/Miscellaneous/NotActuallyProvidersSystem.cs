using ApplianceLib.Api;
using Kitchen;
using KitchenMods;
using Unity.Collections;
using Unity.Entities;

namespace ApplianceLib.Customs
{
    [UpdateInGroup(typeof(CreationGroup))]
    [UpdateBefore(typeof(CreateNewAppliances))]
    internal class NotActuallyProvidersSystem : GenericSystemBase, IModSystem
    {
        private EntityQuery Appliances;

        protected override void Initialise()
        {
            Appliances = GetEntityQuery(new QueryHelper().All(typeof(CCreateAppliance)));
        }

        protected override void OnUpdate()
        {
            using var appliances = Appliances.ToEntityArray(Allocator.TempJob);
            foreach (var appliance in appliances)
            {
                int applianceId = EntityManager.GetComponentData<CCreateAppliance>(appliance).ID;
                if (NotActuallyProviders.Appliances.Contains(applianceId))
                {
                    EntityManager.RemoveComponent<CItemProvider>(appliance);
                }
            }
        }
    }
}
