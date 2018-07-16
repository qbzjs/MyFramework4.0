using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace MyFramework.Tools
{
    public class AssetBundleTools
    {
        /// <summary>
        /// 检索资源路径
        /// </summary>
        /// <param name="srcPath"></param>
        /// <param name="filetree"></param>
        public static void GetResPathFileInfo(string srcPath, AssetBundleInfo filetree)
        {
            try
            {
                string[] fileList = Directory.GetFileSystemEntries(srcPath);
                foreach (string file in fileList)
                {
                    AssetBundleInfo folder = new AssetBundleInfo(file, filetree);
                    if (folder.FileType != FileType.UselessFile)
                        filetree.AddChild(folder);
                    if (folder.FileType == FileType.Folder)
                    {
                        GetResPathFileInfo(file, folder);
                    }
                }
            }
            catch (Exception e)
            {
                LoggerHelper.Debug(e);
                throw;
            }
        }


        /// <summary>
        /// 刷新资源打包列表
        /// </summary>
        public static void RefreshResFileInfo(AssetBundleInfo filetree, out AppBuileInfo Builder)
        {
            Builder = new AppBuileInfo();
            Builder.AppResInfo = new Dictionary<string, ResBuileInfo>();
            for (int i = 0; i < filetree.Childs.Count; i++)
            {
                if (filetree.Childs[i].FileType == FileType.Folder)
                {
                    string _AssetBundleName = filetree.Childs[i].FlieName + AppConfig.ResFileSuffix;
                    ((AssetBundleInfo)filetree.Childs[i]).SetAssetBundleName(_AssetBundleName, filetree, ref Builder.AppResInfo);
                    AssetDatabase.Refresh();
                }
            }
        }
        public static void WriteAppBuilderInfo(string SavePath, int AppVersion, int AssetVersion, AppBuileInfo buildinfo)
        {
            string path = AppPathConfig.PlatformRoot + SavePath + SavePath.Substring(SavePath.LastIndexOf("/"));
            AssetBundle assetBundle = AssetBundle.LoadFromFile(path);
            UnityEngine.Object obj = assetBundle.LoadAsset("AssetBundleManifest");
            assetBundle.Unload(false);
            AssetBundleManifest manif = obj as AssetBundleManifest;
            string[] AssetBundles = manif.GetAllAssetBundles();
            for (int i = 0; i < AssetBundles.Length; i++)
            {
                if (buildinfo.AppResInfo.ContainsKey(AssetBundles[i]))
                {
                    ResBuileInfo BuildeInfo = buildinfo.AppResInfo[AssetBundles[i]];
                    FileInfo fiInput = new FileInfo(AppPathConfig.PlatformRoot + SavePath + "/" + BuildeInfo.Id);
                    BuildeInfo.Size = fiInput.Length / 1024.0f;
                    BuildeInfo.Model = AssetBundles[i].Substring(0, AssetBundles[i].IndexOf("/"));
                    BuildeInfo.Md5 = manif.GetAssetBundleHash(AssetBundles[i]).ToString();
                    BuildeInfo.IsNeedUpdata = false;
                    string[] Dependencie = manif.GetDirectDependencies(AssetBundles[i]);
                    BuildeInfo.Dependencies = new string[Dependencie.Length];
                    for (int n = 0; n < Dependencie.Length; n++)
                    {
                        BuildeInfo.Dependencies[n] = Dependencie[n];
                    }
                }
                else
                {
                    LoggerHelper.Error("No AssetBundles Key=" + AssetBundles[i]);
                }

            }
            string Json = JsonConvert.SerializeObject(buildinfo);
            EditorDataTools.WirteStrToFile(AppPathConfig.PlatformRoot + SavePath + "/VersionInfo.json", "{\"AppVersion\":" + AppVersion + ",\"AssetVersion\":" + AssetVersion + "}");
            EditorDataTools.WirteStrToFile(AppPathConfig.PlatformRoot + SavePath + "/AssetInfo.json", Json);

        }
    }
}
