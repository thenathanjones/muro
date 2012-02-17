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
    public class PipelineViewModelTest
    {
        [Test]
        public void CalculatesDimensionsBasedOnNumberPipelines1()
        {
            var pipelineVM = GenerateViewModel(1);

            Assert.AreEqual(1, pipelineVM.Dimensions["Rows"]);
            Assert.AreEqual(1, pipelineVM.Dimensions["Columns"]);
        }

        [Test]
        public void CalculatesDimensionsBasedOnNumberPipelines2()
        {
            var pipelineVM = GenerateViewModel(2);

            Assert.AreEqual(2, pipelineVM.Dimensions["Rows"]);
            Assert.AreEqual(1, pipelineVM.Dimensions["Columns"]);
        }

        [Test]
        public void CalculatesDimensionsBasedOnNumberPipelines3()
        {
            var pipelineVM = GenerateViewModel(3);

            Assert.AreEqual(3, pipelineVM.Dimensions["Rows"]);
            Assert.AreEqual(1, pipelineVM.Dimensions["Columns"]);
        }

        [Test]
        public void CalculatesDimensionsBasedOnNumberPipelines5()
        {
            var pipelineVM = GenerateViewModel(5);

            Assert.AreEqual(3, pipelineVM.Dimensions["Rows"]);
            Assert.AreEqual(2, pipelineVM.Dimensions["Columns"]);
        }

        [Test]
        public void CalculatesDimensionsBasedOnNumberPipelines7()
        {
            var pipelineVM = GenerateViewModel(7);

            Assert.AreEqual(4, pipelineVM.Dimensions["Rows"]);
            Assert.AreEqual(2, pipelineVM.Dimensions["Columns"]);
        }

        private PipelinesViewModel GenerateViewModel(int numberOfPipelines)
        {
            var pipelines = new int[numberOfPipelines].Select(s => new Mock<PipelineReport>().Object).ToArray();

            return new PipelinesViewModel(pipelines);
        }
    }
}
