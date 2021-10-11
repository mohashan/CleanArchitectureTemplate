using Application;
using Application.Common.Interfaces;
using Application.Common.ServiceLifetimes;
using Autofac;
using Domain;
using Infrastructure.Services;

namespace Infrastructure.Configurations
{
    public static class AutofacConfigurationExtensions
    {
        public static void AddServices(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
            

            var domainAssembly = typeof(IDomainAssembly).Assembly;
            var applicationAssembly = typeof(IApplicationAssembly).Assembly;
            var infrastructureAssembly = typeof(IInfrastructureAssembly).Assembly;

            containerBuilder.RegisterAssemblyTypes(domainAssembly, applicationAssembly, infrastructureAssembly)
                .AssignableTo<ISingletonDependency>()
                .AsImplementedInterfaces()
                .SingleInstance();

            containerBuilder.RegisterAssemblyTypes(domainAssembly, applicationAssembly, infrastructureAssembly)
                .AssignableTo<ITransientDependency>()
                .AsImplementedInterfaces()
                .InstancePerDependency();

            containerBuilder.RegisterAssemblyTypes(domainAssembly, applicationAssembly, infrastructureAssembly)
                .AssignableTo<IScopedDependency>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}