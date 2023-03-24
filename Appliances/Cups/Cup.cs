using ApplianceLib.Api.Prefab;
using ApplianceLib.Util;
using KitchenData;
using KitchenLib.Customs;
using UnityEngine;
using static ApplianceLib.Api.References.ApplianceLibGDOs;

namespace ApplianceLib.Appliances.Cups
{
    public class Cup : CustomItem
    {
        public override string UniqueNameID => Ids.Cup;
        public override GameObject Prefab => Prefabs.Find("Cup");
        public override Appliance DedicatedProvider => Refs.CupProvider;

        public override void SetupPrefab(GameObject prefab)
        {
            prefab.AttachCup();
        }
    }
}
