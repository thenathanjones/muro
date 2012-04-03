using System;
using Muro.Models;

namespace Muro
{
    public class RealTimeSource : ITimeSource
    {
        public DateTime Now
        {
            get { return DateTime.Now; }
        }
    }
}