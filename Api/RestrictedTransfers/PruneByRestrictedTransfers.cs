using Kitchen;
using KitchenMods;
using System;
using Unity.Entities;

namespace ApplianceLib.Api.RestrictedTransfers
{
    [UpdateInGroup(typeof(ItemTransferEarlyPrune))]
    internal class PruneByRestrictedTransfers : GenericSystemBase, IModSystem
    {
        protected override void OnUpdate()
        {
            throw new NotImplementedException();
        }
    }
}
