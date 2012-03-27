using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Burro;
using Ninject;
using Ninject.Parameters;

namespace Muro.Models
{
    public class PipelinesViewModel
    {
        private readonly IKernel _kernel;

        public PipelinesViewModel(IKernel kernel, IEnumerable<PipelineReport> pipelines)
        {
            _kernel = kernel;
            Pipelines = pipelines.Select(p => _kernel.Get<PipelineReportViewModel>(new ConstructorArgument("pipeline", p)));
        }

        public IEnumerable<PipelineReportViewModel> Pipelines { get; set; }

        public IDictionary<string, int> Dimensions
        {
            get
            {
                var columns = 1;
                if (Pipelines.Count() >= 4)
                {
                    columns = 2;
                }
                if (Pipelines.Count() >= 10)
                {
                    columns = 3;
                }
                if (Pipelines.Count() >= 21)
                {
                    columns = 4;
                }

                var rows = (int)Math.Ceiling(Pipelines.Count() / (double)columns);

                var dimensions = new Dictionary<string, int>();
                dimensions["Columns"] = columns;
                dimensions["Rows"] = rows;

                return dimensions;
            }
        }
    }
}
