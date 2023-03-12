using ApplianceLib.Api;
using ApplianceLib.Api.Prefab;
using ApplianceLib.Api.References;
using ApplianceLib.Customs.GDO;
using ApplianceLib.Util;
using KitchenData;
using UnityEngine;
using static ApplianceLib.Api.References.ApplianceLibGDOs;

namespace ApplianceLib.Appliances.Blender
{
    public class BlenderCup: ModItem
    {
        public override string UniqueNameID => Ids.BlenderCup;
        public override GameObject Prefab => Prefabs.Find("BlenderCup");
        public override bool IsIndisposable => true;

        protected override void SetupPrefab(GameObject prefab)
        {
            prefab.AttachBlenderCup();
        }

        protected override void Modify(Item item)
        {
            RestrictedItemTransfers.AllowItem(RestrictedTransferKeys.Blender, item);
        }
    }
}
