using ApplianceLib.Customs;
using KitchenData;
using KitchenLib.Utils;
using System.Collections.Generic;

namespace ApplianceLib.Appliances.Blender
{
    public class BlendProcess : ModProcess
    {
        public override string UniqueNameID => "BlendProcess";
        public override GameDataObject BasicEnablingAppliance => Refs.Blender;
        public override int EnablingApplianceCount => 1;
        public override bool CanObfuscateProgress => true;
        public override List<(Locale, ProcessInfo)> InfoList => new()
        {
            (Locale.English, LocalisationUtils.CreateProcessInfo("Blend", "<sprite name=\"knead\">"))
        };
    }
}
