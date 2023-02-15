using ApplianceLib.Appliances.Blender;
using ApplianceLib.Appliances.DeepFryer;
using ApplianceLib.Appliances.MixingBowl;
using KitchenData;
using KitchenLib.Customs;
using KitchenLib.References;
using KitchenLib.Utils;

namespace ApplianceLib
{
    internal class Refs
    {
        #region Vanilla References
        public static Item Tomato => Find<Item>(ItemReferences.Tomato);
        #endregion

        #region Modded References
        public static Item MixingBowl => Find<Item, MixingBowl>();
        public static Appliance DeepFryer => Find<Appliance, DeepFryer>();
        public static Process DeepFryProcess => Find<Process, DeepFryProcess>();
        public static Item BlenderCup => Find<Item, BlenderCup>();
        public static Appliance Blender => Find<Appliance, Blender>();
        public static Process BlendProcess => Find<Process, BlendProcess>();
        #endregion

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

        private static Appliance.ApplianceProcesses FindApplianceProcess<C>() where C : CustomSubProcess
        {
            ((CustomApplianceProccess)CustomSubProcess.GetSubProcess<C>()).Convert(out var process);
            return process;
        }
    }
}
