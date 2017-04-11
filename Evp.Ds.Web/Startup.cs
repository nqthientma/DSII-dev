using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Evp.Ds.Core.Services;
using Evp.Ds.Data.Core;
using Evp.Ds.Data.Repository;
using Evp.Ds.Web;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace Evp.Ds.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            var builder = new ContainerBuilder();

            // STANDARD MVC SETUP:

            // Register your MVC controllers.
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterGeneric(typeof(EFRepository<>))
                .As(typeof(IRepository<>))
                .InstancePerRequest();

            builder.RegisterType<EvpContext>()
                .As<IDbContext>()
                .InstancePerRequest();

            //Service Registration to IoC
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            builder.RegisterAssemblyTypes(assemblies)
                .AssignableTo<IService>()
                .AsImplementedInterfaces()
                .InstancePerRequest();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            // Set the dependency resolver for Web API.
            var webApiResolver = new AutofacWebApiDependencyResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver = webApiResolver;

            // OWIN MVC SETUP:
            // Register the Autofac middleware FIRST, then the Autofac MVC middleware.
            app.UseAutofacMiddleware(container);
            app.UseAutofacMvc();
            app.UseAutofacWebApi(config);
        }
    }
}