using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFramework.Tools
{
    [Serializable]
    public class AeeetDataInfo : EditorFileinfo
    {
        public bool IsExport = false;
        public DataFileType DataType;
        public AeeetDataInfo(string _Path, AeeetDataInfo _Parent, bool _IsRootNode = false)
            :base(_Path, _Parent, _IsRootNode)
        {
            if (FileType == FileType.DataFile)
            {
                if (Path.EndsWith(".xml"))
                {
                    DataType = DataFileType.Xml;
                }
                else if (Path.EndsWith(".json"))
                {
                    DataType = DataFileType.Josn;
                }
                else if (Path.EndsWith(".xlsx"))
                {
                    DataType = DataFileType.Excel;
                }
                else if (Path.EndsWith(".asset"))
                {
                    DataType = DataFileType.Asset;
                }
            }
        }

        public void SetIsExport(bool _IsExport)
        {
            IsExport = _IsExport;
            if (FileType == FileType.Folder)
            {
                for (int i = 0; i < Childs.Count; i++)
                {
                    ((AeeetDataInfo)Childs[i]).SetIsExport(_IsExport);
                }
            }
        }

        /// <summary>
        /// 获取相对路径
        /// </summary>
        /// <returns></returns>
        public string GetRelativePath()
        {
            EditorFileinfo FileInfo = GetRootNode();
            if (FileInfo != null)
            {
                return Path.Substring(FileInfo.Path.Length);
            }
            return Path;
        }

    }
}
