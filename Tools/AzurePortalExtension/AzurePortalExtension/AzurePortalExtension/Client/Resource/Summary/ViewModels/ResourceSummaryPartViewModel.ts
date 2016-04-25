/// <reference path="../../../TypeReferences.d.ts" />

import ExtensionDefinition = require("../../../_generated/ExtensionDefinition");
import ClientResources = require("ClientResources");
import ResourceArea = require("../../ResourceArea");

import Def = ExtensionDefinition.ViewModels.Resource.ResourceSummaryPartViewModel;

/**
 * The ResourceSummray provides quick access to key properties that the consumers of the extension may want to use and also quick links
 * to common blades keys, quickstart, users, tags, settings
 */
export class ResourceSummaryPartViewModel
    extends MsPortalFx.ViewModels.Parts.ResourceSummary.ViewModel
    implements Def.Contract {

    private _resourceId: KnockoutObservableBase<string>;
    private _bladeSelection: KnockoutObservableBase<MsPortalFx.ViewModels.DynamicBladeSelection>;

    constructor(container: MsPortalFx.ViewModels.PartContainerContract, initialState: any, dataContext: ResourceArea.DataContext) {
        super(initialState, this._getOptions(), container);

        this._resourceId = ko.observable<string>();
        this._bladeSelection = ko.observable<MsPortalFx.ViewModels.DynamicBladeSelection>({
            detailBlade: ExtensionDefinition.BladeNames.resourceBlade,
            detailBladeInputs: {}
        });

        var properties: MsPortalFx.ViewModels.Parts.Properties.Property[] = [];

        // Text property
        properties.push(new MsPortalFx.ViewModels.Parts.Properties.TextProperty(ClientResources.textPropertyLabel, this._resourceId));
        // Link property
        properties.push(new MsPortalFx.ViewModels.Parts.Properties.LinkProperty(ClientResources.linkPropertyLabel, ClientResources.microsoftUri, ClientResources.linkPropertyLabel));

        // Open blade property
        properties.push(new MsPortalFx.ViewModels.Parts.Properties.OpenBladeProperty(
            ExtensionDefinition.BladeNames.resourceBlade,
            ko.observable(ExtensionDefinition.BladeNames.resourceBlade),
            this._bladeSelection
            ));

        this.setProperties(properties);
    }

    // if subclass needs to run some additional logic in onInputsSet,
    // it should chain the promise returned by the base class.
    public onInputsSet(inputs: Def.InputsContract, settings: any): MsPortalFx.Base.Promise {
        return super.onInputsSet(inputs, settings).then(() => {
            this._resourceId(inputs.resourceId);

            this._bladeSelection({
                detailBlade: ExtensionDefinition.BladeNames.resourceBlade,
                detailBladeInputs: {
                    id: inputs.resourceId
                }
            });
        });
    }

    private _getOptions(): MsPortalFx.ViewModels.Parts.ResourceSummary.Options {
        var getQuickStartSelection: MsPortalFx.ViewModels.Parts.ResourceSummary.GetDynamicBladeSelection = (inputs: Def.InputsContract) => {
            return <MsPortalFx.ViewModels.DynamicBladeSelection> {
                detailBlade: ExtensionDefinition.BladeNames.quickStartBlade,
                detailBladeInputs: {
                    id: inputs.resourceId
                }
            }
        };

        var getAllSettingsSelection: MsPortalFx.ViewModels.Parts.ResourceSummary.GetDynamicBladeSelection = (inputs: Def.InputsContract) => {
            return <MsPortalFx.ViewModels.DynamicBladeSelection> {
                detailBlade: ExtensionDefinition.BladeNames.settingsBlade,
                detailBladeInputs: {
                    id: inputs.resourceId
                }
            }
        };

        return <MsPortalFx.ViewModels.Parts.ResourceSummary.Options>{
            getQuickStartSelection: getQuickStartSelection,
            getSettingsSelection: getAllSettingsSelection,
            collapsed: false,
        };
    }
}
