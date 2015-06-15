using System;
using System.IO;
using System.Xml.Serialization;

namespace Hawk
{
    public static class Serializer
    {
        public static string Serialize<T>(this T obj)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (StringWriter textWriter = new StringWriter())
            {            
                xmlSerializer.Serialize(textWriter, obj);
                return textWriter.ToString();                
            }
        }

        public static Object Deserialize<T>(string msg)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (StringReader textReader = new StringReader(msg))
            {
                return xmlSerializer.Deserialize(textReader);
            }
        }
    }
}
