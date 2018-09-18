using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using AutoUpgrade.VersionUpdaters;
using McMaster.Extensions.CommandLineUtils;

namespace AutoUpgrade
{
    public class FileUpdater
    {
//        private readonly IFileSystem _fileSystem;
        private readonly IConsole _console;

        public FileUpdater(IConsole console)
        {
            _console = console;
        }

        //TODO: Lots of duplication in this file that could be cleaned up

        public void UpdateProjectFiles(IEnumerable<FileInfo> projectFiles, IDotNetVersionUpdater dotNetVersionUpdater)
        {
            if (projectFiles == null || !projectFiles.Any())
            {
                _console.WriteLine("No Project files found!");
                return;
            }

            foreach (var fileInfo in projectFiles)
            {
//                _console.WriteLine($"Updating {fileInfo.FullName}");
                try
                {
                    string xmlContent;
                    using (var streamReader = File.OpenText(fileInfo.FullName))
                    {
                        xmlContent = streamReader.ReadToEnd();
                    }

                    var updatedXml = dotNetVersionUpdater.UpdateProjectFileContents(xmlContent);

                    File.WriteAllText(fileInfo.FullName, updatedXml);
                }
                catch (XmlException e)
                {
                    _console.WriteLine($"Updating {fileInfo.FullName} failed: File is not valid Xml");
                }
                catch (Exception ex)
                {
                    _console.WriteLine($"Updating {fileInfo.FullName} failed:{ex.Message}");
                }
            }
        }

        public void UpdateDockerFiles(IEnumerable<FileInfo> dockerFiles, IDotNetVersionUpdater dotNetVersionUpdater)
        {
            if (dockerFiles == null || !dockerFiles.Any())
            {
                _console.WriteLine("No Docker files found!");
                return;
            }

            foreach (var fileInfo in dockerFiles)
            {
                //                _console.WriteLine($"Updating {fileInfo.FullName}");
                try
                {
                    string content;
                    using (var streamReader = File.OpenText(fileInfo.FullName))
                    {
                        content = streamReader.ReadToEnd();
                    }

                    var updatedContent = dotNetVersionUpdater.UpdateDockerFileContent(content);

                    File.WriteAllText(fileInfo.FullName, updatedContent);
                }
                catch (Exception e)
                {
                    _console.WriteLine($"Updating {fileInfo.FullName} failed: {e.Message}");
                }
            }
        }

        public void UpdateEnvironmentFiles(IEnumerable<FileInfo> envFiles, Version2Point1Updater dotNetVersionUpdater)
        {
            if (envFiles == null || !envFiles.Any())
            {
                _console.WriteLine("No .env files found!");
                return;
            }

            foreach (var fileInfo in envFiles)
            {
                //                _console.WriteLine($"Updating {fileInfo.FullName}");
                try
                {
                    string content;
                    using (var streamReader = File.OpenText(fileInfo.FullName))
                    {
                        content = streamReader.ReadToEnd();
                    }

                    var updatedContent = dotNetVersionUpdater.UpdateEnvFileContent(content);

                    File.WriteAllText(fileInfo.FullName, updatedContent);
                }
                catch (Exception e)
                {
                    _console.WriteLine($"Updating {fileInfo.FullName} failed: {e.Message}");
                }
            }
        }
    }
}