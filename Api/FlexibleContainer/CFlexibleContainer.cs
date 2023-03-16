using ApplianceLib.Api.Miscellaneous;
using KitchenData;
using KitchenMods;

namespace ApplianceLib.Api.FlexibleContainer
{
    public struct CFlexibleContainer : IApplianceProperty, IModComponent
    {
        public int Maximum;
        public ComponentItemList Items;
    }
}
