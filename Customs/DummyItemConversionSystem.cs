using ApplianceLib.Api;
using Kitchen;
using KitchenMods;
using Unity.Collections;
using Unity.Entities;

namespace ApplianceLib.Customs
{
    internal class DummyItemConversionSystem : GenericSystemBase, IModSystem
    {
        private EntityQuery AllItems;

        protected override void Initialise()
        {
            AllItems = GetEntityQuery(new QueryHelper()
                .All(typeof(CItem))
            );
        }

        protected override void OnUpdate()
        {
            using var entities = AllItems.ToEntityArray(Allocator.Temp);
            using var itemComponents = AllItems.ToComponentDataArray<CItem>(Allocator.Temp);

            for (var i = 0; i < entities.Length; ++i)
            {
                var entity = entities[i];
                var itemComponent = itemComponents[i];

                foreach (var conversion in DummyItemConversions.ItemConversions)
                {
                    if (itemComponent.ID == conversion.FromId)
                    {
                        EntityManager.SetComponentData(entity, new CItem
                        {
                            ID = conversion.ToId,
                            IsPartial = true,
                            IsTransient = itemComponent.IsTransient,
                            IsGroup = true,
                            Items = conversion.ToComponents,
                            Category = itemComponent.Category
                        });
                    }
                }
            }
        }
    }
}
