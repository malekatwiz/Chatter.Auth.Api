using Chatter.Auth.MongoIdentity.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Chatter.Auth.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddTransient<IRepository, MongoRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IRoleRepository, RoleRepository>();
        }
    }
}
