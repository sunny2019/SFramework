﻿using HybridCLR.Editor.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace HybridCLR.Editor
{
    public static class BuildAssetsCommand
    {
        [MenuItem("HybridCLR/Build/BuildAssetsAndCopyToStreamingAssets")]
        public static void BuildAndCopyABAOTHotUpdateDlls()
        {
            BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
            CompileDllCommand.CompileDll(target);
            CopyABAOTHotUpdateDlls(target);
        }

        public static void CopyABAOTHotUpdateDlls(BuildTarget target)
        {
            CopyAOTAssembliesToStreamingAssets();
            CopyHotUpdateAssembliesToStreamingAssets();
        }


   
        public static void CopyAOTAssembliesToStreamingAssets()
        {
            var target = EditorUserBuildSettings.activeBuildTarget;
            string aotAssembliesSrcDir = SettingsUtil.GetAssembliesPostIl2CppStripDir(target);
            string aotAssembliesDstDir = Application.streamingAssetsPath;

            foreach (var dll in SettingsUtil.AOTAssemblyNames)
            {
                string srcDllPath = $"{aotAssembliesSrcDir}/{dll}.dll";
                if (!File.Exists(srcDllPath))
                {
                    Debug.LogError($"ab中添加AOT补充元数据dll:{srcDllPath} 时发生错误,文件不存在。裁剪后的AOT dll在BuildPlayer时才能生成，因此需要你先构建一次游戏App后再打包。");
                    continue;
                }
                string dllBytesPath = $"{aotAssembliesDstDir}/{dll}.dll.bytes";
                File.Copy(srcDllPath, dllBytesPath, true);
                Debug.Log($"[CopyAOTAssembliesToStreamingAssets] copy AOT dll {srcDllPath} -> {dllBytesPath}");
            }
        }

        public static void CopyHotUpdateAssembliesToStreamingAssets()
        {
            var target = EditorUserBuildSettings.activeBuildTarget;

            string hotfixDllSrcDir = SettingsUtil.GetHotUpdateDllsOutputDirByTarget(target);
            string hotfixAssembliesDstDir = Application.streamingAssetsPath;
#if NEW_HYBRIDCLR_API
            foreach (var dll in SettingsUtil.HotUpdateAssemblyFilesExcludePreserved)
#else
            foreach (var dll in SettingsUtil.HotUpdateAssemblyFiles)
#endif
            {
                string dllPath = $"{hotfixDllSrcDir}/{dll}";
                string dllBytesPath = $"{hotfixAssembliesDstDir}/{dll}.bytes";
                File.Copy(dllPath, dllBytesPath, true);
                Debug.Log($"[CopyHotUpdateAssembliesToStreamingAssets] copy hotfix dll {dllPath} -> {dllBytesPath}");
            }
        }

       
    }
}
