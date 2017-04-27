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
    public class ServerlessContext
    {
        public static List<Action<ContainerBuilder>> Builders { get; set; }
            = new List<Action<ContainerBuilder>>();

        /// <summary>
        /// The <see cref="LocatorFactory"/> ultimately describes the behavior of the context by providing
        /// a hook for configuring the dependency pipeline.
        /// 
        /// A sane set of defaults is provided along with ways to extend or augment the default 
        /// behavior as well as the ability to mock it all together for the purpose of unit testing
        /// Functions
        /// </summary>
        public static Func<IServiceLocator> LocatorFactory { get; set; } = () =>
        {
            var builder = new ContainerBuilder();

            Builders.ForEach(action => action(builder));

            var container = builder.Build();
            var serviceLocator = new AutofacServiceLocator(container);

            Microsoft.Practices.ServiceLocation.ServiceLocator.SetLocatorProvider(() => serviceLocator);

            return Microsoft.Practices.ServiceLocation.ServiceLocator.Current;
        };

        /// <summary>
        /// The single instance of the ServiceLocator for this <see cref="ServerlessContext"/>.
        /// Once the final state of the <see cref="LocatorFactory"/> has been been resolved by
        /// invoking it, this Instance becomes immutable.
        /// </summary>
        public static Lazy<IServiceLocator> Instance = new Lazy<IServiceLocator>(() =>
        {
            return LocatorFactory();
        });
    }
}
