using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Burro;
using Muro.Models;
using Nancy;
using Ninject;
using Ninject.Parameters;

namespace Muro
{
    public class MuroModule : NancyModule
    {
        public MuroModule(IKernel kernel, IMuroCore core)
        {
            Get["/"] = parameters =>
                           {
                               var pipelines = core.PipelineReports.Values;
                               var pipelineVM = kernel.Get<PipelineReportViewModel>(new ConstructorArgument("pipelines", pipelines));
                               return View["index", pipelineVM];
                           };
        }
    }
}
