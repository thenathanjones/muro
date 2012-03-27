using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.AccessControl;
using Burro;
using Burro.BuildServers;

namespace Muro
{
    public interface IMuroCore
    {
        IDictionary<string, PipelineReport> PipelineReports { get; }
        void Initialise(string s);
        void Initialise();
        void Shutdown();
    }

    public class MuroCore : IMuroCore
    {
        private readonly IBurroCore _parser;

        public MuroCore(IBurroCore parser)
        {
            _parser = parser;
            PipelineReports = new Dictionary<string, PipelineReport>();
        }

        #region IMuroCore Members

        public IDictionary<string, PipelineReport> PipelineReports { get; private set; }

        public void Initialise()
        {
            Assembly assembly = Assembly.GetEntryAssembly();
            string baseDir = ".";

            if (assembly != null)
            {
                baseDir = Path.GetDirectoryName(assembly.Location);
            }
            string configPath = baseDir + "/muro.yml";

            EnsureConfigExists(configPath);

            Initialise(configPath);
        }

        public void Initialise(string configFile)
        {
            InitialiseParser(configFile);

            RegisterForUpdates();
        }

        public void Shutdown()
        {
        }

        #endregion

        private void EnsureConfigExists(string configPath)
        {
            if (!File.Exists(configPath))
            {
                Assembly resourceAssembly = Assembly.GetExecutingAssembly();
                Stream defaultConfig = resourceAssembly.GetManifestResourceStream("Muro.Config.muro.yml");
                WriteStreamToFile(defaultConfig, configPath);

                GiveWriteAccessToUsers(configPath);

                throw new FileLoadException("No config file found.  Put default at " + configPath);
            }
        }

        private void GiveWriteAccessToUsers(string configPath)
        {
            FileSecurity fileSecurity = File.GetAccessControl(configPath);
            fileSecurity.AddAccessRule(new FileSystemAccessRule("BUILTIN\\Users", FileSystemRights.Write,
                                                                AccessControlType.Allow));
            File.SetAccessControl(configPath, fileSecurity);
        }

        private void WriteStreamToFile(Stream stream, string fileName)
        {
            var outputFile = new FileStream(fileName, FileMode.Create);

            try
            {
                int length = 256;
                var buffer = new Byte[length];

                int bytesRead = stream.Read(buffer, 0, length);
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

        private void InitialiseParser(string configFile)
        {
            _parser.Initialise(configFile);
        }

        private void RegisterForUpdates()
        {
            foreach (IBuildServer buildServer in _parser.BuildServers)
            {
                buildServer.PipelinesUpdated += HandlePipelineUpdate;
            }

            _parser.StartMonitoring();
        }

        private void HandlePipelineUpdate(IEnumerable<PipelineReport> update)
        {
            foreach (PipelineReport pipelineReport in update)
            {
                PipelineReports[pipelineReport.Name] = pipelineReport;
            }
        }
    }
}