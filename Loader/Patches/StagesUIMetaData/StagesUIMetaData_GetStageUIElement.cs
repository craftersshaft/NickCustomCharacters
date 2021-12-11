using HarmonyLib;
using Loader.Core;

namespace CustomCharacterLoader.Patches
{
    [HarmonyPatch(typeof(StagesUIMetaData), "GetStageUIElement")]
    class StagesUIMetaData_GetStageUIElement
    {
        static void Postfix(string id, ref StagesUIMetaData.StageUIElements __result)
        {
            if (__result == null && CharacterManager.stages.TryGetValue(id, out var stage))
            {
                __result = stage.UIElements;
            }
        }
    }
}
