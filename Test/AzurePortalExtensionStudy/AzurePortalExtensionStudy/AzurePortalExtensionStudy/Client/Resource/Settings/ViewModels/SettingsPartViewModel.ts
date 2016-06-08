/// <reference path="../../../TypeReferences.d.ts" />

import ResourceArea = require("../../ResourceArea");
import ClientResources = require("ClientResources");
import ExtensionDefinition = require("../../../_generated/ExtensionDefinition");

import Def = ExtensionDefinition.ViewModels.Resource.SettingsPartViewModel;

export class SettingsPartViewModel extends MsPortalFx.ViewModels.Parts.SettingList.ViewModel implements Def.Contract {

    private _propertiesSettingBladeInputs: KnockoutObservable<any>;
   
    constructor(container: MsPortalFx.ViewModels.PartContainerContract, initialState: any, dataContext: ResourceArea.DataContext) {
        var options: MsPortalFx.ViewModels.Parts.SettingList.Options = {
            enableRbac: true,
            enableTags: true            
        };
        
        this._propertiesSettingBladeInputs = ko.observable({});

        super(container, initialState, this._getSettings(), options);
    }

    public onInputsSet(inputs: Def.InputsContract): MsPortalFx.Base.Promise {
        this.resourceId(inputs.id);

        this._propertiesSettingBladeInputs({ id: inputs.id });
       
        return null;
    }

    private _getSettings(): MsPortalFx.ViewModels.Parts.SettingList.Setting[] {
        var propertiesSetting = new MsPortalFx.ViewModels.Parts.SettingList.Setting("properties", ExtensionDefinition.BladeNames.propertiesBlade, this._propertiesSettingBladeInputs);
        propertiesSetting.displayText(ClientResources.propertiesBladeTitle);
        propertiesSetting.icon(MsPortalFx.Base.Images.Polychromatic.Controls());
        propertiesSetting.keywords([
            ClientResources.status,
            ClientResources.AssetTypeNames.Resource.plural,
            ClientResources.resourceLocationColumn,
            ClientResources.subscription
        ]);

        return [
            propertiesSetting         
        ];
    }
}