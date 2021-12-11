using Nick;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;

public class StageExportWindow : EditorWindow
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

    // Serialized stuff
    SerializedObject so;
    SerializedProperty propName;
    SerializedProperty propAuthor;
    SerializedProperty propStagePrefab;
	SerializedProperty propMusicId;
	SerializedProperty propBlastzoneDistUp;
    SerializedProperty propBlastzoneDistDown;
    SerializedProperty propBlastzoneDistLeft;
    SerializedProperty propBlastzoneDistRight;
    SerializedProperty propCameraDistUp;
    SerializedProperty propCameraDistDown;
    SerializedProperty propCameraDistLeft;
    SerializedProperty propCameraDistRight;
    SerializedProperty propCameraMinDist;	
    SerializedProperty propSmallPortrait;
    SerializedProperty propMediumPortrait;
    SerializedProperty propLargePortrait;
    SerializedProperty propThumbPortrait;


    Vector2 scrollPosition = Vector2.zero;
    string saveDataPath = string.Empty;

    [MenuItem("NASB/Stage Exporter")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        StageExportWindow window = (StageExportWindow)EditorWindow.GetWindow(typeof(StageExportWindow));
        window.titleContent.text = "Stage Exporter";
        window.Show();
    }

    private void OnEnable()
    {
        saveDataPath = Path.Combine("Assets", "SDK", "Editor", "exporter_save.asset");
        Load();

        so = new SerializedObject(this);
        propName = so.FindProperty("stageName");
        propAuthor = so.FindProperty("author");
        propStagePrefab = so.FindProperty("stagePrefab");
		propMusicId = so.FindProperty("musicId");
		propBlastzoneDistUp = so.FindProperty("blastzoneDistUp");
		propBlastzoneDistDown = so.FindProperty("blastzoneDistDown");
		propBlastzoneDistLeft = so.FindProperty("blastzoneDistLeft");
		propBlastzoneDistRight = so.FindProperty("blastzoneDistRight");
		propCameraDistUp = so.FindProperty("cameraDistUp");
		propCameraDistDown = so.FindProperty("cameraDistDown");
		propCameraDistLeft = so.FindProperty("cameraDistLeft");
		propCameraDistRight = so.FindProperty("cameraDistRight");		
		propCameraMinDist = so.FindProperty("cameraMinDist");		
		
        propSmallPortrait = so.FindProperty("smallPortrait");
        propMediumPortrait = so.FindProperty("mediumPortrait");
        propLargePortrait = so.FindProperty("largePortrait");
        propThumbPortrait = so.FindProperty("thumbPortrait");
    }

    private void OnDisable()
    {
        Save();
    }

    void Save()
    {
        var saveData = Resources.Load("exporter_save.asset") as StageEditorSaveData;

        if (saveData == null)
        {
            saveData = ScriptableObject.CreateInstance<StageEditorSaveData>();
            AssetDatabase.CreateAsset(saveData, saveDataPath);
        }

        saveData.stageName = stageName;
        saveData.author = author;
        saveData.stagePrefab = stagePrefab;
		saveData.musicId = musicId;
		saveData.blastzoneDistUp = blastzoneDistUp;
		saveData.blastzoneDistDown = blastzoneDistDown;
		saveData.blastzoneDistLeft = blastzoneDistLeft;
		saveData.blastzoneDistRight = blastzoneDistRight;
		saveData.cameraDistUp = cameraDistUp;
		saveData.cameraDistDown = cameraDistDown;
		saveData.cameraDistLeft = cameraDistLeft;
		saveData.cameraDistRight = cameraDistRight;		
		
        saveData.smallPortrait = smallPortrait;
        saveData.mediumPortrait = mediumPortrait;
        saveData.largePortrait = largePortrait;
        saveData.thumbPortrait = thumbPortrait;

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    void Load()
    {
        var saveData = Resources.FindObjectsOfTypeAll<StageEditorSaveData>().FirstOrDefault();

        if (saveData == null)
        {
            return;
        }

        stageName = saveData.stageName;
        author = saveData.author;
        stagePrefab = saveData.stagePrefab;
		musicId = saveData.musicId;
		blastzoneDistUp = saveData.blastzoneDistUp;
		blastzoneDistDown = saveData.blastzoneDistDown;
		blastzoneDistLeft = saveData.blastzoneDistLeft;
		blastzoneDistRight = saveData.blastzoneDistRight;		
		cameraDistUp = saveData.cameraDistUp;
		cameraDistDown = saveData.cameraDistDown;
		cameraDistLeft = saveData.cameraDistLeft;
		cameraDistRight = saveData.cameraDistRight;	
		cameraMinDist = saveData.cameraMinDist;	

		
        smallPortrait = saveData.smallPortrait;
        mediumPortrait = saveData.mediumPortrait;
        largePortrait = saveData.largePortrait;
        thumbPortrait = saveData.thumbPortrait;
        
        OnValidate();
        Repaint();
    }

    private void OnGUI()
    {
        so.Update();

        GUILayout.Space(5);

        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        GUILayout.BeginVertical("Metadata", "window");
        EditorGUILayout.PropertyField(propName);
        EditorGUILayout.PropertyField(propAuthor);
		EditorGUILayout.PropertyField(propMusicId);
        GUILayout.EndVertical();

        GUILayout.Space(5);

        GUILayout.BeginVertical("Prefabs", "window");
        EditorGUILayout.PropertyField(propStagePrefab);
        GUILayout.EndVertical();

        GUILayout.Space(5);
		
        GUILayout.BeginVertical("Blastzones and Camera", "window");
        EditorGUILayout.PropertyField(propBlastzoneDistUp);
		EditorGUILayout.PropertyField(propBlastzoneDistDown);
		EditorGUILayout.PropertyField(propBlastzoneDistLeft);
		EditorGUILayout.PropertyField(propBlastzoneDistRight);
        EditorGUILayout.PropertyField(propCameraDistUp);
		EditorGUILayout.PropertyField(propCameraDistDown);
		EditorGUILayout.PropertyField(propCameraDistLeft);
		EditorGUILayout.PropertyField(propCameraDistRight);	
		EditorGUILayout.PropertyField(propCameraMinDist);	
        GUILayout.EndVertical();

        GUILayout.Space(5);

        GUILayout.BeginVertical("Portraits", "window");
        EditorGUILayout.PropertyField(propSmallPortrait);
        EditorGUILayout.PropertyField(propMediumPortrait);
        EditorGUILayout.PropertyField(propLargePortrait);
		EditorGUILayout.PropertyField(propThumbPortrait);
        GUILayout.EndVertical();

        GUILayout.Space(5);

        GUILayout.EndScrollView();

        //GUILayout.BeginHorizontal();

        //GUI.backgroundColor = Color.red;
        //if (GUILayout.Button("Clear"))
        //{
        //    Clear();
        //}

        //GUI.backgroundColor = Color.yellow;
        //if (GUILayout.Button("Reload"))
        //{
        //    Load();
        //}

        //GUILayout.EndHorizontal();

        GUILayout.Space(3);

        using (new EditorGUI.DisabledScope(!valid))
        {
            GUI.backgroundColor = Color.green;
            if(GUILayout.Button("Export Stage", GUILayout.Height(30)))
            {
                ExportStage();
            }
        }


        GUILayout.Space(10);

        so.ApplyModifiedProperties();
    }

    void Clear()
    {
        stageName = default;
        author = default;

        stagePrefab = default;
		musicId = default;
		
		blastzoneDistUp = default;
		blastzoneDistDown = default;
		blastzoneDistLeft = default;
		blastzoneDistRight = default;	
		cameraDistUp = default;
		cameraDistDown = default;
		cameraDistLeft = default;
		cameraDistRight = default;
		cameraMinDist = default;

        smallPortrait = default;
        mediumPortrait = default;
        largePortrait = default;
        thumbPortrait = default;

        OnValidate();
        Repaint();
    }

    bool valid;
    private void OnValidate()
    {
        valid = true;
        if (stageName == string.Empty) valid = false;
        if (author == string.Empty) valid = false;
		if (musicId == string.Empty) valid = false;

        if (!stagePrefab || PrefabUtility.IsPartOfPrefabInstance(stagePrefab))
        {
            valid = false;
            stagePrefab = null;
            Repaint();
        }

        if (!smallPortrait) valid = false;
        if (!mediumPortrait) valid = false;
        if (!largePortrait) valid = false;
        if (!thumbPortrait) valid = false;
		
        if (exporting)
        {
            valid = false;
            Repaint();
        }

        Save();
    }


    bool exporting;
    void ExportStage()
    {
        if (exporting) return;
        exporting = true;

        var currentSceneSetup = EditorSceneManager.GetSceneManagerSetup();
        if(!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            exporting = false;
            return;
        }

        try
        {
            var exportPath = Path.Combine("Assets", "Export");
            Directory.CreateDirectory(Path.Combine(Application.dataPath, "Export", "images"));

            #region Create Stage Scene
            var stageScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            stageScene.name = stagePrefab.gameUniqueIdentifier;

            var loadedAgent = new GameObject("LoadedAgent").AddComponent<LoadedAgent>();
            loadedAgent.agentId = stagePrefab.gameUniqueIdentifier;
            loadedAgent.agentPrefab = stagePrefab;

            var stageScenePath = Path.Combine(exportPath, stagePrefab.gameUniqueIdentifier + ".unity");
            EditorSceneManager.SaveScene(stageScene, stageScenePath);
            #endregion

            #region Create AssetBundles

            var build = new AssetBundleBuild
            {
                assetNames = new string[] { stageScenePath },
                assetBundleName = $"{stagePrefab.gameUniqueIdentifier}.assetbundle"
            };

            BuildPipeline.BuildAssetBundles(exportPath, new AssetBundleBuild[] { build }, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);

            #endregion

            #region Copy Images

            var smallPortraitPath = Path.Combine("images", "small");
            AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(smallPortrait), Path.Combine(exportPath, smallPortraitPath));

            var mediumPortraitPath = Path.Combine("images", "medium");
            AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(mediumPortrait), Path.Combine(exportPath, mediumPortraitPath));

            var largePortraitPath = Path.Combine("images", "large");
            AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(largePortrait), Path.Combine(exportPath, largePortraitPath));

            var thumbPortraitPath = Path.Combine("images", "thumbnail");
            AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(thumbPortrait), Path.Combine(exportPath, thumbPortraitPath));
            #endregion

            #region Create package.json
            var package = new StagePackageJSON
            {
                id = stagePrefab.gameUniqueIdentifier,
                name = stageName,
                author = author,
				musicId = musicId,
                filePath = build.assetBundleName,
                stagePath = stageScenePath,
                smallPortraitPath = smallPortraitPath,
                mediumPortraitPath = mediumPortraitPath,
                largePortraitPath = largePortraitPath,
                thumbPortraitPath = thumbPortraitPath
            };

            var jsonPath = Path.Combine(Application.dataPath, "Export", "package.json");
            var packageJSON = JsonUtility.ToJson(package, true).Replace(@"\\", @"/");

            File.WriteAllText(jsonPath, packageJSON);
            #endregion

            #region Create Zip

            AssetDatabase.OpenAsset(stagePrefab);

            AssetDatabase.DeleteAsset(stageScenePath);
            AssetDatabase.DeleteAsset(Path.Combine(exportPath, "Export"));
            AssetDatabase.DeleteAsset(Path.Combine(exportPath, "Export.manifest"));
            AssetDatabase.DeleteAsset(Path.Combine(exportPath, $"{build.assetBundleName}.manifest"));

            var initialPath = EditorPrefs.GetString("CHARACTER_EXPORT_PATH", Application.dataPath);

            var savePath = EditorUtility.SaveFilePanel(
            "Save Custom Stage",
            initialPath,
            ".nickstage",
            "nickstage");

            if (savePath.Length == 0) return;

            EditorPrefs.SetString("CHARACTER_EXPORT_PATH", Path.GetDirectoryName(savePath));

            if (File.Exists(savePath)) File.Delete(savePath);
            ZipFile.CreateFromDirectory(Path.Combine(Application.dataPath, "Export"), savePath);

            #endregion

            #region Restore Scene
            //if (currentSceneSetup != null && currentSceneSetup.Length > 0 &&
            //    currentSceneSetup.Any(x => x.isLoaded) && currentSceneSetup.Any(x => x.isActive))
            //    EditorSceneManager.RestoreSceneManagerSetup(currentSceneSetup);
            #endregion

        }
        catch (Exception e)
        {
            Debug.LogError($"{e.Message}\n{e.StackTrace}");
        }
        finally
        {
            exporting = false;
        }
    }
}
