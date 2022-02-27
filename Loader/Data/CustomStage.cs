using Nick;
using SMU.Utilities;
using System.Collections;
using System.IO;
using System.IO.Compression;
using UnityEngine;

namespace CustomCharacterLoader.Data
{
    class CustomStage
    {
        public string id;
        public string name;
        public string author;

        public string filePath;

        public string stagePath;
        public string skinPath;
        public string musicId;

        public float blastzoneDistUp;
        public float blastzoneDistDown;
        public float blastzoneDistLeft;
        public float blastzoneDistRight;

        public float cameraDistUp;
        public float cameraDistDown;
        public float cameraDistLeft;
        public float cameraDistRight;
        public float cameraMinDist;



        public string smallPortraitPath;
        private Sprite _smallPortrait;
        public Sprite SmallPortrait => _smallPortrait ??= LoadSpriteFromZip(smallPortraitPath);

        public string mediumPortraitPath;
        private Sprite _mediumPortrait;
        public Sprite MediumPortrait => _mediumPortrait ??= LoadSpriteFromZip(mediumPortraitPath);

        public string largePortraitPath;
        private Sprite _largePortrait;
        public Sprite LargePortrait => _largePortrait ??= LoadSpriteFromZip(largePortraitPath);

        public string thumbPortraitPath;
        private Sprite _thumbPortrait;
        public Sprite ThumbPortrait => _thumbPortrait ??= LoadSpriteFromZip(thumbPortraitPath);

        public GameAgent agent;
        public bool loading = false;
        public string zipPath;
        public StageMetaData meta;
        public AssetBundle bundle;

        private StagesUIMetaData.StageUIElements _UIElements;
        public StagesUIMetaData.StageUIElements UIElements
        {
            get
            {
                _UIElements ??= new StagesUIMetaData.StageUIElements()
                {
                    ID = id,
                    StageLarge = LargePortrait,
                    StageMedium = MediumPortrait,
                    StageSmall = SmallPortrait,
                    StageSelectThumbnail = ThumbPortrait
                };
                return _UIElements;
            }
        }

        Sprite LoadSpriteFromZip(string path)
        {
            using var zip = ZipFile.OpenRead(zipPath);
            var stream = new BufferedStream(zip.GetEntry(path).Open());
            byte[] bytes;
            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                bytes = ms.ToArray();
            }

            var sprite = ImageHelper.LoadSpriteRaw(bytes);
            sprite.texture.wrapMode = TextureWrapMode.Clamp;

            return (sprite);
        }

        public void LoadAssetBundle()
        {
            if (bundle == null && !loading)
                SharedCoroutineStarter.StartCoroutine(LoadAssetBundleCoroutine());
        }

        IEnumerator LoadAssetBundleCoroutine()
        {
            Plugin.LogInfo($"Loading Assetbundle for Stage {name}...");
            Plugin.LogInfo("Zip Path: " + zipPath);
            Plugin.LogInfo("File Path: " + filePath);
            loading = true;
            using (var zip = ZipFile.OpenRead(zipPath))
            {
                var stream = new MemoryStream();
                zip.GetEntry(filePath).Open().CopyTo(stream);
                var request = AssetBundle.LoadFromStreamAsync(stream);

                while (!request.isDone) yield return null;

                bundle = request.assetBundle;
                Plugin.LogInfo($"about to see if idScenesDict will accept {id} paired with {stagePath}");
                var idScenesDict = GameObject.FindObjectOfType<AgentLoading>().idScenes.IdDict;
                idScenesDict.Add(id, stagePath);
            }
            Plugin.LogInfo($"Stage {name} loaded!");
            loading = false;
        }
    }
}
