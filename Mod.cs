using ApplianceLib.Api;
using ApplianceLib.Customs;
using KitchenData;
using KitchenLib;
using KitchenLib.Event;
using KitchenLib.References;
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
        public const string MOD_VERSION = "0.1.0a";
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
            //AddProcessIcons();

            // Perform actions when game data is built
            Events.BuildGameDataEvent += delegate (object s, BuildGameDataEventArgs args)
            {
                Item tomato = (Item)GDOUtils.GetExistingGDO(ItemReferences.Tomato);
                if (tomato != null)
                {
                    tomato.DerivedProcesses.Add(new Item.ItemProcess
                    {
                        Process = Refs.BlendProcess,
                        Duration = 1,
                        Result = (Item)GDOUtils.GetExistingGDO(ItemReferences.TomatoSauce)
                    });
                }

                Item pizza = (Item)GDOUtils.GetExistingGDO(ItemReferences.PizzaCooked);
                if (pizza != null)
                {
                    RestrictedItemSplits.BlacklistItem(pizza);
                    RestrictedItemSplits.AllowItem("test", pizza);
                }
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
