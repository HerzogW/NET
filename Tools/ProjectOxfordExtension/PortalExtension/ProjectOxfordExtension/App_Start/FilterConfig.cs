//-----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//-----------------------------------------------------------------------------

namespace Microsoft.Portal.Extensions.ProjectOxfordExtension
{
    using System.Web.Mvc;

    /// <summary>
    /// Contains the configuration for the Filter
    /// </summary>
    public static class FilterConfig
    {
        /// <summary>
        /// Registers the global filters
        /// </summary>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            if (filters == null)
            {
                return;
            }

            filters.Add(new HandleErrorAttribute());
        }
    }
}