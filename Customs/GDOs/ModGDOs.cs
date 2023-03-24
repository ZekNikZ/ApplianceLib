using ApplianceLib.Customs.GDO;
using KitchenLib;
using KitchenLib.Customs;
using System.Reflection;

namespace ApplianceLib.Customs
{
    public static class ModGDOs
    {
        public static void RegisterModGDOs(this BaseMod mod, Assembly assembly)
        {
            MethodInfo mAddGameDataObject = typeof(BaseMod).GetMethod(nameof(BaseMod.AddGameDataObject));
            MethodInfo mAddSubProcess = typeof(BaseMod).GetMethod(nameof(BaseMod.AddSubProcess));
            foreach (var type in assembly.GetTypes())
            {
                if (type.IsAbstract)
                {
                    continue;
                }

                if (typeof(CustomGameDataObject).IsAssignableFrom(type) && !typeof(IPreventRegistration).IsAssignableFrom(type))
                {
                    MethodInfo generic = mAddGameDataObject.MakeGenericMethod(type);
                    generic.Invoke(mod, null);
                    mod.Log($"Registered custom GDO of type {type.Name}");
                }
            }
        }
    }
}
