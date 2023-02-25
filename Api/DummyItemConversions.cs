using KitchenData;
using System.Collections.Generic;

namespace ApplianceLib.Api
{
    public class DummyItemConversions
    {
        internal struct ItemConversion
        {
            public int FromId;
            public int ToId;
            public ItemList ToComponents;
        }

        internal static List<ItemConversion> ItemConversions = new();

        /// <summary>
        /// Register an item to itemgroup conversion. The intention of this is that you can make
        /// a dummy item ("from") be the result of a process but turn into a partially-complete
        /// item group ("to").
        /// </summary>
        /// <param name="fromItem">The source item.</param>
        /// <param name="toItem">The target item.</param>
        /// <param name="toComponents">The components of the new item.</param>
        public static void AddItemConversion(Item fromItem, Item toItem, ItemList toComponents)
        {
            AddItemConversion(fromItem.ID, toItem.ID, toComponents);
        }

        /// <summary>
        /// Register an item to itemgroup conversion. The intention of this is that you can make
        /// a dummy item ("from") be the result of a process but turn into a partially-complete
        /// item group ("to").
        /// </summary>
        /// <param name="fromId">The source item.</param>
        /// <param name="toId">The target item.</param>
        /// <param name="toComponents">The components of the new item.</param>
        public static void AddItemConversion(int fromId, int toId, ItemList toComponents)
        {
            ItemConversions.Add(new ItemConversion
            {
                FromId = fromId,
                ToId = toId,
                ToComponents = toComponents
            });
        }
    }
}
