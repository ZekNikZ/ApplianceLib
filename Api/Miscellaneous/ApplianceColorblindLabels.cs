using ApplianceLib.Util;
using KitchenData;
using KitchenLib.References;
using UnityEngine;

namespace ApplianceLib.Api
{
    public static class ApplianceColorblindLabels
    {
        private static GameObject _template;
        private static GameObject Template
        {
            get
            {
                if (_template == null)
                {
                    _template = GameData.Main.Get<Appliance>(ApplianceReferences.SourceIceCream).Prefab.GetChildFromPath("Colour Blind");
                }

                return _template;
            }
        }

        /// <summary>
        /// Attaches an appliance colorblind label to a GameObject.
        /// </summary>
        /// <param name="holder">The parent of the label.</param>
        /// <param name="title">The label text.</param>
        public static void AddApplianceColorblindLabel(this GameObject holder, string title)
        {
            var colorblindLabel = Object.Instantiate(Template);
            colorblindLabel.name = "Colour Blind";
            colorblindLabel.transform.SetParent(holder.transform);
            colorblindLabel.transform.localPosition = Vector3.zero;
            colorblindLabel.GetChild("Title").GetComponent<TMPro.TextMeshPro>().text = title;
        }
    }
}
