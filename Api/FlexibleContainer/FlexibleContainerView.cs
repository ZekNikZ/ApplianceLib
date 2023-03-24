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
        public List<Transform> Transforms = new();

        private List<GameObject> Items = new();

        protected override void UpdateData(ViewData data)
        {
            foreach (var item in Items)
                Destroy(item);
            Items.Clear();

            for (int i = 0; i < Transforms.Count; i++)
            {
                if (i >= data.Items.Count)
                    break;

                if (!GameData.Main.TryGet<Item>(data.Items[i], out var item))
                    continue;

                GameObject duped = Instantiate(item.Prefab);
                duped.transform.SetParent(Transforms[i], false);
                Items.Add(duped);
            }
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
                    SendUpdate(views[i], new()
                    {
                        Items = containers[i].Items.GetItems()
                    }, MessageType.SpecificViewUpdate);
                }
            }
        }

        [MessagePackObject(false)]
        public class ViewData : ISpecificViewData, IViewData, IViewResponseData, IViewData.ICheckForChanges<ViewData>
        {
            [Key(0)] public ItemList Items;

            public IUpdatableObject GetRelevantSubview(IObjectView view) => view.GetSubView<FlexibleContainerView>();

            public bool IsChangedFrom(ViewData check) => !Items.Equals(check.Items);
        }
    }
}
