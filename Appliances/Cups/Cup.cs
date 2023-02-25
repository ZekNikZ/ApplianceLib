using ApplianceLib.Api.Prefab;
using ApplianceLib.Customs.GDO;
using ApplianceLib.Util;
using UnityEngine;
using static ApplianceLib.Api.References.ApplianceLibGDOs;

namespace ApplianceLib.Appliances.Cups
{
    public class Cup : ModItem<CupProvider>
    {
        public override string UniqueNameID => Ids.Cup;
        public override GameObject Prefab => Prefabs.Find("Cup");
        protected override void SetupPrefab(GameObject prefab)
        {
            prefab.AttachCup();
        }
    }
}
