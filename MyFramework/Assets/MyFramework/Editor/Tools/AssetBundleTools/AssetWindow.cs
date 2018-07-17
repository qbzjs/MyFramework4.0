using MyFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MyFramework.Tools
{
    public class AssetWindow : EditorWindow
    {
        [MenuItem("MyFrameworkTools/AssetBundle/手动打包")]
        static void GameResBuilderTools()
        {
            AssetBundleInfoTableData BuildConfig = AssetDatabase.LoadAssetAtPath("Assets/AssetBundleInfoTableData.asset", typeof(AssetBundleInfoTableData)) as AssetBundleInfoTableData; ;
            AssetWindow newWindow = GetWindowWithRect<AssetWindow>(new Rect(100, 100, 600, 400), false, "资源打包界面");
            if (BuildConfig != null)
            {
                newWindow.SavePath = BuildConfig.SavePath;
                newWindow.ResPath = BuildConfig.ResPath;
                newWindow.BuildPlatform = BuildConfig.BuildPlatform;
                newWindow.Bundlefiletree = SetBuildInfo(null, BuildConfig.Config);
            }
            else
            {
                newWindow.SavePath = Application.streamingAssetsPath.Substring(AppPathConfig.PlatformRoot.Length);
                newWindow.ResPath = "/Resources";
            }
        }

        void OnGUI()
        {
            GUILayout.BeginVertical();
            SelectTarget();
            ShowAssetView();
            BuildMenm();
            GUILayout.EndVertical();
        }

        #region 顶部资源目标界面
        private string ResPath = "";
        private string SavePath = "";
        private AssetBundleInfo Bundlefiletree;
        AppBuileInfo Builderinfo;
        private void SelectTarget()
        {
            GUILayout.BeginHorizontal(EditorStyles.helpBox);
            GUILayout.Label("目标:", EditorStyles.largeLabel, GUILayout.Width(30));
            GUILayout.Label(ResPath, EditorStyles.textField);
            bool getExcelBtnClick = GUILayout.Button("选择打包路径", GUILayout.Width(100));
            if (getExcelBtnClick)
            {
                ResPath = EditorUtility.OpenFolderPanel("SelectResFile", Application.dataPath, "");
                if (ResPath.Contains(Application.dataPath))
                {
                    ResPath = ResPath.Substring(Application.dataPath.Length, ResPath.Length - Application.dataPath.Length);
                }
                else
                {
                    ResPath = "";
                }
            }
            bool IsRefresh = GUILayout.Button("刷新", GUILayout.Width(50));
            if (IsRefresh)
            {
                RefreshResPath();
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(EditorStyles.helpBox);
            GUILayout.Label("目标:", EditorStyles.largeLabel, GUILayout.Width(30));
            GUILayout.Label(SavePath, EditorStyles.textField);
            bool getSaveClick = GUILayout.Button("选择保存路径", GUILayout.Width(100));
            if (getSaveClick)
            {
                SavePath = EditorUtility.OpenFolderPanel("SelectSaveFile", AppPathConfig.PlatformRoot, "");
                if (SavePath.Contains(AppPathConfig.PlatformRoot))
                {
                    SavePath = SavePath.Substring(AppPathConfig.PlatformRoot.Length, SavePath.Length - AppPathConfig.PlatformRoot.Length);
                }
                else
                {
                    SavePath = "";
                }
            }
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// 刷新
        /// </summary>
        private void RefreshResPath()
        {
            string _Path = Application.dataPath + ResPath;
            Bundlefiletree = new AssetBundleInfo(_Path, null, true);
            AssetBundleTools.GetResPathFileInfo(_Path, Bundlefiletree);
            AssetBundleTools.RefreshResFileInfo(Bundlefiletree, out Builderinfo);
        }
        #endregion
        #region 中部资源编辑界面
        private Vector2 scrollPosition;
        private void ShowAssetView()
        {
            GUILayout.BeginHorizontal(EditorStyles.helpBox);
            GUILayout.Label("资源打包编辑界面", EditorStyles.centeredGreyMiniLabel);
            bool totxtclick = GUILayout.Button("LuaToTxt", GUILayout.Width(100));
            if (totxtclick)
            {
                string _Path = Application.dataPath + ResPath;
                DataImportTools.LuaToTxt(_Path);
                RefreshResPath();
            }
            bool toluaclick = GUILayout.Button("TxtToLua", GUILayout.Width(100));
            if (toluaclick)
            {
                string _Path = Application.dataPath + ResPath;
                DataImportTools.TxtToLua(_Path);
                RefreshResPath();
            }
            GUILayout.EndHorizontal();
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true, GUILayout.MaxHeight(300));
            DrawModelReslistView(Bundlefiletree);
            GUILayout.EndScrollView();
            
        }

        private void DrawModelReslistView(EditorFileinfo _Bundlefiletree)
        {
            if (_Bundlefiletree == null) return;
            GUILayout.BeginHorizontal();
            EditorGUI.indentLevel = _Bundlefiletree.Layer;
            GUILayout.BeginVertical(GUILayout.MaxWidth(200));
            _Bundlefiletree.IsShow = EditorGUILayout.Foldout(_Bundlefiletree.IsShow, _Bundlefiletree.FlieName);
            GUILayout.EndVertical();
            if (((AssetBundleInfo)_Bundlefiletree).IsSelectAssetBundle)
            {
                bool IsMergerAssetBundle = EditorGUILayout.Toggle(((AssetBundleInfo)_Bundlefiletree).IsMergerAssetBundle,GUILayout.Width(100));
                if (((AssetBundleInfo)_Bundlefiletree).IsMergerAssetBundle)
                {
                    AssetCheckMode checkmodel = (AssetCheckMode)EditorGUILayout.EnumPopup(((AssetBundleInfo)_Bundlefiletree).CheckMode, GUILayout.MinWidth(50));
                    if (checkmodel != ((AssetBundleInfo)_Bundlefiletree).CheckMode)
                    {
                        ((AssetBundleInfo)_Bundlefiletree).SetCheckMode(checkmodel);
                    }
                }
                else
                {
                    if (_Bundlefiletree.FileType == FileType.Folder)
                    {
                        AssetCheckMode checkmodel = (AssetCheckMode)EditorGUILayout.EnumPopup(((AssetBundleInfo)_Bundlefiletree).CheckMode, GUILayout.MinWidth(50));
                        if (checkmodel != ((AssetBundleInfo)_Bundlefiletree).CheckMode)
                        {
                            ((AssetBundleInfo)_Bundlefiletree).SetCheckMode(checkmodel);
                        }
                    }
                }
                if (IsMergerAssetBundle != ((AssetBundleInfo)_Bundlefiletree).IsMergerAssetBundle)
                {
                    ((AssetBundleInfo)_Bundlefiletree).IsMergerAssetBundle = IsMergerAssetBundle;
                    AssetBundleTools.RefreshResFileInfo(Bundlefiletree, out Builderinfo);
                }
            }
            GUILayout.EndHorizontal();

            if (_Bundlefiletree.IsShow)
            {
                for (int i = 0; i < _Bundlefiletree.Childs.Count; i++)
                {
                    if (_Bundlefiletree.Childs[i].FileType == FileType.Folder)
                    {
                       DrawModelReslistView(_Bundlefiletree.Childs[i]);
                    }
                    else
                    {
                        GUILayout.BeginHorizontal(GUILayout.MaxHeight(10));

                        GUIContent FileIcon0 = new GUIContent();
                        switch (_Bundlefiletree.Childs[i].FileType)
                        {
                            case FileType.Image:
                                FileIcon0 = EditorGUIUtility.IconContent("MeshRenderer Icon");
                                break;
                            case FileType.UnityFile:
                                FileIcon0 = EditorGUIUtility.IconContent("PreMatCube");
                                break;
                            case FileType.Audio:
                                FileIcon0 = EditorGUIUtility.IconContent("AudioMixerController Icon");
                                break;
                            case FileType.DataFile:
                                FileIcon0 = EditorGUIUtility.IconContent("TextAsset Icon");
                                break;
                            case FileType.ScriptFile:
                                FileIcon0 = EditorGUIUtility.IconContent("TextAsset Icon");
                                break;
                            default:
                                break;
                        }
                        FileIcon0.text = _Bundlefiletree.Childs[i].FlieName;
                        EditorGUI.indentLevel = _Bundlefiletree.Childs[i].Layer;
                        EditorGUILayout.LabelField(FileIcon0);
                        GUILayout.EndHorizontal();
                    }
                }
            }
        }

        #endregion
        #region 底部编辑界面
        AppPlatform BuildPlatform = AppPlatform.Windows;
        int AppVersion = 1;
        int AssetVersion = 1;
        private void BuildMenm()
        {
            GUILayout.BeginHorizontal(EditorStyles.helpBox, GUILayout.Height(50));
            GUILayout.BeginVertical(GUILayout.Width(250));
            BuildPlatform = (AppPlatform)EditorGUILayout.EnumPopup("目标平台", BuildPlatform, EditorStyles.popup, GUILayout.Width(300));
            AppVersion = EditorGUILayout.IntField("程序版号", AppVersion);
            AssetVersion = EditorGUILayout.IntField("资源版号", AssetVersion);
            GUILayout.EndVertical();
            EditorGUILayout.Space();
            bool getBuilResClick = GUILayout.Button("编译资源", EditorStyles.miniButtonRight, GUILayout.Width(100), GUILayout.Height(50));
            //bool getSaveClick = GUILayout.Button("打包Apk", EditorStyles.miniButtonRight, GUILayout.Width(100), GUILayout.Height(50));
            bool getSaveBuildConfig = GUILayout.Button("保存编译设置", EditorStyles.miniButtonRight, GUILayout.Width(100), GUILayout.Height(50));
            GUILayout.EndHorizontal();
            if (getBuilResClick)
            {
                BuildResCheckClick();
            }
            //if (getSaveClick)
            //{
            //    //BuildPipeline.BuildAssetBundles(AppPathConfig.PlatformRoot+SavePath, 0, BuildPlatform);
            //    //AssetDatabase.Refresh();
            //}
            if (getSaveBuildConfig)
            {
                SaveBuildConfig();
            }
        }

        /// <summary>
        /// 打包资源文件
        /// </summary>
        private void BuildResCheckClick()
        {
            if (string.IsNullOrEmpty(SavePath))
            {
                bool option = EditorUtility.DisplayDialog(
                    "未指定保存路径",
                    "请指定保存路径",
                    "ok");
                if (option)
                {
                    Debug.LogError("知道错了!");
                }
            }
            else
            {
                FilesTools.ClearDirectory(AppPathConfig.PlatformRoot + SavePath);
                BuildRes();
                WriteBuilderInfo();
                FilesTools.ClearDirFile(AppPathConfig.PlatformRoot + SavePath, new string[] { AppConfig.ResFileSuffix, ".json" });
                Application.OpenURL(AppPathConfig.PlatformRoot + SavePath);
            }
        }

        private void BuildRes()
        {
            BuildTarget BuildTargetPlatform = BuildTarget.StandaloneWindows;
            switch (BuildPlatform)
            {
                case AppPlatform.Android:
                    BuildTargetPlatform = BuildTarget.Android;
                    break;
                case AppPlatform.IOS:
                    BuildTargetPlatform = BuildTarget.iOS;
                    break;
                case AppPlatform.Windows:
                    BuildTargetPlatform = BuildTarget.StandaloneWindows;
                    break;
                default:
                    BuildTargetPlatform = BuildTarget.StandaloneWindows;
                    break;
            }
            AssetBundleBuild[] builds = new AssetBundleBuild[Builderinfo.AppResInfo.Count];
            List<string> BuilderKeys = new List<string>(Builderinfo.AppResInfo.Keys);
            for (int i = 0; i < BuilderKeys.Count; i++)
            {
                builds[i] = new AssetBundleBuild();
                builds[i].assetBundleName = BuilderKeys[i];
                builds[i].assetNames = Builderinfo.AppResInfo[BuilderKeys[i]].Assets.ToArray();
            }
            BuildPipeline.BuildAssetBundles(AppPathConfig.PlatformRoot + SavePath, builds, BuildAssetBundleOptions.None, BuildTargetPlatform);
            AssetDatabase.Refresh();
        }

        private void WriteBuilderInfo()
        {
            AssetBundleTools.WriteAppBuilderInfo(SavePath, AppVersion, AssetVersion, Builderinfo);
        }

        /// <summary>
        /// 保存编译设置
        /// </summary>
        private void SaveBuildConfig()
        {
            AssetBundleInfoData BuildConfig = SetBuildConfig(Bundlefiletree);
            AssetBundleInfoTableData Data = new AssetBundleInfoTableData();
            Data.BuildPlatform = BuildPlatform;
            Data.SavePath = SavePath;
            Data.ResPath = ResPath;
            Data.Config = BuildConfig;
            AssetDatabase.CreateAsset(Data, "Assets/AssetBundleInfoTableData.asset");
        }

        private AssetBundleInfoData SetBuildConfig(AssetBundleInfo BundleInfo)
        {
            AssetBundleInfoData Config = new AssetBundleInfoData();
            Config.IsRootNode = BundleInfo.IsRootNode;
            Config.Path = BundleInfo.Path;
            Config.IsMergerAssetBundle = BundleInfo.IsMergerAssetBundle;
            Config.IsSelectAssetBundle = BundleInfo.IsSelectAssetBundle;
            Config.CheckMode = BundleInfo.CheckMode;
            Config.Chiles = new List<AssetBundleInfoData>();
            for (int i = 0; i < BundleInfo.Childs.Count; i++)
            {
                Config.Chiles.Add(SetBuildConfig((AssetBundleInfo)BundleInfo.Childs[i]));
            }
            return Config;
        }

        private static AssetBundleInfo SetBuildInfo(AssetBundleInfo Parent, AssetBundleInfoData Config)
        {
            AssetBundleInfo BundleInfo = new AssetBundleInfo(Config.Path, Parent, Config.IsRootNode);
            BundleInfo.IsMergerAssetBundle = Config.IsMergerAssetBundle;
            BundleInfo.IsSelectAssetBundle = Config.IsSelectAssetBundle;
            BundleInfo.CheckMode = Config.CheckMode;
            for (int i = 0; i < Config.Chiles.Count; i++)
            {
                BundleInfo.Childs.Add(SetBuildInfo(BundleInfo, Config.Chiles[i]));
            }
            return BundleInfo;
        }
        #endregion
    }
}
