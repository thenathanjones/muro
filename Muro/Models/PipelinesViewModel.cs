using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Burro;

namespace Muro.Models
{
    public class PipelinesViewModel
    {
        public PipelinesViewModel(IEnumerable<PipelineReport> pipelines)
        {
            Pipelines = pipelines.Select(p => new PipelineReportViewModel(p));
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
