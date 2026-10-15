using System.IO;
using UnityEditor;
using UnityEngine;

public class KeywordReplace : UnityEditor.AssetModificationProcessor
{

    //public static void OnWillCreateAsset(string path)
    //{
    //    Debug.Log("Path");
    //    Debug.Log(path);

    //    //path = path.Replace(".meta", "");
    //    //int index = path.LastIndexOf(".");
    //    //string file = path.Substring(index);
    //    //if (file != ".cs" && file != ".js" && file != ".boo") return;
    //    //index = Application.dataPath.LastIndexOf("Assets");
    //    //path = Application.dataPath.Substring(0, index) + path;
    //    //file = System.IO.File.ReadAllText(path);

    //    ///*
    //    //file = file.Replace("#CREATIONDATE#", System.DateTime.Now + "");
    //    //file = file.Replace("#PROJECTNAME#", PlayerSettings.productName);
    //    //file = file.Replace("#SMARTDEVELOPERS#", PlayerSettings.companyName);
    //    //*/

    //    //file = file.Replace("#PENTANAMESPACE#", "Test.Name.Space");

    //    //System.IO.File.WriteAllText(path, file);
    //    //AssetDatabase.Refresh();
    //}

    public static void OnWillCreateAsset(string path)
    {
        if (!path.EndsWith(".cs.meta"))
        {
            return;
        }

        string originalFilePath = AssetDatabase.GetAssetPathFromTextMetaFilePath(path);
        string file = File.ReadAllText(originalFilePath);

        var fullNamespace = originalFilePath;

        fullNamespace = fullNamespace.Substring(0, fullNamespace.LastIndexOf("/"));
        fullNamespace = fullNamespace.Replace('/', '.');

        Debug.Log(fullNamespace);

        //change whatever you want (you can add stuff below, just be sure to add the tags in the script template too!)
        file = file.Replace("#PENTANAMESPACE#", fullNamespace);

        //Write the changes in the new script
        File.WriteAllText(originalFilePath, file);
        AssetDatabase.Refresh();
    }

    //public static void OnWillCreateAsset(string path)
    //{
    //    path = path.Replace(".meta", string.Empty);

    //    Debug.Log($"File exists: {System.IO.File.Exists(path)}");

    //    if (!path.EndsWith(".cs"))
    //    {
    //        return;
    //    }

    //    var systemPath = path.Insert(0, Application.dataPath.Substring(0, Application.dataPath.LastIndexOf("Assets")));

    //    ReplaceScriptKeywords(systemPath, path);

    //    AssetDatabase.Refresh();
    //}


    //private static void ReplaceScriptKeywords(string systemPath, string projectPath)
    //{
    //    projectPath = projectPath.Substring(projectPath.IndexOf("/SCRIPTS/") + "/SCRIPTS/".Length);
    //    projectPath = projectPath.Substring(0, projectPath.LastIndexOf("/"));
    //    projectPath = projectPath.Replace("/Scripts/", "/").Replace('/', '.');

    //    var rootNamespace = string.IsNullOrWhiteSpace(EditorSettings.projectGenerationRootNamespace) ?
    //        string.Empty :
    //        $"{EditorSettings.projectGenerationRootNamespace}.";

    //    var fullNamespace = $"{rootNamespace}{projectPath}";

    //    Debug.Log(systemPath);
    //    Debug.Log(projectPath);

    //    var fileData = File.ReadAllText(systemPath);

    //    fileData = fileData.Replace("#PENTANAMESPACE#", fullNamespace);

    //    File.WriteAllText(systemPath, fileData);
    //}
}
