using Chatter.Auth.MongoIdentity.Entities;
using Chatter.Auth.MongoIdentity.Options;
using Chatter.Auth.MongoIdentity.Repository;
using Chatter.Auth.MongoIdentity.Stores;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Chatter.Auth.MongoIdentity
{
    public static class MongoIdentityExtensions
    {
        public static IdentityBuilder RegisterMongoIdentity<TUser, TRole>(this IServiceCollection services,
            Action<IdentityOptions> setupIdentityOptions, Action<MongoIdentityOptions> setupMongoOptions)
            where TUser : ApplicationUser
            where TRole : IdentityRole
        {
            var dbOptions = new MongoIdentityOptions();
            setupMongoOptions(dbOptions);

            var builder = services.AddIdentity<TUser, TRole>(setupIdentityOptions ?? (x => { }));

            builder.AddUserStore<UserStore<TUser, TRole>>()
                .AddRoleStore<RoleStore<TRole>>()
                .AddDefaultTokenProviders();

            var userRepository = new UserRepository(dbOptions.ConnectionString, dbOptions.DbName);
            var roleRepository = new RoleRepository(dbOptions.ConnectionString, dbOptions.DbName);

            services.AddTransient<IUserStore<TUser>>(x => new UserStore<TUser, TRole>(userRepository));
            services.AddTransient<IRoleStore<TRole>>(x => new RoleStore<TRole>(roleRepository));

            return builder;
        }
    }
}
