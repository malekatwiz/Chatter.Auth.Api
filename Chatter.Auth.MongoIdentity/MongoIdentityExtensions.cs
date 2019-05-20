using Chatter.Auth.MongoIdentity.Entities;
using Chatter.Auth.MongoIdentity.Options;
using Chatter.Auth.MongoIdentity.Repository;
using Chatter.Auth.MongoIdentity.Stores;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

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

        public static void RegisterIdentityServer(this IServiceCollection services)
        {
            services.AddIdentityServer(options =>
            {
                options.UserInteraction.LoginUrl = "/account/login";
                options.UserInteraction.LogoutUrl = "/account/logout";
            })
            .AddDeveloperSigningCredential()
            .AddInMemoryIdentityResources(GetIdentityResources())
            .AddInMemoryApiResources(GetApiResources())
            .AddInMemoryClients(GetClients())
            .AddAspNetIdentity<ApplicationUser>();
        }

        private static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        private static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("Chatter.Api", "Chatter Api")
            };
        }

        private static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "Chatter.Api",
                    ClientName = "Chatter Api",
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "Chatter.Api"
                    },
                    RequireConsent = false,
                    RedirectUris = {"https://localhost:44391/signin-oidc"},
                    PostLogoutRedirectUris = { "https://localhost:44391/signout-callback-oidc" }
                },
                new Client
                {
                    ClientId = "Chatter.UI",
                    ClientSecrets = {new Secret("Secret".Sha256())},
                    ClientName = "Chatter UI",
                    AllowedGrantTypes = GrantTypes.ImplicitAndClientCredentials,
                    RequireConsent = false,
                    RedirectUris = { "https://localhost:44391/signin-oidc" },
                    PostLogoutRedirectUris = { "https://localhost:44391/signout-callback-oidc" },
                    AllowAccessTokensViaBrowser = true,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "Chatter.Api"
                    }
                }
            };
        }
    }
}
