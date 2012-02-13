using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using Burro;
using Burro.BuildServers;
using Moq;
using NUnit.Framework;
using Ninject;

namespace Muro.Tests
{
    [TestFixture]
    public class CoreTest
    {
        private IKernel _kernel;
        private Mock<IBurroCore> _burro;

        private PipelineReport SUCCESSFUL_IDLE_PIPELINE = new PipelineReport() { Name = "A", BuildState = BuildState.Success, Activity = Activity.Idle };
        private PipelineReport FAILED_IDLE_PIPELINE = new PipelineReport() { Name = "B", BuildState = BuildState.Failure, Activity = Activity.Idle };
        private PipelineReport SUCCESSFUL_BUILDING_PIPELINE = new PipelineReport() { Name = "C", BuildState = BuildState.Success, Activity = Activity.Busy };
        private PipelineReport FAILED_BUILDING_PIPELINE = new PipelineReport() { Name = "D", BuildState = BuildState.Failure, Activity = Activity.Busy };

        private const string DEFAULT_CONFIG_FILE = "./muro.yml";

        [SetUp]
        public void Setup()
        {
            _kernel = new StandardKernel();
            _burro = new Mock<IBurroCore>();
            _kernel.Bind<IBurroCore>().ToConstant(_burro.Object);

            // make sure there is always a config file for testing
            if (!File.Exists(DEFAULT_CONFIG_FILE))
            {
                File.Copy("Config/mock.yml", DEFAULT_CONFIG_FILE, true);
            }


        }

        [Test]
        public void CreatesConfigFileEditableByAllUsersIfNotPresent()
        {
            if (File.Exists(DEFAULT_CONFIG_FILE))
            {
                File.Delete(DEFAULT_CONFIG_FILE);
            }

            var core = _kernel.Get<MuroCore>();
            try
            {
                core.Initialise();
                Assert.Fail("This should have thrown an exception to force it to close");
            }
            catch (FileLoadException e) { }

            Assert.IsTrue(File.Exists(DEFAULT_CONFIG_FILE));

            var fileSecurity = File.GetAccessControl(DEFAULT_CONFIG_FILE);
            var accessRules = fileSecurity.GetAccessRules(true, false, typeof(System.Security.Principal.NTAccount)).Cast<FileSystemAccessRule>();
            accessRules.First(r => r.AccessControlType == AccessControlType.Allow &&
                              (r.FileSystemRights & FileSystemRights.Write) == FileSystemRights.Write &&
                              r.IdentityReference.Value == "BUILTIN\\Users");

            File.Delete(DEFAULT_CONFIG_FILE);
        }

        [Test]
        public void InitialisationStartsParserWithDefaultConfig()
        {
            var core = _kernel.Get<MuroCore>();
            core.Initialise();
            _burro.Verify(b => b.Initialise(DEFAULT_CONFIG_FILE), Times.Once());
        }

        [Test]
        public void InitialisationAllowsConfigPassedIn()
        {
            var core = _kernel.Get<MuroCore>();
            core.Initialise("test2.yml");
            _burro.Verify(b => b.Initialise("test2.yml"), Times.Once());
        }

        [Test]
        public void InitialiseStartsMonitoring()
        {
            var core = _kernel.Get<MuroCore>();

            core.Initialise();

            _burro.Verify(b => b.StartMonitoring(), Times.Once());
        }

        [Test]
        public void ReportsMergedAndExposedOnCore()
        {
            var core = _kernel.Get<MuroCore>();

            var bs1 = new Mock<IBuildServer>();
            var bs2 = new Mock<IBuildServer>();
            var buildServers = new List<Mock<IBuildServer>>() { bs1, bs2 };

            _burro.Setup(b => b.BuildServers).Returns(new List<IBuildServer>(buildServers.Select(bs => bs.Object)));

            core.Initialise();

            bs1.Raise(b => b.PipelinesUpdated += null, new List<PipelineReport> {SUCCESSFUL_IDLE_PIPELINE});
            Assert.AreEqual(1, core.PipelineReports.Count());
            bs2.Raise(b => b.PipelinesUpdated += null, new List<PipelineReport> { FAILED_IDLE_PIPELINE });
            Assert.AreEqual(2, core.PipelineReports.Count());
            var nowFailed = new PipelineReport()
                                {
                                    Activity = FAILED_IDLE_PIPELINE.Activity,
                                    BuildState = FAILED_IDLE_PIPELINE.BuildState,
                                    Name = SUCCESSFUL_IDLE_PIPELINE.Name
                                };
            bs1.Raise(b => b.PipelinesUpdated += null, new List<PipelineReport> {nowFailed});
            Assert.AreEqual(2, core.PipelineReports.Count());
            Assert.IsTrue(core.PipelineReports.All(pr => pr.Value.BuildState == BuildState.Failure));
        }
    }
}
