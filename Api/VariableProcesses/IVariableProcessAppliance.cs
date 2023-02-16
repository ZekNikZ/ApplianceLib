using KitchenData;
using System.Collections.Generic;

namespace ApplianceLib.Api
{
    public interface IVariableProcessAppliance
    {
        public struct VariableApplianceProcess
        {
            public ItemList Items;
            public List<Appliance.ApplianceProcesses> Processes;
        }

        public List<VariableApplianceProcess> VariableApplianceProcesses { get; }
    }
}
