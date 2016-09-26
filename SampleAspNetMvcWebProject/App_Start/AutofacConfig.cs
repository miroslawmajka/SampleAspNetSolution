using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using SampleAspNetMvcWebProject.Abstract;
using System.Reflection;

namespace SampleAspNetMvcWebProject
{
    public class AutofacConfig
    {
        public static IContainer RegisterDependencies()
        {
            var builder = new ContainerBuilder();
            
            // MVC controllers
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // WebAPI controllers
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            var types = Assembly.GetExecutingAssembly().GetTypes();

            builder.RegisterTypes(types)
                .AsClosedTypesOf(typeof(IQueryHandler<,>))
                .InstancePerRequest();

            // Register Model Binders
            //builder.RegisterModelBinders(Assembly.GetExecutingAssembly());
            //builder.RegisterModelBinderProvider();

            return builder.Build();
        }
    }
}