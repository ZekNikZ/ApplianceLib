using Kitchen;
using KitchenMods;
using MessagePack;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace ApplianceLib.Api.VariableProvider
{
    public class ExpandedVariableProviderView: UpdatableObjectView<ExpandedVariableProviderView.ViewData>
    {
        [SerializeField]
        public Animator Animator;

        private static readonly int ID = Animator.StringToHash("ID");

        private static readonly int Index = Animator.StringToHash("Index");

        protected override void UpdateData(ViewData viewData)
        {
            Animator.SetInteger(ID, viewData.ProviderID);
            Animator.SetInteger(Index, viewData.ProviderIndex);
        }

        public class UpdateView : IncrementalViewSystemBase<VariableProviderView.ViewData>, IModSystem
        {
            private EntityQuery Views;

            protected override void Initialise()
            {
                base.Initialise();
                Views = GetEntityQuery(new QueryHelper()
                    .All(typeof(CLinkedView), typeof(CExpandedVariableProvider))
                );
            }

            protected override void OnUpdate()
            {
                using var views = Views.ToComponentDataArray<CLinkedView>(Allocator.Temp);
                using var components = Views.ToComponentDataArray<CExpandedVariableProvider>(Allocator.Temp);

                for (var i = 0; i < views.Length; i++)
                {
                    var view = views[i];
                    var data = components[i];

                    SendUpdate(view, new ViewData
                    {
                        ProviderID = data.Provide,
                        ProviderIndex = data.Current
                    }, MessageType.SpecificViewUpdate);
                }
            }
        }

        [MessagePackObject(false)]
        public struct ViewData : ISpecificViewData, IViewData, IViewResponseData, IViewData.ICheckForChanges<ViewData>
        {
            [Key(1)]
            public int ProviderID;

            [Key(2)]
            public int ProviderIndex;

            public bool IsChangedFrom(ViewData check)
            {
                return ProviderID != check.ProviderID || ProviderIndex != check.ProviderIndex;
            }

            public IUpdatableObject GetRelevantSubview(IObjectView view)
            {
                return view.GetSubView<ExpandedVariableProviderView>();
            }
        }
    }
}
