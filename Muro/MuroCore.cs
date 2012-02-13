using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using Burro;
using Ninject;

namespace Muro
{
    public class MuroCore
    {
        private IKernel _kernel;
        private IBurroCore _parser;

        public MuroCore(IKernel kernel, IBurroCore parser)
        {
            _kernel = kernel;
            _parser = parser;
        }

        public void Initialise()
        {
            var assembly = System.Reflection.Assembly.GetEntryAssembly();
            var baseDir = ".";

            if (assembly != null)
            {
                baseDir = System.IO.Path.GetDirectoryName(assembly.Location);
            }
            var configPath = baseDir + "/muro.yml";

            EnsureConfigExists(configPath);

            Initialise(configPath);
        }

        private void EnsureConfigExists(string configPath)
        {
            if (!File.Exists(configPath))
            {
                var resourceAssembly = Assembly.GetExecutingAssembly();
                var defaultConfig = resourceAssembly.GetManifestResourceStream("Muro.Config.muro.yml");
                WriteStreamToFile(defaultConfig, configPath);

                GiveWriteAccessToUsers(configPath);

                throw new FileLoadException("No config file found.  Put default at " + configPath);
            }
        }

        private void GiveWriteAccessToUsers(string configPath)
        {
            var fileSecurity = File.GetAccessControl(configPath);
            fileSecurity.AddAccessRule(new FileSystemAccessRule("BUILTIN\\Users", FileSystemRights.Write, AccessControlType.Allow));
            File.SetAccessControl(configPath, fileSecurity);
        }

        private void WriteStreamToFile(Stream stream, string fileName)
        {
            var outputFile = new FileStream(fileName, FileMode.Create);

            try
            {
                var length = 256;
                var buffer = new Byte[length];

                var bytesRead = stream.Read(buffer, 0, length);
                while (bytesRead > 0)
                {
                    outputFile.Write(buffer, 0, bytesRead);
                    bytesRead = stream.Read(buffer, 0, length);
                }
            }
            finally
            {
                stream.Close();
                outputFile.Close();
            }
        }

        public void Initialise(string configFile)
        {
            InitialiseParser(configFile);

            RegisterForUpdates();
        }

        private void InitialiseParser(string configFile)
        {
            _parser.Initialise(configFile);
        }

        private void RegisterForUpdates()
        {
            foreach (var buildServer in _parser.BuildServers)
            {
                buildServer.PipelinesUpdated += HandlePipelineUpdate;
            }

            _parser.StartMonitoring();
        }

        private void HandlePipelineUpdate(IEnumerable<PipelineReport> update)
        {
           
        }
    }
}
