using KitchenData;
using KitchenLib.Customs;

namespace ApplianceLib.Customs.GDO
{
    public abstract class ModUnlockCard : CustomUnlockCard, IModGDO
    {
        public abstract override string UniqueNameID { get; }
        public override CardType CardType => CardType.Default;
        public override UnlockGroup UnlockGroup => UnlockGroup.Generic;
        public override bool IsUnlockable => true;

        public override sealed void OnRegister(GameDataObject gdo)
        {
            Modify(gdo as UnlockCard);
        }

        protected virtual void Modify(UnlockCard dish) { }
    }
}
