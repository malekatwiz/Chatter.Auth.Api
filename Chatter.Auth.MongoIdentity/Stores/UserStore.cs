using Chatter.Auth.MongoIdentity.Entities;
using Chatter.Auth.MongoIdentity.Repository;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Chatter.Auth.MongoIdentity.Stores
{
    public class UserStore<TUser, TRole> : 
        IUserStore<TUser>,
        IUserPasswordStore<TUser>,
        IUserRoleStore<TUser>

        where TUser : ApplicationUser
        where TRole : IdentityRole
    {
        private readonly IUserRepository _userRepository;

        public UserStore(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task AddToRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
        {
            if (user.Roles == null)
            {
                user.Roles = new List<string>();
            }

            user.Roles.Add(roleName);
            return UpdateAsync(user, cancellationToken);
        }

        public async Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken)
        {
            await _userRepository.Create(user);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken)
        {
            await _userRepository.Delete(user);
            return IdentityResult.Success;
        }

        public void Dispose()
        {
        }

        public async Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
           return (TUser)await _userRepository.FindById(userId);
        }

        public async Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            return (TUser)await _userRepository.FindByUsername(normalizedUserName);
        }

        public Task<string> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetPasswordHashAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user?.PasswordHash);
        }

        public async Task<IList<string>> GetRolesAsync(TUser user, CancellationToken cancellationToken)
        {
            var u = await FindByIdAsync(user.Id, cancellationToken);
            return u?.Roles;
        }

        public Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user?.Id);
        }

        public Task<string> GetUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user?.UserName);
        }

        public async Task<IList<TUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            return (IList<TUser>)(await _userRepository.FindUsersInRole(roleName)).ToList();
        }

        public async Task<bool> HasPasswordAsync(TUser user, CancellationToken cancellationToken)
        {
            var u = await _userRepository.FindById(user.Id);
            if (u != null)
            {
                return !string.IsNullOrEmpty(u.PasswordHash);
            }
            return false;
        }

        public async Task<bool> IsInRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
        {
            var u = await FindByIdAsync(user.Id, cancellationToken);
            return u?.Roles.Contains(roleName) ?? false;
        }

        public Task RemoveFromRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
        {
            user.Roles.Remove(roleName);
            return UpdateAsync(user, cancellationToken);
        }

        public Task SetNormalizedUserNameAsync(TUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return _userRepository.Update(user);
        }

        public Task SetPasswordHashAsync(TUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return _userRepository.Update(user);
        }

        public Task SetUserNameAsync(TUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return _userRepository.Update(user);
        }

        public async Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken)
        {
            await _userRepository.Update(user);
            return IdentityResult.Success;
        }
    }
}
