using KitchenData;
using KitchenMods;

namespace ApplianceLib.Api.FlexibleContainer
{
    public struct CAppliesProcessToFlexible : IApplianceProperty, IModComponent
    {
        public int Minimum; // Minimum before process can apply

        public float ProcessTimeMultiplier;
        public float MinimumProcessTime;

        // Whitelist (optional)
        public int Process;
        public bool TransferWhitelist;
    }
}
