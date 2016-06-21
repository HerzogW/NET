/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */

"use strict";
import ExtensionDefinition = require("../../../_generated/ExtensionDefinition");
import ClientResources = require("ClientResources");
import ResourceArea = require("../../ResourceArea");
import SubscriptionsData = require("../../Data/SubscriptionsData");
import Utilities = require("../../../Shared/Utilities");

import Def = ExtensionDefinition.ViewModels.Resource.ResourceSummaryPartViewModel;
import DynamicBladeSelection = MsPortalFx.ViewModels.DynamicBladeSelection;
import Properties = MsPortalFx.ViewModels.Parts.Properties;
import GetDynamicBladeSelection = MsPortalFx.ViewModels.Parts.ResourceSummary.GetDynamicBladeSelection;

export class ResourceSummaryPartViewModel extends MsPortalFx.ViewModels.Parts.ResourceSummary.ViewModel2
    implements Def.Contract {
    private _accountView: MsPortalFx.Data.EntityView<ProjectOxfordExtension.DataModels.Account, any>;
    private _resourceId: KnockoutObservableBase<string>;
    private _bladeSelection: KnockoutObservableBase<DynamicBladeSelection>;
    private _dataContext: ResourceArea.DataContext;

    constructor(container: MsPortalFx.ViewModels.PartContainerContract, initialState: any, dataContext: ResourceArea.DataContext) {
        this._dataContext = dataContext;
        this._accountView = dataContext.resourceData.resourceEntities.createView(container);
        this._resourceId = ko.observable<string>();
        this._bladeSelection = ko.observable<DynamicBladeSelection>({
            detailBlade: ExtensionDefinition.BladeNames.resourceBlade,
            detailBladeInputs: {}
        });

        super(initialState, this._getOptions(container), container);
    }

    public onInputsSet(inputs: Def.InputsContract, settings: any): MsPortalFx.Base.Promise {      
        this._resourceId(inputs.resourceId);
        return super.onInputsSet(inputs, settings).then(() => {
            this._bladeSelection({
                detailBlade: ExtensionDefinition.BladeNames.resourceBlade,
                detailBladeInputs: {
                    id: inputs.resourceId
                }
            });

            return this._accountView.fetch(inputs.resourceId);
        });
    }

    private _getOptions(lifetime: MsPortalFx.Base.LifetimeManager): MsPortalFx.ViewModels.Parts.ResourceSummary.Options2 {
        var getKeysSelection: GetDynamicBladeSelection = (inputs: Def.InputsContract) => {
            return <DynamicBladeSelection>{
                detailBlade: ExtensionDefinition.BladeNames.keySettingBlade,
                detailBladeInputs: {
                    id: inputs.resourceId
                }
            }
        };

        var getQuickStartSelection: GetDynamicBladeSelection = (inputs: Def.InputsContract) => {
            return <DynamicBladeSelection> {
                detailBlade: ExtensionDefinition.BladeNames.quickStartBlade,
                detailBladeInputs: {
                    id: inputs.resourceId
                }
            }
        };

        var getSetingsSelection: GetDynamicBladeSelection = (inputs: Def.InputsContract) => {
            return <DynamicBladeSelection> {
                detailBlade: ExtensionDefinition.BladeNames.settingsBlade,
                detailBladeInputs: {
                    id: inputs.resourceId
                }
            }
        };

        var accountView = this._accountView;
        var properties: Properties.Property[] = [];
        properties.push(new Properties.TextProperty({
            label: ClientResources.apiType,
            value: ko.computed(lifetime, () => {
				if (accountView.item()) {
					return Utilities.getApiDisplayName(accountView.item().kind(), this._dataContext.createData.apiTypeList());
				}
			}),
            isLoading: accountView.loading
        }));

        properties.push(new Properties.TextProperty({
            label: ClientResources.pricingTier,
            value: ko.computed(lifetime, () => {
                var pricing = accountView.item() && accountView.item().sku().name();
                return Utilities.getPricingText(pricing);
            }),
            isLoading: accountView.loading
        }));

        properties.push(new Properties.TextProperty({
            label: ClientResources.ResourceBlade.endPoint,
            value: ko.computed(lifetime, () => {
                // Following code should work, once all resources would be provisioned with the new RP
                if (accountView.item() && accountView.item().properties().endpoint && accountView.item().properties().endpoint()) { // endpoint is defined and set (not empty)
                    return accountView.item().properties().endpoint();
                }
                // backup in case the previous code didn't work
                return this._dataContext.apiData.getEndPoint(accountView.item() && accountView.item().kind());
            }),
            isLoading: accountView.loading
        }));

        var statusValue = ko.computed(lifetime, () => {
            // TODO: Disable some features if the provisioningState is not active
            if (accountView.item() == null) {
                return "";
            }
            else {
                return Utilities.getResourceStatus(this._resourceId(), accountView.item().properties().provisioningState());
            }
        });

        return <MsPortalFx.ViewModels.Parts.ResourceSummary.Options2>{            
            getQuickStartSelection: getQuickStartSelection,
            getSettingsSelection: getSetingsSelection,
            getKeysSelection: getKeysSelection,
            collapsed: false,
            supportsResourceMove: MsPortalFx.Azure.ResourceManager.MoveType.SubscriptionAndResourceGroup,            
            status: {
                value: statusValue,
                isLoading: accountView.loading
            },
            staticProperties: properties
        };
    }
}