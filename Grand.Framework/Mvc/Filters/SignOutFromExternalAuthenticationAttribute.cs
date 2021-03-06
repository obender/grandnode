﻿using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Grand.Services.Authentication;
using Microsoft.AspNetCore.Authentication;

namespace Grand.Framework.Mvc.Filters
{
    /// <summary>
    /// Represents filter attribute that sign out from the external authentication scheme
    /// </summary>
    public class SignOutFromExternalAuthenticationAttribute : TypeFilterAttribute
    {
        /// <summary>
        /// Create instance of the filter attribute
        /// </summary>
        public SignOutFromExternalAuthenticationAttribute() : base(typeof(SignOutFromExternalAuthenticationFilter))
        {
        }

        #region Nested filter

        /// <summary>
        /// Represents a filter that sign out from the external authentication scheme
        /// </summary>
        private class SignOutFromExternalAuthenticationFilter : IAuthorizationFilter
        {
            #region Methods

            /// <summary>
            /// Called early in the filter pipeline to confirm request is authorized
            /// </summary>
            /// <param name="filterContext">Authorization filter context</param>
            public void OnAuthorization(AuthorizationFilterContext filterContext)
            {
                var authenticationManager = filterContext?.HttpContext;
                if (authenticationManager == null)
                    return;

                //sign out from the external authentication scheme
                var userPrincipal = authenticationManager.AuthenticateAsync(GrandCookieAuthenticationDefaults.ExternalAuthenticationScheme).Result;
                var userIdentity = userPrincipal?.Principal?.Identities?.FirstOrDefault(identity => identity.IsAuthenticated);
                if (userIdentity != null)
                    authenticationManager.SignOutAsync(GrandCookieAuthenticationDefaults.ExternalAuthenticationScheme).Wait();
            }

            #endregion
        }

        #endregion
    }
}