using KitchenData;
using KitchenLib.Customs;
using System.Collections.Generic;

namespace ApplianceLib.Api
{
    public class NotActuallyProviders
    {
        internal static HashSet<int> Appliances = new HashSet<int>();

        public static void RemoveProvidersFrom(int applianceId)
        {
            Appliances.Add(applianceId);
        }

        public static void RemoveProvidersFrom(Appliance appliance)
        {
            RemoveProvidersFrom(appliance.ID);
        }

        public static void RemoveProvidersFrom(CustomAppliance appliance)
        {
            RemoveProvidersFrom(appliance.ID);
        }
    }
}
