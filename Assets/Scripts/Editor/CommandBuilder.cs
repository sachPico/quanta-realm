using UnityEditor;
using System.Collections.Generic;

public class CommandBuilder {
    static void build() {

        // Place all your scenes here
        // string[] scenes = {"Assets/Scenes/SampleScene.unity"};

        // string pathToDeploy = "Builds/PC";

        //BuildPipeline.BuildPlayer(scenes, pathToDeploy, BuildTarget.StandaloneWindows64, BuildOptions.None);      
        BuildPipeline.BuildPlayer(GetDefaultPlayerOptions());
    }

    public static UnityEditor.BuildPlayerOptions GetDefaultPlayerOptions()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();

        List<string> listScenes = new List<string>();
        foreach (var s in EditorBuildSettings.scenes)
        {
            if (s.enabled)
                listScenes.Add(s.path);
        }

        buildPlayerOptions.target = EditorUserBuildSettings.activeBuildTarget;
        buildPlayerOptions.scenes = listScenes.ToArray();
        buildPlayerOptions.options = BuildOptions.None;
        buildPlayerOptions.locationPathName = "Builds/Program";

        // To define
        // buildPlayerOptions.locationPathName = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "\\LightGunBuild\\Android\\LightGunMouseArcadeRoom.apk";
        // buildPlayerOptions.target = BuildTarget.Android;

        return buildPlayerOptions;
    }
}