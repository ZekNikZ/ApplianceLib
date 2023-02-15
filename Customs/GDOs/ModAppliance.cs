using KitchenData;
using KitchenLib.Customs;
using UnityEngine;

namespace ApplianceLib.Customs
{
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

        internal ModAppliance() { }
    }
}
