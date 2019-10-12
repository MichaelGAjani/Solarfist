using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Jund.NETHelper.ConvertHelper
{
    /// <summary>
    /// 序列化帮助类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SerializeHelper<T> where T : new()
    {
        #region XML序列化
        /// <summary>
        /// 把对象序列化为xml文档
        /// </summary>
        /// <param name="t">对象</param>
        /// <param name="outputfile">输出文档</param>
        public static void XmlSerialized(T t, string outputfile)
        {
            FileStream fs =  new FileStream(outputfile, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(fs, t);
            fs.Close();
        }
        /// <summary>
        /// 反序列化xml
        /// </summary>
        /// <param name="xmlFile">xml文档</param>
        /// <returns></returns>
        public static object XmlDeSerialized(string xmlFile)
        {
            FileStream fs = new FileStream(xmlFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            if (fs != null) fs.Close();
            return serializer.Deserialize(fs);            
        }
        /// <summary>
        /// 把对象序列化为xml字符串
        /// </summary>
        /// <param name="t">对象</param>
        /// <returns>xml字符串</returns>
        public static  string XmlSerialized(T t)
        {
            XmlSerializer serializer = new XmlSerializer(t.GetType());
            StringBuilder builder = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(builder))
            {
                serializer.Serialize(writer, t);
                return builder.ToString();
            }
        }
        /// <summary>
        /// 把xml反序列化成对象
        /// </summary>
        /// <param name="xml">xml字符串</param>
        /// <returns></returns>
        public T XmlDeSerializedString(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (XmlReader reader = new XmlTextReader(new StringReader(xml)))
            {
                return (T)serializer.Deserialize(reader);
            }
        }
        #endregion

        #region SoapFormatter序列化
        public string SoapSerialized(T item)
        {
            SoapFormatter formatter = new SoapFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                formatter.Serialize(ms, item);
                ms.Position = 0;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(ms);
                return xmlDoc.InnerXml;
            }
        }

        public T SoapDeSerialized(string str)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(str);
            SoapFormatter formatter = new SoapFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                xmlDoc.Save(ms);
                ms.Position = 0;
                return (T)formatter.Deserialize(ms);
            }
        }
        #endregion

        #region BinaryFormatter序列化
        public string BinarySerialized(T t)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                formatter.Serialize(ms, t);
                ms.Position = 0;
                byte[] bytes = ms.ToArray();
                StringBuilder builder = new StringBuilder();
                foreach (byte bt in bytes)
                {
                    builder.Append(string.Format("{0:X2}", bt));
                }
                return builder.ToString();
            }
        }

        public T BinaryDeSerialized(string str)
        {
            int intLen = str.Length / 2;
            byte[] bytes = new byte[intLen];
            for (int i = 0; i < intLen; i++)
            {
                int ibyte = Convert.ToInt32(str.Substring(i * 2, 2), 16);
                bytes[i] = (byte)ibyte;
            }
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                return (T)formatter.Deserialize(ms);
            }
        }
        #endregion

        #region Json序列化
        public string ConvertObjectToJsonString(Object obj)=>Newtonsoft.Json.JsonConvert.SerializeObject(obj);

        public T ConvertJsonStringToObject(string str)=> Newtonsoft.Json.JsonConvert.DeserializeObject<T>(str);
        #endregion
    }
}
