using Excel;
using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace MyFramework.Tools
{
    public static class EditorDataTools
    {
        #region 字符串处理
        /// <summary>
        /// 过滤特殊字符
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>json字符串</returns>
        private static string String2Json(String s)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char c = s.ToCharArray()[i];
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\""); break;
                    case '\\':
                        sb.Append("\\\\"); break;
                    case '/':
                        sb.Append("\\/"); break;
                    case '\b':
                        sb.Append("\\b"); break;
                    case '\f':
                        sb.Append("\\f"); break;
                    case '\n':
                        sb.Append("\\n"); break;
                    case '\r':
                        sb.Append("\\r"); break;
                    case '\t':
                        sb.Append("\\t"); break;
                    default:
                        sb.Append(c); break;
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 格式化字符型、日期型、布尔型
        /// </summary>
        /// <param name="str"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static string StringFormat(string str, Type type)
        {
            if (type == typeof(string))
            {
                str = String2Json(str);
                str = "\"" + str + "\"";
            }
            else if (type == typeof(DateTime))
            {
                str = "\"" + str + "\"";
            }
            else if (type == typeof(bool))
            {
                str = str.ToLower();
            }
            else if (type != typeof(string) && string.IsNullOrEmpty(str))
            {
                str = "\"" + str + "\"";
            }
            return str;
        }
        #endregion

        /// <summary>
        /// 写入字符串到文件
        /// </summary>
        /// <param name="FilePath"></param>
        /// <param name="WStr"></param>
        public static void WirteStrToFile(string FilePath,string WStr)
        {
            FileStream wstream = new FileStream(FilePath, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(wstream);
            sw.WriteLine(WStr);
            sw.Close();
            wstream.Close();
        }

        #region Excel文件处理接口
        /// <summary>
        /// 读取Excel文件
        /// </summary>
        /// <param name="ExcelFile"></param>
        /// <returns></returns>
        public static DataSet ReadExcelFile(string ExcelFile)
        {
            FileStream stream = File.Open(ExcelFile, FileMode.Open, FileAccess.Read);
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            DataSet result = excelReader.AsDataSet();
            return result;
        }

        /// <summary>
        /// 读取excel到xml字符串
        /// </summary>
        /// <param name="mData"></param>
        /// <returns></returns>
        public static string ExcelToXmlStr(DataSet mData)
        {
            StringBuilder XmlStr = new StringBuilder();
            DataRowCollection drc = mData.Tables[0].Rows;
            XmlStr.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><Root>");
            string lineInfo = "";
            for (int i = 2; i < drc.Count; i++)
            {
                lineInfo = "<Row>";
                XmlStr.Append(lineInfo);
                for (int j = 0; j < mData.Tables[0].Columns.Count; j++)
                {
                    string value = drc[i][j].ToString();
                    if (value != "")
                    {
                        lineInfo = string.Format("<{0}>{1}</{0}>", drc[0][j].ToString(),value);
                        XmlStr.Append(lineInfo);
                    }
                }
                lineInfo = "</Row>";
                XmlStr.Append(lineInfo);
            }
            XmlStr.Append("</Root>");
            return XmlStr.ToString();
        }

        /// <summary>
        /// Excel转换到Json
        /// </summary>
        /// <param name="mData"></param>
        /// <returns></returns>
        public static string ExcelToJsonStr(DataSet mData)
        {
            StringBuilder jsonString = new StringBuilder();
            jsonString.Append("[");
            DataRowCollection drc = mData.Tables[0].Rows;
            for (int i = 2; i < drc.Count; i++)
            {
                jsonString.Append("{");
                for (int j = 0; j < mData.Tables[0].Columns.Count; j++)
                {
                    string strKey = drc[0][j].ToString();
                    string strValue = drc[i][j].ToString();
                    jsonString.Append("\"" + strKey + "\":");
                    Type type = mData.Tables[0].Columns[j].DataType;
                    strValue = StringFormat(strValue, type);
                    if (j < mData.Tables[0].Columns.Count - 1)
                    {
                        jsonString.Append(strValue + ",");
                    }
                    else
                    {
                        jsonString.Append(strValue);
                    }
                }
                jsonString.Append("},");
            }
            jsonString.Remove(jsonString.Length - 1, 1);
            jsonString.Append("]");
            return jsonString.ToString();
        }

        /// <summary>
        ///  Excel转换到Asset
        /// </summary>
        public static ScriptableObject ExcelToAsset(DataSet mData, string ClassName)
        {
            ScriptableObject data = ScriptableObject.CreateInstance(ClassName);
            if (data is ListDataTable)
            {
                FieldInfo DatasField = data.GetType().GetField("Datas");
                Type DataType = DatasField.FieldType.GetGenericArguments()[0];
                MethodInfo AddDataMethodInfo = data.GetType().GetMethod("AddData");
                DataRowCollection drc = mData.Tables[0].Rows;
                FieldInfo[] DataFields = new FieldInfo[mData.Tables[0].Columns.Count];
                for (int m = 0; m < mData.Tables[0].Columns.Count; m++)
                {
                    DataFields[m] = DataType.GetField(drc[0][m].ToString());
                }
                for (int n = 2; n < drc.Count; n++)
                {
                    object dataitem = DataType.GetConstructor(Type.EmptyTypes).Invoke(null);
                    for (int m = 0; m < mData.Tables[0].Columns.Count; m++)
                    {
                        string value = drc[n][m].ToString();
                        if (value != "")
                        {
                            var obj = DataTools.GetValue(value, DataFields[m].FieldType);
                            DataFields[m].SetValue(dataitem, obj);
                        }
                    }
                    AddDataMethodInfo.Invoke(data, new object[] { dataitem });
                }
            }
            else if (data is ConfigData)
            {
                DataRowCollection drc = mData.Tables[0].Rows;
                Type DataType = data.GetType();
                for (int n = 2; n < drc.Count; n++)
                {
                    string Name = drc[n][0].ToString();
                    string Value = drc[n][1].ToString();
                    var value = DataTools.GetValue(Value, DataType.GetField(Name).FieldType);
                    DataType.GetField(Name).SetValue(data, value);
                }
            }
            return data;
        }
        #endregion

        #region  Xml文件处理接口
        public static ScriptableObject XmlToAsset(DataSet mData, string ClassName)
        {
            ScriptableObject ddata = ScriptableObject.CreateInstance(ClassName);
            return ddata;
        }
        #endregion
    }
}
