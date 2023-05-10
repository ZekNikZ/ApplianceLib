using ApplianceLib.Api.Prefab;
using ApplianceLib.Customs.GDO;
using ApplianceLib.Util;
using KitchenData;
using KitchenLib.Customs;
using UnityEngine;
using static ApplianceLib.Api.References.ApplianceLibGDOs;

namespace ApplianceLib.Appliances.MixingBowl
{
    public class MixingBowl : CustomItem, IPreventRegistration
    {
        public override string UniqueNameID => Ids.MixingBowl;
        public override GameObject Prefab => Prefabs.Create("MixingBowl");
        public override Appliance DedicatedProvider => Refs.MixingBowlProvider;
        public override bool IsIndisposable => true;

        public override void SetupPrefab(GameObject prefab)
        {
            prefab.AttachMixingBowl();
        }
    }
}
