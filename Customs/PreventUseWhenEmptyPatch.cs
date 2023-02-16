using ApplianceLib.Api;
using HarmonyLib;
using Kitchen;
using KitchenMods;
using Unity.Collections;
using Unity.Entities;

namespace ApplianceLib.Customs
{
    [HarmonyPatch(typeof(TriggerActivation), "Initialise")]
    internal class TriggerActivationInitialisePatch
    {
        internal static EntityManager EntityManager;

        static void Prefix(ref TriggerActivation __instance)
        {
            EntityManager = __instance.EntityManager;
        }
    }

    [HarmonyPatch(typeof(TriggerActivation), "IsPossible")]
    internal class TriggerActivationIsPossiblePatch
    {
        static bool Prefix(ref InteractionData data, ref bool __result)
        {
            if (TriggerActivationInitialisePatch.EntityManager.RequireComponent(data.Target, out CItemHolder holder))
            {
                if (!TriggerActivationInitialisePatch.EntityManager.HasComponent<CItem>(holder.HeldItem))
                {
                    __result = false;
                    return false;
                }
            }

            return true;
        }
    }
}
