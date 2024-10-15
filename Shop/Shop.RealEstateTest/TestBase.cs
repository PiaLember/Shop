using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shop.ApplicationServices.Services;
using Shop.Core.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.RealEstateTest
{
    public abstract class TestBase
    {
        protected IServiceProvider serviceProvider { get; set; }
        protected TestBase()
        {
            var services = new ServiceCollection();
            SetupServices(services);
            serviceProvider = services.BuildServiceProvider();
        }

        protected T Svc<T>()
        {
            return serviceProvider.GetService<T>();
        }

        public virtual void SetupServices(IServiceCollection services)
        {
           
            
        }
    }
}
