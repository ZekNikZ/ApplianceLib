using KitchenData;
using KitchenLib.References;
using static ApplianceLib.Api.References.ApplianceLibGDOs;

namespace ApplianceLib.Api.References
{
    public class RestrictedTransferKeys
    {
        public const string Blender = "Blender";
        public const string Cleanable = "Cleanable";
        public const string CleanedItems = "CleanedItems";

        internal static void Setup(GameData gameData)
        {
            RestrictedItemTransfers.AllowProcessableItems(Cleanable, Refs.Find<Process>(ProcessReferences.Clean));

            foreach (var item in gameData.Get<Item>())
            {
                foreach (var process in item.DerivedProcesses)
                {
                    if (process.Process == Refs.Find<Process>(ProcessReferences.Clean))
                    {
                        RestrictedItemTransfers.AllowItem(CleanedItems, process.Result);
                    }
                }
            }
        }
    }
}
