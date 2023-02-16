using KitchenData;
using KitchenMods;
using Unity.Collections;

namespace ApplianceLib.Api
{
    public struct CRestrictedReceiver: IApplianceProperty, IModComponent
    {
        public FixedString32 ApplianceKey;
    }
}
