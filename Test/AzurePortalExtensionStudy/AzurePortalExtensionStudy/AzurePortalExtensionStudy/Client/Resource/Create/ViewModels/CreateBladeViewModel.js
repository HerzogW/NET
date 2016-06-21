var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
define(["require", "exports", "ClientResources", "../../../Shared/Constants"], function (require, exports, ClientResources, Constants) {
    "use strict";
    var FxAzure = MsPortalFx.Azure;
    var Arm = FxAzure.ResourceManager;
    var Forms = MsPortalFx.ViewModels.Forms;
    var SubscriptionsDropDown = FxAzure.Subscriptions.DropDown;
    var LocationsDropDown = FxAzure.Locations.DropDown;
    var ResourceGroups = FxAzure.ResourceGroups;
    var resourceType = Constants.sdkResourceProvider + "/" + Constants.rootResource;
    var CreateBladeViewModel = (function (_super) {
        __extends(CreateBladeViewModel, _super);
        function CreateBladeViewModel(container, initialState, dataContext) {
            _super.call(this, container);
            this.title = ko.observable(ClientResources.AssetTypeNames.Resource.singular);
            this.subtitle = ko.observable(ClientResources.AssetTypeNames.Resource.singular);
            this.icon = ko.observable(MsPortalFx.Base.Images.Polychromatic.CloudService());
            this._dataContext = dataContext;
            this.actionBar = new MsPortalFx.ViewModels.ActionBars.CreateActionBar.ViewModel(container);
            this.valid.subscribe(container, this.actionBar.valid);
            this.parameterProvider = new MsPortalFx.ViewModels.ParameterProvider(container, {
                mapOutgoingDataForCollector: function (outgoing) { return outgoing; },
                mapIncomingDataForEditScope: this._mapIncomingDataForEditScope.bind(this)
            });
            this.editScope = this.parameterProvider.editScope;
            this.armProvisioner = new Arm.Provisioner(container, initialState, {
                supplyTemplateDeploymentOptions: this._supplyProvisioningPromise.bind(this),
                actionBar: this.actionBar,
                parameterProvider: this.parameterProvider
            });
            this._initializeFormFields(container, initialState);
        }
        Object.defineProperty(CreateBladeViewModel.prototype, "_dataModel", {
            get: function () {
                var editScope = this.editScope();
                return editScope && editScope.root;
            },
            enumerable: true,
            configurable: true
        });
        CreateBladeViewModel.prototype._initializeFormFields = function (container, initialState) {
            var _this = this;
            this.nameTextBox = new MsPortalFx.ViewModels.Forms.TextBox.ViewModel(container, this, "name", {
                label: ko.observable(ClientResources.resourceName),
                emptyValueText: ko.observable(ClientResources.enterTheName),
                validations: ko.observableArray([
                    new MsPortalFx.ViewModels.RequiredValidation(ClientResources.resourceNameRequired)
                ])
            });
            this.nameTextBox.delayValidationTimeout(500);
            this.nameTextBox.valueUpdateTrigger = MsPortalFx.ViewModels.Controls.ValueUpdateTrigger.Input;
            var subscriptionsDropDownOptions = {
                options: ko.observableArray([]),
                form: this,
                accessor: this.createEditScopeAccessor(function (data) {
                    return data.subscription;
                }),
                validations: ko.observableArray([
                    new MsPortalFx.ViewModels.RequiredValidation(ClientResources.selectSubscription)
                ])
            };
            this.subscriptionsDropDown = new SubscriptionsDropDown(container, subscriptionsDropDownOptions);
            var locationsDropDownOptions = {
                options: ko.observableArray([]),
                form: this,
                accessor: this.createEditScopeAccessor(function (data) {
                    return data.location;
                }),
                subscriptionIdObservable: this.subscriptionsDropDown.subscriptionId,
                resourceTypesObservable: ko.observable([resourceType]),
                validations: ko.observableArray([
                    new MsPortalFx.ViewModels.RequiredValidation(ClientResources.selectLocation)
                ]),
            };
            this.locationsDropDown = new LocationsDropDown(container, locationsDropDownOptions);
            var resourceGroupsDropDownOptions = {
                options: ko.observableArray([]),
                form: this,
                accessor: this.createEditScopeAccessor(function (data) {
                    return data.resourceGroup;
                }),
                subscriptionIdObservable: this.subscriptionsDropDown.subscriptionId,
                validations: ko.observableArray([
                    new MsPortalFx.ViewModels.RequiredValidation(ClientResources.selectResourceGroup)
                ])
            };
            this.resourceGroupDropDown = new ResourceGroups.DropDown(container, resourceGroupsDropDownOptions);
            this.resourceGroupDropDown.value.subscribe(container, function (resourceGroup) {
                if (_this.locationsDropDown) {
                    var locationsDropDown = _this.locationsDropDown.control;
                    var resourceGroupLocation_1 = resourceGroup && resourceGroup.value && resourceGroup.value.location;
                    var location_1 = locationsDropDown.items().first(function (item) {
                        return item.value === resourceGroupLocation_1;
                    });
                    if (location_1) {
                        locationsDropDown.value(location_1.text());
                    }
                }
            });
            var sectionOptions = {
                children: ko.observableArray([
                    this.nameTextBox,
                    this.subscriptionsDropDown.control,
                    this.resourceGroupDropDown.control,
                    this.locationsDropDown.control,
                ])
            };
            this.generalSection = new Forms.Section.ViewModel(container, sectionOptions);
            this.sections.push(this.generalSection);
        };
        CreateBladeViewModel.prototype._mapIncomingDataForEditScope = function (incoming) {
            var data = incoming;
            var galleryCreateOptions = this.armProvisioner.armProvisioningConfig
                && this.armProvisioner.armProvisioningConfig.galleryCreateOptions;
            var model = {
                name: ko.observable(),
                subscription: ko.observable(),
                resourceGroup: ko.observable({
                    value: {
                        name: ClientResources.selectResourceGroup,
                        location: null
                    },
                    createNew: false
                }),
                location: ko.observable(),
            };
            return model;
        };
        CreateBladeViewModel.prototype._supplyProvisioningPromise = function (data) {
            var galleryCreateOptions = this.armProvisioner.armProvisioningConfig
                && this.armProvisioner.armProvisioningConfig.galleryCreateOptions;
            var name = data.name();
            var subscriptionId = data.subscription().subscriptionId;
            var location = data.location();
            var isNewResourceGroup = this.resourceGroupDropDown.value().createNew;
            var resourceGroupName = this.resourceGroupDropDown.value().value.name;
            var resourceGroupLocation = this.resourceGroupDropDown.value().createNew ? location.name : this.resourceGroupDropDown.value().value.location;
            var resourceIdFormattedString = "/subscriptions/" + subscriptionId + "/resourcegroups/" + resourceGroupName + "/providers/" + resourceType + "/" + name;
            var deferred = Q.defer();
            if (data.name()) {
                var parameters = {
                    name: name,
                    location: location.name,
                    customProperty: name
                };
                var templateDeploymentOptions = {
                    subscriptionId: subscriptionId,
                    resourceGroupName: resourceGroupName,
                    resourceGroupLocation: resourceGroupLocation,
                    parameters: parameters,
                    deploymentName: galleryCreateOptions.deploymentName,
                    resourceProviders: [Constants.sdkResourceProvider],
                    resourceId: resourceIdFormattedString,
                    templateJson: this._getResourceTemplateJson(),
                };
                deferred.resolve(templateDeploymentOptions);
            }
            else {
                deferred.reject();
            }
            return deferred.promise;
        };
        CreateBladeViewModel.prototype._getResourceTemplateJson = function () {
            return JSON.stringify({
                $schema: "http://schema.management.azure.com/schemas/2014-04-01-preview/deploymentTemplate.json#",
                contentVersion: "1.0.0.0",
                parameters: {
                    name: { type: "string" },
                    location: { type: "string" },
                    customProperty: { type: "string" }
                },
                resources: [
                    {
                        apiVersion: "2014-04-01",
                        name: "[parameters('name')]",
                        location: "[parameters('location')]",
                        type: resourceType,
                        properties: {
                            customProperty: "[parameters('customProperty')]"
                        }
                    }
                ]
            });
        };
        return CreateBladeViewModel;
    }(Forms.Form.ViewModel));
    exports.CreateBladeViewModel = CreateBladeViewModel;
});
