using Nick;
using System;
using UnityEngine;

[Serializable]
class StageEditorSaveData : ScriptableObject
{
    public string stageName;
    public string author;

    public GameAgent stagePrefab;
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

    public Texture smallPortrait;
    public Texture mediumPortrait;
    public Texture largePortrait;
    public Texture thumbPortrait;
}
