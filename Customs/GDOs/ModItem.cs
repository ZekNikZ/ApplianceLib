﻿using KitchenData;
using KitchenLib.Customs;
using UnityEngine;

namespace ApplianceLib.Customs
{
    public abstract class ModItem : CustomItem, IModGDO
    {
        public abstract override string UniqueNameID { get; }
        public override ItemCategory ItemCategory => ItemCategory.Generic;
        public override ItemStorage ItemStorageFlags => ItemStorage.None;

        public override sealed void OnRegister(GameDataObject gdo)
        {
            if (Prefab != null)
            {
                SetupPrefab(Prefab);
            }

            Modify(gdo as Item);
        }

        protected virtual void SetupPrefab(GameObject prefab) { }

        protected virtual void Modify(Item item) { }

        internal ModItem() { }
    }

    public abstract class ModItem<T> : ModItem where T : CustomAppliance
    {
        public override Appliance DedicatedProvider => Refs.Find<Appliance, T>();
    }
}
