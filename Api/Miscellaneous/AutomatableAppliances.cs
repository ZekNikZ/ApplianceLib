using KitchenData;
using KitchenLib.Customs;
using KitchenLib.References;
using System.Collections.Generic;

namespace ApplianceLib.Api
{
    public class AutomatableAppliances
    {
        public enum Automator
        {
            Portioner = ApplianceReferences.Portioner,
            Combiner = ApplianceReferences.Combiner
        }

        internal static readonly Dictionary<Automator, HashSet<int>> MarkedAppliances = new()
        {
            { Automator.Portioner, new() },
            { Automator.Combiner, new() },
        };

        /// <summary>
        /// Enables the specific appliance to have its processes be automatable
        /// by placing the specified automator facing it.
        /// </summary>
        /// <param name="automator">The automator which enables this automation.</param>
        /// <param name="appliance">The appliance to make automatable.</param>
        public static void MakeAutomatable(Automator automator, CustomAppliance appliance)
        {
            MakeAutomatable(automator, appliance.ID);
        }

        /// <summary>
        /// Enables the specific appliance to have its processes be automatable
        /// by placing the specified automator facing it.
        /// </summary>
        /// <param name="automator">The automator which enables this automation.</param>
        /// <param name="appliance">The appliance to make automatable.</param>
        public static void MakeAutomatable(Automator automator, Appliance appliance)
        {
            MakeAutomatable(automator, appliance.ID);
        }

        /// <summary>
        /// Enables the specific appliance to have its processes be automatable
        /// by placing the specified automator facing it.
        /// </summary>
        /// <param name="automator">The automator which enables this automation.</param>
        /// <param name="applianceId">The appliance to make automatable.</param>
        public static void MakeAutomatable(Automator automator, int applianceId)
        {
            MarkedAppliances[automator].Add(applianceId);
        }
    }
}
