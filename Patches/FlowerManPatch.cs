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

            // disable default mesh renderers
            foreach (var mesh in __instance.GetComponentsInChildren<Renderer>())
            {
                mesh.enabled = false;
            }

            // add Jackie Chan sprite
            var spriteRenderer = __instance.gameObject.AddComponent<SpriteRenderer>();
            var texture = BrandonLeeBrackenBase.Instance.relaxedTexture;
            spriteRenderer.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(.5f, 0f));

            // size testing
            __instance.transform.localScale = Vector3.one;

            // setup new audio
            SoundTool.ReplaceAudioClip("Scan", BrandonLeeBrackenBase.Instance.relaxedClips[0]);

            BrandonLeeBrackenBase.LogMessage("Done!", BepInEx.Logging.LogLevel.Debug);
        }
    }
}