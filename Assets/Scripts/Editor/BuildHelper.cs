using System;
using System.IO;
using System.Linq;
using Core.Editor.Debugger;
using UnityEditor;
using UnityEditor.Build.Reporting;

namespace Core.Editor
{
    public static class BuildHelper
    {
        private const string DevDirective = "REVENKO_DEVELOP";
        private const string DebugDirective = "CUSTOM_DEBUG";
        private const string ReleaseDirective = "REVENKO_RELEASE";
        
        private const string FileNameFormat = "rel{0}.apk";
        private const string DevFileNameFormat = "dev{0}.apk";
        private const string ReleaseFileNameFormat = "Release{0}.aab";
        
        private const string RelativeOutputFolder = "Documents/GameProjects/Capybara World/Builds";

        [MenuItem("Tools/CapybaraWorld/Build/Set DEFAULT State", false, 50)]
        public static void SetDefaultState()
        {
            RemoveScriptingDefineSymbol(DevDirective);
            RemoveScriptingDefineSymbol(DebugDirective);
            RemoveScriptingDefineSymbol(ReleaseDirective);
        }
        
        [MenuItem("Tools/CapybaraWorld/Build/Set DEV State", false, 51)]
        public static void SetDevState()
        {
            AddScriptingDefineSymbol(DevDirective);
            AddScriptingDefineSymbol(DebugDirective);
            RemoveScriptingDefineSymbol(ReleaseDirective);
        }
        
        [MenuItem("Tools/CapybaraWorld/Build/Set RELEASE State", false, 52)]
        public static void SetReleaseState()
        {
            AddScriptingDefineSymbol(ReleaseDirective);
            RemoveScriptingDefineSymbol(DevDirective);
            RemoveScriptingDefineSymbol(DebugDirective);
        }

#if UNITY_ANDROID
        [MenuItem("Tools/CapybaraWorld/Build/Build DEV for Android", false, 70)]
#else
        [MenuItem("Tools/CapybaraWorld/Build/Build DEV for <unknown> platform", false, 70)]
#endif
        public static void BuildDev()
        {
            SetDevState();

            EditorUserBuildSettings.buildAppBundle = false;
            EditorUserBuildSettings.development = true;
            
            BuildOptions buildOptions = BuildOptions.StrictMode | BuildOptions.Development | BuildOptions.CompressWithLz4;
        
            buildOptions = EditorUserBuildSettings.allowDebugging == true
                ? buildOptions | BuildOptions.AllowDebugging
                : buildOptions;
            
            BuildCurrentPlatform(buildOptions, DevFileNameFormat);
        }
        
#if UNITY_ANDROID
        [MenuItem("Tools/CapybaraWorld/Build/Build RELEASE for Android", false, 71)]
#else
        [MenuItem("Tools/CapybaraWorld/Build/Build RELEASE for <unknown> platform", false, 71)]
#endif
        public static void BuildRelease()
        {
            SetReleaseState();
            
            EditorUserBuildSettings.development = false;
            EditorUserBuildSettings.allowDebugging = false;
            
            BuildOptions buildOptions = BuildOptions.StrictMode | BuildOptions.CompressWithLz4HC;
            
            buildOptions = EditorUserBuildSettings.exportAsGoogleAndroidProject == true
                ? buildOptions | BuildOptions.AcceptExternalModificationsToPlayer
                : buildOptions;

            string fileNameFormat = EditorUserBuildSettings.buildAppBundle == true
                ? ReleaseFileNameFormat
                : FileNameFormat;
            
            BuildCurrentPlatform(buildOptions, fileNameFormat);
        }

        [MenuItem("Tools/CapybaraWorld/Build/Open builds folder", false, 72)]
        public static void OpenBuildsFolder() =>
            EditorUtility.RevealInFinder(GetAbsoluteOutputFolder());
        
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

        private static void BuildCurrentPlatform(BuildOptions buildOptions = BuildOptions.StrictMode, string fileNameFormat = FileNameFormat)
        {
            BuildTarget buildTarget = EditorUserBuildSettings.activeBuildTarget;
            
            string absoluteOutputFolder = GetAbsoluteOutputFolder();
            string defaultFileName = string.Format(fileNameFormat, PlayerSettings.bundleVersion);
            
            string chosenOutput = EditorUtility.SaveFilePanel("Save Build", absoluteOutputFolder, defaultFileName, "");
            if (string.IsNullOrEmpty(chosenOutput) == true)
                return;
            
            BuildPlayerOptions buildPlayerOptions = new()
            {
                scenes = GetScenes(),
                locationPathName = chosenOutput,
                target = buildTarget,
                options = buildOptions
            };
            
            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);

            if (report.summary.result == BuildResult.Succeeded)
                EditorUtility.RevealInFinder(chosenOutput);
            
            LogBuild(report, buildPlayerOptions);
        }

        private static string GetAbsoluteOutputFolder()
        {
            string homeDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string absoluteOutputFolder = Path.Combine(homeDirectory, RelativeOutputFolder);

            if (Directory.Exists(absoluteOutputFolder) == false)
                Directory.CreateDirectory(absoluteOutputFolder);
            
            return absoluteOutputFolder;
        }

        private static string[] GetScenes()
        {
            return EditorBuildSettings.scenes
                .Where(scene => scene.enabled)
                .Select(scene => scene.path)
                .ToArray();
        }

        private static void LogBuild(BuildReport report, BuildPlayerOptions buildPlayerOptions)
        {
            if (report.summary.result == BuildResult.Succeeded)
            {
                RDebug.Info($"Build complete: {buildPlayerOptions.locationPathName}");
            }
            else
            {
                RDebug.Error($"Build failed: {report.summary.result.ToString()}");
            }
        }
    }
}