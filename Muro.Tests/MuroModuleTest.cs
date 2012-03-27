using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Burro;
using Moq;
using NUnit.Framework;
using Ninject;

namespace Muro.Tests
{
    [TestFixture]
    public class MuroModuleTest
    {
        [Test]
        public void GetDisplaysPipelineReportsFromCore()
        {
            var core = new Mock<IMuroCore>();
            core.SetupGet(c => c.PipelineReports).Returns(new Dictionary<string, PipelineReport>()
                                                              {{"A", new PipelineReport()}});

            var muroModule = new MuroModule(new Mock<IKernel>().Object, core.Object);
            
        }
    }

}
