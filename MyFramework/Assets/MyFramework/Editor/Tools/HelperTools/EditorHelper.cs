using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 调试帮助类
/// </summary>
namespace MyFramework.Tools
{
    public static class EditorHelper
    {
        private static Func<Rect> VisibleRect;
        public static void InitType()
        {
            var tyGUIClip = Type.GetType("UnityEngine.GUIClip,UnityEngine");
            if (tyGUIClip != null)
            {
                var piVisibleRect = tyGUIClip.GetProperty("visibleRect", BindingFlags.Static | BindingFlags.Public);
                if (piVisibleRect != null)
                    VisibleRect = (Func<Rect>)Delegate.CreateDelegate(typeof(Func<Rect>), piVisibleRect.GetGetMethod());
            }
        }

        public static Rect visibleRect
        {
            get
            {
                InitType();
                return VisibleRect();
            }
        }


        /// <summary>
        /// 检索资源路径
        /// </summary>
        /// <param name="srcPath"></param>
        /// <param name="filetree"></param>
        public static void GetAssetPathFileInfo(string srcPath, EditorFileinfo filetree,string[] Suffix = null)
        {
            try
            {
                string[] fileList = Directory.GetFileSystemEntries(srcPath);
                foreach (string file in fileList)
                {
                    EditorFileinfo folder = new EditorFileinfo(file, filetree);
                    if (folder.FileType != FileType.UselessFile)
                        filetree.AddChild(folder);
                    if (folder.FileType == FileType.Folder)
                    {
                        GetAssetPathFileInfo(file, folder);
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
        /// 获取选择目录
        /// </summary>
        /// <returns></returns>
        public static string GetSelectedPathOrFallback()
        {
            string path = "Assets";
            foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
            {
                path = AssetDatabase.GetAssetPath(obj);
                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                {
                    path = Path.GetDirectoryName(path);
                    break;
                }
            }
            return path;
        }

    }
}