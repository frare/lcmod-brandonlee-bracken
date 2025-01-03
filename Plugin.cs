using BepInEx;
using BepInEx.Logging;
using BrandonLeeBracken.Patches;
using HarmonyLib;
using LCSoundTool;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace BrandonLeeBracken
{
    [BepInPlugin(modGUID, modName, modVersion)]
    [BepInDependency("LCSoundTool", BepInDependency.DependencyFlags.HardDependency)]
    public class BrandonLeeBrackenBase : BaseUnityPlugin
    {
        // mod info constants
        internal const string modGUID = "frare.brandonleebracken";
        internal const string modName = "Brandon Lee Bracken";
        internal const string modVersion = "1.0.0";

        // singleton
        internal static BrandonLeeBrackenBase Instance;

        // harmony instance
        private readonly Harmony harmony = new Harmony(modGUID);

        // for debugging
        internal ManualLogSource logger;

        // custom assets
        public static AssetBundle CustomTextures, CustomAudio;
        internal Sprite relaxedSprite, fleeingSprite, angrySprite;
        internal List<AudioClip> fleeingClips = new List<AudioClip>();
        internal List<AudioClip> killClips = new List<AudioClip>();
        internal AudioClip angryClip;

        private void Awake()
        {
            if (Instance == null) Instance = this;

            logger = BepInEx.Logging.Logger.CreateLogSource(modGUID);
            logger.LogInfo($"Mod started! :)");

            LoadCustomTextures();
            LoadCustomAudioClips();
            // AssetBundle.UnloadAllAssetBundles(false);
            SoundTool.ReplaceAudioClip("Scan", killClips[0], 1f);

            harmony.PatchAll(typeof(BrandonLeeBrackenBase));
            harmony.PatchAll(typeof(FlowerManPatch));
            // harmony.PatchAll(typeof(PlayerControllerBPatch));
        }

        private void LoadCustomTextures()
        {
            var sAssemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            CustomTextures = AssetBundle.LoadFromFile(Path.Combine(sAssemblyLocation, "modtextures"));
            if (CustomTextures == null)
            {
                logger.LogError("Failed to load custom textures");
                return;
            }

            foreach (string assetName in CustomTextures.GetAllAssetNames())
            {
                Texture2D texture = CustomTextures.LoadAsset<Texture2D>(assetName);
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(.5f, 0f));

                if (assetName.Contains("relaxed")) relaxedSprite = sprite;
                else if (assetName.Contains("fleeing")) fleeingSprite = sprite;
                else if (assetName.Contains("angry")) angrySprite = sprite;
                else logger.LogWarning("Found an asset in texture bundle that does not match any category");
            }
            
            if (relaxedSprite == null || fleeingSprite == null || angrySprite == null)
                logger.LogError("Failed to load custom textures");
            else
                logger.LogDebug("Custom textures loaded!");
        }

        private void LoadCustomAudioClips()
        {
            var sAssemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            CustomAudio = AssetBundle.LoadFromFile(Path.Combine(sAssemblyLocation, "modaudio"));
            if (CustomAudio == null)
            {
                logger.LogError("Failed to load custom audio");
                return;
            }

            foreach (string assetName in CustomAudio.GetAllAssetNames())
            {
                if (assetName.Contains("fleeing")) fleeingClips.Add(CustomAudio.LoadAsset<AudioClip>(assetName));
                else if (assetName.Contains("kill")) killClips.Add(CustomAudio.LoadAsset<AudioClip>(assetName));
                else if (assetName.Contains("angry")) angryClip = CustomAudio.LoadAsset<AudioClip>(assetName);
                else logger.LogWarning("Found an asset in audio bundle that does not match any category");
            }

            if (fleeingClips.Count == 0 || killClips.Count == 0)
                logger.LogError("Failed to load custom audio");
            else
                logger.LogDebug("Custom audio loaded!");

            SoundTool.ReplaceAudioClip("Scan", killClips[0]);
        }

        public static void LogMessage(string message, LogLevel logLevel = LogLevel.Debug)
        {
            Instance.logger.Log(logLevel, message);
        }
    }
}