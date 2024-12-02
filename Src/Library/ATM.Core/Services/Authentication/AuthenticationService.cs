using System.Globalization;
using System.Security.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using ATM.Core.Domain.Users;
using ATM.Core.Domain;
using ATM.Core.Results;
using System.Threading;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ATM.Core.Services.Authentication
{
    /// <summary>
    /// Authentication and authorization management service.
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        // dependencies
        private readonly IHttpContextAccessor _httpContextAccessor;

        private AuthenticatedUser? _cachedUser;

        public AuthenticationService(
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        /// <summary>
        /// Sign in a user.
        /// </summary>
        /// <param name="user">User to authenticate.</param>user
        /// <param name="rememberMe">Value whether to persist the cookie.</param>user
        /// <param name="cancellationToken">Cancellation token.</param>
        public virtual async Task<Result> SignInAsync(
            User user,
            bool rememberMe,
            CancellationToken cancellationToken = default)
        {
            // everything went well, create claims
            var claims = BuildClaims(user);


            //create principal for the current authentication scheme
            var userIdentity = new ClaimsIdentity(claims, AuthenticationSettings.AuthenticationScheme);
            var userPrincipal = new ClaimsPrincipal(userIdentity);

            //set value indicating whether session is persisted and the time at which the authentication was issued
            var authenticationProperties = new AuthenticationProperties
            {
                IsPersistent = rememberMe,
                IssuedUtc = DateTime.UtcNow
            };

            //sign in
            await _httpContextAccessor.HttpContext.SignInAsync(AuthenticationSettings.AuthenticationScheme, userPrincipal, authenticationProperties);

            return Result.Ok();
        }

        /// <summary>
        /// Sign out.
        /// </summary>
        public virtual async Task SignOutAsync()
        {
            //reset cached user
            _cachedUser = null;

            //and sign out from the current authentication scheme
            await _httpContextAccessor.HttpContext.SignOutAsync(AuthenticationSettings.AuthenticationScheme);
        }

        /// <summary>
        /// Gets currently authenticated user.
        /// </summary>
        public virtual async Task<AuthenticatedUser?> GetAuthenticatedUserAsync()
        {
            //whether there is a cached user
            if (_cachedUser != null)
                return _cachedUser;

            //try to get authenticated user identity
            var authenticatedUser = await GetAuthenticatedUserFromClaimsAsync();

            _cachedUser = authenticatedUser;

            return _cachedUser;
        }

        /// <summary>
        /// Gets currently authenticated user.
        /// </summary>
        public virtual async Task<AuthenticatedUser> GetAuthenticatedUserOrGuestAsync()
        {
            //whether there is a cached user
            if (_cachedUser != null)
                return _cachedUser;

            //try to get authenticated user identity
            var authenticatedUser = await GetAuthenticatedUserFromClaimsAsync() ?? new AuthenticatedUser
            {
                Id = Seeds.SUsers.Users.Guest.Id,
                Name = Seeds.SUsers.Users.Guest.Name,
                Username = Seeds.SUsers.Users.Guest.Username,
                Role = Seeds.SUsers.Users.Guest.Role,
           
            };

            _cachedUser = authenticatedUser;

            return _cachedUser;
        }

        #region Helpers

        private static List<Claim> BuildClaims(User user)
        {
            var claims = new List<Claim>();

            var idClaim = new Claim(ClaimTypes.Sid, user.Id.ToString(), ClaimValueTypes.String, AuthenticationSettings.ClaimsIssuer);
            var nameClaim = new Claim(ClaimTypes.Name, user.Name, ClaimValueTypes.String, AuthenticationSettings.ClaimsIssuer);
            //var emailClaim = new Claim(ClaimTypes.Email, user.Username, ClaimValueTypes.String, AuthenticationSettings.ClaimsIssuer);
            var usernameClaim = new Claim(ClaimTypes.NameIdentifier, user.Username, ClaimValueTypes.String,
                AuthenticationSettings.ClaimsIssuer);
            var dateLoggedInClaim = new Claim(ClaimTypes.AuthenticationInstant,
                DateTime.UtcNow.ToString(CultureInfo.InvariantCulture), ClaimValueTypes.DateTime,
                AuthenticationSettings.ClaimsIssuer);


            var roleClaim = new Claim(ClaimTypes.Role, user.RoleString, ClaimValueTypes.String,
                AuthenticationSettings.ClaimsIssuer);

            claims.Add(idClaim);
            claims.Add(nameClaim);
            //claims.Add(emailClaim);
            claims.Add(usernameClaim);
            claims.Add(dateLoggedInClaim);

            claims.Add(roleClaim);
            return claims;
        }

        private async Task<AuthenticatedUser?> GetAuthenticatedUserFromClaimsAsync()
        {
            var authenticateResult =
                await _httpContextAccessor.HttpContext.AuthenticateAsync(AuthenticationSettings.AuthenticationScheme);
            if (!authenticateResult.Succeeded)
                return null;
            //throw new AuthenticationException("No user is logged in.");

            // get claims
            var idClaim = authenticateResult.Principal.FindFirst(claim =>
                claim.Type == ClaimTypes.Sid &&
                claim.Issuer.Equals(AuthenticationSettings.ClaimsIssuer, StringComparison.InvariantCultureIgnoreCase));

            var nameClaim = authenticateResult.Principal.FindFirst(claim =>
                claim.Type == ClaimTypes.Name &&
                claim.Issuer.Equals(AuthenticationSettings.ClaimsIssuer, StringComparison.InvariantCultureIgnoreCase));

            var usernameClaim = authenticateResult.Principal.FindFirst(claim =>
                claim.Type == ClaimTypes.NameIdentifier &&
                claim.Issuer.Equals(AuthenticationSettings.ClaimsIssuer, StringComparison.InvariantCultureIgnoreCase));

            var roleClaim = authenticateResult.Principal.FindFirst(claim =>
                claim.Type == ClaimTypes.Role &&
                claim.Issuer.Equals(AuthenticationSettings.ClaimsIssuer, StringComparison.InvariantCultureIgnoreCase));

      

            // means user is found
            if (idClaim == null)
                return null;
            //throw new AuthenticationException("No user is logged in. (Claims not found)");

            var authenticatedUser = new AuthenticatedUser
            {
                Id = Guid.Parse(idClaim.Value),
                Name = nameClaim.Value,
                Username = usernameClaim.Value
            };



            authenticatedUser.Role = roleClaim.Value switch
            {
                "Administrator" => UserRole.Administrator,
                "User" => UserRole.User,            
                _ => throw new AuthenticationException("Role not found.")
            };

            return authenticatedUser;
        }

        #endregion
    }
}