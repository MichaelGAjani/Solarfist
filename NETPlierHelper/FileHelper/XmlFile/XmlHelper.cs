using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Jund.NETHelper.FileHelper.XmlFile
{
    public class XmlHelper
    {
        public static List<XmlNodeObject> AccessXml(string xml)
        {
            List<XmlNodeObject> node_list = new List<XmlNodeObject>();
            XmlReader reader = XmlReader.Create(xml);
            reader.Settings.CheckCharacters = true;
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.CDATA:
                        node_list.Add(new XmlNodeObject(reader.Depth, XmlNodeType.CDATA, reader.Name, reader.Value));
                        break;
                    case XmlNodeType.Comment:
                        node_list.Add(new XmlNodeObject(reader.Depth, XmlNodeType.Comment, reader.Name, reader.Value));
                        break;
                    case XmlNodeType.DocumentType:
                        node_list.Add(new XmlNodeObject(reader.Depth, XmlNodeType.DocumentType, reader.Name, reader.Value));
                        break;
                    case XmlNodeType.Element:
                        XmlNodeObject node = new XmlNodeObject(reader.Depth, XmlNodeType.Element, reader.Name, reader.Value);
                        while (reader.MoveToNextAttribute())
                        {
                            node.Child_list.Add(new XmlNodeObject(reader.Depth, XmlNodeType.Attribute, reader.Name, reader.Value));
                        }
                        node_list.Add(node);
                        break;
                    case XmlNodeType.EndElement:
                        //level--;
                        break;
                    case XmlNodeType.EntityReference:
                        node_list.Add(new XmlNodeObject(reader.Depth, XmlNodeType.EntityReference, reader.Name, reader.Value));
                        break;
                    case XmlNodeType.ProcessingInstruction:
                        node_list.Add(new XmlNodeObject(reader.Depth, XmlNodeType.ProcessingInstruction, reader.Name, reader.Value));
                        break;
                    case XmlNodeType.Text:
                        node_list.Add(new XmlNodeObject(reader.Depth, XmlNodeType.Text, reader.Name, reader.Value));
                        break;
                    case XmlNodeType.XmlDeclaration:
                        node_list.Add(new XmlNodeObject(reader.Depth, XmlNodeType.XmlDeclaration, reader.Name, reader.Value));
                        break;
                }
            }

            return node_list;
        }

        public static List<string> QueryXml(string xml_file,string xml_element,string xml_attribute,string attribute_value,string node_value)
        {
            List<string> result = new List<string>();
            XDocument xDoc = XDocument.Load(xml_file);
            // set up the query looking for the married female participants
            // who were witnesses
            var query = from p in xDoc.Root.Elements(xml_element)
                        where p.Attribute(xml_attribute).Value == attribute_value
                        &&p.Value.Contains(node_value)
                        orderby p.Value
                        select p.Value;
            foreach (string s in query)
            {
                result.Add(s);
            }

            return result;
        }
        public static void AddElement(string xmlFile,string key,string value)
        {
            XDocument xDoc = XDocument.Load(xmlFile);
            xDoc.Add(new XElement(key, value));
            xDoc.Save(xmlFile);
        }
        public static void AddElement(string xmlFile, string key, List<string> value)
        {
            XDocument xDoc = XDocument.Load(xmlFile);
            xDoc.Add(new XElement(key, value));
            xDoc.Save(xmlFile);
        }
        public static void AddAttribute(string xmlFile, string nodeName, string attributeName,string value)
        {
            XDocument xDoc = XDocument.Load(xmlFile);
            xDoc.Element(nodeName).Add(new XAttribute(attributeName, value));
            xDoc.Save(xmlFile);
        }
        public static void RemoveElement(string xmlFile,string name)
        {
            XDocument xDoc = XDocument.Load(xmlFile);
            xDoc.Element(name).Remove();
            xDoc.Save(xmlFile);
        }
        public static void RemoveAttribute(string xmlFile, string nodeName, string AttributeName)
        {
            XDocument xDoc = XDocument.Load(xmlFile);
            xDoc.Element(nodeName).Attribute(AttributeName).Remove();
            xDoc.Save(xmlFile);
        }
        public static void ReplaceElement(string xmlFile, string name,string newName)
        {
            XDocument xDoc = XDocument.Load(xmlFile);
            xDoc.Element(name).ReplaceNodes(newName);
            xDoc.Save(xmlFile);
        }
        public static void ReplaceAttribute(string xmlFile, string NodeName, string AttributeName,string newAttribute)
        {
            XDocument xDoc = XDocument.Load(xmlFile);
            xDoc.Element(NodeName).Attribute(AttributeName).Value=newAttribute;
            xDoc.Save(xmlFile);
        }
    }
}
