using AutoMapper;
using LightInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TestWS.Managers;
using TestWS.Services;

namespace TestWS
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var container = new ServiceContainer();
            container.RegisterControllers();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            InitAutomapperProfiles(container);

            //container.Register<ITicketService, JsonTicketService>(new PerRequestLifeTime());
            container.Register<ITicketService, SqlTicketService>(new PerRequestLifeTime());
            container.Register<ICacheManager, CacheManager>(new PerRequestLifeTime());
            container.EnableMvc();
        }

        private static void InitAutomapperProfiles(ServiceContainer container)
        {
            var assembly = Assembly.GetCallingAssembly();
            var definedTypes = assembly.DefinedTypes;

            var profiles = definedTypes.Where(type => typeof(Profile).GetTypeInfo().IsAssignableFrom(type) && !type.IsAbstract).ToArray();

            void ConfigAction(IMapperConfigurationExpression cfg)
            {
                foreach (var profile in profiles.Select(t => t.AsType()))
                {
                    cfg.AddProfile(profile);
                    //cfg.CreateMap();
                }
            }

            //Mapper.Initialize(ConfigAction);
            //MapperConfiguration config = (MapperConfiguration)Mapper.Configuration;

            var config = new MapperConfiguration(ConfigAction);

            var mapper = config.CreateMapper();
            config.AssertConfigurationIsValid();

            container.Register(sp => config.CreateMapper(), new PerRequestLifeTime());
            
        }
    }
}
