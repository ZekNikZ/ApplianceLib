using ApplianceLib.Api;
using ApplianceLib.Api.References;
using ApplianceLib.Util;
using KitchenData;
using KitchenLib.Customs;
using KitchenLib.References;
using KitchenLib.Utils;
using System.Collections.Generic;
using UnityEngine;
using static ApplianceLib.Api.References.ApplianceLibGDOs;

namespace ApplianceLib.Appliances.Blender
{
    public class DirtyBlenderCup: CustomItem
    {
        public override string UniqueNameID => Ids.DirtyBlenderCup;
        public override GameObject Prefab => Prefabs.Find("DirtyBlenderCup");
        public override bool IsIndisposable => true;

        public override List<Item.ItemProcess> Processes => new() { 
            new() { 
                Duration = 0.75f,
                Process = Refs.Find<Process>(ProcessReferences.Clean),
                Result = Refs.BlenderCup
            }
        };

        public override void SetupPrefab(GameObject prefab)
        {
            //prefab.AttachBlenderCup();
            prefab.ApplyMaterialToChild("Cup", MaterialUtils.GetMaterialArray("Door Glass", "Door Glass", "Door Glass"));
            prefab.ApplyMaterialToChild("Leftovers", MaterialUtils.GetMaterialArray("Mess", "Mess", "Mess", "Mess"));
        }

        public override void OnRegister(Item item)
        {
            RestrictedItemTransfers.AllowItem(RestrictedTransferKeys.Blender, item);
        }
    }
}
