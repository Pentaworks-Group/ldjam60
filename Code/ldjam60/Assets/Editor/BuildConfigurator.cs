using System;
using System.Collections.Generic;
using System.IO;

using Assets.Scripts.Constants;

using GameFrame.Core;

using Newtonsoft.Json;

using UnityEditor;

using UnityEngine;

public static class BuildConfigurator
{
    class BuildInfo
    {
        public string GameVersion = Application.version;
        public string[] scenes;
    }

    private const String locationPath = "../../Deployments/web";
    private const String prefix = "Assets/Scenes/";
    private const String postfix = ".unity";

    public static void BuildProjectDevelopment()
    {
        //Debug.LogError($"SceneNames.scenes: {Scenes.GetGameScenes()}");
        //Debug.Log($"SceneNames.scenesDevelopment: {SceneNames.scenesDevelopment}");

        string[] levels = GetScenePaths(Scenes.GetGameScenes(), Scenes.GetDevelopmentScenes());
        var report = BuildPipeline.BuildPlayer(levels, locationPath, BuildTarget.WebGL, BuildOptions.Development);

        //var report = BuildPipeline.BuildPlayer(GetSampleScene(), locationPath, BuildTarget.WebGL, BuildOptions.Development);
        //Debug.Log($"Build result: {report.summary.result}, {report.summary.totalErrors} errors");

        BuildInfo buildInfo = new();
        buildInfo.scenes = levels;
        var json = GameFrame.Core.Json.Handler.Serialize(buildInfo, Formatting.None, new JsonSerializerSettings());

        //var json = JsonUtility.ToJson(new BuildInfo());
        File.WriteAllTextAsync(locationPath + "/BuildInfo.json", json);

        if (report.summary.totalErrors > 0)
        {
            EditorApplication.Exit(1);
        }
    }

    public static void BuildProjectProduction()
    {
        var report = BuildPipeline.BuildPlayer(GetScenePaths(Scenes.GetGameScenes()), locationPath, BuildTarget.WebGL, BuildOptions.None);

        //var report = BuildPipeline.BuildPlayer(GetSampleScene(), locationPath, BuildTarget.WebGL, BuildOptions.Development);
        //Debug.Log($"Build result: {report.summary.result}, {report.summary.totalErrors} errors");
        var json = GameFrame.Core.Json.Handler.Serialize(new BuildInfo(), Formatting.None, new JsonSerializerSettings());

        //var json = JsonUtility.ToJson(new BuildInfo());
        _ = File.WriteAllTextAsync(locationPath + "/BuildInfo.json", json);

        if (report.summary.totalErrors > 0)
        {
            EditorApplication.Exit(1);
        }
    }

    private static String[] GetScenePaths(params List<Scene>[] sceneListArray)
    {
        List<String> sceneNames = new List<String>();

        if (sceneListArray?.Length > 0)
        {
            foreach (var scenes in sceneListArray)
            {
                foreach (var scene in scenes)
                {
                    sceneNames.Add(scene.Name);
                }
            }
        }

        string[] sceneArray = new string[sceneNames.Count];

        for (int i = 0; i < sceneNames.Count; i++)
        {
            sceneArray[i] = prefix + sceneNames[i] + postfix;
        }

        return sceneArray;
    }
}