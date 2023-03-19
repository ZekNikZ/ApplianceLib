using KitchenData;
using KitchenLib.Customs;
using System;

namespace ApplianceLib.Customs.GDO
{
    [Obsolete("Will be removed in version 0.2.0. Use KitchenLib CustomProcess instead.", true)]
    public abstract class ModProcess : CustomProcess, IModGDO
    {
        public abstract override string UniqueNameID { get; }

        public override sealed void OnRegister(GameDataObject gdo)
        {
            Modify(gdo as Process);
        }

        protected virtual void Modify(Process process) { }
    }
}
