using KitchenData;
using System.Collections.Generic;

namespace ApplianceLib.Api
{
    public interface IVariableProcessAppliance
    {
        public List<Appliance.ApplianceProcesses> VariableApplianceProcesses { get; }
    }
}
