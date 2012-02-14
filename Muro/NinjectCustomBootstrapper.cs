using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy.Bootstrappers.Ninject;

namespace Muro
{
    public class NinjectCustomBootstrapper : NinjectNancyBootstrapper
    {
        protected override Ninject.IKernel GetApplicationContainer()
        {
            return Program.Kernel;
        }
    }
}
