using System.Xml;
using System.IO;
using System.Collections.Generic;

namespace MyFramework
{
    public static class Xml_Parser
    {
        #region 读
        /// <summary>
        /// 根据xml路径读取数据
        /// </summary>
        /// <returns><c>true</c>, if xml path to dictionary was  read, <c>false</c> otherwise.</returns>
        /// <param name="filePath">File path.</param>
        /// <param name="map">Map.</param>
        public static bool ReadXmlPathToDictionary(string filePath, out Dictionary<string, string[]> map)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filePath);
                return ReadXmlToDictionary(doc, out map);
            }
            catch (System.Exception e)
            {
                LoggerHelper.Except(e);
            }
            map = new Dictionary<string, string[]>();
            return false;
        }

        /// <summary>
        /// 根据xmlStr 读取数据
        /// </summary>
        /// <returns><c>true</c>, if xml text to dictionary was  read, <c>false</c> otherwise.</returns>
        /// <param name="filePath">xml 格式的字符串.</param>
        /// <param name="map">Map.</param>
        public static bool ReadXmlTextToDictionary(string XmlStr, out Dictionary<string, string[]> map)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(XmlStr);
                return ReadXmlToDictionary(doc, out map);
            }
            catch (System.Exception e)
            {
                LoggerHelper.Except(e);
            }
            map = new Dictionary<string, string[]>();
            return false;
        }
        public static bool ReadXmlToDictionary(XmlReader reader, out Dictionary<string, string[]> map)
        {
            map = new Dictionary<string, string[]>();
            while (reader.Read())
            {
                //通过rdr.Name得到节点名  
                string elementName = reader.Name;
                LoggerHelper.Debug(elementName);
            }
            return false;
        }



        public static bool ReadXmlToDictionary(XmlDocument doc, out Dictionary<string, string[]> map)
        {
            map = new Dictionary<string, string[]>();
            XmlElement root = null;
            root = doc.DocumentElement;
            for (int i = 0; i < root.ChildNodes.Count; i++)
            {
                if (root.ChildNodes[i].Name == "Row")
                {
                    for (int j = 0; j < root.ChildNodes[i].ChildNodes.Count; j++)
                    {
                        if (root.ChildNodes[i].ChildNodes[j].Name == "Cell")
                        {
                            if (i == 0)
                            {
                                map[root.ChildNodes[0].ChildNodes[j].InnerText] = new string[root.ChildNodes.Count];
                            }
                            map[root.ChildNodes[0].ChildNodes[j].InnerText][i] = root.ChildNodes[i].ChildNodes[j].InnerText;
                        }
                    }
                }
            }
            return true;
        }

        #endregion

        #region 写
        /// <summary>
        /// 储存数据到本地xml
        /// </summary>
        /// <typeparam name="Key"></typeparam>
        /// <typeparam name="Value"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="m_dataMap"></param>
        public static void DataToXml<Value>(string filePath, Value m_dataMap)
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(dec);
            //创建一个根节点（一级）
            XmlElement root = doc.CreateElement("Root");
            doc.AppendChild(root);
            System.Type type = typeof(Value);
            var props = type.GetProperties();//获取实体属性
            for (int i = -2; i < 0; i++)
            {
                XmlNode node1 = doc.CreateNode(XmlNodeType.Element, "Row", null);
                if (i == -2)
                {
                    XmlElement element1 = doc.CreateElement("Cell");
                    element1.InnerText = "Key";
                    node1.AppendChild(element1);
                    XmlElement element2 = doc.CreateElement("Cell");
                    element2.InnerText = "Value";
                    node1.AppendChild(element2);
                }
                else if (i == -1)
                {
                    XmlElement element1 = doc.CreateElement("Cell");
                    element1.InnerText = "Key is Describe";
                    node1.AppendChild(element1);
                    XmlElement element2 = doc.CreateElement("Cell");
                    element2.InnerText = "Value is Describe";
                    node1.AppendChild(element2);
                }
                root.AppendChild(node1);
            }
            foreach (var prop in props)
            {
                XmlNode node1 = doc.CreateNode(XmlNodeType.Element, "Row", null);
                XmlElement element1 = doc.CreateElement("Cell");
                element1.InnerText = prop.Name;
                node1.AppendChild(element1);
                XmlElement element2 = doc.CreateElement("Cell");
                element2.InnerText = DataTools.GetString(prop.GetValue(m_dataMap, null));
                node1.AppendChild(element2);
                root.AppendChild(node1);
            }
            doc.Save(filePath);
        }


        /// <summary>
        /// 储存数据到本地xml
        /// </summary>
        /// <typeparam name="Key"></typeparam>
        /// <typeparam name="Value"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="m_dataMap"></param>
        public static void DataToXml<Key, Value>(string filePath, Dictionary<Key, Value> m_dataMap)
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(dec);
            //创建一个根节点（一级）
            XmlElement root = doc.CreateElement("Root");
            doc.AppendChild(root);

            System.Type type = typeof(Value);
            var props = type.GetProperties();//获取实体属性

            List<Key> Keys = new List<Key>(m_dataMap.Keys);

            for (int i = -2; i < m_dataMap.Count; i++)
            {
                XmlNode node1 = doc.CreateNode(XmlNodeType.Element, "Row", null);
                foreach (var prop in props)
                {
                    XmlElement element1 = doc.CreateElement("Cell");
                    if (i == -2)
                    {
                        element1.InnerText = prop.Name;
                    }
                    else if (i == -1)
                    {
                        element1.InnerText = prop.Name + " is Describe";
                    }
                    else
                    {
                        element1.InnerText = DataTools.GetString(prop.GetValue(m_dataMap[Keys[i]], null));
                    }
                    node1.AppendChild(element1);
                }
                root.AppendChild(node1);
            }

            DirectoryInfo dir = new DirectoryInfo(filePath);
            if (!dir.Parent.Exists)
                dir.Parent.Create();
            doc.Save(filePath);
        }

        #endregion
    }
}
