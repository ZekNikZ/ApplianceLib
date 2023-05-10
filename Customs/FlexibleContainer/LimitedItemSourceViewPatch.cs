using HarmonyLib;
using Kitchen;

namespace ApplianceLib.Customs.FlexibleContainer
{
    [HarmonyPatch(typeof(LimitedItemSourceView), "UpdateData")]
    internal class LimitedItemSourceViewPatch
    {
        static bool Prefix(ref LimitedItemSourceView __instance)
        {
            return __instance.enabled;
        }
    }
}
