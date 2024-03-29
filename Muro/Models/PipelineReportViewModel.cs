﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using Burro;

namespace Muro.Models
{
    public class PipelineReportViewModel
    {
        private readonly PipelineReport _pipelineReport;
        private readonly ITimeSource _timeSource;

        public PipelineReportViewModel(PipelineReport pipelineReport, ITimeSource timeSource)
        {
            _pipelineReport = pipelineReport;
            _timeSource = timeSource;
        }

        public string LastBuildTime
        {
            get
            {
                var delta = (_timeSource.Now - _pipelineReport.LastBuildTime);

                if (delta.TotalSeconds < 60)
                {
                    return "under a minute ago";
                }
                if (delta.TotalSeconds < 120)
                {
                    return "a minute ago";
                }
                if (delta.TotalMinutes < 60)
                {
                    var minutes = (int)(delta.TotalMinutes - (delta.TotalMinutes % 5));
                    return "about " + minutes + " minutes ago";
                }
                if (delta.TotalHours < 2)
                {
                    return "about an hour ago";
                }
                if (delta.TotalHours < 24)
                {
                    return "about " + (int)delta.TotalHours + " hours ago";
                }
                return "over " + (int)delta.TotalDays + " day(s) ago";
            }
        }

        public string Name
        {
            get { return _pipelineReport.Name; }
        }

        public string Activity
        {
            get { return _pipelineReport.Activity.ToString(); }
        }

        public string BuildState
        {
            get { return _pipelineReport.BuildState.ToString(); }
        }
    }
}
