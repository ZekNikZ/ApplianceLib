using ApplianceLib.Api;
using ApplianceLib.Api.References;
using ApplianceLib.Customs.GDO;
using KitchenData;
using KitchenLib.Utils;
using System.Collections.Generic;
using static ApplianceLib.Api.References.ApplianceLibGDOs;

namespace ApplianceLib.Appliances.Blender
{
    public class BlendProcess : ModProcess
    {
        public override string UniqueNameID => Ids.BlendProcess;
        public override GameDataObject BasicEnablingAppliance => Refs.Blender;
        public override int EnablingApplianceCount => 1;
        public override bool CanObfuscateProgress => true;
        public override List<(Locale, ProcessInfo)> InfoList => new()
        {
            (Locale.English, LocalisationUtils.CreateProcessInfo("Blend", "<sprite name=\"blend\">"))
        };

        protected override void Modify(Process process)
        {
            RestrictedItemTransfers.AllowProcessableItems(RestrictedTransferKeys.Blender, process);
        }
    }
}
