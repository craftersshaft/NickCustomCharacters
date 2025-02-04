﻿using UnityEngine;

namespace CustomCharacterLoader.Utils
{
    class GameAssetLoader : MonoBehaviour
    {
        [Header("State Layers")]
        public bool blastzoneKOBase = true;
        public bool grabbableBase = true;
        public bool launchableBase = true;
        public bool characterBase = true;
        public bool stageBase = true;

        [Header("Data Layers")]
        public bool Data_blastzoneKOBase = true;
        public bool Data_grabbableBase = true;
        public bool Data_launchableBase = true;
        public bool Data_characterBase = true;
        public bool Data_stageBase = true;

        [Header("Atk Prop Layers")]
        public bool Atk_characterBase = true;

        [Header("Spawn FX Layers")]
        public bool FX_common = true;
        public bool FX_characterBase = true;

        [Header("SFX Layers")]
        public bool SFX_common = true;
        public bool SFX_characterBase = true;
    }
}
