using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public class EditorStartInit {
    [MenuItem("æ¿ Ω««‡/Ω√¿€ æ¿∫Œ≈Õ Ω√¿€ ")]
    public static void SetupFromStartScene() {
        var pathOfFirstScene = EditorBuildSettings.scenes[0].path;
        var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(pathOfFirstScene);
        EditorSceneManager.playModeStartScene = sceneAsset;
        UnityEditor.EditorApplication.isPlaying = true;
    }

    [MenuItem("æ¿ Ω««‡/«ˆ¿Á æ¿∫Œ≈Õ Ω√¿€ ")]
    public static void StartFromThisScene() {
        EditorSceneManager.playModeStartScene = null;
        UnityEditor.EditorApplication.isPlaying = true;
    }
}