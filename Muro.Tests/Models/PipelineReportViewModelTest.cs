using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Burro;
using Muro.Models;
using NUnit.Framework;

namespace Muro.Tests.Models
{
    [TestFixture]
    public class PipelineReportViewModelTest
    {
        private PipelineReport _pipeline;

        [SetUp]
        public void Setup()
        {
            _pipeline = new PipelineReport() {LastBuildTime = DateTime.Now, Name = "Ricky Bobby", Activity = Activity.Idle, BuildState = BuildState.Failure};
        }

        [Test]
        public void ShouldSeeNameAsPerConfig()
        {
            var pipelineReportVM = new PipelineReportViewModel(_pipeline);

            Assert.AreEqual("Ricky Bobby", pipelineReportVM.Name);
        }

        [Test]
        public void ShouldSeeRawActivity()
        {
            var pipelineReportVM = new PipelineReportViewModel(_pipeline);

            Assert.AreEqual("Idle", pipelineReportVM.Activity);
        }

        [Test]
        public void ShouldSeeRawState()
        {
            var pipelineReportVM = new PipelineReportViewModel(_pipeline);

            Assert.AreEqual("Failure", pipelineReportVM.BuildState);
        }

        [Test]
        public void LastBuildTimeUnder60Seconds()
        {
            _pipeline.LastBuildTime = DateTime.Now.AddSeconds(-50);
            var pipelineReportVM = new PipelineReportViewModel(_pipeline);

            Assert.AreEqual("under a minute ago", pipelineReportVM.LastBuildTime);
        }

        [Test]
        public void LastBuildTimeUnder120Seconds()
        {
            _pipeline.LastBuildTime = DateTime.Now.AddSeconds(-110);
            var pipelineReportVM = new PipelineReportViewModel(_pipeline);

            Assert.AreEqual("a minute ago", pipelineReportVM.LastBuildTime);
        }

        [Test]
        public void LastBuildTimeUnderAnHourToNearest5()
        {
            _pipeline.LastBuildTime = DateTime.Now.AddMinutes(-46);
            var pipelineReportVM = new PipelineReportViewModel(_pipeline);

            Assert.AreEqual("about 45 minutes ago", pipelineReportVM.LastBuildTime);
        }

        [Test]
        public void LastBuildTimeUnder2Hours()
        {
            _pipeline.LastBuildTime = DateTime.Now.AddMinutes(-75);
            var pipelineReportVM = new PipelineReportViewModel(_pipeline);

            Assert.AreEqual("about an hour ago", pipelineReportVM.LastBuildTime);
        }

        [Test]
        public void LastBuildTimeUnder24Hours()
        {
            _pipeline.LastBuildTime = DateTime.Now.AddHours(-23);
            var pipelineReportVM = new PipelineReportViewModel(_pipeline);

            Assert.AreEqual("about 23 hours ago", pipelineReportVM.LastBuildTime);
        }

        [Test]
        public void LastBuildTimeOver24Hours()
        {
            _pipeline.LastBuildTime = DateTime.Now.AddHours(-100);
            var pipelineReportVM = new PipelineReportViewModel(_pipeline);

            Assert.AreEqual("over 4 days ago", pipelineReportVM.LastBuildTime);
        } 
    }
}
