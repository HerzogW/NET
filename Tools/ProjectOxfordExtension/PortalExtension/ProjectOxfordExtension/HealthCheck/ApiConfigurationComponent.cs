/********************************************************
*                                                        *
*   Copyright (c) Microsoft. All rights reserved.        *
*                                                        *
*********************************************************/

namespace Microsoft.Portal.Extensions.ProjectOxfordExtension.HealthCheck
{
    using Microsoft.WindowsAzure.IntelligentServices.WebRole;
    using System.Threading.Tasks;

    /// <summary>
    /// Api items component.
    /// </summary>
    /// <seealso cref="Microsoft.WindowsAzure.IntelligentServices.WebRole.Component" />
    public class ApiConfigurationComponent : Component
    {
        /// <summary>
        /// The component name
        /// </summary>
        private const string ComponentName = "ApiConfiguration";

        /// <summary>
        /// Gets component type.
        /// </summary>
        public override string Type
        {
            get
            {
                return "Api Configuration Items";
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiConfigurationComponent"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="apiConfigurationController">The API configuration controller.</param>
        /// <param name="supportedHealthCheckItems">The supported health check items.</param>
        public ApiConfigurationComponent(string name) :base(name)
        {
        }

        /// <summary>
        /// Update component health status asynchronously.
        /// </summary>
        /// <returns>
        /// Update task.
        /// </returns>
        public override Task UpdateHealthStatusAsync()
        {
            if (string.IsNullOrEmpty(MvcApplication.ApiConfigurationManager.ErrorMessage))
            {
                Status = HealthStatus.Healthy;
                Message = "Healthy";
            }
            else
            {
                Status = HealthStatus.Sick;
                Message = MvcApplication.ApiConfigurationManager.ErrorMessage;
            }

            return Task.FromResult<object>(null);
        }
    }
}
