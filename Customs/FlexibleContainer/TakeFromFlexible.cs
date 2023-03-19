using ApplianceLib.Api;
using ApplianceLib.Util;
using Kitchen;
using KitchenMods;
using Unity.Entities;

namespace ApplianceLib.Customs
{
    [UpdateInGroup(typeof(ItemTransferPropose))]
    public class TakeFromFlexible : TransferInteractionProposalSystem, IModSystem
    {
        protected override void Initialise()
        {
            base.Initialise();
            this.RegisterTransfer();
        }

        protected override bool IsPossible(ref InteractionData data)
        {
            var target = data.Target;
            if (Has<CPreventUse>(target) || Has<CPreventItemTransfer>(target) || !Require<CFlexibleContainer>(target, out var storage))
                return false;

            if (!Has<CIsInactive>() && Has<CLockedWhileDuration>())
                return false;

            if (storage.Maximum < 1 || storage.Items.Count < 1)
                return false;

            var currentIndex = storage.Items.Count - 1;
            var item = storage.Items.GetItem(currentIndex);
            var components = storage.Items[currentIndex];

            var proposal = InteractionTransferProposal(data.Interactor);
            proposal.AllowGrab = true;
            Context.Set(TransferProposalSystem.CreateProposal(Context, this, Context.CreateItemGroup(item, components), data.Attempt.Target, data.Interactor, TransferFlags.Interaction), proposal);
            return false;
        }

        public override void SendTransfer(Entity transfer, Entity acceptance, EntityContext ctx)
        {
            if (Require<CItemTransferProposal>(transfer, out var proposal) && Require<CFlexibleContainer>(proposal.Source, out var storage))
            {
                storage.Items.Remove(storage.Items.Count - 1);
                SetComponent(proposal.Source, storage);
            }
        }

        public override void Tidy(EntityContext ctx, CItemTransferProposal proposal)
        {
            if (proposal.Status != ItemTransferStatus.Resolved)
            {
                ctx.Destroy(proposal.Item);
            }
        }

        public override void ReceiveResult(Entity result, Entity transfer, Entity acceptance, EntityContext ctx) { }
    }
}
