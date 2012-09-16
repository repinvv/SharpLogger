using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using SharpOptions;

namespace SharpLogger
{
   internal static class LogConfig
   {
      private static IContainer container = ConfigureContainer();

      static IContainer ConfigureContainer()
      {
         var builder = new ContainerBuilder();

         builder
            .RegisterType<CheckingLogger>()
            .As<InternalLogger>();

         builder
            .Register<Sender>((c,p) => 
               new Sender(c.Resolve<LogCollector>().Send));

         builder
            .RegisterType<LoggerContainer>()
            .SingleInstance();

         builder.
            RegisterType<LogCollector>()
            .SingleInstance();

         builder
            .RegisterType<PoolThreadStarter>()
            .As<IThreadStarter>();

         builder
            .RegisterGeneric(typeof(LockQueued<>))
            .As(typeof(IQueued<>));

         builder
            .Register<IOptions>((c, p) =>
               new LogDefaultOptions(new Options("Log")).GetOptions())
            .SingleInstance();            

         builder.Register<ILogWriter>((c, p) =>
            new LogWriterFactory(c.Resolve<IOptions>())
            .GetWriter());

         return builder.Build();         
      }

      public static IContainer Container
      {
         get
         {
            return container;
         }
      }

      public static IOptions Options
      {
         get
         {
            return container
               .Resolve<IOptions>();
         }
      }
   }
}
