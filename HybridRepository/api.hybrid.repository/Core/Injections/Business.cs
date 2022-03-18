namespace api.hybrid.repository.Core.Injections
{
    using core.hybrid.repository.Contracts.Business;
    using core.hybrid.repository.Implementation;
    using Microsoft.Extensions.DependencyInjection;
    public static class Business
	{
		public static IServiceCollection AddBusinessInjectionScoped(this IServiceCollection services)
		{
            services.Scan(action => action.FromAssembliesOf(typeof(ReferenceClass)).AddClasses().AsImplementedInterfaces().WithScopedLifetime());

            return services;
		}
	}
}
