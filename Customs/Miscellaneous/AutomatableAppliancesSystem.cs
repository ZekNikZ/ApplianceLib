using ApplianceLib.Api;
using Kitchen;
using KitchenMods;
using System;
using Unity.Collections;
using Unity.Entities;

namespace ApplianceLib.Customs
{
    public struct CAutomatedApplianceInteractor : IModComponent
    {
    }

    public struct CAutomatedApplianceInteractorProvider : IModComponent
    {
    }

    public struct CAutomatedApplianceInteractorReciever : IModComponent
    {
    }

    internal class AutoDrinkDispensingSystem : StartOfDaySystem, IModSystem
    {
        private EntityQuery Appliances;

        protected override void Initialise()
        {
            Appliances = GetEntityQuery(new QueryHelper()
                .All(typeof(CAppliance), typeof(CPosition))
                .None(typeof(CAutomatedApplianceInteractorProvider))
            );
        }

        protected override void OnUpdate()
        {
            using var appliances = Appliances.ToEntityArray(Allocator.Temp);
            foreach (var appliance in appliances)
            {
                // Ensure this is an appliance
                if (!Require(appliance, out CAppliance cAppliance) || !Require(appliance, out CPosition position))
                {
                    continue;
                }

                // Ensure this is a portioner
                if (!Enum.IsDefined(typeof(AutomatableAppliances.Automator), cAppliance.ID))
                {
                    continue;
                }
                AutomatableAppliances.Automator automatorKey = (AutomatableAppliances.Automator)cAppliance.ID;

                // Check if it is facing a tea dispenser
                Entity facingAppliance = GetOccupant(position.ForwardPosition);
                if (!CanReach(position, position.ForwardPosition)
                    || facingAppliance == default
                    || HasComponent<CAutomatedApplianceInteractorReciever>(facingAppliance)
                    || !Require(facingAppliance, out CAppliance cFacingAppliance)
                    || !AutomatableAppliances.MarkedAppliances[automatorKey].Contains(cFacingAppliance.ID))
                {
                    continue;
                }

                // Create automated interactor
                var interactor = EntityManager.CreateEntity(typeof(CAutomatedInteractor), typeof(CAutomatedApplianceInteractor), typeof(CPosition));
                EntityManager.SetComponentData(interactor, new CAutomatedInteractor
                {
                    Type = InteractionType.Act,
                    DoNotReceive = true,
                    IsHeld = true
                });
                EntityManager.SetComponentData(interactor, position);

                // Mark the portioner so that we don't make multiple interactors
                EntityManager.AddComponent<CAutomatedApplianceInteractorProvider>(appliance);

                // Mark the reciever so that we don't allow speeding up the process with multiple automators
                // EntityManager.AddComponent<CAutomatedDrinkInteractorReciever>(facingAppliance);
            }
        }
    }

    internal class AutoDrinkCleanupSystem : StartOfNightSystem, IModSystem
    {
        private EntityQuery Interactors;
        private EntityQuery InteractorProviders;

        protected override void Initialise()
        {
            Interactors = GetEntityQuery(new QueryHelper().All(typeof(CAutomatedApplianceInteractor)));
            InteractorProviders = GetEntityQuery(new QueryHelper().Any(typeof(CAutomatedApplianceInteractorProvider), typeof(CAutomatedApplianceInteractorReciever)));
        }

        protected override void OnUpdate()
        {
            // Destroy interactors
            using var interactors = Interactors.ToEntityArray(Allocator.Temp);
            EntityManager.DestroyEntity(interactors);

            // Remove markers
            using var appliances = InteractorProviders.ToEntityArray(Allocator.Temp);
            EntityManager.RemoveComponent<CAutomatedApplianceInteractorProvider>(appliances);
            EntityManager.RemoveComponent<CAutomatedApplianceInteractorReciever>(appliances);
        }
    }
}
