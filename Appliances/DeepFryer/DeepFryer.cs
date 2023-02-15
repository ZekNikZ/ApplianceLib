using Kitchen;
using ApplianceLib.Customs;
using ApplianceLib.Util;
using KitchenData;
using KitchenLib.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace ApplianceLib.Appliances.DeepFryer
{
    public class DeepFryer : ModAppliance
    {
        public override string UniqueNameID => "DeepFryerAppliance";
        public override GameObject Prefab => Prefabs.Find("DeepFryer");
        public override PriceTier PriceTier => PriceTier.Medium;
        public override bool SellOnlyAsDuplicate => true;
        public override bool IsPurchasable => true;
        public override ShoppingTags ShoppingTags => ShoppingTags.Cooking | ShoppingTags.Misc;
        public override List<(Locale, ApplianceInfo)> InfoList => new()
        {
            (Locale.English, LocalisationUtils.CreateApplianceInfo("Deep Fryer", "Deep fries food", new(), new()))
        };
        public override List<IApplianceProperty> Properties => new()
        {
            new CItemHolder(),
            new CRequiresActivation(),
            new CIsInactive(),
            new CItemTransferRestrictions
            {
                AllowWhenActive = false,
                AllowWhenInactive = true
            },
            new CRestrictProgressVisibility
            {
                HideWhenActive = false,
                HideWhenInactive = false,
                ObfuscateWhenActive = true,
                ObfuscateWhenInactive = false
            }
        };
        public override List<Appliance.ApplianceProcesses> Processes => new()
        {
            new Appliance.ApplianceProcesses
            {
                Process = Refs.DeepFryProcess,
                IsAutomatic = true,
                Speed = 1
            }
        };

        protected override void SetupPrefab(GameObject prefab)
        {
            prefab.ApplyMaterialToChild("DeepFryerModel/Base", MaterialHelpers.GetMaterialArray("Metal- Shiny"));
            prefab.ApplyMaterialToChild("DeepFryerModel/Basket", MaterialHelpers.GetMaterialArray("Metal- Shiny"));
            prefab.ApplyMaterialToChild("DeepFryerModel/Liquid", MaterialHelpers.GetMaterialArray("Metal- Shiny"));
        }
    }
}
