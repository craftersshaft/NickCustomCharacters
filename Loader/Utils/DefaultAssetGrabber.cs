﻿using Nick;
using SMU.Reflection;
using SMU.Utilities;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CustomCharacterLoader.Utils
{
    class DefaultAssetGrabber
    {
        public static TextAsset blastzoneKOBase;
        public static TextAsset grabbableBase;
        public static TextAsset launchableBase;
        public static TextAsset characterBase;
        public static TextAsset stageBase;

        public static AgentDataLayer Data_blastzoneKOBase;
        public static AgentDataLayer Data_grabbableBase;
        public static AgentDataLayer Data_launchableBase;
        public static AgentDataLayer Data_characterBase;
        public static AgentDataLayer Data_stageBase;

        public static AtkPropBank Atk_characterBase;

        public static GameFXBank FX_common;
        public static GameFXBank FX_characterBase;

        public static GameSFXBank SFX_common;
        public static GameSFXBank SFX_characterBase;

        internal static void GrabAssets()
        {
            SharedCoroutineStarter.StartCoroutine(GrabAssetsCoroutine());
            SharedCoroutineStarter.StartCoroutine(GrabStageAssetsCoroutine());
        }

        static IEnumerator GrabAssetsCoroutine()
        {
            Plugin.LogInfo("Grabbing default assets...");

            var request = SceneManager.LoadSceneAsync("char_cb", LoadSceneMode.Additive);

            while (!request.isDone) yield return null;

            var agent = Object.FindObjectOfType<LoadedAgent>().agentPrefab;

            agent.TryGetStateMachine(out var stateMachine);
            var stateLayers = stateMachine.GetField<GameAgentStateMachine, TextAsset[]>("stateLayers");

            blastzoneKOBase = stateLayers[0];
            grabbableBase = stateLayers[1];
            launchableBase = stateLayers[2];
            characterBase = stateLayers[3];

            agent.TryGetData(out var data);
            var dataLayers = data.GetField<GameAgentData, AgentDataLayer[]>("dataLayers");

            Data_blastzoneKOBase = dataLayers[0];
            Data_grabbableBase = dataLayers[1];
            Data_launchableBase = dataLayers[2];
            Data_characterBase = dataLayers[3];

            agent.TryGetAttack(out var attack);
            var atkLayers = attack.GetField<GameAgentAttack, AtkPropBank[]>("atkPropLayers");

            Atk_characterBase = atkLayers[0];

            agent.TryGetFX(out var fx);
            var fxLayers = fx.fx.GetField<FXHandler, GameFXBank[]>("spawnFXLayers");

            FX_common = fxLayers[0];
            FX_characterBase = fxLayers[1];

            agent.TryGetSFX(out var sfx);
            var sfxLayers = sfx.GetField<GameAgentSFX, GameSFXBank[]>("sfxLayers");

            SFX_common = sfxLayers[0];
            SFX_characterBase = sfxLayers[1];

            SceneManager.UnloadSceneAsync("char_cb");


            Plugin.LogInfo("Finished grabbing default assets!");
        }

        static IEnumerator GrabStageAssetsCoroutine()
        {
            Plugin.LogInfo("Grabbing default stage assets...");

            var request = SceneManager.LoadSceneAsync("stage_apple_fields", LoadSceneMode.Additive);

            while (!request.isDone) yield return null;

            var agent = Object.FindObjectOfType<LoadedAgent>().agentPrefab;

            agent.TryGetStateMachine(out var stateMachine);
            var stateLayers = stateMachine.GetField<GameAgentStateMachine, TextAsset[]>("stateLayers");

            stageBase = stateLayers[0];

            agent.TryGetData(out var data);
            var dataLayers = data.GetField<GameAgentData, AgentDataLayer[]>("dataLayers");

            Data_stageBase = dataLayers[0];
            //this might be missing stuff, let us know if it breaks
            SceneManager.UnloadSceneAsync("stage_apple_fields");


            Plugin.LogInfo("Finished grabbing default stage assets!");
        }

    }
}
