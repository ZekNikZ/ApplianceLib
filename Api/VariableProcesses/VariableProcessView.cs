using Kitchen;
using KitchenMods;
using MessagePack;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace ApplianceLib.Api
{
    public class VariableProcessView : UpdatableObjectView<VariableProcessView.ViewData>
    {
        [SerializeField]
        public Animator Animator;

        [SerializeField]
        public HoldPointContainer HoldPointContainer;

        [SerializeField]
        public List<Transform> HoldPoints;

        private static readonly int ID = Animator.StringToHash("ID");

        private static readonly int Index = Animator.StringToHash("Index");
        protected override void UpdateData(ViewData viewData)
        {
            if (Animator != null)
            {
                Animator.SetInteger(ID, viewData.ContainerCurrent);
                Animator.SetInteger(Index, viewData.ContainerCurrent);
            }

            if (HoldPoints != null && HoldPointContainer != null)
            {
                HoldPointContainer.HoldPoint = HoldPoints[viewData.ContainerCurrent];
                GameObject.transform.parent.parent.GetComponent<ApplianceView>().HeldItemPosition = HoldPointContainer.HoldPoint;
            }
        }

        public class UpdateView : IncrementalViewSystemBase<VariableProviderView.ViewData>, IModSystem
        {
            private EntityQuery Views;

            protected override void Initialise()
            {
                base.Initialise();
                Views = GetEntityQuery(new QueryHelper()
                    .All(typeof(CLinkedView), typeof(CVariableProcessContainer))
                );
            }

            protected override void OnUpdate()
            {
                using var views = Views.ToComponentDataArray<CLinkedView>(Allocator.Temp);
                using var components = Views.ToComponentDataArray<CVariableProcessContainer>(Allocator.Temp);

                for (var i = 0; i < views.Length; i++)
                {
                    var view = views[i];
                    var data = components[i];

                    SendUpdate(view, new ViewData
                    {
                        ContainerCurrent = data.Current
                    }, MessageType.SpecificViewUpdate);
                }
            }
        }

        [MessagePackObject(false)]
        public struct ViewData : ISpecificViewData, IViewData, IViewResponseData, IViewData.ICheckForChanges<ViewData>
        {
            [Key(1)]
            public int ContainerCurrent;

            public bool IsChangedFrom(ViewData check)
            {
                return ContainerCurrent != check.ContainerCurrent;
            }

            public IUpdatableObject GetRelevantSubview(IObjectView view)
            {
                return view.GetSubView<VariableProcessView>();
            }
        }
    }
}
