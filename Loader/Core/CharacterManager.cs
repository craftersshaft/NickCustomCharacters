using BepInEx;
using CustomCharacterLoader.Data;
using Newtonsoft.Json;
using Nick;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CustomCharacterLoader.Core
{
    static class CharacterManager
    {
        private static readonly string baseFolder = Path.Combine(Paths.BepInExRootPath, "Custom Characters");
        private static readonly string baseFolderStage = Path.Combine(Paths.BepInExRootPath, "Custom Stages");

        public static Dictionary<string, CustomCharacter> characters = new Dictionary<string, CustomCharacter>();
        public static Dictionary<string, CustomStage> stages = new Dictionary<string, CustomStage>();
        static readonly List<string> allowedFileTypes = new List<string> { ".zip", ".nickchar", ".nickstage" };
        public static List<string> localizationStrings = new List<string>();

        internal static void Init()
        {
            foreach (var file in Directory.GetFiles(baseFolder).Where(x => allowedFileTypes.Contains(Path.GetExtension(x).ToLower())))
            {
                using var archive = ZipFile.OpenRead(file);
                var jsonFile = archive.Entries.Where(x => x.Name.ToLower() == "package.json").FirstOrDefault();

                if (jsonFile != null)
                {
                    var stream = new StreamReader(jsonFile.Open(), Encoding.Default);
                    var jsonString = stream.ReadToEnd();
                    var character = JsonConvert.DeserializeObject<CustomCharacter>(jsonString);
                    character.zipPath = file;
                    if (characters.ContainsKey(character.id))
                        Plugin.LogWarning($"Character with ID '{character.id}' already exists. Skipping!");
                    else
                        characters.Add(character.id, character);
                }
            }
            foreach (var file in Directory.GetFiles(baseFolderStage).Where(x => allowedFileTypes.Contains(Path.GetExtension(x).ToLower())))
            {
                using var archive = ZipFile.OpenRead(file);
                var jsonFile = archive.Entries.Where(x => x.Name.ToLower() == "package.json").FirstOrDefault();

                if (jsonFile != null)
                {
                    var stream = new StreamReader(jsonFile.Open(), Encoding.Default);
                    var jsonString = stream.ReadToEnd();
                    var stage = JsonConvert.DeserializeObject<CustomStage>(jsonString);
                    stage.zipPath = file;
                    if (stages.ContainsKey(stage.id))
                        Plugin.LogWarning($"Stage with ID '{stage.id}' already exists. Skipping!");
                    else
                        stages.Add(stage.id, stage);
                }
            }

            PrepareCharacterMetadatas();
            PrepareStageMetadatas();
        }

        static void PrepareCharacterMetadatas()
        {
            var gameMeta = Resources.FindObjectsOfTypeAll<GameMetaData>().FirstOrDefault();
            var charMeta = gameMeta.characterMetas.ToList();

            foreach(var character in characters.Values)
            {
                var meta = ScriptableObject.CreateInstance<CharacterMetaData>();

                meta.hide = false;
                meta.id = character.id;
                meta.locName = character.name;
                meta.showId = character.id;
                meta.isDLC = false;
                meta.skins = new CharacterMetaData.CharacterSkinMetaData[]
                {
                    new CharacterMetaData.CharacterSkinMetaData()
                    {
                        id = character.skinId,
                        locNames = new string[]
                        {
                            character.name
                        },
                        resPortraits = new string[]
                        {
                            string.Empty
                        },
                        resMediumPortraits = new string[]
                        {
                            string.Empty
                        },
                        resMiniPortraits = new string[]
                        {
                            string.Empty
                        },
                        unlockSkin = string.Empty,
                        unlockedByUnlockIds = new string[]
                        {
                            string.Empty
                        }
                    }
                };
                meta.charNameAnnouncerId = null;
                meta.unlockedByUnlockIds = null;

                character.meta = meta;

                charMeta.Add(meta);
                localizationStrings.Add(character.name);
            }
            gameMeta.characterMetas = charMeta.ToArray();
        }

        static void PrepareStageMetadatas()
        {
            var gameMeta = Resources.FindObjectsOfTypeAll<GameMetaData>().FirstOrDefault();
            var stageMeta = gameMeta.stageMetas.ToList();

            foreach (var stage in stages.Values)
            {
                var meta = ScriptableObject.CreateInstance<StageMetaData>();

                meta.hide = false;
                meta.id = stage.id;
                meta.locName = stage.name;
                meta.showId = stage.id;
                meta.musicIdDefault = stage.musicId;
                meta.skins = new StageMetaData.StageSkinMetaData[0];
                meta.resPortrait = stage.largePortraitPath;
                meta.resMediumPortrait = stage.mediumPortraitPath;
                meta.resMiniPortrait = stage.smallPortraitPath;
                meta.resPortraitGray = stage.smallPortraitPath;
                meta.blastzoneDistUp = stage.blastzoneDistUp;
                meta.blastzoneDistDown = stage.blastzoneDistDown;
                meta.blastzoneDistLeft = stage.blastzoneDistLeft;
                meta.blastzoneDistRight = stage.blastzoneDistRight;
                meta.cameraDistUp = stage.cameraDistUp;
                meta.cameraDistDown = stage.cameraDistDown;
                meta.cameraDistLeft = stage.cameraDistLeft;
                meta.cameraDistRight = stage.cameraDistRight;
                meta.cameraMinDist = stage.cameraMinDist;
                meta.stageNameAnnouncerId = null;
                meta.unlockedByUnlockIds = null;

                stage.meta = meta;

                stageMeta.Add(meta);
                localizationStrings.Add(stage.name);
            }
            gameMeta.stageMetas = stageMeta.ToArray();
        }
    }
}
