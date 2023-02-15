using ApplianceLib.Api;
using ApplianceLib.Customs;
using ApplianceLib.Util;
using KitchenData;
using System.Collections.Generic;
using UnityEngine;

namespace ApplianceLib.Appliances.MixingBowl
{
    public class MixingBowl : ModItem<MixingBowlProvider>
    {
        public override string UniqueNameID => "MixingBowlItem";
        public override GameObject Prefab => Prefabs.Find("MixingBowl");
        public override bool IsIndisposable => true;
        public override List<Item.ItemProcess> Processes => new()
        {
            new()
            {
                Process = Refs.BlendProcess,
                Duration = 1,
                Result = Refs.Tomato
            }
        };

        protected override void SetupPrefab(GameObject prefab)
        {
            prefab.ApplyMaterialToChild("Model", MaterialReferences.MixingBowl);
        }
    }
}
