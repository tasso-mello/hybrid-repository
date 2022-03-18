namespace api.hybrid.repository.Core.Injections
{
    using data.hybrid.repository.Repository;
    using Microsoft.Extensions.DependencyInjection;
    public static class Repository
	{
		public static IServiceCollection AddRepositoryInjectionScoped(this IServiceCollection services)
		{
			services.Scan(action => action.FromAssembliesOf(typeof(ReferenceClass)).AddClasses().AsImplementedInterfaces().WithScopedLifetime());

			return services;
		}
	}
}
