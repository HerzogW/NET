//-----------------------------------------------------------
// Copyright (c) AzurePortalExtensionResourceProvider.  All rights reserved.
//-----------------------------------------------------------

namespace AzurePortalExtensionResourceProvider.RequestHandlers
{
    /// <summary>
    /// OperationResourceLock used to thread-safely change culture of the OperationResources.
    /// </summary>
    internal static class OperationResourcesLock
    {
        /// <summary>
        /// The lock object
        /// </summary>
        private static readonly object LockObject = new object();

        /// <summary>
        /// Gets the lock object.
        /// </summary>
        public static object Instance
        {
            get
            {
                return LockObject;
            }
        }
    }
}