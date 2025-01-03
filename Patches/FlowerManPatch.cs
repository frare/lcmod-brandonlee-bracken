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

            // TESTING
            __instance.transform.localScale = Vector3.one;
            if (BrandonLeeBrackenBase.Instance.killClips[0] != null)
                AudioSource.PlayClipAtPoint(BrandonLeeBrackenBase.Instance.killClips[0], __instance.transform.position);

            // setup new audio
            // chances are (1f / number of  clips) for easy dynamic size of clips lists
            /*
            foreach (var audioClip in BrandonLeeBrackenBase.Instance.fleeingClips)
                SoundTool.ReplaceAudioClip("Found1", audioClip, 1f / BrandonLeeBrackenBase.Instance.fleeingClips.Count);
            foreach (var audioClip in BrandonLeeBrackenBase.Instance.killClips)
                SoundTool.ReplaceAudioClip("CrackNeck", audioClip, 1f / BrandonLeeBrackenBase.Instance.killClips.Count);
            SoundTool.ReplaceAudioClip("Angered", BrandonLeeBrackenBase.Instance.angryClip, 1f);
            */

            BrandonLeeBrackenBase.LogMessage("Done!", BepInEx.Logging.LogLevel.Debug);
        }

        /*
        [HarmonyPatch("Update"), HarmonyPostfix]
        internal static void UpdatePostFix(ref FlowermanAI __instance)
        {
            BrandonLeeBrackenBase.LogMessage(
                $"Current behaviour state: {__instance.currentBehaviourStateIndex}", 
                BepInEx.Logging.LogLevel.Debug
            );
        }
        */
    }
}