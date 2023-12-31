using UnityEditor;

namespace Core.Editor
{
    public static class BuildHelper
    {
        private const string DevDirective = "REVENKO_DEVELOP";
        private const string ReleaseDirective = "REVENKO_RELEASE";

        [MenuItem("Tools/CapybaraWorld/Build/Set DEFAULT State", false, 50)]
        public static void SetDefaultState()
        {
            RemoveScriptingDefineSymbol(DevDirective);
            RemoveScriptingDefineSymbol(ReleaseDirective);
        }
        
        [MenuItem("Tools/CapybaraWorld/Build/Set DEV State", false, 51)]
        public static void SetDevState()
        {
            AddScriptingDefineSymbol(DevDirective);
            RemoveScriptingDefineSymbol(ReleaseDirective);
        }
        
        [MenuItem("Tools/CapybaraWorld/Build/Set RELEASE State", false, 52)]
        public static void SetReleaseState()
        {
            AddScriptingDefineSymbol(ReleaseDirective);
            RemoveScriptingDefineSymbol(DevDirective);
        }
        
        private static void AddScriptingDefineSymbol(string defineSymbol)
        {
            string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            if (defines.Contains(defineSymbol) == false)
            {
                defines = defines + ";" + defineSymbol;
                PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, defines);
            }
        }

        private static void RemoveScriptingDefineSymbol(string defineSymbol)
        {
            string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            if (defines.Contains(defineSymbol) == true)
            {
                defines = defines.Replace(defineSymbol, "");
                PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, defines);
            }
        }
    }
}