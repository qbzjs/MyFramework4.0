using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace MyFramework.Tools
{
    public class ZipEditorTools
    {
        [MenuItem("Assets/MyFrameworkTools/压缩文件", false, 80)]
        public static void CreateZip()
        {
            string ZipFile = Application.dataPath + "/" + EditorHelper.GetSelectedPathOrFallback().Substring("Assets/".Length);
            DirectoryInfo dir = new DirectoryInfo(ZipFile);
            FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
            string[] Files = new string[fileinfo.Length];
            for (int i = 0; i < fileinfo.Length; i++)
            {
                Files[i] = fileinfo[i].FullName;
            }
            EditorCoroutineRunner.StartEditorCoroutine(ZipTools.Zip(Files, ZipFile + "/Asset.zip", "1152", new string[] { ".meta" }, UpdataZipProgress));
            
        }

        public static void UpdataZipProgress(string _Describe, float _Progress)
        {
            EditorUtility.DisplayProgressBar("压缩文件", _Describe, _Progress);
            if(_Progress >= 1)
                EditorUtility.ClearProgressBar();
        }
    }
}
