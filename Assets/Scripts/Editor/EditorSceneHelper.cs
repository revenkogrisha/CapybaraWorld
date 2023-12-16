using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityToolbarExtender;

namespace Core.Editor
{
    [InitializeOnLoad]
    public static class EditorSceneHelper
    {
        private const string BootscenePath = "Assets/Scenes/Bootscene.unity";
        private const string GameScenePath = "Assets/Scenes/Game.unity";
        
        private static bool IsReady => EditorApplication.isCompiling == false && EditorApplication.isPlayingOrWillChangePlaymode == false;
        
        static EditorSceneHelper()
        {   
            ToolbarExtender.LeftToolbarGUI.Add(OnLeftOnToolbarGUI);
            EditorSceneManager.playModeStartScene = null;
        }

        [MenuItem("Tools/CapybaraWorld/To Bootscene", false, 0)]
        private static void ToBootscene()
        {
            if (IsReady == false)
                return;

            EditorSceneManager.OpenScene(BootscenePath);
        }
        
        [MenuItem("Tools/CapybaraWorld/To Game", false, 1)]
        private static void ToGame()
        {
            if (EditorApplication.isCompiling == true || EditorApplication.isPlaying == true)
                return;

            EditorSceneManager.OpenScene(GameScenePath);
        }

        private static void OnLeftOnToolbarGUI()
        {
            if (IsReady == false)
                return;

            OnStartBootsceneButton();
        }

        private static void OnStartBootsceneButton()
        {
            GUIContent content = new("Play Bootscene",
                "Starts playmode from Bootscene.unity, but doesn't open it");
            
            if (GUILayout.Button(content, ToolbarStyles.GetCommandButtonStyle()))
            {
                EditorSceneManager.playModeStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(BootscenePath);
                EditorApplication.EnterPlaymode();
            }
        }
    }
}
