using ApplianceLib.Customs.GDO;
using KitchenData;
using KitchenLib.Utils;
using System.Collections.Generic;
using static ApplianceLib.Api.References.ApplianceLibGDOs;

namespace ApplianceLib.Appliances.DeepFryer
{
    public class DeepFryProcess : ModProcess
    {
        public override string UniqueNameID => Ids.DeepFryProcess;
        public override GameDataObject BasicEnablingAppliance => Refs.DeepFryer;
        public override int EnablingApplianceCount => 1;
        public override bool CanObfuscateProgress => true;
        public override List<(Locale, ProcessInfo)> InfoList => new()
        {
            (Locale.English, LocalisationUtils.CreateProcessInfo("Deep Fry", "deepfry"))
        };
    }
}
