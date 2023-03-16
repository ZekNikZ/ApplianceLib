using ApplianceLib.Api.FlexibleContainer;
using Kitchen;
using KitchenData;
using KitchenMods;
using System;
using Unity.Collections;
using Unity.Entities;

namespace ApplianceLib.Customs.FlexibleContainer
{
    [UpdateInGroup(typeof(DurationLocks))]
    public class UpdateFlexibleDuration : GameSystemBase, IModSystem
    {
        private EntityQuery query;
        protected override void Initialise()
        {
            base.Initialise();
            query = GetEntityQuery(new QueryHelper().All(typeof(CTakesDuration), typeof(CFlexibleContainer), typeof(CAppliesProcessToFlexible)));
        }

        protected override void OnUpdate()
        {
            using var entities = query.ToEntityArray(Allocator.Temp);

            foreach (var entity in entities)
            {
                if (!Require<CTakesDuration>(entity, out var duration) ||
                    !Require<CAppliesProcessToFlexible>(entity, out var processComp) ||
                    !Require<CFlexibleContainer>(entity, out var container) ||
                    !Require<CAppliance>(entity, out var applianceComp) ||
                    !GameData.Main.TryGet<Appliance>(applianceComp.ID, out var appliance))
                    continue;

                var oldTotal = duration.Total;
                float newTotal = 0;
                foreach (var itemID in container.Items.GetItems())
                {
                    if (!GameData.Main.TryGet<Item>(itemID, out var item))
                        continue;

                    var process = item.DerivedProcesses.Find(ip => appliance.Processes.Exists(ap => ip.Process.ID == ap.Process.ID));
                    if (process.Equals(default(Item.ItemProcess)))
                        continue;
                    newTotal += process.Duration * processComp.ProcessTimeMultiplier;
                }

                duration.IsLocked = container.Maximum > 0 && processComp.Minimum > container.Items.Count || newTotal == 0.0f;
                var minTotal = Math.Max(processComp.MinimumProcessTime, newTotal);
                if (!duration.IsLocked && minTotal != oldTotal)
                {
                    duration.Total = minTotal;
                    duration.Remaining += minTotal - oldTotal;
                }
                Set(entity, duration);
            }
        }
    }
}
