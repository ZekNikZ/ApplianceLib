using ApplianceLib.Api;
using HarmonyLib;
using Kitchen;
using KitchenData;
using System.Reflection;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace ApplianceLib.Customs
{
    [HarmonyPatch]
    internal class ConveyorPushPatch
    {
        static MethodBase TargetMethod()
        {
            var type = AccessTools.FirstInner(typeof(PushItems), t => typeof(IJobChunk).IsAssignableFrom(t));
            return AccessTools.FirstMethod(type, method => method.Name.Contains("OriginalLambdaBody"));
        }

        static bool Prefix(Entity e, ref CConveyCooldown cooldown, ref CConveyPushItems push, ref CItemHolder held, in CPosition pos,
            ref EntityContext ___ctx, ref EntityCommandBuffer ___ecb, float ___speed, float ___dt, ref bool ___has_performed_action)
        {
            if (___has_performed_action || !push.Push || push.State == CConveyPushItems.ConveyState.Grab)
                return true;

            if (cooldown.Remaining > 0f || !___ctx.Require(held.HeldItem, out CItem itemComp))
            {
                push.State = CConveyPushItems.ConveyState.None;
                push.Progress = 0f;
                return true;
            }

            Orientation o = Orientation.Up;
            if (___ctx.Require(e, out CConveyPushRotatable comp) && comp.Target != 0)
            {
                o = comp.Target;
            }

            Vector3 vector = pos.Rotation.RotateOrientation(o).ToOffset() * (!push.Reversed ? 1 : -1);
            Entity occupant = PruneByRestrictedTransfers.GetOccupantAt(vector + pos);
            if (PruneByRestrictedTransfers.CanReachFrom(pos, vector + pos) && !PruneByRestrictedTransfers.AttachedEntityManager.HasComponent<CPreventItemTransfer>(occupant))
            {
                #region Restricted Transfer
                if (___ctx.Require(occupant, out CRestrictedReceiver receiver) && !RestrictedItemTransfers.IsAllowed(receiver.ApplianceKey.ConvertToString(), itemComp.ID))
                {
                    push.State = CConveyPushItems.ConveyState.None;
                    push.Progress = 0f;
                    return false;
                }
                #endregion

                #region Flexible Container
                if (___ctx.Require<CFlexibleContainer>(occupant, out var flexible) && GameData.Main.TryGet<Item>(itemComp, out var item, true))
                {
                    if (flexible.Items.Count < flexible.Maximum && !(!___ctx.Has<CIsInactive>(occupant) && ___ctx.Has<CLockedWhileDuration>(occupant)))
                    {
                        if (push.Progress < push.Delay)
                        {
                            push.Progress += ___speed * ___dt;
                            push.State = CConveyPushItems.ConveyState.Push;
                            if (___ctx.Require<CItemUndergoingProcess>(held.HeldItem, out var beingProcessed))
                                ___ecb.RemoveComponent<CItemUndergoingProcess>(held.HeldItem);
                        }
                        else
                        {
                            flexible.Items.Add(itemComp.ID, itemComp.Items);
                            ___ctx.Set(occupant, flexible);

                            ___ctx.Destroy(held.HeldItem);
                            held.HeldItem = default(Entity);

                            ___has_performed_action = true;
                            push.Progress = 0;
                            cooldown.Remaining = cooldown.Total;
                            push.State = CConveyPushItems.ConveyState.None;
                        }
                        return false;
                    }
                }
                #endregion
            }

            return true;
        }
    }
}
