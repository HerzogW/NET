import ExtensionDefinition = require("../../../_generated/ExtensionDefinition");
//import ClientResources = require("ClientResources");
import ResourceArea = require("../../ResourceArea");

import Def = ExtensionDefinition.ViewModels.Resource.ApiUsagePartViewModel;

var endpoint = window.fx.environment && window.fx.environment.armEndpoint && window.fx.environment.armEndpoint.replace(/\/$/, "");
var armVersion = "api-version=2016-02-01-preview";

export class ApiUsagePartViewModel
    implements Def.Contract
{
    
    public title = ko.observable("");
    public gauge: MsPortalFx.ViewModels.Controls.Visualization.StepGauge.ViewModel;

    public resourceId = ko.observable<string>();

    constructor(container: MsPortalFx.ViewModels.PartContainerContract, initialState: any, dataContext: ResourceArea.DataContext) {
      
        this.gauge = new MsPortalFx.ViewModels.Controls.Visualization.StepGauge.ViewModel(container);

        this.gauge.maximum(100);

  
    } 

    public onInputsSet(inputs: Def.InputsContract): MsPortalFx.Base.Promise {
        this.resourceId(inputs.resourceId);

        var uri = MsPortalFx.Data.uriFormatter(endpoint + "{id}/getUsage?" + armVersion, false)(this.resourceId());

        var updateCount = () => {

            MsPortalFx.Base.Net.ajax<ApiUsageData>({
                uri: uri,
                type: "POST",
                dataType: "json",
                cache: false,
                traditional: true,
                // headers: headers,
                contentType: "application/json",
                setAuthorizationHeader: true,
                //data: data
            }).done(result => {

                console.info("Quota: " + result.quota + " Used: " + result.used);
               

                this.title("Remaining quota: " + (result.quota - result.used));
                this.gauge.current(result.used * 100 / result.quota);
            });

            setTimeout(() => {
                updateCount();
            }, 20000);
        };
        updateCount();
       


        return null;
    }
}

class ApiUsageData {
    public used: number;
    public quota: number;
}