﻿using Kitchen;
using ApplianceLib.Util;
using KitchenData;
using KitchenLib.Utils;
using System.Collections.Generic;
using UnityEngine;
using ApplianceLib.Api;
using ApplianceLib.Api.References;
using static ApplianceLib.Api.References.ApplianceLibGDOs;
using ApplianceLib.Api.Prefab;
using KitchenLib.Customs;

namespace ApplianceLib.Appliances.Blender
{
    public class FastBlender : CustomAppliance
    {
        public override string UniqueNameID => Ids.FastBlenderAppliance;
        public override GameObject Prefab => Prefabs.Find("Blender", "Fast");
        public override PriceTier PriceTier => PriceTier.Expensive;
        public override bool SellOnlyAsDuplicate => true;
        public override bool IsPurchasableAsUpgrade => true;
        public override ShoppingTags ShoppingTags => ShoppingTags.Cooking | ShoppingTags.Misc;
        public override List<(Locale, ApplianceInfo)> InfoList => new()
        {
            (Locale.English, LocalisationUtils.CreateApplianceInfo("Fast Blender", "Blends ingredients together", new(), new()))
        };
        public override List<Appliance> Upgrades => new()
        {
            Refs.AutoBlender
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
            },
            new CPreventUseWhenEmpty()
        };
        public override List<Appliance.ApplianceProcesses> Processes => new()
        {
            new Appliance.ApplianceProcesses
            {
                Process = Refs.BlendProcess,
                IsAutomatic = true,
                Speed = 2
            }
        };

        public override void SetupPrefab(GameObject prefab)
        {
            // Materials
            prefab.AttachCounter(CounterType.Drawers);
            prefab.ApplyMaterialToChild("BlenderModel/Base", MaterialUtils.GetMaterialArray("Plastic - Yellow", "Plastic - Yellow", "Metal", "Metal"));
            prefab.ApplyMaterialToChild("BlenderModel/Blade", MaterialUtils.GetMaterialArray("Metal Black"));
            prefab.ApplyMaterialToChild("BlenderModel/Lid", MaterialUtils.GetMaterialArray("Plastic - Yellow", "Metal", "Metal"));
            prefab.ApplyMaterialToChild("HoldPoint/BlenderCup/Cup", MaterialUtils.GetMaterialArray("Door Glass", "Door Glass", "Door Glass"));

            // Hold point
            var holdTransform = prefab.GetChild("HoldPoint").transform;
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
