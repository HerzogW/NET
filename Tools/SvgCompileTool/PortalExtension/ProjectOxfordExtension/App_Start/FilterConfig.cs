//-----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//-----------------------------------------------------------------------------

using System.Web.Mvc;

namespace Microsoft.Portal.Extensions.ProjectOxfordExtension
{
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