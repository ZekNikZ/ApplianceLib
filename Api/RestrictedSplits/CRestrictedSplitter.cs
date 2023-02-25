using KitchenData;
using KitchenMods;
using Unity.Collections;

namespace ApplianceLib.Api
{
    public struct CRestrictedSplitter: IApplianceProperty, IModComponent
    {
        public FixedString32 ApplianceKey;
    }
}
