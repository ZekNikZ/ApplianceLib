using KitchenData;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ApplianceLib.Api
{
    public class FlexibleContainerLightsView : FlexibleContainerView
    {
        public List<MeshRenderer> Lights = new();
        public int ProcessID = 0;

        protected override void UpdateData(ViewData data)
        {
            base.UpdateData(data);
            if (Lights != null)
            {
                for (int i = 0; i < Lights.Count; i++)
                {
                    if (data.Items.Count <= i)
                    {
                        Lights[i].material.color = InactiveColor;
                    }
                    else
                    {
                        if (ProcessID != 0 && GameData.Main.TryGet<Item>(data.Items[i], out var item))
                        {
                            var hasProcess = item.DerivedProcesses.Any(p => p.Process.ID == ProcessID);
                            Lights[i].material.color = hasProcess ? ActiveUnprocessedColor : ActiveMiscColor;
                        }
                        else
                        {
                            Lights[i].material.color = ActiveUnprocessedColor;
                        }
                    }
                }
            }
        }

        private static Color ActiveMiscColor = Color.green;
        private static Color ActiveUnprocessedColor = Color.red;
        private static Color InactiveColor = new Color(0.05f, 0.05f, 0.05f, 1f);
    }
}
