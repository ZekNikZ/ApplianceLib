using ApplianceLib.Api;
using ApplianceLib.Customs;
using ApplianceLib.Util;
using KitchenData;
using UnityEngine;

namespace ApplianceLib.Appliances.Blender
{
    public class BlenderCup: ModItem
    {
        public override string UniqueNameID => "BlenderCup";
        public override GameObject Prefab => Prefabs.Find("BlenderCup");
        public override bool IsIndisposable => true;

        protected override void SetupPrefab(GameObject prefab)
        {
            prefab.ApplyMaterialToChild("Cup", MaterialHelpers.GetMaterialArray("Door Glass", "Door Glass", "Door Glass"));
        }

        protected override void Modify(Item item)
        {
            RestrictedItemTransfers.AllowItem(RestrictedTransferKeys.Blender, item);
        }
    }
}
