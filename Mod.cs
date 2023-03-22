using ApplianceLib.Api;
using ApplianceLib.Api.References;
using ApplianceLib.Customs;
using Kitchen;
using KitchenData;
using KitchenLib;
using KitchenLib.Event;
using KitchenLib.References;
using KitchenLib.Registry;
using KitchenLib.Utils;
using KitchenMods;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using static ApplianceLib.Api.References.ApplianceLibGDOs;

namespace ApplianceLib
{
    public class Mod : BaseMod, IModSystem
    {
        public const string MOD_GUID = "io.zkz.plateup.appliancelib";
        public const string MOD_NAME = "ApplianceLib";
        public const string MOD_VERSION = "0.1.1";
        public const string MOD_AUTHOR = "ZekNikZ";
        public const string MOD_GAMEVERSION = ">=1.1.3";

#if DEBUG
        public const bool DEBUG_MODE = true;
#else
        public const bool DEBUG_MODE = false;
#endif

        public static AssetBundle Bundle;

        public Mod() : base(MOD_GUID, MOD_NAME, MOD_AUTHOR, MOD_VERSION, MOD_GAMEVERSION, Assembly.GetExecutingAssembly()) { }

        protected override void OnInitialise()
        {
            LogWarning($"{MOD_GUID} v{MOD_VERSION} in use!");
        }

        private void AddGameData()
        {
            LogInfo("Attempting to register game data...");

            ModGDOs.RegisterModGDOs(this, Assembly.GetExecutingAssembly());

            LogInfo("Done loading game data.");
        }

        private void AddProcessIcons()
        {
            Bundle.LoadAllAssets<Texture2D>();
            Bundle.LoadAllAssets<Sprite>();
            var spriteAsset = Bundle.LoadAsset<TMP_SpriteAsset>("blend");
            Mod.LogInfo(spriteAsset);
            TMP_Settings.defaultSpriteAsset.fallbackSpriteAssets.Add(spriteAsset);
            spriteAsset.material = Object.Instantiate(TMP_Settings.defaultSpriteAsset.material);
            spriteAsset.material.mainTexture = Bundle.LoadAsset<Texture2D>("blendTex");
        }

        protected override void OnUpdate()
        {
        }

