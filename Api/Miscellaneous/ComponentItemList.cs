using JetBrains.Annotations;
using KitchenData;
using MessagePack;
using MessagePack.Formatters;
using Unity.Collections;

namespace ApplianceLib.Api.Miscellaneous
{
    [MessagePackObject(false)]
    [MessagePackFormatter(typeof(ComponentItemListFormatter))]
    public struct ComponentItemList
    {
        public void Add(int item, ItemList components)
        {
            Keys.Add(Components.Length);
            Items.Add(item);
            foreach (var comp in components)
                Components.Add(comp);
        }

        public void Add(int item)
        {
            Keys.Add(Components.Length);
            Items.Add(item);
            Components.Add(item);
        }

        public void Remove(int index)
        {
            if (index >= Count)
                return;

            // Determine the range in the component list
            var startIndex = Keys[index];
            var endIndex = index + 1 >= Keys.Length ? Components.Length : Keys[index + 1];
            var numComponents = endIndex - startIndex;

            // Remove the components
            for (int i = 0; i < numComponents ; i++)
                Components.RemoveAt(startIndex);
            Items.RemoveAt(index);
            Keys.RemoveAt(index);

            // Fix the remaining keys
            for (int i = index; i < Keys.Length; i++)
            {
                Keys[i] -= numComponents;
            }
        }

        public int GetItem(int index) => Items[index];

        public ItemList GetItems() => new ItemList(Items);

        [Pure] public bool IsEquivalent(ComponentItemList list)
        {
            bool result = false;
            for (int i = 0; i < list.Items.Length; i++)
                result = (result == true) ? true : (list.Items[i] == Items[i]);
            return result;
        }

        [IgnoreMember] public ItemList this[int index]
        {
            get
            {
                if (index >= Keys.Length)
                    return default(ItemList);

                int key = Keys[index];
                int maxKey = index + 1 >= Keys.Length ? Keys.Length : Keys[index + 1];

                ItemList result = new();
                for (int i = key; i < maxKey; i++)
                    result.Add(Components[i]);

                return result;
            }
        }

        [IgnoreMember] public int Count { get => Items.Length; }

        [SerializationConstructor]
        public ComponentItemList(FixedListInt64 Items, FixedListInt512 Components, FixedListInt64 Keys)
        {
            this.Items = Items;
            this.Components = Components;
            this.Keys = Keys;
        }

        [Key(0)] private FixedListInt64 Items;
        [Key(1)] private FixedListInt512 Components;
        [Key(2)] private FixedListInt64 Keys;

        private class ComponentItemListFormatter : IMessagePackFormatter<ComponentItemList>
        {
            private T Deserialise<T>(ref MessagePackReader reader, MessagePackSerializerOptions options) where T : INativeList<int>
            {
                int[] array = options.Resolver.GetFormatterWithVerify<int[]>().Deserialize(ref reader, options);
                T result = default(T);
                for (int i = 0; i < array.Length; i++)
                {
                    result[i] = array[i];
                }
                return result;
            }

            private void Serialise<T>(ref MessagePackWriter writer, T input, MessagePackSerializerOptions options) where T : INativeList<int>
            {
                int[] array = new int[input.Length];
                for (int i = 0; i < input.Length; i++)
                {
                    array[i] = input[i];
                }
                options.Resolver.GetFormatterWithVerify<int[]>().Serialize(ref writer, array, options);
            }

            public ComponentItemList Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
            {
                return new()
                {
                    Items = Deserialise<FixedListInt64>(ref reader, options),
                    Components = Deserialise<FixedListInt512>(ref reader, options),
                    Keys = Deserialise<FixedListInt64>(ref reader, options)
                };
            }

            public void Serialize(ref MessagePackWriter writer, ComponentItemList value, MessagePackSerializerOptions options)
            {
                Serialise(ref writer, value.Items, options);
                Serialise(ref writer, value.Components, options);
                Serialise(ref writer, value.Keys, options);
            }
        }
    }
}
