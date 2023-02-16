using ApplianceLib.Api;
using HarmonyLib;
using Kitchen;
using System.Reflection;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace ApplianceLib.Customs.RestrictedTransfers
{
    [HarmonyPatch]
    internal class ConveyorPushPatch
    {
        static MethodBase TargetMethod()
        {
            var type = AccessTools.FirstInner(typeof(PushItems), t => typeof(IJobChunk).IsAssignableFrom(t));
            return AccessTools.FirstMethod(type, method => method.Name.Contains("OriginalLambdaBody"));
        }

        static bool Prefix(Entity e, ref CConveyCooldown cooldown, ref CConveyPushItems push, ref CItemHolder held, in CPosition pos)
        {
            if (PruneByRestrictedTransfers.AttachedEntityManager.RequireComponent(held.HeldItem, out CItem item))
            {
                Orientation o = Orientation.Up;
                if (PruneByRestrictedTransfers.AttachedEntityManager.RequireComponent(e, out CConveyPushRotatable comp) && comp.Target != 0)
                {
                    o = comp.Target;
                }
                Vector3 vector = pos.Rotation.RotateOrientation(o).ToOffset() * ((!push.Reversed) ? 1 : (-1));
                Entity occupant = PruneByRestrictedTransfers.GetOccupantAt(vector + pos);
                if (PruneByRestrictedTransfers.CanReachFrom(pos, vector + pos) && !PruneByRestrictedTransfers.AttachedEntityManager.HasComponent<CPreventItemTransfer>(occupant))
                {
                    if (PruneByRestrictedTransfers.AttachedEntityManager.RequireComponent(occupant, out CRestrictedReceiver receiver) && !RestrictedItemTransfers.IsAllowed(receiver.ApplianceKey.ConvertToString(), item.ID))
                    {
                        push.State = CConveyPushItems.ConveyState.None;
                        push.Progress = 0f;
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
