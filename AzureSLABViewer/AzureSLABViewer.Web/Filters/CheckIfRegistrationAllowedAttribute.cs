using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AzureSLABViewer.Web.Filters
{
    public class CheckIfRegistrationAllowedAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!ConfigSettings.AllowRegistration)
            {
                throw new HttpException(401, "Unauthorized");
            }
        }
    }
}