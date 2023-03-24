using ApplianceLib.Api;
using Kitchen;
using KitchenData;
using KitchenMods;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Entities;

namespace ApplianceLib.Customs
{
    [UpdateInGroup(typeof(DurationLocks))]
    internal class UpdateFlexibleTakesDuration : GameSystemBase, IModSystem
    {
        private EntityQuery query;
        protected override void Initialise()
        {
            base.Initialise();
            query = GetEntityQuery(new QueryHelper().All(
                typeof(CAppliance),
                typeof(CTakesDuration),
                typeof(CFlexibleContainer),
                typeof(CAppliesProcessToFlexible)
            ));
        }

        protected override void OnUpdate()
        {
            using var entities = query.ToEntityArray(Allocator.Temp);

            foreach (var entity in entities)
            {
                if (!Require<CTakesDuration>(entity, out var duration)
                    || !Require<CAppliesProcessToFlexible>(entity, out var processModifiers)
                    || !Require<CFlexibleContainer>(entity, out var container)
                    || !Require<CAppliance>(entity, out var cAppliance)
                    || !GameData.Main.TryGet<Appliance>(cAppliance.ID, out var appliance)
                    || processModifiers.ProcessType == FlexibleProcessType.UseTakesDurationTime)
                {
                    continue;
                }

                var oldTotal = duration.Total;
                List<float> totals = new();
                foreach (var itemID in container.Items.GetItems())
                {
                    if (!GameData.Main.TryGet<Item>(itemID, out var item))
                        continue;

                    var process = item.DerivedProcesses.Find(ip => appliance.Processes.Exists(ap => ip.Process.ID == ap.Process.ID));
                    if (process.Equals(default(Item.ItemProcess)))
                        continue;
                    totals.Add(process.Duration * processModifiers.ProcessTimeMultiplier);
                }

                var preferredTotal = processModifiers.ProcessType switch
                {
                    FlexibleProcessType.Additive => totals.Sum(),
                    FlexibleProcessType.Average => totals.Count == 0 ? 0 : totals.Average(),
                    _ => oldTotal,
                };
                duration.IsLocked = duration.IsLocked || (container.Maximum > 0 && container.Items.Count < processModifiers.MinimumItems) || totals.Count == 0;
                var newTotal = Math.Max(processModifiers.MinimumProcessTime, preferredTotal);
                if (!duration.IsLocked && newTotal != oldTotal)
                {
                    duration.Total = newTotal;
                    duration.Remaining += newTotal - oldTotal;
                }
                Set(entity, duration);
            }
        }
    }
}
