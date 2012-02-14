using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using Nancy.Hosting.Self;
using Ninject;
using Burro;
using Burro.Util;

namespace Muro
{
    class Program : ServiceBase
    {
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
                Console.ReadKey();
                service.OnStop();
            }
            else
            {
                ServiceBase.Run(service);
            }
        }

        private static IKernel _kernel;
        private NancyHost _host;
        private IMuroCore _core;

        public static IKernel Kernel
        {
            get
            {
                return _kernel;
            }

            set { _kernel = value; }
        }

        public Program()
        {
            this.ServiceName = "Muro";
        }

        protected override void OnStart(string[] args)
        {
            base.OnStart(args);

            ConfigureBindings();

            _host = new NancyHost(new Uri("http://localhost:12345"));
            _host.Start();

            _core = _kernel.Get<IMuroCore>();

            try
            {
                if (args.Any())
                {
                    _core.Initialise(args[0]);
                }
                else
                {
                    _core.Initialise();
                }
            }
            catch (Exception)
            {
                _core.Shutdown();
                throw;
            }
        }

        private void ConfigureBindings()
        {
            _kernel = new StandardKernel();

            _kernel.Bind<ITimer>().ToConstant(new TimersTimer(new TimeSpan(0, 0, 5)));

            _kernel.Bind<IBurroCore>().To<BurroCore>();

            _kernel.Bind<IMuroCore>().ToConstant(_kernel.Get<MuroCore>());
        }

        protected override void OnStop()
        {
            base.OnStop();

            _host.Stop();
        }
    }
}
