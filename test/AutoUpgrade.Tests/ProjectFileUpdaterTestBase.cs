using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace AutoUpgrade.Tests
{
    public class ProjectFileUpdaterTestBase
    {
        protected static XmlDocument ConvertToXmlDoc(string xml)
        {
            var updatedXmlDoc = new XmlDocument();
            updatedXmlDoc.LoadXml(xml);
            return updatedXmlDoc;
        }

        protected static Assembly GetExecutingAssembly => Assembly.GetExecutingAssembly();
        
        protected static string ReadResourceFile(string resourcesFileName)
        {
            var assembly = GetExecutingAssembly;
            string sampleDocument;
            var resourceName = ResourceFiles(assembly).Find(t => t.EndsWith(resourcesFileName));
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                sampleDocument = reader.ReadToEnd();
            }

            return sampleDocument;
        }

        protected static List<string> ResourceFiles(Assembly assembly)
        {
            return assembly
                .GetManifestResourceNames()
                .ToList();
        }

        protected string ReadFileFromFileSystem(FileInfo fileInfo, MockFileSystem mockFileSystem)
        {
            string content;
            using (var streamReader = mockFileSystem.File.OpenText(fileInfo.FullName))
            {
                content = streamReader.ReadToEnd();
            }

            return content;
        }
    }
}