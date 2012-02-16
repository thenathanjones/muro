using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Burro;
using Muro.Models;
using Nancy;

namespace Muro
{
    public class MuroModule : NancyModule
    {
        public MuroModule(IMuroCore core)
        {
            Get["/"] = parameters =>
                           {
                               var pipelines = core.PipelineReports.Values;
                               var pipelineVM = new PipelineViewModel {Pipelines = pipelines};
                               return View["index", pipelineVM];
                           };
        }
    }
}
