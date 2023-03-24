using ApplianceLib.Api.Miscellaneous;
using KitchenData;
using KitchenMods;

namespace ApplianceLib.Api
{
    public struct CFlexibleContainer : IApplianceProperty, IModComponent
    {
        public int Maximum;
        public ComponentItemList Items;
    }
}
