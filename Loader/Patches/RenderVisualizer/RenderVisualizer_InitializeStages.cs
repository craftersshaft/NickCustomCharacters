using HarmonyLib;
using CustomCharacterLoader.Core;
using UnityEngine;
using UnityEngine.UI;

namespace CustomCharacterLoader.Patches
{
    [HarmonyPatch(typeof(RenderVisualizer), "InitializeStages")]
    class RenderVisualizer_InitializeStages
    {
        static void Prefix(RenderVisualizer __instance, Transform ___stagesParent)
        {
            var go = ___stagesParent.GetChild(0).gameObject;

            foreach (var stage in CharacterManager.stages.Values)
            {
                if (___stagesParent.Find(stage.id)) continue;

                var instance = Object.Instantiate(go, go.transform.position, go.transform.rotation, ___stagesParent);
                instance.name = stage.id;
                instance.GetComponent<RenderImage>().StageMetaData = stage.meta;
                Image imagetoreplace = instance.transform.GetChild(0).gameObject.GetComponent<Image>();
                imagetoreplace.sprite = stage.SmallPortrait;
            }
        }
    }
}
