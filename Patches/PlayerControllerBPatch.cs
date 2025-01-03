using GameNetcodeStuff;
using HarmonyLib;
using LCSoundTool;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BrandonLeeBracken.Patches
{
    [HarmonyPatch(typeof(PlayerControllerB))]
    internal class PlayerControllerBPatch
    {
        private static InputAction keyPressAction;

        [HarmonyPatch("Awake"), HarmonyPostfix]
        internal static void AwakePostFix(ref PlayerControllerB __instance)
        {
            if (__instance.IsOwner)
            {
                BrandonLeeBrackenBase.LogMessage("PlayerControllerB Awake");

                /*
                // Create a new InputAction for detecting the key press
                keyPressAction = new InputAction("KeyPressAction", binding: "<Keyboard>/f5");
                keyPressAction.performed += OnKeyPress;  // Subscribe to the event when the key is pressed
                keyPressAction.Enable();  // Enable the action
                */
            }
        }

        [HarmonyPatch("Update"), HarmonyPostfix]
        internal static void UpdatePostfix(ref float ___sprintMeter)
        {
            if (SoundTool.Instance != null || BrandonLeeBrackenBase.Instance.testClip == null) SoundTool.ReplaceAudioClip("Scan", BrandonLeeBrackenBase.Instance.testClip, 1f);
            else BrandonLeeBrackenBase.LogMessage("Something is null!!!");
        }

        private static void OnKeyPress(InputAction.CallbackContext context)
        {
            BrandonLeeBrackenBase.LogMessage("F5 pressed");
        }
    }
}