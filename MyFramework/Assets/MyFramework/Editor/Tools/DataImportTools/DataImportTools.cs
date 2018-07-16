using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace MyFramework.Tools
{
    /// <summary>
    /// 数据导入工具
    /// </summary>
    public class DataImportTools : EditorWindow
    {
        public readonly GUIStyle listItemBackground1 = new GUIStyle("IN Foldout");
        public readonly GUIStyle listItemBackground2 = new GUIStyle("PR Label");

        [MenuItem("Assets/MyFrameworkTools/导入数据", false, 80)]
        public static void CreatNewLua()
        {
            ResPath = "/" + EditorHelper.GetSelectedPathOrFallback().Substring("Assets/".Length);
            SavePath = "/" + EditorHelper.GetSelectedPathOrFallback().Substring("Assets/".Length);
            GameResBuilderTools();
        }

        [MenuItem("MyFrameworkTools/数据导入工具")]
        static void GameResBuilderTools()
        {
            DataImportTools newWindow = GetWindowWithRect<DataImportTools>(new Rect(100, 100, 600, 400), false, "数据转换界面");
        }
        void OnGUI()
        {
            GUILayout.BeginVertical();
            SelectTarget();
            GUILayout.BeginHorizontal(EditorStyles.helpBox);
            ShowLeftView();
            ShowRightView();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        #region 顶部资源目标界面
        static string ResPath = "";
        static string SavePath = "";
        private void SelectTarget()
        {
            GUILayout.BeginHorizontal(EditorStyles.helpBox);
            GUILayout.Label("目标:", EditorStyles.largeLabel, GUILayout.Width(30));
            GUILayout.Label(ResPath, EditorStyles.textField);
            bool getExcelBtnClick = GUILayout.Button("选择数据导入接口", GUILayout.Width(100));
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
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(EditorStyles.helpBox);
            GUILayout.Label("目标:", EditorStyles.largeLabel, GUILayout.Width(30));
            GUILayout.Label(SavePath, EditorStyles.textField);
            bool getSaveClick = GUILayout.Button("选择导出路径", GUILayout.Width(100));
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
        #endregion

        #region 左边资源列表刷新界面
        public int CurrIndex = -1;
        private AeeetDataInfo filetree;
        private Vector2 LeftscrollPosition;
        private void ShowLeftView()
        {
            GUILayout.BeginVertical(GUILayout.MaxWidth(400));
            GUILayout.Label("资源目录", EditorStyles.whiteLabel);
            GUILayout.BeginHorizontal(EditorStyles.helpBox);
            LeftscrollPosition = GUILayout.BeginScrollView(LeftscrollPosition, false, true, GUILayout.MaxWidth(400), GUILayout.MaxHeight(300));
            if (filetree != null)
            {
                Rect ScrollView = filetree.GetLastAssetFile().GetGuiRect();
                GUILayoutUtility.GetRect(1, ScrollView.y + ScrollView.height);              //填充滑动列表
                DrawResflielistView(filetree);
            }
            GUILayout.EndScrollView();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        /// <summary>
        /// 绘制文件列表
        /// </summary>
        /// <param name="_Bundlefiletree"></param>
        private void DrawResflielistView(EditorFileinfo _Bundlefiletree)
        {
            if (_Bundlefiletree == null) return;
            Rect TargetRect = _Bundlefiletree.GetGuiRect();
            _Bundlefiletree.IsShow = EditorGUI.Foldout(TargetRect, _Bundlefiletree.IsShow, _Bundlefiletree.FlieName);
            bool IsToggle = EditorGUI.Toggle(new Rect(TargetRect.x + 250, TargetRect.y, 50, TargetRect.height), ((AeeetDataInfo)_Bundlefiletree).IsExport, EditorStyles.toggle);
            if (IsToggle != ((AeeetDataInfo)_Bundlefiletree).IsExport)
            {
                ((AeeetDataInfo)_Bundlefiletree).SetIsExport(IsToggle);

            }
            if (_Bundlefiletree.IsShow)
            {
                for (int i = 0; i < _Bundlefiletree.Childs.Count; i++)
                {
                    if (_Bundlefiletree.Childs[i].FileType == FileType.Folder)
                    {
                        DrawResflielistView(_Bundlefiletree.Childs[i]);
                    }
                    else
                    {
                        GUILayout.BeginVertical(GUILayout.MaxHeight(10));
                        GUIContent FileIcon0 = new GUIContent();
                        switch (_Bundlefiletree.Childs[i].FileType)
                        {
                            case FileType.Image:
                                FileIcon0 = EditorGUIUtility.IconContent("MeshRenderer Icon");
                                break;
                            case FileType.Audio:
                                FileIcon0 = EditorGUIUtility.IconContent("AudioMixerController Icon");
                                break;
                            case FileType.DataFile:
                                FileIcon0 = EditorGUIUtility.IconContent("TextAsset Icon");
                                break;
                            default:
                                break;
                        }
                        FileIcon0.text = _Bundlefiletree.Childs[i].FlieName;
                        TargetRect = _Bundlefiletree.Childs[i].GetGuiRect();

                        if (Event.current.type == EventType.Repaint)
                        {
                            listItemBackground2.Draw(TargetRect, FileIcon0, false, false, false, true);
                        }
                        ((AeeetDataInfo)_Bundlefiletree.Childs[i]).IsExport = EditorGUI.Toggle(new Rect(TargetRect.x + 250, TargetRect.y, 50, TargetRect.height), ((AeeetDataInfo)_Bundlefiletree.Childs[i]).IsExport, EditorStyles.toggle);
                        GUILayout.EndVertical();
                    }
                }
            }
        }
        #endregion

        #region 右边控制界面
        private ImputFileType importDataType = ImputFileType.Excel;
        private ExportFileType exportDataType = ExportFileType.Asset;
        private void ShowRightView()
        {
            GUILayout.BeginVertical();
            GUILayout.Space(20);
            GUILayout.BeginVertical(EditorStyles.textField);
            GUILayout.Label("选择导入数据类型");
            importDataType = (ImputFileType)EditorGUILayout.EnumPopup(importDataType);
            GUILayout.Space(10);
            GUILayout.Label("刷新数据文件列表",EditorStyles.centeredGreyMiniLabel);
            bool RefreshClick = GUILayout.Button("刷新", EditorStyles.miniButtonRight, GUILayout.Height(50));
            GUILayout.Space(10);
            GUILayout.EndVertical();
            GUILayout.Space(50);
            GUILayout.BeginVertical(EditorStyles.textField);
            GUILayout.Label("选择导出数据类型");
            exportDataType = (ExportFileType)EditorGUILayout.EnumPopup(exportDataType);
            GUILayout.Space(10);
            GUILayout.Label("导出对应的数据文件", EditorStyles.centeredGreyMiniLabel);
            bool ExportClick = GUILayout.Button("导出", EditorStyles.miniButtonRight, GUILayout.Height(50));
            GUILayout.Space(10);
            GUILayout.EndVertical();
            GUILayout.EndVertical();
            if (RefreshClick)
            {
                string _Path = Application.dataPath + ResPath;
                CurrIndex = -1;
                filetree = new AeeetDataInfo(_Path, null, true);
                AssetDataHelp.GetPathDataFileInfo(_Path, filetree,(DataFileType)importDataType);
            }
            if (ExportClick)
            {
                Check();
            }
        }

        private void Check()
        {
            if (string.IsNullOrEmpty(SavePath))
            {
                bool option = EditorUtility.DisplayDialog(
                    "错误",
                    "请指定保存路径",
                    "ok");
                if (option)
                {
                    Debug.LogError("知道错了!");
                }
            }
            else
            {
                if (importDataType.ToString() == exportDataType.ToString())
                {
                    bool option = EditorUtility.DisplayDialog(
                       "错误",
                       "输出文件类型等于输入文件类型",
                       "ok");
                    if (option)
                    {
                        Debug.LogError("知道错了!");
                    }
                }
                else
                {
                    AssetDataHelp.DataShift(filetree, (DataFileType)exportDataType, Application.dataPath + SavePath);
                    Application.OpenURL(AppPathConfig.PlatformRoot + SavePath);
                }
            }
        }
        #endregion

        #region LuaToTxt 
        [MenuItem("Assets/MyFrameworkTools/LuaToTxt", false, 81)]
        public static void LuaToTxt()
        {
            string TargetPath = Path.Combine(Application.dataPath, EditorHelper.GetSelectedPathOrFallback().Substring("Assets/".Length));
            LuaToTxt(TargetPath);
        }

        public static void LuaToTxt(string TargetPath)
        {
            AssetDataHelp.DealFile(TargetPath, (file) =>
            {
                if (file.EndsWith(".lua"))
                {
                    string AssetPath = file.Replace(".lua", ".txt");
                    if (!File.Exists(AssetPath))
                    {
                        System.IO.File.Copy(file, AssetPath);
                    }
                    File.Delete(file);
                }
            });
            AssetDatabase.Refresh();
        }

        [MenuItem("Assets/MyFrameworkTools/TxtToLua", false, 81)]
        public static void TxtToLua()
        {
            string TargetPath = Path.Combine(Application.dataPath, EditorHelper.GetSelectedPathOrFallback().Substring("Assets/".Length));
            TxtToLua(TargetPath);
        }

        public static void TxtToLua(string TargetPath)
        {
            AssetDataHelp.DealFile(TargetPath, (file) =>
            {
                if (file.EndsWith(".txt"))
                {
                    string AssetPath = file.Replace(".txt", ".lua");
                    if (!File.Exists(AssetPath))
                    {
                        System.IO.File.Copy(file, AssetPath);
                    }
                    File.Delete(file);
                }
            });
            AssetDatabase.Refresh();
        }

        #endregion
    }


}
