﻿using ApplianceLib.Api;
using ApplianceLib.Api.References;
using ApplianceLib.Util;
using KitchenData;
using KitchenLib.Customs;
using KitchenLib.Utils;
using UnityEngine;
using static ApplianceLib.Api.References.ApplianceLibGDOs;

namespace ApplianceLib.Appliances.Blender
{
    public class BlenderCup: CustomItem
    {
        public override string UniqueNameID => Ids.BlenderCup;
        public override GameObject Prefab => Prefabs.Find("BlenderCup");
        public override bool IsIndisposable => true;

        public override void SetupPrefab(GameObject prefab)
        {
            //prefab.AttachBlenderCup();
            prefab.ApplyMaterialToChild("Cup", MaterialUtils.GetMaterialArray("Door Glass", "Door Glass", "Door Glass"));
        }

        public override void OnRegister(Item item)
        {
            RestrictedItemTransfers.AllowItem(RestrictedTransferKeys.Blender, item);
        }
    }
}
