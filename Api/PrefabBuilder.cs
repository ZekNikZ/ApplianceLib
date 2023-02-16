using ApplianceLib.Util;
using UnityEngine;

namespace ApplianceLib.Api
{
    public static class PrefabBuilder
    {
        public static void AttachMixingBowl(this GameObject parent)
        {
            parent.AttachPrefabAsChild(Prefabs.Find("MixingBowl"));
        }

        public static void AttachBlenderCup(this GameObject parent)
        {
            parent.AttachPrefabAsChild(Prefabs.Find("BlenderCup"));
        }

        public static void AttachPrefabAsChild(this GameObject parent, GameObject prefab)
        {
            Object.Instantiate(prefab, parent.transform);
        }
    }
}
