using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MyFramework
{
    public static class DataTools
    {
        #region XmlStr 转 TypeObject
        #region 单数据
        /// <summary>
        /// 根据xmlstr 获取数据
        /// </summary>
        /// <typeparam name="Value"></typeparam>
        /// <param name="XmlStr"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object FormatDataForXmlStr<Value>(string XmlStr) where Value : new()
        {
            object result = null;
            Type type = typeof(Value);
            try
            {
                result = type.GetConstructor(Type.EmptyTypes).Invoke(null);
                Dictionary<String, String> map;//int32 为 id, string 为 属性名, string 为 属性值

                if (DataFileReadForXmlStr(XmlStr, out map))
                {
                    var props = type.GetProperties();//获取实体属性
                    foreach (var prop in props)
                    {
                        if (map.ContainsKey(prop.Name))
                        {
                            var value = GetValue(map[prop.Name], prop.PropertyType);
                            prop.SetValue(result, value, null);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("FormatData Error: " + type.Name + "  " + ex.Message);
            }
            return result;
        }

        /// <summary>
        /// xmlstring 转data
        /// </summary>
        /// <param name="XmlStr"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        private static bool DataFileReadForXmlStr(string XmlStr, out Dictionary<String, String> map)
        {
            Dictionary<String, String[]> tmpdata;

            if (Xml_Parser.ReadXmlTextToDictionary(XmlStr, out tmpdata))
            {
                AssemblyData(tmpdata, out map);
                return true;
            }
            else
            {
                map = new Dictionary<String, String>();
                return false;
            }
        }

        /// <summary>
        /// 将解析出来的数据组装成需要的格式
        /// </summary>
        /// <param name="tmpdata"></param>
        /// <param name="map"></param>
        private static void AssemblyData(Dictionary<String, String[]> tmpdata, out Dictionary<String, String> map)
        {

            map = new Dictionary<String, String>();
            String[] Keys = tmpdata["Key"];
            for (int i = 2; i < Keys.Length; i++)
            {
                map.Add(Keys[i], tmpdata["Value"][i]);
            }
            return;
        }

        #endregion

        #region 多数据
        /// <summary>
        /// 读取xmlstring 到data
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="dicType"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object FormatDataForXmlStr<Key, Value>(string XmlStr)
        {
            object result = null;
            Type dicType = typeof(Dictionary<Key, Value>);
            Type type = typeof(Value);
            try
            {
                result = dicType.GetConstructor(Type.EmptyTypes).Invoke(null);
                Dictionary<Key, Dictionary<String, String>> map;//int32 为 id, string 为 属性名, string 为 属性值

                if (DataFileReadForXmlStr<Key>(XmlStr, out map))
                {
                    var props = type.GetProperties();//获取实体属性
                    foreach (var item in map)
                    {
                        var t = type.GetConstructor(Type.EmptyTypes).Invoke(null);//构造实体实例
                        foreach (var prop in props)
                        {
                            if (item.Value.ContainsKey(prop.Name))
                            {
                                var value = GetValue(item.Value[prop.Name], prop.PropertyType);
                                prop.SetValue(t, value, null);
                            }
                        }
                        dicType.GetMethod("Add").Invoke(result, new object[] { item.Key, t });
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("FormatData Error: " + type.Name + "  " + ex.Message);
            }
            return result;
        }


        /// <summary>
        /// 从本地xmlstr中读取数据
        /// </summary>
        /// <param name="filePath"></param>
        private static bool DataFileReadForXmlStr<Key>(string XmlStr, out Dictionary<Key, Dictionary<String, String>> map)
        {
            Dictionary<String, String[]> tmpdata;
            if (Xml_Parser.ReadXmlTextToDictionary(XmlStr, out tmpdata))
            {
                AssemblyData(tmpdata, out map);
                return true;
            }
            else
            {
                map = new Dictionary<Key, Dictionary<String, String>>();
                return false;
            }
        }

        /// <summary>
        /// 将解析出来的数据组装成需要的格式
        /// </summary>
        /// <param name="tmpdata"></param>
        /// <param name="map"></param>
        private static void AssemblyData<Key>(Dictionary<String, String[]> tmpdata, out Dictionary<Key, Dictionary<String, String>> map)
        {
            map = new Dictionary<Key, Dictionary<String, String>>();
            String[] Keys = tmpdata["Id"];

            for (int i = 2; i < Keys.Length; i++)
            {
                Dictionary<String, String> children = new Dictionary<String, String>();
                map[(Key)GetValue(Keys[i], typeof(Key))] = children;
                foreach (String PropertyName in tmpdata.Keys)
                {
                    children.Add(PropertyName, tmpdata[PropertyName][i]);
                }
            }
            return;
        }
        #endregion

        #region 复合数据
        public static object FormatListDataForXmlStr<Key, Value>(string XmlStr)
        {
            object result = null;
            Type dicType = typeof(Dictionary<Key, List<Value>>);
            Type type = typeof(Value);
            try
            {
                result = dicType.GetConstructor(Type.EmptyTypes).Invoke(null);
                Dictionary<Key, List<Dictionary<String, String>>> map;//int32 为 id, string 为 属性名, string 为 属性值

                if (DataFileReadForXmlStr<Key>(XmlStr, out map))
                {
                    var props = type.GetProperties();//获取实体属性
                    foreach (var item in map)
                    {
                        List<Value> listValue = new List<Value>();
                        for (int i = 0; i < item.Value.Count; i++)
                        {
                            var t = type.GetConstructor(Type.EmptyTypes).Invoke(null);//构造实体实例
                            foreach (var prop in props)
                            {
                                if (item.Value[i].ContainsKey(prop.Name))
                                {
                                    var value = GetValue(item.Value[i][prop.Name], prop.PropertyType);
                                    prop.SetValue(t, value, null);
                                }
                            }
                            listValue.Add((Value)t);
                        }
                        dicType.GetMethod("Add").Invoke(result, new object[] { item.Key, listValue });
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("FormatData Error: " + type.Name + "  " + ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 从本地xmlstr中读取数据
        /// </summary>
        /// <param name="filePath"></param>
        private static bool DataFileReadForXmlStr<Key>(string XmlStr, out Dictionary<Key, List<Dictionary<String, String>>> map)
        {
            Dictionary<String, String[]> tmpdata;
            if (Xml_Parser.ReadXmlTextToDictionary(XmlStr, out tmpdata))
            {
                AssemblyData(tmpdata, out map);
                return true;
            }
            else
            {
                map = new Dictionary<Key, List<Dictionary<string, string>>>();
                return false;
            }
        }

        /// <summary>
        /// 将解析出来的数据组装成需要的格式
        /// </summary>
        /// <param name="tmpdata"></param>
        /// <param name="map"></param>
        private static void AssemblyData<Key>(Dictionary<String, String[]> tmpdata, out Dictionary<Key, List<Dictionary<String, String>>> map)
        {
            map = new Dictionary<Key, List<Dictionary<String, String>>>();
            String[] Keys = tmpdata["Id"];

            for (int i = 2; i < Keys.Length; i++)
            {
                Dictionary<String, String> children = new Dictionary<String, String>();
                foreach (String PropertyName in tmpdata.Keys)
                {
                    children.Add(PropertyName, tmpdata[PropertyName][i]);
                }
                Key key = (Key)GetValue(Keys[i], typeof(Key));
                if (!map.ContainsKey(key))
                {
                    map[key] = new List<Dictionary<string, string>>();
                }
                map[key].Add(children);
            }
            return;
        }

        #endregion

        #region LuaTable
        /// <summary>
        /// 单数据
        /// </summary>
        /// <param name="XmlStr"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ArrayDataForXmlStr(string XmlStr)
        {
            Dictionary<String, String> map;
            DataFileReadForXmlStr(XmlStr, out map);
            return map;
        }
        /// <summary>
        /// 多数据
        /// </summary>
        /// <param name="XmlStr"></param>
        /// <returns></returns>
        public static Dictionary<string, Dictionary<string, string>> TableDataForXmlStr(string XmlStr)
        {
            Dictionary<string, Dictionary<string, string>> map;
            DataFileReadForXmlStr(XmlStr, out map);
            return map;
        }

        #endregion
        #endregion

        #region 字符串数据转换

        #region 常量
        /// <summary>
        /// 键值分隔符： ‘:’
        /// </summary>
        private const Char KEY_VALUE_SPRITER = ':';
        /// <summary>
        /// 数据对外分割
        /// </summary>
        private const Char DATA_FSPRITER = '|';
        /// <summary>
        /// 特殊数据对内分割
        /// </summary>
        private const Char DATA_NSPRITER = ',';
        #endregion

        #region String To TypeValue

        /// <summary>
        /// 将字符串转换为对应类型的值。
        /// </summary>
        /// <param name="value">字符串值内容</param>
        /// <param name="type">值的类型</param>
        /// <returns>对应类型的值</returns>
        public static object GetValue(String value, Type type)
        {
            if (type == null)
                return null;
            else if (type == typeof(string))
                return value;
            else if (type == typeof(String))
                return value;
            else if (type == typeof(Int32))
                return Convert.ToInt32(Convert.ToDouble(value));
            else if (type == typeof(float))
                return float.Parse(value);
            else if (type == typeof(byte))
                return Convert.ToByte(Convert.ToDouble(value));
            else if (type == typeof(sbyte))
                return Convert.ToSByte(Convert.ToDouble(value));
            else if (type == typeof(UInt32))
                return Convert.ToUInt32(Convert.ToDouble(value));
            else if (type == typeof(Int16))
                return Convert.ToInt16(Convert.ToDouble(value));
            else if (type == typeof(Int64))
                return Convert.ToInt64(Convert.ToDouble(value));
            else if (type == typeof(UInt16))
                return Convert.ToUInt16(Convert.ToDouble(value));
            else if (type == typeof(UInt64))
                return Convert.ToUInt64(Convert.ToDouble(value));
            else if (type == typeof(double))
                return double.Parse(value);
            else if (type == typeof(bool))
            {
                if (value == "0")
                    return false;
                else if (value == "1")
                    return true;
                else
                    return bool.Parse(value);
            }
            else if (type.BaseType == typeof(Enum))
            {
                return Enum.Parse(type, value);
            }
            else if (type == typeof(Vector2))
            {
                Vector2 result;
                ParseVector2(value, out result);
                return result;
            }
            else if (type == typeof(Vector3))
            {
                Vector3 result;
                ParseVector3(value, out result);
                return result;
            }
            else if (type == typeof(Quaternion))
            {
                Quaternion result;
                ParseQuaternion(value, out result);
                return result;
            }
            else if (type == typeof(Color))
            {
                Color result;
                ParseColor(value, out result);
                return result;
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                Type[] types = type.GetGenericArguments();
                var map = ParseMap(value);
                var result = type.GetConstructor(Type.EmptyTypes).Invoke(null);
                foreach (var item in map)
                {
                    var key = GetValue(item.Key, types[0]);
                    var v = GetValue(item.Value, types[1]);
                    type.GetMethod("Add").Invoke(result, new object[] { key, v });
                }
                return result;
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
                Type t = type.GetGenericArguments()[0];
                var list = ParseList(value);
                var result = type.GetConstructor(Type.EmptyTypes).Invoke(null);
                foreach (var item in list)
                {
                    var v = GetValue(item, t);
                    type.GetMethod("Add").Invoke(result, new object[] { v });
                }
                return result;
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Array))
            {
                Type t = type.GetGenericArguments()[0];
                var list = ParseArray(value);
                var result = type.GetConstructor(Type.EmptyTypes).Invoke(null);
                for (int i = 0; i < list.Length; i++)
                {
                    var v = GetValue(list[i], t);
                    type.GetMethod("SetValue").Invoke(result, new object[] { v, i });
                }
                return result;
            }
            else
            {
                return null;
            }


        }

        /// <summary>
        /// 将指定格式 (1.0, 2) 转换为 Vector2
        /// </summary>
        /// <param name="_inputString"></param>
        /// <param name="result"></param>
        /// <returns>返回 true/false 表示是否成功</returns>
        public static bool ParseVector2(string _inputString, out Vector2 result)
        {
            string trimString = _inputString.Trim();
            result = new Vector2();
            if (trimString.Length < 5)
            {
                return false;
            }
            try
            {
                string[] _detail = trimString.Substring(1, trimString.Length - 2).Split(DATA_NSPRITER);
                if (_detail.Length != 2)
                {
                    return false;
                }
                result.x = float.Parse(_detail[0]);
                result.y = float.Parse(_detail[1]);
                return true;
            }
            catch (Exception e)
            {
                LoggerHelper.Except(e);
                return false;
            }
        }


        /// <summary>
        /// 将指定格式 (1.0, 2, 3.4)转换为 Vector3 
        /// </summary>
        /// <param name="_inputString"></param>
        /// <param name="result"></param>
        /// <returns>返回 true/false 表示是否成功</returns>
        public static bool ParseVector3(string _inputString, out Vector3 result)
        {
            string trimString = _inputString.Trim();
            result = new Vector3();
            if (trimString.Length < 7)
            {
                return false;
            }
            try
            {
                string[] _detail = trimString.Substring(1, trimString.Length - 2).Split(DATA_NSPRITER);
                if (_detail.Length != 3)
                {
                    return false;
                }
                result.x = float.Parse(_detail[0]);
                result.y = float.Parse(_detail[1]);
                result.z = float.Parse(_detail[2]);
                return true;
            }
            catch (Exception e)
            {
                LoggerHelper.Except(e);
                return false;
            }
        }

        /// <summary>
        /// 将指定格式(1.0, 2, 3.4, 1.0) 转换为 Quaternion 
        /// </summary>
        /// <param name="_inputString"></param>
        /// <param name="result"></param>
        /// <returns>返回 true/false 表示是否成功</returns>
        public static bool ParseQuaternion(string _inputString, out Quaternion result)
        {
            string trimString = _inputString.Trim();
            result = new Quaternion();
            if (trimString.Length < 9)
            {
                return false;
            }
            try
            {
                string[] _detail = trimString.Substring(1, trimString.Length - 2).Split(DATA_NSPRITER);
                if (_detail.Length != 4)
                {
                    return false;
                }
                result.x = float.Parse(_detail[0]);
                result.y = float.Parse(_detail[1]);
                result.z = float.Parse(_detail[2]);
                result.w = float.Parse(_detail[3]);
                return true;
            }
            catch (Exception e)
            {
                LoggerHelper.Except(e);
                return false;
            }
        }

        /// <summary>
        /// 将指定格式(255, 255, 255, 255) 转换为 Color 
        /// </summary>
        /// <param name="_inputString"></param>
        /// <param name="result"></param>
        /// <returns>返回 true/false 表示是否成功</returns>
        public static bool ParseColor(string _inputString, out Color result)
        {
            string trimString = _inputString.Trim();
            result = Color.clear;
            if (trimString.Length < 9)
            {
                return false;
            }
            try
            {
                string[] _detail = trimString.Substring(1, trimString.Length - 2).Split(DATA_NSPRITER);
                if (_detail.Length != 4)
                {
                    return false;
                }
                result = new Color(float.Parse(_detail[0]) / 255, float.Parse(_detail[1]) / 255, float.Parse(_detail[2]) / 255, float.Parse(_detail[3]) / 255);
                return true;
            }
            catch (Exception e)
            {
                LoggerHelper.Except(e);
                return false;
            }
        }

        /// <summary>
        /// 将列表字符串转换为字符串的列表对象。
        /// </summary>
        /// <param name="strList">列表字符串</param>
        /// <param name="listSpriter">数组分隔符</param>
        /// <returns>列表对象</returns>
        public static List<String> ParseList(this String strList, Char listSpriter = DATA_FSPRITER)
        {
            var result = new List<String>();
            if (String.IsNullOrEmpty(strList))
                return result;

            var trimString = strList.Trim();
            if (String.IsNullOrEmpty(strList))
            {
                return result;
            }
            var detials = trimString.Split(listSpriter);
            foreach (var item in detials)
            {
                if (!String.IsNullOrEmpty(item))
                    result.Add(item.Trim());
            }
            return result;
        }

        /// <summary>
        /// 将列表字符串转换为字符串的列表对象。
        /// </summary>
        /// <param name="strList">列表字符串</param>
        /// <param name="listSpriter">数组分隔符</param>
        /// <returns>列表对象</returns>
        public static String[] ParseArray(this String strList, Char listSpriter = DATA_FSPRITER)
        {
            String[] result = null;
            if (String.IsNullOrEmpty(strList))
                return result;

            var trimString = strList.Trim();
            if (String.IsNullOrEmpty(strList))
            {
                return result;
            }
            string[] detials = trimString.Split(listSpriter);
            result = new string[detials.Length];
            for (int i = 0; i < detials.Length; i++)
            {
                if (!String.IsNullOrEmpty(detials[i]))
                    result[i] = detials[i].Trim();
            }
            return result;
        }

        /// <summary>
        /// xml自定义数据结构转换
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ParseMap(string Value)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            string[] Values = Value.Split(DATA_FSPRITER);
            for (int i = 0; i < Values.Length; i++)
            {
                string[] tmpvalue = Values[i].Split(KEY_VALUE_SPRITER);
                ret.Add(tmpvalue[0], tmpvalue[1]);
            }
            return ret;
        }
        #endregion

        #region TypeValue To String
        /// <summary>
        /// 将字符串转换为对应类型的值。
        /// </summary>
        /// <param name="value">字符串值内容</param>
        /// <param name="type">值的类型</param>
        /// <returns>对应类型的值</returns>
        public static String GetString(object value)
        {
            if (value == null)
                return null;
            else if (value is string || value is Int16 || value is UInt16 || value is Int32 || value is UInt32 || value is Int64 || value is UInt64 || value is float || value is byte || value is sbyte
                || value is double || value is bool || value is Enum)
                return value.ToString();
            else if (value is Vector2)
            {
                return Vector2ToString((Vector2)value);
            }
            else if (value is Vector3)
            {
                return Vector3ToString((Vector3)value);
            }
            else if (value is Quaternion)
            {
                return QuaternionToString((Quaternion)value);
            }
            else if (value is Color32)
            {
                return Color32ToString((Color32)value);
            }
            else if (value.GetType().IsGenericType && value.GetType().GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                return MapToString(value);
            }
            else if (value.GetType().IsGenericType && value.GetType().GetGenericTypeDefinition() == typeof(List<>))
            {
                return ListToString(value);
            }
            else
                return null;
        }


        /// <summary>
        /// 将指定格式(1.0, 2) 转换为 Vector2 
        /// </summary>
        /// <param name="_inputString"></param>
        /// <param name="result"></param>
        /// <returns>返回 true/false 表示是否成功</returns>
        public static String Vector2ToString(Vector2 result)
        {
            try
            {
                return result.x + "," + result.y;
            }
            catch (Exception e)
            {
                LoggerHelper.Except(e);
                return String.Empty;
            }
        }

        /// <summary>
        /// 将指定格式(1.0, 2, 3.4) 转换为 Vector3 
        /// </summary>
        /// <param name="_inputString"></param>
        /// <param name="result"></param>
        /// <returns>返回 true/false 表示是否成功</returns>
        public static String Vector3ToString(Vector3 result)
        {
            try
            {
                return result.x + "," + result.y + "," + result.z;
            }
            catch (Exception e)
            {
                LoggerHelper.Except(e);
                return String.Empty;
            }
        }

        /// <summary>
        /// 将指定格式(1.0, 2, 3.4, 1.0) 转换为 Quaternion 
        /// </summary>
        /// <param name="_inputString"></param>
        /// <param name="result"></param>
        /// <returns>返回 true/false 表示是否成功</returns>
        public static String QuaternionToString(Quaternion result)
        {
            try
            {
                return result.x + "," + result.y + "," + result.z + "," + result.w;
            }
            catch (Exception e)
            {
                LoggerHelper.Except(e);
                return String.Empty;
            }
        }

        /// <summary>
        /// 将指定格式(255, 255, 255, 255) 转换为 Color 
        /// </summary>
        /// <param name="_inputString"></param>
        /// <param name="result"></param>
        /// <returns>返回 true/false 表示是否成功</returns>
        public static String Color32ToString(Color32 result)
        {
            try
            {
                return result.r + "," + result.g + "," + result.b + "," + result.a;
            }
            catch (Exception e)
            {
                LoggerHelper.Except(e);
                return String.Empty;
            }
        }

        /// <summary>
        /// 将字典字符串转换为键类型与值类型都为字符串的字典对象。
        /// </summary>
        /// <param name="strMap">字典字符串</param>
        /// <param name="keyValueSpriter">键值分隔符</param>
        /// <param name="mapSpriter">字典项分隔符</param>
        /// <returns>字典对象</returns>
        public static String MapToString(object result)
        {
            String ret = "";
            IDictionary lt = result as IDictionary;
            foreach (var item in lt)
            {
                object key = item.GetType().GetProperty("Key").GetValue(item, null);
                object Value = item.GetType().GetProperty("Value").GetValue(item, null);
                ret += GetString(key) + KEY_VALUE_SPRITER + GetString(Value) + DATA_FSPRITER;
            }
            String newString = ret.Substring(0, ret.Length - 1);
            return newString;
        }

        /// <summary>
        /// 将列表字符串转换为字符串的列表对象。
        /// </summary>
        /// <param name="strList">列表字符串</param>
        /// <param name="listSpriter">数组分隔符</param>
        /// <returns>列表对象</returns>
        public static String ListToString(object result)
        {
            String ret = "";
            if (result == null)
                return ret;
            IEnumerable lt = result as IEnumerable;
            foreach (object item in lt)
            {
                ret += GetString(item) + DATA_FSPRITER;
            }
            if (ret.Length > 0)
                ret = ret.Substring(0, ret.Length - 1);
            return ret;
        }
        #endregion

        #endregion

        #region byte数组数据转换
        /// <summary>
        /// 结构体转byte数组
        /// </summary>
        /// <param name="structure"></param>
        /// <returns></returns>
        public static Byte[] StructToBytes(object structure)
        {
            //得到结构体的大小
            int size = Marshal.SizeOf(structure);
            //创建byte数组
            byte[] bytes = new byte[size];
            //分配结构体大小的内存空间
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            //将结构体拷到分配好的内存空间
            Marshal.StructureToPtr(structure, structPtr, false);
            //从内存空间拷到byte数组
            Marshal.Copy(structPtr, bytes, 0, size);
            //释放内存空间
            Marshal.FreeHGlobal(structPtr);
            //返回byte数组
            return bytes;
        }

        /// <summary>
        /// 数组转结构体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bytes"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static T ByteaToStruct<T>(byte[] bytes) where T : struct
        {
            T type = new T();
            int size = Marshal.SizeOf(type);
            if (size > bytes.Length)
            {
                throw new ArgumentOutOfRangeException("byte转结构体异常 Struct =" + typeof(T).Name + "  Data = " + bytes.Length);
            }
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            Marshal.Copy(bytes, 0, structPtr, size);
            object obj = Marshal.PtrToStructure(structPtr, type.GetType());
            Marshal.FreeHGlobal(structPtr);
            return (T)obj;
        }
        #endregion
    }
}
