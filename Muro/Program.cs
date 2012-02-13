using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using Ninject;
using Burro;
using Burro.Util;
using System.Threading;
using System.IO;

namespace Muro
{
    class Program : ServiceBase
    {
        private IKernel _kernel;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            var service = new Program();

            if (Environment.UserInteractive)
            {
                service.OnStart(args);
                Console.WriteLine("Press any key to stop program");
                Console.Read();
                service.OnStop();
            }
            else
            {
                ServiceBase.Run(service);
            }
        }

        public Program()
        {
            this.ServiceName = "Muro";
        }

        protected override void OnStart(string[] args)
        {
            base.OnStart(args);

            ConfigureBindings();

//            _core = _kernel.Get<LucesCore>();
//
//            try
//            {
//                if (args.Any())
//                {
//                    _core.Initialise(args[0]);
//                }
//                else
//                {
//                    _core.Initialise();
//                }
//            }
//            catch (Exception)
//            {
//                _core.Shutdown();
//                throw;
//            }
        }

        private void ConfigureBindings()
        {
            _kernel = new StandardKernel();

            _kernel.Bind<ITimer>().ToConstant(new TimersTimer(new TimeSpan(0, 0, 5)));

            _kernel.Bind<IBurroCore>().To<BurroCore>();
        }

        protected override void OnStop()
        {
            base.OnStop();

//            _core.Shutdown();
        }
    }
}
