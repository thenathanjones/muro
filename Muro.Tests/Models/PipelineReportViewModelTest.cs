using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Burro;
using Moq;
using Muro.Models;
using NUnit.Framework;

namespace Muro.Tests.Models
{
    [TestFixture]
    public class PipelineReportViewModelTest
    {
        private PipelineReport _pipeline;
        private Mock<ITimeSource> _timeSource;
        private DateTime _referenceTime;

        [SetUp]
        public void Setup()
        {
            _pipeline = new PipelineReport() {LastBuildTime = DateTime.Now, Name = "Ricky Bobby", Activity = Activity.Idle, BuildState = BuildState.Failure};
            _timeSource = new Mock<ITimeSource>();
            _referenceTime = new DateTime(2011, 3, 27, 15, 2, 0, 0);
            _timeSource.Setup(t => t.Now).Returns(_referenceTime);
        }

        [Test]
        public void ShouldSeeNameAsPerConfig()
        {
            var pipelineReportVM = new PipelineReportViewModel(_pipeline, _timeSource.Object);

            Assert.AreEqual("Ricky Bobby", pipelineReportVM.Name);
        }

        [Test]
        public void ShouldSeeRawActivity()
        {
            var pipelineReportVM = new PipelineReportViewModel(_pipeline, _timeSource.Object);

            Assert.AreEqual("Idle", pipelineReportVM.Activity);
        }

        [Test]
        public void ShouldSeeRawState()
        {
            var pipelineReportVM = new PipelineReportViewModel(_pipeline, _timeSource.Object);

            Assert.AreEqual("Failure", pipelineReportVM.BuildState);
        }

        [Test]
        public void LastBuildTimeUnder60Seconds()
        {
            _pipeline.LastBuildTime = _referenceTime.AddSeconds(-59);
            var pipelineReportVM = new PipelineReportViewModel(_pipeline, _timeSource.Object);

            Assert.AreEqual("under a minute ago", pipelineReportVM.LastBuildTime);
        }

        [Test]
        public void LastBuildTimeUnder120Seconds()
        {
            _pipeline.LastBuildTime = _referenceTime.AddSeconds(-119);
            var pipelineReportVM = new PipelineReportViewModel(_pipeline, _timeSource.Object);

            Assert.AreEqual("a minute ago", pipelineReportVM.LastBuildTime);
        }

        [Test]
        public void LastBuildTimeUnderAnHourToNearest5()
        {
            _pipeline.LastBuildTime = _referenceTime.AddMinutes(-47);
            var pipelineReportVM = new PipelineReportViewModel(_pipeline, _timeSource.Object);

            Assert.AreEqual("about 45 minutes ago", pipelineReportVM.LastBuildTime);
        }

        [Test]
        public void LastBuildTimeUnder2Hours()
        {
            _pipeline.LastBuildTime = _referenceTime.AddMinutes(-119);
            var pipelineReportVM = new PipelineReportViewModel(_pipeline, _timeSource.Object);

            Assert.AreEqual("about an hour ago", pipelineReportVM.LastBuildTime);
        }

        [Test]
        public void LastBuildTimeUnder24Hours()
        {
            _pipeline.LastBuildTime = _referenceTime.AddHours(-23.9);
            var pipelineReportVM = new PipelineReportViewModel(_pipeline, _timeSource.Object);

            Assert.AreEqual("about 23 hours ago", pipelineReportVM.LastBuildTime);
        }

        [Test]
        public void LastBuildTimeOver24Hours()
        {
            _pipeline.LastBuildTime = _referenceTime.AddHours(-96.1);
            var pipelineReportVM = new PipelineReportViewModel(_pipeline, _timeSource.Object);

            Assert.AreEqual("over 4 day(s) ago", pipelineReportVM.LastBuildTime);
        } 
    }
}
