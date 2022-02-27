using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using CustomCharacterLoader.Core;
using CustomCharacterLoader.Data;
using System.Collections.Generic;
using System.Reflection;

namespace CustomCharacterLoader
{
    [BepInPlugin("com.steven.nasb.characterloader", "Custom Character Loader", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        static Plugin Instance;
        static Dictionary<string, CustomCharacter> characters = new Dictionary<string, CustomCharacter>();
        static Dictionary<string, CustomStage> stages = new Dictionary<string, CustomStage>();

        void Awake()
        {
            Instance = this;

            var harmony = new Harmony(Info.Metadata.GUID);
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            CharacterManager.Init();

            Utils.DefaultAssetGrabber.GrabAssets();
            characters = CharacterManager.characters;
            stages = CharacterManager.stages;
        }

        #region logging
        internal static void LogDebug(string message) => Instance.Log(message, LogLevel.Debug);
        internal static void LogInfo(string message) => Instance.Log(message, LogLevel.Info);
        internal static void LogWarning(string message) => Instance.Log(message, LogLevel.Warning);
        internal static void LogError(string message) => Instance.Log(message, LogLevel.Error);
        private void Log(string message, LogLevel logLevel) => Logger.Log(logLevel, message);
        #endregion
    }
}
