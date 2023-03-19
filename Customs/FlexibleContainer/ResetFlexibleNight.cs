using ApplianceLib.Api;
using Kitchen;
using KitchenMods;
using Unity.Collections;
using Unity.Entities;

namespace ApplianceLib.Customs
{
    public class ResetFlexibleNight : StartOfNightSystem, IModSystem
    {
        private EntityQuery Query;
        protected override void Initialise()
        {
            base.Initialise();
            Query = GetEntityQuery(new QueryHelper().All(typeof(CFlexibleContainer)));
        }

        protected override void OnUpdate()
        {
            using var entities = Query.ToEntityArray(Allocator.TempJob);
            foreach (var entity in entities)
            {
                if (!Require<CFlexibleContainer>(entity, out var flexible))
                    continue;

                for (int i = flexible.Items.Count - 1; i >= 0; i--)
                {
                    flexible.Items.Remove(i);
                }
                Set(entity, flexible);
            }
        }
    }
}