        protected override void OnPostActivate(KitchenMods.Mod mod)
        {
            LogInfo("Attempting to load asset bundle...");
            Bundle = mod.GetPacks<AssetBundleModPack>().SelectMany(e => e.AssetBundles).FirstOrDefault();
            if (Bundle == null)
            {
                LogError("Could not load asset bundle.");
            }
            else
            {
                LogInfo("Done loading asset bundle.");
            }

            // Register custom GDOs
            AddGameData();

            // Load process icons
            AddProcessIcons();

            // Perform actions when game data is built
            Events.BuildGameDataEvent += delegate (object s, BuildGameDataEventArgs args)
            {
                if (!args.firstBuild)
                    return;

                // Custom Restricted Items
                RestrictedTransferKeys.Setup(args.gamedata);

                // Update Tomato Recipe
                Item tomato = (Item)GDOUtils.GetExistingGDO(ItemReferences.Tomato);
                tomato.DerivedProcesses.Add(new Item.ItemProcess
                {
                    Process = Refs.BlendProcess,
                    Duration = 2,
                    Result = (Item)GDOUtils.GetExistingGDO(ItemReferences.TomatoSauce)
                });

                Item turkey = (Item)GDOUtils.GetExistingGDO(ItemReferences.TurkeyIngredient);
                turkey.DerivedProcesses.Add(new Item.ItemProcess
                {
                    Process = (Process)GDOUtils.GetExistingGDO(ProcessReferences.Clean),
                    Duration = 2,
                    Result = (Item)GDOUtils.GetExistingGDO(ItemReferences.TomatoSauce)
                });

                LogInfo("Updating wash basin.");

                // Update Dishwasher & Wash Basin
                #region Update Wash Basin
                var washBasin = Refs.Find<Appliance>(ApplianceReferences.SinkLarge);
                washBasin.Processes = new()
                {
                    new()
                    {
                        Process = Refs.Find<Process>(ProcessReferences.Clean)
                    }
                };
                washBasin.Properties = new()
                {
                    new CDisplayDuration
                    {
                        Process = ProcessReferences.Clean
                    },
                    new CTakesDuration
                    {
                        Manual = true,
                        RelevantTool = DurationToolType.Clean,
                        Mode = InteractionMode.Items,
                        Total = 5
                    },
                    new CFlexibleContainer
                    {
                        Maximum = 4
                    },
                    new CAppliesProcessToFlexible
                    {
                        MinimumItems = 1,
                        ProcessTimeMultiplier = 2.5f,
                        MinimumProcessTime = 2,
                        ProcessType = FlexibleProcessType.Average
                    },
                    new CRestrictedReceiver
                    {
                        ApplianceKey = RestrictedTransferKeys.Cleanable
                    },
                    new CChangeRestrictedReceiverKeyAfterDuration
                    {
                        ApplianceKey = RestrictedTransferKeys.CleanedItems
                    },
                    new CChangeRestrictedReceiverKeyWhenEmpty
                    {
                        ApplianceKey = RestrictedTransferKeys.Cleanable
                    },
                    new CCausesSpills
                    {
                        ID = ApplianceReferences.MopWater,
                        Rate = 1,
                        OverwriteOtherMesses = true
                    }
                };

                Object.Destroy(washBasin.Prefab.GetComponent<LimitedItemSourceView>());
                var view = washBasin.Prefab.AddComponent<FlexibleContainerView>();
                for (int i = 0; i < washBasin.Prefab.GetChildCount(); i++)
                    if (washBasin.Prefab.GetChild(i).name.ToLower().Contains("holdpoint"))
                        view.Transforms.Add(washBasin.Prefab.transform.GetChild(i));
                #endregion

                #region Update Dishwasher
                if (ModRegistery.Registered.Any(modPair => modPair.Value.ModID == "IcedMilo.PlateUp.AutomationPlus"))
                    return;

                LogInfo("Updating dishwasher.");

                var dishwasher = Refs.Find<Appliance>(ApplianceReferences.DishWasher);
                dishwasher.Processes = new()
                {
                    new()
                    {
                        Process = Refs.Find<Process>(ProcessReferences.Clean)
                    }
                };
                dishwasher.Properties = new()
                {
                    new CTakesDuration
                    {
                        Mode = InteractionMode.Items,
                        Total = 10
                    },
                    new CFlexibleContainer
                    {
                        Maximum = 4
                    },
                    new CAppliesProcessToFlexible(),
                    new CRestrictedReceiver
                    {
                        ApplianceKey = RestrictedTransferKeys.Cleanable
                    },
                    new CChangeRestrictedReceiverKeyAfterDuration
                    {
                        ApplianceKey = RestrictedTransferKeys.CleanedItems
                    },
                    new CChangeRestrictedReceiverKeyWhenEmpty
                    {
                        ApplianceKey = RestrictedTransferKeys.Cleanable
                    },
                    new CRequiresActivation(),
                    new CIsInactive(),
                    new CLockedWhileDuration(),
                    new CSetEnabledAfterDuration(),
                    new CDeactivateAtNight()
                };
                var prefab = dishwasher.Prefab;
                Object.Destroy(prefab.GetComponent<LimitedItemSourceView>());
                Object.Destroy(prefab.GetComponent<LimitedItemSourceLightsView>());
                var flexible = prefab.AddComponent<FlexibleContainerView>();
                var dishwasherChild = prefab.GetChild("DishWasher");
                var dishes = dishwasherChild.GetChild("Door/Dishes");
                for (int i = 0; i < dishes.GetChildCount(); i++)
                {
                    Object.Destroy(dishes.GetChild(i).GetChild(0));
                    flexible.Transforms.Add(dishes.transform.GetChild(i));
                }
                // TODO: re-add this when I rework FlexibleLightsView
                //var flexible = prefab.AddComponent<FlexibleContainerLightsView>();
                //var dishwasherChild = prefab.GetChild("DishWasher");
                //var dishes = dishwasherChild.GetChild("Door/Dishes");
                //for (int i = 0; i < dishes.GetChildCount(); i++)
                //{
                //    Object.Destroy(dishes.GetChild(i).GetChild(0));
                //    flexible.Transforms.Add(dishes.transform.GetChild(i));
                //}
                //for (int i = 0; i < dishwasherChild.GetChildCount(); i++)
                //{
                //    var child = dishwasherChild.GetChild(i);
                //    if (!child.name.Contains("Socket") && child.name.Contains("Light"))
                //    {
                //        flexible.Lights.Add(child.GetComponent<MeshRenderer>());
                //    }
                //}
                //flexible.ProcessID = ProcessReferences.Clean;
                #endregion
            };
            Events.BuildGameDataEvent += ApplianceGroups.BuildGameDataEventCallback;
        }

        #region Logging
        public static void LogInfo(string _log) { Debug.Log($"[{MOD_NAME}] " + _log); }
        public static void LogWarning(string _log) { Debug.LogWarning($"[{MOD_NAME}] " + _log); }
        public static void LogError(string _log) { Debug.LogError($"[{MOD_NAME}] " + _log); }
        public static void LogInfo(object _log) { LogInfo(_log.ToString()); }
        public static void LogWarning(object _log) { LogWarning(_log.ToString()); }
        public static void LogError(object _log) { LogError(_log.ToString()); }
        #endregion
    }
}
