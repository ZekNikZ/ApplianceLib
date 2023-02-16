using Kitchen;
using ApplianceLib.Customs;
using ApplianceLib.Util;
using KitchenData;
using KitchenLib.Utils;
using System.Collections.Generic;
using UnityEngine;
using ApplianceLib.Api;

namespace ApplianceLib.Appliances.Blender
{
    public class Blender : ModAppliance
    {
        public override string UniqueNameID => "BlenderAppliance";
        public override GameObject Prefab => Prefabs.Find("Blender");
        public override PriceTier PriceTier => PriceTier.Medium;
        public override bool SellOnlyAsDuplicate => true;
        public override bool IsPurchasable => true;
        public override ShoppingTags ShoppingTags => ShoppingTags.Cooking | ShoppingTags.Misc;
        public override List<(Locale, ApplianceInfo)> InfoList => new()
        {
            (Locale.English, LocalisationUtils.CreateApplianceInfo("Blender", "Blends ingredients together", new(), new()))
        };
        public override List<IApplianceProperty> Properties => new()
        {
            new CItemHolder(),
            KitchenPropertiesUtils.GetCItemProvider(Refs.BlenderCup.ID, 1, 1, false, false, true, false, false, true, false),
            new CRequiresActivation(),
            new CIsInactive(),
            new CItemTransferRestrictions
            {
                AllowWhenActive = false,
                AllowWhenInactive = true
            },
            new CRestrictedReceiver
            {
                ApplianceKey = RestrictedTransferKeys.Blender
            }
        };
        public override List<Appliance.ApplianceProcesses> Processes => new()
        {
            new Appliance.ApplianceProcesses
            {
                Process = Refs.BlendProcess,
                IsAutomatic = true,
                Speed = 1
            }
        };

        protected override void SetupPrefab(GameObject prefab)
        {
            // Materials
            prefab.SetupMaterialsLikeCounter();
            prefab.ApplyMaterialToChild("BlenderModel/Base", MaterialHelpers.GetMaterialArray("Plastic - Red", "Plastic - Red", "Metal", "Metal"));
            prefab.ApplyMaterialToChild("BlenderModel/Blade", MaterialHelpers.GetMaterialArray("Metal Black"));
            prefab.ApplyMaterialToChild("BlenderModel/Lid", MaterialHelpers.GetMaterialArray("Plastic - Red", "Metal", "Metal"));
            prefab.ApplyMaterialToChild("HoldPoint/BlenderCup/Cup", MaterialHelpers.GetMaterialArray("Door Glass", "Door Glass", "Door Glass"));

            // Hold point
            var holdTransform = prefab.GetChildFromPath("HoldPoint").transform;
            var holdPoint = prefab.AddComponent<HoldPointContainer>();
            holdPoint.HoldPoint = holdTransform;

            // Item source view
            var sourceView = prefab.AddComponent<LimitedItemSourceView>();
            sourceView.HeldItemPosition = holdTransform;
            ReflectionUtils.GetField<LimitedItemSourceView>("Items").SetValue(sourceView, new List<GameObject>()
            {
                GameObjectUtils.GetChildObject(prefab, "HoldPoint/BlenderCup")
            });

            // Animations
            var processView = prefab.AddComponent<ApplianceProcessView>();
            processView.HeldItemPosition = holdTransform;
            ReflectionUtils.GetField<ApplianceProcessView>("Animator").SetValue(processView, prefab.GetComponent<Animator>());
        }
    }
}
