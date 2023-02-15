using KitchenData;
using KitchenMods;

namespace ApplianceLib.Customs
{
    public struct CVariableProcessContainer : IApplianceProperty, IModComponent
    {
        public int Current;
        public int Max;
    }
}
