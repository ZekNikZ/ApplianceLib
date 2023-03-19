using ApplianceLib.Api;
using ApplianceLib.Api.Prefab;
using ApplianceLib.Api.References;
using ApplianceLib.Util;
using KitchenData;
using KitchenLib.Customs;
using UnityEngine;
using static ApplianceLib.Api.References.ApplianceLibGDOs;

namespace ApplianceLib.Appliances.Blender
{
    public class BlenderCup: CustomItem
    {
        public override string UniqueNameID => Ids.BlenderCup;
        public override GameObject Prefab => Prefabs.Create("BlenderCup");
        public override bool IsIndisposable => true;

        public override void SetupPrefab(GameObject prefab)
        {
            prefab.AttachBlenderCup();
        }

        public override void OnRegister(Item item)
        {
            RestrictedItemTransfers.AllowItem(RestrictedTransferKeys.Blender, item);
        }
    }
}
