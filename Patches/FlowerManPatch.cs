using HarmonyLib;
using LCSoundTool;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace BrandonLeeBracken.Patches
{
    // will apply patch to "FlowermanAI" script
    [HarmonyPatch(typeof(FlowermanAI))]
    internal static class FlowerManPatch
    {
        #region Harmony patches
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
            __instance.SetSpriteRenderer(spriteRenderer);
            spriteRenderer.sprite = BrandonLeeBrackenBase.Instance.relaxedSprite;

            // TESTING
            __instance.transform.localScale = new Vector3(.75f, .75f, .75f);

            BrandonLeeBrackenBase.LogMessage("Done!", BepInEx.Logging.LogLevel.Debug);
        }

        [HarmonyPatch(typeof(EnemyAI), "SwitchToBehaviourStateOnLocalClient"), HarmonyPostfix]
        internal static void SwitchToStatePostfix(ref EnemyAI __instance, int stateIndex)
        {
            // prevents other EnemyAI to execute this code
            FlowermanAI __newInstance = (FlowermanAI)__instance;
            if (__newInstance == null) return;

            // 0 is stalking
            // 1 is retreating
            // 2 is angered
            switch (stateIndex)
            {
                case 0:
                    __newInstance.GetSpriteRenderer().sprite = BrandonLeeBrackenBase.Instance.relaxedSprite;
                    break;

                case 1:
                    __newInstance.GetSpriteRenderer().sprite = BrandonLeeBrackenBase.Instance.fleeingSprite;
                    __newInstance.creatureSFX.PlayOneShot(BrandonLeeBrackenBase.Instance.GetRandomFleeingAudioClip(), 2f);
                    break;

                case 2:
                    __newInstance.GetSpriteRenderer().sprite = BrandonLeeBrackenBase.Instance.angrySprite;
                    break;
            }
        }
        #endregion



        #region Variables "patch"
        // in reality what this does is create static variables for each instance of this class
        // and allows them to be manipulated, simulating class fields like they would be present in the original

        // dictionary to store the custom variables for each instance
        private static readonly ConditionalWeakTable<FlowermanAI, VariableContainer> Variables = new ConditionalWeakTable<FlowermanAI, VariableContainer>();

        // class to hold custom variables
        private class VariableContainer
        {
            public SpriteRenderer spriteRenderer;
        }

        // getters and setters for custom variables
        public static SpriteRenderer GetSpriteRenderer(this FlowermanAI instance)
        {
            return Variables.GetOrCreateValue(instance).spriteRenderer;
        }

        public static void SetSpriteRenderer(this FlowermanAI instance, SpriteRenderer value)
        {
            Variables.GetOrCreateValue(instance).spriteRenderer = value;
        }
        #endregion
    }
}