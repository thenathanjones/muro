using System;

namespace Muro.Models
{
    public interface ITimeSource
    {
        DateTime Now { get; }
    }
}