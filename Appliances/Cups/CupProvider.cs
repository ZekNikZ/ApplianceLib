using ApplianceLib.Api.Prefab;
using ApplianceLib.Api.References;
using ApplianceLib.Util;
using KitchenData;
using KitchenLib.Customs;
using KitchenLib.Utils;
using System.Collections.Generic;
using UnityEngine;
using static ApplianceLib.Api.References.ApplianceLibGDOs;

namespace ApplianceLib.Appliances.Cups
{
    public class CupProvider : CustomAppliance
    {
        public override string UniqueNameID => Ids.CupSource;
        public override GameObject Prefab => Prefabs.Find("CupProvider");
        public override PriceTier PriceTier => PriceTier.Medium;
        public override bool SellOnlyAsDuplicate => true;
        public override bool IsPurchasable => true;
        public override ShoppingTags ShoppingTags => ShoppingTags.Cooking | ShoppingTags.Misc;
        public override List<(Locale, ApplianceInfo)> InfoList => new()
        {
            (Locale.English, LocalisationUtils.CreateApplianceInfo("Cups", "Provides cups", new(), new()))
        };
        public override List<IApplianceProperty> Properties => new()
        {
            KitchenPropertiesUtils.GetUnlimitedCItemProvider(Refs.Cup.ID)
        };

        public override void SetupPrefab(GameObject prefab)
        {
            prefab.AttachCounter(CounterType.Drawers);

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Prefab.ApplyMaterialToChild($"Cups/CupStack{i}/Cup{j}/Model/Cup", MaterialReferences.CupBase);
                }
            }
        }
    }
}
