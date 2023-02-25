using ApplianceLib.Api;
using Kitchen;
using Unity.Entities;

namespace ApplianceLib.Customs
{
    [UpdateBefore(typeof(ItemTransferGroup))]
    public class SwitchExpandedVariableProvider : ItemInteractionSystem
    {
        private CExpandedVariableProvider VariableProvider;

        private CItemProvider Provider;

        protected override bool IsPossible(ref InteractionData data)
        {
            return Require(data.Target, out VariableProvider) && Require(data.Target, out Provider);
        }

        protected override void Perform(ref InteractionData data)
        {
            VariableProvider.Current = (VariableProvider.Current + 1) % VariableProvider.Provides.Count;
            int provide = VariableProvider.Provide;
            SetComponent(data.Target, VariableProvider);
            Provider.SetAsItem(provide);
            SetComponent(data.Target, Provider);
        }
    }
}
