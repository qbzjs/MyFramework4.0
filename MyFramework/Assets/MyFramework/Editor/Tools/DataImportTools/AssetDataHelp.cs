using System;
using System.Data;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace MyFramework.Tools
{

    public enum DataFileType
    {
        Excel = 1,
        Xml = 2,
        Josn = 3,
        Asset = 4,
    }

    /// <summary>
    /// 导入文件类型
    /// </summary>
    public enum ImputFileType
    {
        Excel = 1,
        Xml = 2,
    }

    /// <summary>
    /// 导出文件类型
    /// </summary>
    public enum ExportFileType
    {
        Asset = 4,
    }

    public static class AssetDataHelp
    {
        /// <summary>
        /// 检索资源路径
        /// </summary>
        /// <param name="srcPath"></param>
        /// <param name="filetree"></param>
        public static void GetPathDataFileInfo(string srcPath, AeeetDataInfo filetree, DataFileType ImportType)
        {
            try
            {
                string[] fileList = Directory.GetFileSystemEntries(srcPath);
                foreach (string file in fileList)
                {
                    AeeetDataInfo folder = new AeeetDataInfo(file, filetree);
                    if (folder.FileType != FileType.UselessFile)
                    {

                        if (folder.FileType == FileType.Folder)
                        {
                            filetree.AddChild(folder);
                            GetPathDataFileInfo(file, folder, ImportType);
                        }
                        else
                        {
                            if (folder.DataType == ImportType)
                            {
                                filetree.AddChild(folder);
                            }
                        }
                    }
                }
                if (filetree.Childs.Count == 0)
                {
                    if (filetree.Parent != null)
                        filetree.Parent.Childs.Remove(filetree);
                }
            }
            catch (Exception e)
            {
                LoggerHelper.Debug(e);
                throw;
            }
        }

        /// <summary>
        /// 数据转换
        /// </summary>
        public static void DataShift(AeeetDataInfo filetree, DataFileType exportDataType, string outPath)
        {
            if (filetree.FileType == FileType.Folder)
            {
                for (int i = 0; i < filetree.Childs.Count; i++)
                {
                    DataShift((AeeetDataInfo)filetree.Childs[i], exportDataType, outPath);
                }
            }
            else if(filetree.FileType == FileType.DataFile && filetree.IsExport)
            {
                string savepath = string.Empty;
                switch (filetree.DataType)
                {
                    case DataFileType.Excel:
                        switch (exportDataType)
                        {
                            case DataFileType.Xml:
                                savepath = outPath + filetree.GetRelativePath().Replace(".xlsx", ".xml");
                                ExcelToXml(filetree.Path, savepath);
                                break;
                            case DataFileType.Josn:
                                savepath = outPath + filetree.GetRelativePath().Replace(".xlsx", ".json");
                                ExcelToJson(filetree.Path, savepath);
                                break;
                            case DataFileType.Asset:
                                savepath = outPath + filetree.GetRelativePath().Replace(".xlsx", ".asset");
                                ExcelToAsset(filetree.Path, savepath);
                                break;
                        }
                        break;
                    case DataFileType.Xml:
                        switch (exportDataType)
                        {
                            case DataFileType.Asset:
                                savepath = outPath + filetree.GetRelativePath().Replace(".xml", ".asset");
                                XmlToAsset(filetree.Path, savepath);
                                break;
                        }
                        break;
                }


            }
        }

        #region Excel 数据文件转换
        private static void ExcelToXml(string ExcelFile, string XmlFile)
        {
            DataSet result = EditorDataTools.ReadExcelFile(ExcelFile);
            string XmlStr = EditorDataTools.ExcelToXmlStr(result);
            EditorDataTools.WirteStrToFile(XmlFile, XmlStr);
        }

        private static void ExcelToJson(string ExcelFile, string JsonFile)
        {
            DataSet result = EditorDataTools.ReadExcelFile(ExcelFile);
            string JonsStr = EditorDataTools.ExcelToJsonStr(result);
            EditorDataTools.WirteStrToFile(JsonFile, JonsStr);
        }

        private static void ExcelToAsset(string ExcelFile, string AssetFile)
        {
            string ClassName = ExcelFile.Substring(ExcelFile.LastIndexOf("/")+1);
            ClassName = ClassName.Substring(0, ClassName.LastIndexOf("."));
            string _AssetFile = AssetFile.Substring(AssetFile.LastIndexOf("Assets"));
            DataSet result = EditorDataTools.ReadExcelFile(ExcelFile);
            ScriptableObject ddata = EditorDataTools.ExcelToAsset(result,ClassName);
            AssetDatabase.CreateAsset(ddata, _AssetFile);
        }
        #endregion

        #region Xml数据文件转换
        private static void XmlToAsset(string ExcelFile, string AssetFile)
        {
            string ClassName = ExcelFile.Substring(ExcelFile.LastIndexOf("/") + 1);
            ClassName = ClassName.Substring(0, ClassName.LastIndexOf("."));
            string _AssetFile = AssetFile.Substring(AssetFile.LastIndexOf("Assets"));
            //DataSet result = EditorDataTools.ReadExcelFile(ExcelFile);
            ScriptableObject ddata = EditorDataTools.XmlToAsset(null, ClassName);
            AssetDatabase.CreateAsset(ddata, _AssetFile);
        }
        #endregion

        #region 目录文件处理

        public static void DealFile(string targetpath, Action<string> backcall)
        {
            try
            {
                string[] fileList = Directory.GetFileSystemEntries(targetpath);
                foreach (string file in fileList)
                {
                    if (!Directory.Exists(file))
                    {
                        if (backcall != null)
                        {
                            backcall(file);
                        }
                    } else
                    {
                        DealFile(file, backcall);
                    }
                }
            }
            catch (Exception e)
            {
                LoggerHelper.Debug(e);
                throw;
            }
        }

        #endregion

    }
}
