using KitchenData;
using System.Collections.Generic;

namespace ApplianceLib.Api
{
    public class RestrictedItemSplits
    {
        private static readonly Dictionary<string, HashSet<int>> AllowedItems = new();
        private static readonly HashSet<int> BlacklistItems = new();

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
        /// Blacklist an item to only be splittable on it's registed
        /// appliance keys.
        /// </summary>
        /// <param name="item">The item to blacklist.</param>
        public static void BlacklistItem(Item item)
        {
            BlacklistItem(item.ID);
        }

        /// <summary>
        /// Blacklist an item to only be splittable on it's registed
        /// appliance keys.
        /// </summary>
        /// <param name="itemId">The item to blacklist.</param>
        public static void BlacklistItem(int itemId)
        {
            BlacklistItems.Add(itemId);
        }

        /// <summary>
        /// Checks if a specific item is allowed to be split on an appliance group.
        /// </summary>
        /// <param name="applianceKey">The appliance key.</param>
        /// <param name="itemId">The item to check.</param>
        /// <returns></returns>
        public static bool IsAllowed(string applianceKey, int itemId)
        {
            return AllowedItems.ContainsKey(applianceKey) && AllowedItems[applianceKey].Contains(itemId);
        }

        /// <summary>
        /// Checks if a specific item is blacklisted.
        /// </summary>
        /// <param name="itemId">The item to check.</param>
        /// <returns></returns>
        public static bool IsBlacklisted(int itemId)
        {
            return BlacklistItems.Contains(itemId);
        }
    }
}
