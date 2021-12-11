using HarmonyLib;
using Loader.Core;
using Nick;

namespace Loader.Patches
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
