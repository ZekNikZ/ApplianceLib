using KitchenData;
using KitchenMods;

namespace ApplianceLib.Api
{
    public struct CExpandedVariableProvider: IApplianceProperty, IModComponent
    {
        public int Current;
        public ItemList Provides;

        public int Provide => Provides[Current];
    }
}
