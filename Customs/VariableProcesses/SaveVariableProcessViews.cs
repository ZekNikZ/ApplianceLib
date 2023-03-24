using ApplianceLib.Api;
using Kitchen;
using KitchenMods;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;

namespace ApplianceLib.Customs.VariableProcesses
{
    public class SaveVariableProcessViews : GameSystemBase, IModSystem
    {
        private EntityQuery VariableProcessProviders;

        private static readonly List<CVariableProcessContainer> Containers = new();
        private static readonly List<CPosition> Positions = new();

        protected override void Initialise()
        {
            base.Initialise();
            VariableProcessProviders = GetEntityQuery(typeof(CVariableProcessContainer), typeof(CPosition));
        }

        protected override void OnUpdate()
        {
            if (!Has<SPracticeMode>())
            {
                return;
            }

            using var cProviders = VariableProcessProviders.ToComponentDataArray<CVariableProcessContainer>(Allocator.Temp);
            using var cPositions = VariableProcessProviders.ToComponentDataArray<CPosition>(Allocator.Temp);

            Containers.Clear();
            Positions.Clear();

            Containers.AddRange(cProviders);
            Positions.AddRange(cPositions);
        }

        public override void AfterLoading(SaveSystemType systemType)
        {
            if (Containers == null || Positions == null)
            {
                return;
            }

            using var entities = VariableProcessProviders.ToEntityArray(Allocator.Temp);
            using var cProviders = VariableProcessProviders.ToComponentDataArray<CVariableProcessContainer>(Allocator.Temp);
            using var cPositions = VariableProcessProviders.ToComponentDataArray<CPosition>(Allocator.Temp);

            for (int i = 0; i < entities.Length; i++)
            {
                for (int j = 0; j < Containers.Count; j++)
                {
                    bool flag3 = (cPositions[i].Position - Positions[j].Position).Chebyshev() < 0.1f;
                    if (flag3)
                    {
                        var cProvider = cProviders[i];
                        cProvider.Current = Containers[j].Current;
                        SetComponent(entities[i], cProvider);
                        break;
                    }
                }
            }

            Containers.Clear();
            Positions.Clear();
        }
    }
}
