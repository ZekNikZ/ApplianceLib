using ApplianceLib.Api;
using Kitchen;
using KitchenLib.Customs;
using KitchenMods;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace ApplianceLib.Customs
{
    [UpdateInGroup(typeof(ItemTransferEarlyPrune))]
    internal class PruneByRestrictedTransfers : GenericSystemBase, IModSystem
    {
        private EntityQuery TransferProposals;
        internal static EntityManager AttachedEntityManager;
        private static PruneByRestrictedTransfers Instance;

        protected override void Initialise()
        {
            TransferProposals = GetEntityQuery(
                typeof(CItemTransferProposal)
            );
            AttachedEntityManager = EntityManager;
            Instance = this;
        }

        protected override void OnUpdate()
        {
            using var entities = TransferProposals.ToEntityArray(Allocator.Temp);
            using var itemTransferProposals = TransferProposals.ToComponentDataArray<CItemTransferProposal>(Allocator.Temp);

            for (int i = 0; i < entities.Length; ++i)
            {
                var entity = entities[i];
                var proposal = itemTransferProposals[i];

                if (proposal.Status == ItemTransferStatus.Pruned)
                {
                    continue;
                }

                if (Require(proposal.Destination, out CRestrictedReceiver reciever) && Require(proposal.Destination, out CAppliance appliance))
                {
                    if (CustomGDO.GDOs.TryGetValue(appliance.ID, out var customAppliance))
                    {
                        string applianceKey = reciever.ApplianceKey.ConvertToString();
                        if (!RestrictedItemTransfers.IsAllowed(applianceKey, proposal.ItemType))
                        {
                            proposal.Status = ItemTransferStatus.Pruned;
                        }
                    }
                }

                if (proposal.Status == ItemTransferStatus.Pruned)
                {
                    proposal.PrunedBy = this;
                }

                SetComponent(entity, proposal);
            }
        }

        internal static Entity GetOccupantAt(Vector3 position)
        {
            return Instance.GetOccupant(position);
        }

        internal static bool CanReachFrom(Vector3 from, Vector3 to)
        {
            return Instance.CanReach(from, to);
        }
    }
}
