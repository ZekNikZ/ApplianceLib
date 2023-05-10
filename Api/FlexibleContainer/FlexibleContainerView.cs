using ApplianceLib.Api.Miscellaneous;
using ApplianceLib.Util;
using Kitchen;
using KitchenData;
using KitchenMods;
using MessagePack;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace ApplianceLib.Api
{
    public class FlexibleContainerView : UpdatableObjectView<FlexibleContainerView.ViewData>
    {
        [SerializeField]
        public List<GameObject> Items = new();

        private ViewData Data;

        private void SetPrefab(int i, GameObject prefab, ItemList itemComponents)
        {
            GameObject oldItem = Items[i];
            GameObject newItem = Instantiate(prefab, oldItem.transform.parent, true);
            newItem.transform.position = oldItem.transform.position;
            newItem.transform.rotation = oldItem.transform.rotation;
            newItem.transform.localScale = oldItem.transform.localScale;
            newItem.SetActive(oldItem.activeSelf);
            if (newItem.TryGetComponent(out ItemGroupView itemGroupView))
            {
                itemGroupView.PerformUpdate(itemComponents.Primary, itemComponents);
            }
            Items[i] = newItem;
            Destroy(oldItem);
        }

        public void SetAmount(int amount)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                Items[i].SetActive(i < amount);
            }
        }

        protected override void UpdateData(ViewData data)
        {
            Mod.LogWarning($"FlexibleContainerView update - {data.Data.Items.Length}");
            if (!data.IsChangedFrom(Data))
            {
                return;
            }
            Data = data;

            ComponentItemList items = data.Data;

            for (int i = 0; i < items.Count; i++)
            {
                SetPrefab(i, GameData.Main.GetPrefab(items.GetItem(i)), items[i]);
            }

            SetAmount(items.Count);
        }

        private class UpdateView : IncrementalViewSystemBase<ViewData>, IModSystem
        {
            private EntityQuery query;

            protected override void Initialise()
            {
                base.Initialise();
                query = GetEntityQuery(new QueryHelper().All(typeof(CFlexibleContainer), typeof(CLinkedView)));
            }

            protected override void OnUpdate()
            {
                using var views = query.ToComponentDataArray<CLinkedView>(Allocator.Temp);
                using var containers = query.ToComponentDataArray<CFlexibleContainer>(Allocator.Temp);

                for (int i = 0; i < views.Length; i++)
                {
                    //if (containers[i].Items.Count > 0)
                    //{
                    //    SendUpdate(views[i], new LimitedItemSourceView.ViewData
                    //    {
                    //        DisplayedType = containers[i].Items.GetItem(0),
                    //        DisplayedComponents = containers[i].Items[0],
                    //        Amount = containers[i].Items.Count
                    //    }, MessageType.SpecificViewUpdate);
                    //}
                    //else
                    //{
                    //    SendUpdate(views[i], new LimitedItemSourceView.ViewData
                    //    {
                    //        DisplayedType = ItemReferences.PlateDirty,
                    //        DisplayedComponents = new ItemList(),
                    //        Amount = containers[i].Items.Count
                    //    }, MessageType.SpecificViewUpdate);
                    //}
                    SendUpdate(views[i], new()
                    {
                        Data = containers[i].Items
                    }, MessageType.SpecificViewUpdate);
                }
            }
        }

        [MessagePackObject]
        public struct ViewData : ISpecificViewData, IViewData, IViewResponseData, IViewData.ICheckForChanges<ViewData>
        {
            [Key(0)]
            public ComponentItemList.MessagePackSurrogate Data;

            public IUpdatableObject GetRelevantSubview(IObjectView view) => view.GetSubView<FlexibleContainerView>();

            public bool IsChangedFrom(ViewData check)
            {
                return !Data.Items.IsEqual(check.Data.Items) || !Data.Components.IsEqual(check.Data.Components) || !Data.Keys.IsEqual(check.Data.Keys);
            }
        }
    }
}
