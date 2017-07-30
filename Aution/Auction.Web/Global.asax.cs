using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Auction.Core.DAL;
using Auction.Core.DAL.Repositories;
using Auction.Core.DAL.Repositories.Abstractions;
using Auction.Core.Services;
using Auction.Core.Services.Abstractions;
using Auction.Web.Controllers;
using Microsoft.Practices.Unity;

namespace Auction.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Bootstrapper.Initialise();
        }
    }

    public class Bootstrapper
    {
        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();
            container.RegisterType<AuctionController, AuctionController>();
            container.RegisterType<ILotRepository, LotRepository>();
            container.RegisterType<IAuctionService, AuctionService>();
            container.RegisterType<IDateTimeRepository, DateTimeRepository>();
            return container;
        }

        public static void Initialise()
        {
            var container = BuildUnityContainer();


            IDependencyResolver resolver = DependencyResolver.Current;

            IDependencyResolver newResolver = new UnityDependencyResolver(container, resolver);

            DependencyResolver.SetResolver(newResolver);
        }
    }
    public class UnityDependencyResolver : IDependencyResolver
    {
        private IUnityContainer container;
        private IDependencyResolver resolver;


        public UnityDependencyResolver(IUnityContainer container, IDependencyResolver resolver)
        {
            this.container = container;
            this.resolver = resolver;
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return this.container.Resolve(serviceType);
            }
            catch
            {
                return this.resolver.GetService(serviceType);
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return this.container.ResolveAll(serviceType);
            }
            catch
            {
                return this.resolver.GetServices(serviceType);
            }
        }
    }
}