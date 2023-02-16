using KitchenData;
using System.Collections.Generic;

namespace ApplianceLib.Api
{
    public class RestrictedItemTransfers
    {
        private static readonly Dictionary<string, HashSet<int>> AllowedItems = new();
        private static readonly Dictionary<string, HashSet<int>> AllowedProcesses = new();

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
            if (!AllowedItems.ContainsKey(applianceKey))
            {
                AllowedItems.Add(applianceKey, new());
            }

            AllowedItems[applianceKey].Add(itemId);
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
            if (!AllowedProcesses.ContainsKey(applianceKey))
            {
                AllowedProcesses.Add(applianceKey, new());
            }

            AllowedProcesses[applianceKey].Add(processId);
        }

        /// <summary>
        /// Checks if a specific item is allowed to be placed on an appliance group.
        /// </summary>
        /// <param name="applianceKey">The appliance key.</param>
        /// <param name="itemId">The item to check.</param>
        /// <returns></returns>
        public static bool IsAllowed(string applianceKey, int itemId)
        {
            if (AllowedItems.ContainsKey(applianceKey) && AllowedItems[applianceKey].Contains(itemId))
            {
                return true;
            }

            if (AllowedProcesses.ContainsKey(applianceKey) && GameData.Main.TryGet(itemId, out Item item))
            {
                foreach (var process in item.DerivedProcesses)
                {
                    if (AllowedProcesses[applianceKey].Contains(process.Process.ID))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
