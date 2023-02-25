using ApplianceLib.Api;
using Kitchen;
using KitchenData;
using KitchenMods;
using Unity.Collections;
using Unity.Entities;

namespace ApplianceLib.Customs
{
    [UpdateInGroup(typeof(ItemTransferEarlyPrune))]
    internal class PruneByRestrictedSplits : GenericSystemBase, IModSystem
    {
        private EntityQuery TransferProposals;
        private EntityQuery CurrentSplits;

        protected override void Initialise()
        {
            TransferProposals = GetEntityQuery(
                typeof(CItemTransferProposal)
            );

            CurrentSplits = GetEntityQuery(
                typeof(CItem),
                typeof(CSplittableItem),
                typeof(CItemUndergoingProcess),
                typeof(CHeldBy)
            );
        }

        protected override void OnUpdate()
        {
            // Split proposals
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

                if (!proposal.Flags.HasFlag(TransferFlags.Split))
                {
                    continue;
                }

                if (Require(proposal.Source, out CAppliance appliance) && Require(proposal.Source, out CItemHolder itemHolder) && Require(itemHolder.HeldItem, out CItem item))
                {
                    if (GameData.Main.TryGet<Appliance>(appliance.ID, out var _))
                    {
                        bool isAllowed = false;
                        if (Require(proposal.Source, out CRestrictedSplitter source))
                        {
                            string applianceKey = source.ApplianceKey.ConvertToString();
                            if (RestrictedItemSplits.IsAllowed(applianceKey, item.ID))
                            {
                                isAllowed = true;
                            }
                        } 
                        
                        if (!isAllowed && RestrictedItemSplits.IsBlacklisted(item.ID))
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

            // Current splits
            using var itemEntities = CurrentSplits.ToEntityArray(Allocator.Temp);
            foreach (var entity in itemEntities)
            {
                if (Require(entity, out CHeldBy heldBy) && Require(heldBy.Holder, out CAppliance appliance) && Require(entity, out CItem item) && Require(entity, out CItemUndergoingProcess process) && process.Process == -1)
                {
                    if (GameData.Main.TryGet<Appliance>(appliance.ID, out var _))
                    {
                        bool isAllowed = false;
                        if (Require(heldBy.Holder, out CRestrictedSplitter source))
                        {
                            string applianceKey = source.ApplianceKey.ConvertToString();
                            if (RestrictedItemSplits.IsAllowed(applianceKey, item.ID))
                            {
                                isAllowed = true;
                            }
                        }

                        if (!isAllowed && RestrictedItemSplits.IsBlacklisted(item.ID))
                        {
                            EntityManager.RemoveComponent<CItemUndergoingProcess>(entity);
                        }
                    }
                }
            }
        }
    }
}
