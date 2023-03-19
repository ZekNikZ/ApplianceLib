using KitchenData;
using KitchenMods;

namespace ApplianceLib.Api
{
    public enum FlexibleProcessType
    {
        UseTakesDurationTime,
        Additive,
        Average
    }

    public struct CAppliesProcessToFlexible : IApplianceProperty, IModComponent
    {
        public int MinimumItems;
        public FlexibleProcessType ProcessType;
        public float ProcessTimeMultiplier;
        public float MinimumProcessTime;
    }
}
