using KitchenLib.Customs;
using KitchenLib.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace ApplianceLib.Util
{
    /**
     * Adapted from https://github.com/DepletedNova/IngredientLib/blob/master/Util/MaterialHelper.cs
     */
    internal static class MaterialHelpers
    {
        public static GameObject ApplyMaterial<T>(this GameObject gameObject, Material[] materials) where T : Renderer
        {
            var comp = gameObject.GetComponent<T>();
            if (comp == null)
                return gameObject;

            comp.materials = materials;

            return gameObject;
        }
        public static GameObject ApplyMaterial(this GameObject gameObject, Material[] materials)
        {
            return ApplyMaterial<MeshRenderer>(gameObject, materials);
        }
        public static GameObject ApplyMaterial(this GameObject gameObject, params string[] materials)
        {
            return ApplyMaterial<MeshRenderer>(gameObject, GetMaterialArray(materials));
        }

        public static GameObject ApplyMaterialToChildren<T>(this GameObject gameObject, string nameMatch, Material[] materials) where T : Renderer
        {
            for (int i = 0; i < gameObject.GetChildCount(); i++)
            {
                GameObject child = gameObject.GetChild(i);
                if (!child.name.ToLower().Contains(nameMatch.ToLower()))
                    continue;
                child.ApplyMaterial<T>(materials);
            }

            return gameObject;
        }
        public static GameObject ApplyMaterialToChildren(this GameObject gameObject, string nameMatch, Material[] materials)
        {
            return ApplyMaterialToChildren<MeshRenderer>(gameObject, nameMatch, materials);
        }
        public static GameObject ApplyMaterialToChildren(this GameObject gameObject, string nameMatch, params string[] materials)
        {
            return ApplyMaterialToChildren<MeshRenderer>(gameObject, nameMatch, GetMaterialArray(materials));
        }

        public static GameObject ApplyMaterialToChild<T>(this GameObject gameObject, string childName, Material[] materials) where T : Renderer
        {
            gameObject.GetChildFromPath(childName).ApplyMaterial<T>(materials);

            return gameObject;
        }
        public static GameObject ApplyMaterialToChild(this GameObject gameObject, string childName, Material[] materials)
        {
            gameObject.GetChildFromPath(childName).ApplyMaterial(materials);

            return gameObject;
        }
        public static GameObject ApplyMaterialToChild(this GameObject gameObject, string childName, params string[] materials)
        {
            gameObject.GetChildFromPath(childName).ApplyMaterial(GetMaterialArray(materials));

            return gameObject;
        }

        public static Material[] GetMaterialArray(params string[] materials)
        {
            List<Material> materialList = new();
            foreach (string matName in materials)
            {
                string formatted = $"ApplianceLib - \"{matName}\"";
                bool flag = CustomMaterials.CustomMaterialsIndex.ContainsKey(formatted);
                if (flag)
                {
                    materialList.Add(CustomMaterials.CustomMaterialsIndex[formatted]);
                }
                else
                {
                    materialList.Add(MaterialUtils.GetExistingMaterial(matName));
                }
            }
            return materialList.ToArray();
        }

        public static Material CreateFlat(string name, Color color, float shininess = 0, float overlayScale = 10)
        {
            Material mat = new(Shader.Find("Simple Flat"))
            {
                name = $"DrinksMod - \"{name}\""
            };
            mat.SetColor("_Color0", color);
            mat.SetFloat("_Shininess", shininess);
            mat.SetFloat("_OverlayScale", overlayScale);
            return mat;
        }
        public static Material CreateFlat(string name, int color, float shininess = 0, float overlayScale = 10)
        {
            return CreateFlat(name, GameObjectHelpers.ColorFromHex(color), shininess, overlayScale);
        }

        public static Material CreateTransparent(string name, Color color)
        {
            Material mat = new(Shader.Find("Simple Transparent"))
            {
                name = $"DrinksMod - \"{name}\""
            };
            mat.SetColor("_Color", color);
            return mat;
        }
        public static Material CreateTransparent(string name, int color, float opacity)
        {
            Color col = GameObjectHelpers.ColorFromHex(color);
            col.a = opacity;
            return CreateTransparent(name, col);
        }

        public static GameObject SetupMaterialsLikeCounter(this GameObject gameObject)
        {
            gameObject.ApplyMaterialToChild("Counter/Block/Counter2/Counter", "Wood 4 - Painted");
            gameObject.ApplyMaterialToChild("Counter/Block/Counter2/Counter Doors", "Wood 4 - Painted");
            gameObject.ApplyMaterialToChild("Counter/Block/Counter2/Counter Surface", "Wood - Default");
            gameObject.ApplyMaterialToChild("Counter/Block/Counter2/Counter Top", "Wood - Default");
            gameObject.ApplyMaterialToChild("Counter/Block/Counter2/Handles", "Knob");

            return gameObject;
        }
    }
}
