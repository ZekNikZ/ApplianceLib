using ApplianceLib.Appliances.Blender;
using ApplianceLib.Appliances.Cups;
using ApplianceLib.Appliances.DeepFryer;
using ApplianceLib.Appliances.MixingBowl;
using KitchenData;
using KitchenLib.Customs;
using KitchenLib.Utils;

namespace ApplianceLib.Api.References
{
    public class ApplianceLibGDOs
    {
        public class Ids
        {
            public const string ModId = Mod.MOD_GUID;

            public const string BlenderAppliance = "BlenderAppliance";
            public const string AutoBlenderAppliance = "AutoBlenderAppliance";
            public const string BlenderCup = "BlenderCup";
            public const string BlendProcess = "BlendProcess";

            public const string Cup = "Cup";
            public const string CupSource = "CupSource";

            public const string DeepFryerAppliance = "DeepFryerAppliance";
            public const string DeepFryProcess = "DeepFryProcess";

            public const string MixingBowl = "MixingBowl";
            public const string MixingBowlSource = "MixingBowlSource";
        }

        public class Refs
        {
            public static Appliance Blender => Find<Appliance, Blender>();
            public static Item BlenderCup => Find<Item, BlenderCup>();
            public static Process BlendProcess => Find<Process, BlendProcess>();

            public static Item Cup => Find<Item, Cup>();
            public static Appliance CupProvider => Find<Appliance, CupProvider>();

            public static Appliance DeepFryer => Find<Appliance, DeepFryer>();
            public static Process DeepFryProcess => Find<Process, DeepFryProcess>();

            public static Item MixingBowl => Find<Item, MixingBowl>();
            public static Appliance MixingBowlProvider => Find<Appliance, MixingBowlProvider>();

            internal static T Find<T>(int id) where T : GameDataObject
            {
                return (T)GDOUtils.GetExistingGDO(id) ?? (T)GDOUtils.GetCustomGameDataObject(id)?.GameDataObject;
            }

            internal static T Find<T, C>() where T : GameDataObject where C : CustomGameDataObject
            {
                return GDOUtils.GetCastedGDO<T, C>();
            }

            internal static T Find<T>(string modName, string name) where T : GameDataObject
            {
                return GDOUtils.GetCastedGDO<T>(modName, name);
            }
        }
    }
}
