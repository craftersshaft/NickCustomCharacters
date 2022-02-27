using HarmonyLib;
using CustomCharacterLoader.Core;
using CustomCharacterLoader.Utils;
using Nick;
using SMU.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomCharacterLoader.Patches
{
    [HarmonyPatch(typeof(Localization), "GetText", new Type[] { typeof(string) })]
    class Localization_GetText
    {
        public static bool Prefix(Nick.Localization __instance, ref string __result, string id)
        {
            if (CharacterManager.localizationStrings.Contains(id))
            {
                __result = id;
                return false; // make sure you only skip if really necessary
            }
            return true;
        }
    }
}
