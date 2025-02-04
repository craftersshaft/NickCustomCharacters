﻿using HarmonyLib;
using CustomCharacterLoader.Core;
using Nick;
using System.Collections.Generic;

namespace CustomCharacterLoader.Patches
{
    [HarmonyPatch(typeof(AgentLoading), "Awake")]
    class AgentLoading_Awake
    {
        static void Postfix(ref Dictionary<string, AgentLoading.LoadState> ___loadStates)
        {
            foreach (var character in CharacterManager.characters.Values)
            {
                ___loadStates.Add(character.id, new AgentLoading.LoadState());
                ___loadStates.Add(character.skinId, new AgentLoading.LoadState());
            }
            foreach (var stage in CharacterManager.stages.Values)
            {
                ___loadStates.Add(stage.id, new AgentLoading.LoadState());
            }
        }
    }
}
