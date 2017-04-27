using Autofac;
using Autofac.Extras.CommonServiceLocator;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandamonium.Serverless
{
    public class ServiceLocator
    {
        public static List<Action<ContainerBuilder>> Builders { get; set; }

        public static Lazy<IServiceLocator> Instance = new Lazy<IServiceLocator>(() =>
        {
            var builder = new ContainerBuilder();
            
            Builders.ForEach(action => action(builder));

            var container = builder.Build();
            var serviceLocator = new AutofacServiceLocator(container);

            Microsoft.Practices.ServiceLocation.ServiceLocator.SetLocatorProvider(() => serviceLocator);

            return Microsoft.Practices.ServiceLocation.ServiceLocator.Current;
        });
    }
}
