using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MyFramework.Tools
{

    public enum FileType
    {
        UselessFile,            //无用文件
        Folder,                 //文件夹
        Audio,                  //音频文件
        Image,                  //图片文件
        UnityFile,              //unity3d资源文件
        DataFile,               //数据配置文件
        ScriptFile,             //脚本文件
    }

    [Serializable]
    public class EditorFileinfo
    {
        public static int UnitX = 10;
        public static int Heght = 15;
        public static int Width = 150;
        public bool IsRootNode = false;
        public string Path;
        public string FlieName;
        public FileType FileType;
        public int Layer = 0;
        public int Index = 0;
        public bool IsShow = false;
        public EditorFileinfo Parent = null;
        public List<EditorFileinfo> Childs;

        private string[] AudioFileSuffix = { ".mp3", ".alff", ".wav", ".ogg" };
        private string[] ImageFileSuffix = { ".psd", ".tiff", ".jpg", ".tga", ".png", ".gif" };
        private string[] UnityFileSuffix = { ".prefab", ".unity", ".fbx",".mat", ".anim", ".controller", ".shader" };
        private string[] DataFileSuffix = { ".xml",".json", ".xlsx" , ".asset" };
        private string[] ScriptFileSuffix = { ".lua" ,".txt"};

        public EditorFileinfo(string _Path, EditorFileinfo _Parent, bool _IsRootNode = false)
        {
            IsRootNode = _IsRootNode;
            IsShow = _IsRootNode ? true : false;
            _Path = _Path.Replace("\\", "/");
            Path = _Path;
            FlieName = Path.Substring(Path.LastIndexOf("/") + 1);
            FileType = GetFileType();
            Parent = _Parent;
            if (Parent != null)
                Layer = Parent.Layer + 1;
            Childs = new List<EditorFileinfo>();
        }
        public void SetfileLayer(int _Layer)
        {
            Layer = _Layer;
            for (int i = 0; i < Childs.Count; i++)
            {
                Childs[i].SetfileLayer(Layer + 1);
            }
        }

        public FileType GetFileType()
        {
            FileType type = FileType.UselessFile;
            if (Directory.Exists(Path))
            {
                return FileType.Folder;
            }
            else if (CheckSuffix(FlieName, AudioFileSuffix))
            {
                return FileType.Audio;
            }
            else if (CheckSuffix(FlieName, ImageFileSuffix))
            {
                return FileType.Image;
            }
            else if (CheckSuffix(FlieName, UnityFileSuffix))
            {
                return FileType.UnityFile;
            }
            else if (CheckSuffix(FlieName, DataFileSuffix))
            {
                return FileType.DataFile;
            }
            else if (CheckSuffix(FlieName, ScriptFileSuffix))
            {
                return FileType.ScriptFile;
            }
            return type;
        }

        public bool IsFolder()
        {
            return FileType == FileType.Folder;
        }


        public bool CheckSuffix(string _FlieName, string[] Suffix)
        {
            for (int i = 0; i < Suffix.Length; i++)
            {
                if (_FlieName.EndsWith(Suffix[i]))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 添加子对象
        /// </summary>
        /// <param name="_Child"></param>
        public void AddChild(EditorFileinfo _Child)
        {
            _Child.Index = Childs.Count;
            Childs.Add(_Child);
        }

        /// <summary>
        /// 搜索同下向下
        /// </summary>
        /// <returns></returns>
        public int GetDownIndex()
        {
            int CurrIndex = 0;
            if (Childs.Count > 0)
            {
                for (int i = 0; i < Childs.Count; i++)
                {
                    CurrIndex++;
                    if (Childs[i].FileType == FileType.Folder && Childs[i].IsShow && Childs[i].Childs.Count > 0)
                    {
                        CurrIndex += Childs[i].GetDownIndex();
                    }
                }
            }
            return CurrIndex;
        }

        public int GetUpIndex()
        {
            int CurrIndex = 0;
            if (Parent != null)
            {
                for (int i = Index; i >= 0; i--)
                {
                    CurrIndex++;
                    if (i != Index && Parent.Childs[i].FileType == FileType.Folder && Parent.Childs[i].IsShow && Parent.Childs[i].Childs.Count > 0)
                    {
                        CurrIndex += Parent.Childs[i].GetDownIndex();
                    }
                }
                CurrIndex += Parent.GetUpIndex();
            }
            else
            {
                CurrIndex = 0;
            }
            return CurrIndex;
        }


        /// <summary>
        /// 过去当前文件的显示位置
        /// </summary>
        /// <returns></returns>
        public int GetAllIndex()
        {
            int CurrIndex = 0;
            CurrIndex += GetUpIndex();
            return CurrIndex;
        }

        /// <summary>
        /// 获取根节点
        /// </summary>
        /// <returns></returns>
        public EditorFileinfo GetRootNode()
        {
            if (IsRootNode)
            {
                return this;
            }
            else
            {
                if (Parent == null)
                    return null;
                else
                {
                    return Parent.GetRootNode();
                }
            }
        }

        /// <summary>
        /// 获取文件树最下面的节点
        /// </summary>
        /// <returns></returns>
        public EditorFileinfo GetLastAssetFile()
        {
            if (Childs.Count > 0 && IsShow)
            {
                return Childs[Childs.Count - 1].GetLastAssetFile();
            }
            else
            {
                return this;
            }
        }

        /// <summary>
        /// 获取对象的相对显示区域
        /// </summary>
        /// <returns></returns>
        public Rect GetGuiRect()
        {
            float x = UnitX * Layer;
            float y = GetAllIndex() * Heght;
            return new Rect(x, y, Width, Heght);
        }

    }
}
