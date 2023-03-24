using KitchenData;
using KitchenLib.Customs;
using System;
using UnityEngine;
using static ApplianceLib.Api.References.ApplianceLibGDOs;

namespace ApplianceLib.Customs.GDO
{
    [Obsolete("Will be removed in version 0.3.0. Use KitchenLib CustomItem instead.", true)]
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
    }

    [Obsolete("Will be removed in version 0.3.0. Use KitchenLib CustomItem instead.", true)]
    public abstract class ModItem<T> : ModItem where T : CustomAppliance
    {
        public override Appliance DedicatedProvider => Refs.Find<Appliance, T>();
    }
}
