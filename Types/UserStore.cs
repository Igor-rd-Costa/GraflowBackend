



using GraflowBackend.Context;
using Microsoft.AspNetCore.Identity;

public class UserStore(GraflowContext context) : IUserStore<User>, IUserPasswordStore<User>, IUserEmailStore<User>
{
    private readonly GraflowContext m_Context = context;

    Task<IdentityResult> IUserStore<User>.CreateAsync(User user, CancellationToken cancellationToken)
    {
        return Task.Factory.StartNew(() =>
        {
            if (m_Context.Users.Any(u => u.NormalizedEmail == user.NormalizedEmail || u.NormalizedUsername == user.NormalizedUsername))
            {
                return IdentityResult.Failed();
            }
            var status = m_Context.Users.Add(user);
            if (status.State != Microsoft.EntityFrameworkCore.EntityState.Added)
            {
                return IdentityResult.Failed();
            }
            m_Context.SaveChanges();
            return IdentityResult.Success;
        }, cancellationToken);
    }

    Task<IdentityResult> IUserStore<User>.DeleteAsync(User user, CancellationToken cancellationToken)
    {
        return Task.Factory.StartNew(() =>
        {
            var status = m_Context.Users.Remove(user);
            if (status.State != Microsoft.EntityFrameworkCore.EntityState.Deleted)
            {
                return IdentityResult.Failed();
            }
            m_Context.SaveChanges();
            return IdentityResult.Success;
        }, cancellationToken);
    }

    void IDisposable.Dispose() {}

    public void Dispose() {}

    Task<User?> IUserEmailStore<User>.FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
    {
        return Task.Factory.StartNew(() =>
        {
            User? user = m_Context.Users.Where(u => u.NormalizedEmail == normalizedEmail).FirstOrDefault();
            return user;
        }, cancellationToken);
    }

    Task<User?> IUserStore<User>.FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        return Task.Factory.StartNew(() =>
        {
            Guid id = Guid.Empty;
            if (!Guid.TryParse(userId, out id))
            {
                return null;
            }
            User? user = m_Context.Users.FirstOrDefault(u => u.Id == id);
            return user;
        }, cancellationToken);
    }

    Task<User?> IUserStore<User>.FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        return Task.Factory.StartNew(() =>
        {
            User? user = m_Context.Users.FirstOrDefault(u => u.NormalizedUsername == normalizedUserName);
            return user;
        }, cancellationToken);
    }

    Task<string?> IUserEmailStore<User>.GetEmailAsync(User user, CancellationToken cancellationToken)
    {
        return Task.FromResult<string?>(user.Email);
    }

    Task<bool> IUserEmailStore<User>.GetEmailConfirmedAsync(User user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.IsEmailConfirmed);
    }

    Task<string?> IUserEmailStore<User>.GetNormalizedEmailAsync(User user, CancellationToken cancellationToken)
    {
        return Task.FromResult<string?>(user.NormalizedEmail);
    }

    Task<string?> IUserStore<User>.GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
    {
        return Task.FromResult<string?>(user.NormalizedUsername);
    }

    Task<string?> IUserPasswordStore<User>.GetPasswordHashAsync(User user, CancellationToken cancellationToken)
    {
        return Task.FromResult<string?>(user.Password);
    }

    Task<string> IUserStore<User>.GetUserIdAsync(User user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.Id.ToString());
    }

    Task<string?> IUserStore<User>.GetUserNameAsync(User user, CancellationToken cancellationToken)
    {
        return Task.FromResult<string?>(user.Username);
    }

    Task<bool> IUserPasswordStore<User>.HasPasswordAsync(User user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.Password != string.Empty);
    }

    Task IUserEmailStore<User>.SetEmailAsync(User user, string? email, CancellationToken cancellationToken)
    {
        return Task.Factory.StartNew(() =>
        {
            if (email == null)
            {
                return;
            }
            user.Email = email;
        }, cancellationToken);
    }

    Task IUserEmailStore<User>.SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken)
    {
        return Task.Factory.StartNew(() =>
        {
            user.IsEmailConfirmed = confirmed;
        }, cancellationToken);
    }

    Task IUserEmailStore<User>.SetNormalizedEmailAsync(User user, string? normalizedEmail, CancellationToken cancellationToken)
    {
        return Task.Factory.StartNew(() =>
        {
            if (normalizedEmail == null) 
            {
                return; 
            }
            user.NormalizedEmail = normalizedEmail;
        }, cancellationToken);
    }

    Task IUserStore<User>.SetNormalizedUserNameAsync(User user, string? normalizedName, CancellationToken cancellationToken)
    {
        return Task.Factory.StartNew(() =>
        {
            if (normalizedName == null)
            {
                return;
            }
            user.NormalizedUsername = normalizedName;
        }, cancellationToken);
    }

    Task IUserPasswordStore<User>.SetPasswordHashAsync(User user, string? passwordHash, CancellationToken cancellationToken)
    {
        return Task.Factory.StartNew(() =>
        {
            if (passwordHash == null)
            {
                return;
            }
            user.Password = passwordHash;
        }, cancellationToken);
    }

    Task IUserStore<User>.SetUserNameAsync(User user, string? userName, CancellationToken cancellationToken)
    {
        return Task.Factory.StartNew(() =>
        {
            if (userName == null)
            {
                return;
            }
            user.Username = userName;
        }, cancellationToken);
    }

    Task<IdentityResult> IUserStore<User>.UpdateAsync(User user, CancellationToken cancellationToken)
    {
        return Task.Factory.StartNew(() =>
        {
            var status = m_Context.Update(user);
            if (status.State != Microsoft.EntityFrameworkCore.EntityState.Modified)
            {
                return IdentityResult.Failed();
            }
            m_Context.SaveChanges();
            return IdentityResult.Success;
        }, cancellationToken);
    }
}