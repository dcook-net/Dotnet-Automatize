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
        protected string SampleProjFileXmlContent;
        protected string InvalidProjFileXmlContent;
        protected XmlDocument ExpectedProjFile;

        public ProjectFileUpdaterTestBase()
        {
            SampleProjFileXmlContent = ReadResourceFile("sampleProjFile.xml");
            InvalidProjFileXmlContent = ReadResourceFile("InvalidXmlFile.xml");
            ExpectedProjFile = ConvertToXmlDoc(ReadResourceFile("ExpectedProjFile.xml"));

            MockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"C:\dev\sampleProjFile.xml", new MockFileData(SampleProjFileXmlContent) },
                { @"C:\dev\InvalidXmlFile.xml", new MockFileData(InvalidProjFileXmlContent) }
            });
        }

        protected static XmlDocument ConvertToXmlDoc(string xml)
        {
            var updatedXmlDoc = new XmlDocument();
            updatedXmlDoc.LoadXml(xml);
            return updatedXmlDoc;
        }

        protected static Assembly GetExecutingAssembly => Assembly.GetExecutingAssembly();

        public MockFileSystem MockFileSystem { get; set; }

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

        protected string ReadFileFromFileSystem(FileInfo fileInfo)
        {
            string content;
            using (var streamReader = MockFileSystem.File.OpenText(fileInfo.FullName))
            {
                content = streamReader.ReadToEnd();
            }

            return content;
        }
    }
}