using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NoiseMapGenerator.Helpers
{
    public class SaveXML
    {
        public static void Save(object obj, string fileName)
        {
            var sr = new XmlSerializer(obj.GetType());
            var writer = new StreamWriter(fileName);
            sr.Serialize(writer, obj);
            writer.Close();
        }

    }
}
