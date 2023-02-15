using KitchenData;
using KitchenLib.Customs;
using KitchenLib.Event;
using KitchenLib.References;
using KitchenLib.Utils;
using System.Collections.Generic;
using System.Linq;

namespace ApplianceLib.Api
{
    public enum ApplianceGroup
    {
        /// <summary>
        /// All surfaces that can be used like a counter.
        /// </summary>
        AllCounters,
        /// <summary>
        /// All surfaces that can be used like a counter at normal speed.
        /// </summary>
        NormalCounters,
        /// <summary>
        /// All surfaces that can be used like a counter at an increased speed.
        /// </summary>
        FastCounters,
        /// <summary>
        /// All mixers.
        /// </summary>
        Mixers,
        /// <summary>
        /// All hobs.
        /// </summary>
        Hobs,
        /// <summary>
        /// All sinks.
        /// </summary>
        Sinks,
        /// <summary>
        /// All tables, including coffee tables. See also ServableTables.
        /// </summary>
        AllTables,
        /// <summary>
        /// All tables that customers can order food from. See also AllTables.
        /// </summary>
        ServableTables,
        /// <summary>
        /// All bins.
        /// </summary>
        Bins
    }

    public static class ApplianceGroups
    {
        private static readonly Dictionary<ApplianceGroup, List<Appliance>> Appliances = new()
        {
            {
                ApplianceGroup.AllCounters,
                AppliancesFromIds(
                    ApplianceReferences.Countertop,
                    ApplianceReferences.Workstation,
                    ApplianceReferences.TrayStand,
                    ApplianceReferences.SourceOil
                )
            },
            {
                ApplianceGroup.NormalCounters,
                AppliancesFromIds(
                    ApplianceReferences.Countertop,
                    ApplianceReferences.TrayStand,
                    ApplianceReferences.SourceOil
                )
            },
            {
                ApplianceGroup.FastCounters,
                AppliancesFromIds(
                    ApplianceReferences.Workstation
                )
            },
            {
                ApplianceGroup.Mixers,
                AppliancesFromIds(
                    ApplianceReferences.Mixer,
                    ApplianceReferences.MixerHeated,
                    ApplianceReferences.MixerPusher,
                    ApplianceReferences.MixerRapid
                )
            },
            {
                ApplianceGroup.Hobs,
                AppliancesFromIds(
                    ApplianceReferences.Hob,
                    ApplianceReferences.HobDanger,
                    ApplianceReferences.HobSafe,
                    ApplianceReferences.HobStarting
                )
            },
            {
                ApplianceGroup.Sinks,
                AppliancesFromIds(
                    ApplianceReferences.SinkStarting,
                    ApplianceReferences.SinkNormal,
                    ApplianceReferences.SinkLarge,
                    ApplianceReferences.SinkPower,
                    ApplianceReferences.SinkSoak
                )
            },
            {
                ApplianceGroup.AllTables,
                AppliancesFromIds(
                    ApplianceReferences.TableBar,
                    ApplianceReferences.TableBasicCloth,
                    ApplianceReferences.TableCheapMetal,
                    ApplianceReferences.TableFancyCloth,
                    ApplianceReferences.TableLarge,
                    ApplianceReferences.CoffeeTable
                )
            },
            {
                ApplianceGroup.ServableTables,
                AppliancesFromIds(
                    ApplianceReferences.TableBar,
                    ApplianceReferences.TableBasicCloth,
                    ApplianceReferences.TableCheapMetal,
                    ApplianceReferences.TableFancyCloth,
                    ApplianceReferences.TableLarge
                )
            },
            {
                ApplianceGroup.Bins,
                AppliancesFromIds(
                    ApplianceReferences.Bin,
                    ApplianceReferences.BinCompactor,
                    ApplianceReferences.BinComposter,
                    ApplianceReferences.BinExpanded,
                    ApplianceReferences.BinStarting,
                    ApplianceReferences.AffordableBin
                )
            }
        };
        private static readonly Dictionary<ApplianceGroup, List<Appliance.ApplianceProcesses>> Processes = new();

        public static void AddApplianceToGroup(ApplianceGroup group, CustomAppliance appliance)
        {
            AddApplianceToGroup(group, appliance.GameDataObject as Appliance);
        }

        public static void AddApplianceToGroup(ApplianceGroup group, Appliance appliance)
        {
            Appliances[group].Add(appliance);
        }

        public static void AddProcessToGroup(ApplianceGroup group, Appliance.ApplianceProcesses applianceProcess)
        {
            if (!Processes.ContainsKey(group))
            {
                Processes.Add(group, new());
            }

            Processes[group].Add(applianceProcess);
        }

        internal static void BuildGameDataEventCallback(object sender, BuildGameDataEventArgs args)
        {
            foreach (var entry in Processes)
            {
                foreach (var appliance in Appliances[entry.Key])
                {
                    appliance.Processes.AddRange(entry.Value);
                }
            }
        }

        private static List<Appliance> AppliancesFromIds(params int[] applianceIds)
        {
            return applianceIds.Select(id => (Appliance)GDOUtils.GetExistingGDO(id)).ToList();
        }
    }
}
