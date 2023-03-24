using KitchenData;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ApplianceLib.Api
{
    public class FlexibleColorableContainerView : FlexibleContainerView
    {
        [SerializeField]
        public List<MeshRenderer> Renderers = new();

        [SerializeField]
        public Color PresentProcessedMaterial = Color.green;

        [SerializeField]
        public Color PresentUnprocessedMaterial = Color.red;

        [SerializeField]
        public Color AbsentMaterial = new(0.05f, 0.05f, 0.05f, 1f);

        [SerializeField]
        public int ProcessID = 0;

        protected override void UpdateData(ViewData data)
        {
            base.UpdateData(data);
            if (Renderers != null)
            {
                for (int i = 0; i < Renderers.Count; i++)
                {
                    if (data.Items.Count <= i)
                    {
                        Renderers[i].material.color = AbsentMaterial;
                    }
                    else
                    {
                        if (ProcessID != 0 && GameData.Main.TryGet<Item>(data.Items[i], out var item))
                        {
                            var hasProcess = item.DerivedProcesses.Any(p => p.Process.ID == ProcessID);
                            Renderers[i].material.color = hasProcess ? PresentUnprocessedMaterial : PresentProcessedMaterial;
                        }
                        else
                        {
                            Renderers[i].material.color = PresentUnprocessedMaterial;
                        }
                    }
                }
            }
        }
    }
}
