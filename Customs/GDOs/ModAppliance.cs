using KitchenData;
using KitchenLib.Customs;
using System;
using UnityEngine;

namespace ApplianceLib.Customs.GDO
{
    [Obsolete("Will be removed in version 0.3.0. Use KitchenLib CustomAppliance instead.", true)]
    public abstract class ModAppliance : CustomAppliance, IModGDO
    {
        public abstract override string UniqueNameID { get; }

        public override sealed void OnRegister(GameDataObject gdo)
        {
            if (Prefab != null)
            {
                SetupPrefab(Prefab);
            }

            Modify(gdo as Appliance);
        }

        protected virtual void SetupPrefab(GameObject prefab) { }

        protected virtual void Modify(Appliance appliance) { }
    }
}
