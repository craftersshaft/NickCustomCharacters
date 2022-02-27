using HarmonyLib;
using CustomCharacterLoader.Core;
using Nick;

namespace CustomCharacterLoader.Patches
{
    [HarmonyPatch(typeof(AgentLoading), "RequestLoad")]
    class AgentLoading_RequestLoad
    {
        static void Prefix(ref AgentLoading.LoadRequest req)
        {
            if (CharacterManager.characters.TryGetValue(req.Id, out var character))
                character.LoadAssetBundle();
            if (CharacterManager.stages.TryGetValue(req.Id, out var stage))
                stage.LoadAssetBundle();
        }
    }
}
