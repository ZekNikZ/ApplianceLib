using ApplianceLib.Api;
using ApplianceLib.Api.FlexibleContainer;
using ApplianceLib.Util;
using Kitchen;
using KitchenData;
using KitchenMods;
using System.Linq;
using Unity.Collections;
using Unity.Entities;

namespace ApplianceLib.Customs.FlexibleContainer
{
    [UpdateInGroup(typeof(ItemTransferAccept))]
    public class AcceptIntoFlexible : TransferAcceptSystem, IModSystem
    {
        private EntityQuery proposalQuery;
        protected override void Initialise()
        {
            base.Initialise();
            proposalQuery = GetEntityQuery(new QueryHelper().All(typeof(CItemTransferProposal)));
            this.RegisterTransfer();
        }

        public override void AcceptTransfer(Entity proposer, Entity accepter, EntityContext ctx, out Entity return_item)
        {
            return_item = default;
            if (!Require<CItemTransferProposal>(proposer, out var proposal) || !Require<CFlexibleContainer>(proposal.Destination, out var storage))
                return;

            storage.Items.Add(proposal.ItemData.ID, proposal.ItemComponents);
            ctx.Set(proposal.Destination, storage);
            ctx.Destroy(proposal.Item);
        }

        protected override void OnUpdate()
        {
            using var entities = proposalQuery.ToEntityArray(Allocator.Temp);

            foreach (var entity in entities)
            {
                Require<CItemTransferProposal>(entity, out var proposal);
                if (proposal.Status == ItemTransferStatus.Pruned || (proposal.Flags & TransferFlags.RequireDrop) != 0 || proposal.ItemData.IsPartial ||
                    !Require<CFlexibleContainer>(proposal.Destination, out var storage) || !Data.TryGet<Item>(proposal.ItemData.ID, out var item) || storage.Items.Count >= storage.Maximum)
                    continue;

                if (!Has<CIsInactive>(proposal.Destination) && Has<CLockedWhileDuration>(proposal.Destination))
                    continue;

                if (Has<CPreventUse>(proposal.Destination) || Has<CPreventItemTransfer>(proposal.Destination))
                    continue;

                if (Require<CAppliesProcessToFlexible>(proposal.Destination, out var flexibleProcess) && flexibleProcess.TransferWhitelist && !item.DerivedProcesses.Any(process => process.Process.ID == flexibleProcess.Process))
                    continue;

                if (Require<CRestrictedReceiver>(proposal.Destination, out var restrictedReceiver) && !RestrictedItemTransfers.IsAllowed(restrictedReceiver.ApplianceKey.ToString(), proposal.ItemData.ID))
                    continue;

                Accept(entity);
            }
        }
    }
}
