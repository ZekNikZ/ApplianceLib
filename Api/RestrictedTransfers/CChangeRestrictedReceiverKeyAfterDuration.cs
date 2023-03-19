using KitchenData;
using KitchenMods;
using Unity.Collections;

namespace ApplianceLib.Api
{
    public struct CChangeRestrictedReceiverKeyAfterDuration : IApplianceProperty, IModComponent
    {
        public FixedString32 ApplianceKey;
    }
}
