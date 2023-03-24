using ApplianceLib.Util;
using Kitchen;
using KitchenData;
using KitchenLib.Colorblind;
using KitchenLib.Customs;
using System;
using UnityEngine;

namespace ApplianceLib.Customs.GDO
{
    [Obsolete("Will be removed in version 0.3.0. Use KitchenLib CustomItemGroup instead.", true)]
    public abstract class ModItemGroup<T> : CustomItemGroup<T>, IModGDO where T : ItemGroupView
    {
        public abstract override string UniqueNameID { get; }
        protected virtual Vector3 ColorblindLabelPosition { get; private set; } = Vector3.zero;
        protected virtual bool AddColorblindLabel { get; private set; } = true;

        public override ItemCategory ItemCategory => ItemCategory.Generic;
        public override ItemStorage ItemStorageFlags => ItemStorage.None;

        public override sealed void OnRegister(GameDataObject gdo)
        {
            if (Prefab != null)
            {
                SetupPrefab(Prefab);
            }

            Modify(gdo as ItemGroup);

            if (AddColorblindLabel && Prefab.TryGetComponent<ItemGroupView>(out var itemGroupView))
            {
                GameObject clonedColourBlind = ColorblindUtils.cloneColourBlindObjectAndAddToItem(GameDataObject as ItemGroup);
                ColorblindUtils.setColourBlindLabelObjectOnItemGroupView(itemGroupView, clonedColourBlind);
                clonedColourBlind.transform.localPosition = ColorblindLabelPosition;
            }
        }

        protected virtual void SetupPrefab(GameObject prefab) { }

        protected virtual void Modify(ItemGroup itemGroup) { }
    }

    [Obsolete("Will be removed in version 0.3.0. Use KitchenLib CustomItem instead.", true)]
    public abstract class ModItemGroup: ModItemGroup<DummyItemGroupView>
    {

    }
}
