using Kitchen;

namespace ApplianceLib.Util
{
    public class DummyItemGroupView : ItemGroupView
    {
        public void Awake()
        {
            ComponentGroups = new();
        }
    }
}
