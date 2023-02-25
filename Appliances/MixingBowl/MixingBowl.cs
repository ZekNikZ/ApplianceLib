using ApplianceLib.Api.Prefab;
using ApplianceLib.Customs.GDO;
using ApplianceLib.Util;
using UnityEngine;
using static ApplianceLib.Api.References.ApplianceLibGDOs;

namespace ApplianceLib.Appliances.MixingBowl
{
    public class MixingBowl : ModItem<MixingBowlProvider>
    {
        public override string UniqueNameID => Ids.MixingBowl;
        public override GameObject Prefab => Prefabs.Create("MixingBowl");
        public override bool IsIndisposable => true;

        protected override void SetupPrefab(GameObject prefab)
        {
            prefab.AttachMixingBowl();
        }
    }
}
