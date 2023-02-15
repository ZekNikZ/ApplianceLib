using Kitchen;

namespace ApplianceLib.Util
{
    internal class DummyItemGroupView : ItemGroupView
    {
        public void Awake()
        {
            ComponentGroups = new();
        }
    }
}
