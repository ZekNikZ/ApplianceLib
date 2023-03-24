using KitchenData;
using KitchenMods;
using Unity.Collections;

namespace ApplianceLib.Api
{
    public struct CChangeRestrictedReceiverKeyWhenEmpty : IApplianceProperty, IModComponent
    {
        public FixedString32 ApplianceKey;
    }
}
