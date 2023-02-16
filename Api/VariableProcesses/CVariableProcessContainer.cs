using KitchenData;
using KitchenMods;

namespace ApplianceLib.Api
{
    public struct CVariableProcessContainer : IApplianceProperty, IModComponent
    {
        public int Current;
        public int Max;
    }
}
