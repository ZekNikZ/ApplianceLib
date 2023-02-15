using KitchenData;

namespace ApplianceLib.Api
{
    public class RestrictedItemTransfers
    {
        /// <summary>
        /// Adds an item to the allow list for an appliance key.
        /// </summary>
        /// <param name="applianceKey">The appliance key.</param>
        /// <param name="item">The item to allow.</param>
        public static void AllowItem(string applianceKey, Item item)
        {
            AllowItem(applianceKey, item.ID);
        }

        /// <summary>
        /// Adds an item to the allow list for an appliance key.
        /// </summary>
        /// <param name="applianceKey">The appliance key.</param>
        /// <param name="itemId">The item to allow.</param>
        public static void AllowItem(string applianceKey, int itemId)
        {

        }

        /// <summary>
        /// Adds all items with a certain process to the allow list for an appliance key.
        /// </summary>
        /// <param name="applianceKey">The appliance key.</param>
        /// <param name="process">The process to allow.</param>
        public static void AllowProcessableItems(string applianceKey, Process process)
        {
            AllowProcessableItems(applianceKey, process.ID);
        }

        /// <summary>
        /// Adds all items with a certain process to the allow list for an appliance key.
        /// </summary>
        /// <param name="applianceKey">The appliance key.</param>
        /// <param name="processId">The process to allow.</param>
        public static void AllowProcessableItems(string applianceKey, int processId)
        {

        }
    }
}
