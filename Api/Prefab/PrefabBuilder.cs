using ApplianceLib.Api.References;
using ApplianceLib.Util;
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

        public static void AttachBlenderCup(this GameObject parent)
        {
            var child = parent.AttachPrefabAsChild(Prefabs.Find("BlenderCup"));
            child.ApplyMaterialToChild("Cup", MaterialHelpers.GetMaterialArray("Door Glass", "Door Glass", "Door Glass"));
        }

        public static void AttachCounter(this GameObject parent, CounterType type)
        {
            var child = parent.AttachPrefabAsChild(Prefabs.Find($"Counter{type}"));
            switch (type)
            {
                case CounterType.DoubleDoors:
                    child.ApplyMaterialToChild("Block/Counter2/Counter", MaterialHelpers.GetMaterialArray("Wood 4 - Painted"));
                    child.ApplyMaterialToChild("Block/Counter2/Counter Doors", MaterialHelpers.GetMaterialArray("Wood 4 - Painted"));
                    child.ApplyMaterialToChild("Block/Counter2/Counter Surface", MaterialHelpers.GetMaterialArray("Wood - Default"));
                    child.ApplyMaterialToChild("Block/Counter2/Counter Top", MaterialHelpers.GetMaterialArray("Wood - Default"));
                    child.ApplyMaterialToChild("Block/Counter2/Handles", MaterialHelpers.GetMaterialArray("Knob"));
                    break;
                case CounterType.Drawers:
                    child.ApplyMaterialToChild("Base_L_Counter.blend", MaterialHelpers.GetMaterialArray("Wood - Default", "Wood 4 - Painted", "Wood 4 - Painted"));
                    child.ApplyMaterialToChild("Base_L_Counter.blend/Handle_L_Counter.blend", MaterialHelpers.GetMaterialArray("Knob"));
                    child.ApplyMaterialToChild("Top", MaterialHelpers.GetMaterialArray("Wood - Default"));
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

            child.ApplyMaterialToChild("Model/Cup", MaterialHelpers.GetMaterialArray(MaterialReferences.CupBase));

            if (liquidMaterial != null)
            {
                child.ApplyMaterialToChild("Model/Liquid", new Material[] { liquidMaterial });
            }
            child.GetChildFromPath("Model/Liquid").SetActive(liquidMaterial != null);

            if (withStraw)
            {
                child.ApplyMaterialToChild("Model/Straw", MaterialHelpers.GetMaterialArray(MaterialReferences.CupStraw));
            }
            child.GetChildFromPath("Model/Straw").SetActive(withStraw);
        }
    }
}
