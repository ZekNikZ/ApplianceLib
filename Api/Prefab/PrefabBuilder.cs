using ApplianceLib.Api.References;
using ApplianceLib.Util;
using KitchenLib.Utils;
using System;
using System.Reflection;
using UnityEngine;

namespace ApplianceLib.Api.Prefab
{
    public enum CounterType
    {
        Drawers,
        DoubleDoors
    }

    public static class PrefabBuilder
    {
        /// <summary>
        /// Create an empty prefab. Useful if your prefab will only consist of parts from other methods found here.
        /// </summary>
        /// <param name="uniqueId">The unique ID to assign to this prefab. Reuse this ID to reference the same prefab as generated earlier.</param>
        /// <returns>The new empty prefab or cached existing prefab.</returns>
        [Obsolete("This method causes some errors, use other methods.", true)]
        public static GameObject CreateEmptyPrefab(string uniqueId)
        {
            return Prefabs.Create($"{uniqueId} (PrefabBuilder) ({Assembly.GetCallingAssembly().FullName})");
        }

        public static GameObject AttachPrefabAsChild(this GameObject parent, GameObject prefab)
        {
            // Attach prefab
            var instance = UnityEngine.Object.Instantiate(prefab);
            instance.transform.parent = parent.transform;
            instance.transform.localPosition = Vector3.zero;
            
            // TODO: Remove all children tagged with ALToRemove

            return instance;
        }

        public static void AttachMixingBowl(this GameObject parent)
        {
            var child = parent.AttachPrefabAsChild(Prefabs.Find("MixingBowl"));
            child.ApplyMaterialToChild("Model", MaterialReferences.MixingBowl);
        }

        [Obsolete("This method causes some errors, use other methods.", true)]
        public static void AttachBlenderCup(this GameObject parent)
        {
            var child = parent.AttachPrefabAsChild(Prefabs.Find("BlenderCup"));
            child.ApplyMaterialToChild("Cup", MaterialUtils.GetMaterialArray("Door Glass", "Door Glass", "Door Glass"));
        }

        public static void AttachCounter(this GameObject parent, CounterType type)
        {
            var child = parent.AttachPrefabAsChild(Prefabs.Find($"Counter{type}"));
            switch (type)
            {
                case CounterType.DoubleDoors:
                    child.ApplyMaterialToChild("Block/Counter2/Counter", MaterialUtils.GetMaterialArray("Wood 4 - Painted"));
                    child.ApplyMaterialToChild("Block/Counter2/Counter Doors", MaterialUtils.GetMaterialArray("Wood 4 - Painted"));
                    child.ApplyMaterialToChild("Block/Counter2/Counter Surface", MaterialUtils.GetMaterialArray("Wood - Default"));
                    child.ApplyMaterialToChild("Block/Counter2/Counter Top", MaterialUtils.GetMaterialArray("Wood - Default"));
                    child.ApplyMaterialToChild("Block/Counter2/Handles", MaterialUtils.GetMaterialArray("Knob"));
                    break;
                case CounterType.Drawers:
                    child.ApplyMaterialToChild("Base_L_Counter.blend", MaterialUtils.GetMaterialArray("Wood - Default", "Wood 4 - Painted", "Wood 4 - Painted"));
                    child.ApplyMaterialToChild("Base_L_Counter.blend/Handle_L_Counter.blend", MaterialUtils.GetMaterialArray("Knob"));
                    child.ApplyMaterialToChild("Top", MaterialUtils.GetMaterialArray("Wood - Default"));
                    break;
            }
        }

        public static void AttachCrate(this GameObject parent)
        {
            parent.AttachPrefabAsChild(Prefabs.Find($"Crate"));
        }

        public static void AttachCup(this GameObject parent, Material liquidMaterial = null, bool withStraw = false)
        {
            var child = parent.AttachPrefabAsChild(Prefabs.Find($"Cup"));

            child.ApplyMaterialToChild("Model/Cup", MaterialUtils.GetMaterialArray(MaterialReferences.CupBase));

            if (liquidMaterial != null)
            {
                child.ApplyMaterialToChild("Model/Liquid", new Material[] { liquidMaterial });
            }
            child.GetChild("Model/Liquid").SetActive(liquidMaterial != null);

            if (withStraw)
            {
                child.ApplyMaterialToChild("Model/Straw", MaterialUtils.GetMaterialArray(MaterialReferences.CupStraw));
            }
            child.GetChild("Model/Straw").SetActive(withStraw);
        }
    }
}
