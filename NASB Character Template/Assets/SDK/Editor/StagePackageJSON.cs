using System;

[Serializable]
public class StagePackageJSON
{
    public string id;
    public string name;
    public string author;
    public string filePath;
    public string stagePath;
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
    public string mediumPortraitPath;
    public string largePortraitPath;
    public string thumbPortraitPath;
}
