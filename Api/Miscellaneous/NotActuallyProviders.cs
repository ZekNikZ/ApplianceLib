using KitchenData;
using KitchenLib.Customs;
using System.Collections.Generic;

namespace ApplianceLib.Api
{
    public class NotActuallyProviders
    {
        internal static HashSet<int> Appliances = new HashSet<int>();

        /// <summary>
        /// Removes all CItemProviders from the specified appliance.
        /// </summary>
        /// <param name="applianceId">The appliance to remove providers from.</param>
        public static void RemoveProvidersFrom(int applianceId)
        {
            Appliances.Add(applianceId);
        }

        /// <summary>
        /// Removes all CItemProviders from the specified appliance.
        /// </summary>
        /// <param name="appliance">The appliance to remove providers from.</param>
        public static void RemoveProvidersFrom(Appliance appliance)
        {
            RemoveProvidersFrom(appliance.ID);
        }

        /// <summary>
        /// Removes all CItemProviders from the specified appliance.
        /// </summary>
        /// <param name="appliance">The appliance to remove providers from.</param>
        public static void RemoveProvidersFrom(CustomAppliance appliance)
        {
            RemoveProvidersFrom(appliance.ID);
        }
    }
}
