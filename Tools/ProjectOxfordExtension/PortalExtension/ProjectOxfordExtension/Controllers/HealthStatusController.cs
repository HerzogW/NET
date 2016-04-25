/********************************************************
*                                                        *
*   Copyright (c) Microsoft. All rights reserved.        *
*                                                        *
*********************************************************/

namespace Microsoft.Portal.Extensions.ProjectOxfordExtension.Controllers
{
    using Microsoft.Portal.Extensions.ProjectOxfordExtension.HealthCheck;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using WindowsAzure.IntelligentServices.WebRole;

    public class HealthStatusController : ApiController
    {
        public async Task<HttpResponseMessage> GetHealthStatus()
        {
            var component = new ApiConfigurationComponent("ApiConfiguration");
            await component.UpdateHealthStatusAsync();

            return await Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(new HealthStatusResponse()
                {
                    Status = component.Status,
                    Components = new List<Component>() { component }
                }))
            });
        }
    }
}