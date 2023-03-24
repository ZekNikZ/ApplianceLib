using ApplianceLib.Api;
using ApplianceLib.Api.References;
using KitchenData;
using KitchenLib.Customs;
using KitchenLib.Utils;
using System.Collections.Generic;
using static ApplianceLib.Api.References.ApplianceLibGDOs;

namespace ApplianceLib.Appliances.Blender
{
    public class BlendProcess : CustomProcess
    {
        public override string UniqueNameID => Ids.BlendProcess;
        public override GameDataObject BasicEnablingAppliance => Refs.Blender;
        public override int EnablingApplianceCount => 1;
        public override bool CanObfuscateProgress => true;
        public override List<(Locale, ProcessInfo)> InfoList => new()
        {
            (Locale.English, LocalisationUtils.CreateProcessInfo("Blend", "<sprite name=\"blend\">"))
        };

        public override void OnRegister(Process process)
        {
            RestrictedItemTransfers.AllowProcessableItems(RestrictedTransferKeys.Blender, process);
        }
    }
}
