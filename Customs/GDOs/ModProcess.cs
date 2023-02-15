using KitchenData;
using KitchenLib.Customs;

namespace ApplianceLib.Customs
{
    public abstract class ModProcess : CustomProcess, IModGDO
    {
        public abstract override string UniqueNameID { get; }

        public override sealed void OnRegister(GameDataObject gdo)
        {
            Modify(gdo as Process);
        }

        protected virtual void Modify(Process process) { }

        internal ModProcess () { }
    }
}
