using ApplianceLib.Api;
using Kitchen;
using KitchenMods;
using Unity.Collections;
using Unity.Entities;

namespace ApplianceLib.Customs
{
    internal class ChangeApplianceKeyAfterDuration : GameSystemBase, IModSystem
    {
        private EntityQuery Appliances;

        protected override void Initialise()
        {
            Appliances = GetEntityQuery(new QueryHelper()
                .All(
                    typeof(CAppliance),
                    typeof(CRestrictedReceiver),
                    typeof(CTakesDuration),
                    typeof(CChangeRestrictedReceiverKeyAfterDuration)
                )
                .Any(
                    typeof(CItemHolder),
                    typeof(CFlexibleContainer)
                ));
        }

        protected override void OnUpdate()
        {
            using var entities = Appliances.ToEntityArray(Allocator.Temp);
            using var receivers = Appliances.ToComponentDataArray<CRestrictedReceiver>(Allocator.Temp);
            using var durations = Appliances.ToComponentDataArray<CTakesDuration>(Allocator.Temp);
            using var changes = Appliances.ToComponentDataArray<CChangeRestrictedReceiverKeyAfterDuration>(Allocator.Temp);

            for (int i = 0; i < entities.Length; i++)
            {
                var entity = entities[i];
                var receiver = receivers[i];
                var duration = durations[i];
                var change = changes[i];

                if (receiver.ApplianceKey != change.ApplianceKey && duration.Remaining <= 0f && duration.Active)
                {
                    receiver.ApplianceKey = change.ApplianceKey;
                    Set(entity, receiver);
                    Mod.LogInfo("changed after duration");
                }
            }
        }
    }
}
