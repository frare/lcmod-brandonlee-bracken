using HarmonyLib;
using LCSoundTool;
using UnityEngine;

namespace BrandonLeeBracken.Patches
{
    // will apply patch to "FlowermanAI" script
    [HarmonyPatch(typeof(FlowermanAI))]
    internal class FlowerManPatch
    {
        // adds this snippet at the end of "Start" method
        [HarmonyPatch("Start"), HarmonyPostfix]
        internal static void StartPostfix(ref FlowermanAI __instance)
        {
            BrandonLeeBrackenBase.LogMessage(
                $"Patching \"FlowermanAI Start\"... for gameObject {__instance.gameObject.name}", 
                BepInEx.Logging.LogLevel.Debug
            );

            // disable components that won't be used
            __instance.GetComponentInChildren<Animator>().enabled = false;
            __instance.GetComponentsInChildren<Renderer>().Do(renderer => renderer.enabled = false);

            // add Jackie Chan sprite renderer
            var spriteRenderer = __instance.gameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = BrandonLeeBrackenBase.Instance.relaxedSprite;

            // size testing
            __instance.transform.localScale = Vector3.one;

            // setup new audio
            SoundTool.ReplaceAudioClip("Scan", BrandonLeeBrackenBase.Instance.relaxedClips[0]);

            BrandonLeeBrackenBase.LogMessage("Done!", BepInEx.Logging.LogLevel.Debug);
        }
    }
}