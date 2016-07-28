/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */
"use strict";
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
define(["require", "exports", "ClientResources", "../../../_generated/Svg", "../../../_generated/SvgLogo", "../../../Shared/Constants", "../../../Shared/Utilities", "./ApiTypePartViewModel"], function (require, exports, ClientResources, Svg, SvgLogo, Constants, Utilities, ApiTypePart) {
    var Azure = MsPortalFx.Azure;
    var ViewModels = MsPortalFx.ViewModels;
    var Forms = ViewModels.Forms;
    var Subscriptions = Azure.Subscriptions;
    var Locations = Azure.Locations;
    var Arm = Azure.ResourceManager;
    var ResourceGroups = Azure.ResourceGroups;
    var SpecPicker = Arm.Pickers.Specs;
    var ApiInfo = ApiTypePart.ApiInfo;
    var CreateBladeViewModel = (function (_super) {
        __extends(CreateBladeViewModel, _super);
        function CreateBladeViewModel(container, initialState, dataContext, title, subtitle, icon, apiTypeCategory, resourceNamespace, resourceType) {
            var _this = this;
            _super.call(this, container);
            this.showLegalTerms = ko.observable(false);
            this.title = ko.observable(title);
            this.subtitle = ko.observable(subtitle);
            this.icon = ko.observable(icon);
            this._apiTypeCategory = apiTypeCategory;
            this._resourceNamespace = resourceNamespace;
            this._resourceFqdn = resourceNamespace + "/" + resourceType;
            this._container = container;
            this._dataContext = dataContext;
            this._resourcesPerSubscriptionView = dataContext.resourceData.apiEntitiesPerSubscription.createView(container, { interceptNotFound: false });
            this.actionBar = new ViewModels.ActionBars.CreateActionBar.ViewModel(container);
            this.parameterProvider = new ViewModels.ParameterProvider(container, {
                mapIncomingDataForEditScope: this._mapIncomingDataForEditScope.bind(this),
                mapOutgoingDataForCollector: function (outgoing) { return outgoing; }
            });
            this.editScope = this.parameterProvider.editScope;
            this._initializeSection();
            this.armProvisioner = new Arm.Provisioner(container, initialState, {
                supplyTemplateDeploymentOptions: this._supplyProvisioningPromise.bind(this),
                actionBar: this.actionBar,
                parameterProvider: this.parameterProvider
            });
            this.valid = ko.pureComputed(function () {
                return _this.nameTextBox.valid()
                    && _this.apiTypeSelector.valid()
                    && _this.accountSpecSelector.control.valid()
                    && _this.subscriptionDropDown.valid()
                    && _this.resourceGroupDropDown.valid()
                    && _this.locationDropDown.valid()
                    && _this.legalSelector.valid();
            });
            this.valid.subscribe(container, this.actionBar.valid);
            this._configResourceExistsValidationChain();
        }
        CreateBladeViewModel.prototype.onInputsSet = function (inputs) {
            return Q();
        };
        CreateBladeViewModel.prototype._initializeName = function () {
            var _this = this;
            this.nameTextBox = new Forms.TextBox.ViewModel(this._container, this, "name", {
                label: ko.observable(ClientResources.resourceName),
                emptyValueText: ko.observable(ClientResources.CreateBlade.enterTheName),
                validations: ko.observableArray([
                    new ViewModels.RequiredValidation(ClientResources.CreateBlade.resourceNameRequired),
                    new ViewModels.LengthRangeValidation(2, 64, ClientResources.CreateBlade.resourceNameLengthValidationMessage),
                    new ViewModels.RegExMatchValidation("^(?!.*_$)(?!.*-$)[a-zA-Z0-9_-]*$", ClientResources.CreateBlade.resourceNameInvalidMessage),
                    new MsPortalFx.ViewModels.CustomValidation("", function (value) {
                        return _this._checkResourceNameAvailability(value, _this.subscriptionDropDown.value, _this.resourceGroupDropDown.value);
                    })
                ])
            });
            this.nameTextBox.delayValidationTimeout(Constants.NameDelayValidationTimeout);
            this.nameTextBox.valueUpdateTrigger = ViewModels.Controls.ValueUpdateTrigger.AfterKeyDown;
        };
        CreateBladeViewModel.prototype._initializeLegal = function () {
            var _this = this;
            this.legalSelector = new ViewModels.Forms.Selector.ViewModel(this._container, this, this.createEditScopeAccessor(function (data) { return data.legalForm; }), {
                label: ko.observable(ClientResources.CreateBlade.LegalTerm.title),
                validations: ko.observableArray([new ViewModels.RequiredValidation(ClientResources.CreateBlade.LegalTerm.title)]),
                displayText: ko.pureComputed(function () {
                    var value = _this.legalSelector.value();
                    if (value && value.agreed) {
                        return ClientResources.CreateBlade.LegalTerm.agreed;
                    }
                    else {
                        return ClientResources.CreateBlade.LegalTerm.description;
                    }
                }),
            });
            this.legalCollector = new ViewModels.ParameterCollector(this._container, {
                supplyInitialData: function () {
                    return {
                        agreed: false
                    };
                },
                receiveResult: function (result) {
                    _this.legalSelector.value(result);
                },
                selectable: this.legalSelector.selectable
            });
        };
        CreateBladeViewModel.prototype._initializeApiType = function () {
            var _this = this;
            this.apiTypeSelector = new ViewModels.Forms.Selector.ViewModel(this._container, this, this.createEditScopeAccessor(function (data) { return data.apiType; }), {
                label: ko.observable(ClientResources.apiType),
                validations: ko.observableArray([new ViewModels.RequiredValidation(ClientResources.CreateBlade.selectApiType)]),
                displayText: ko.pureComputed(function () {
                    var value = _this.apiTypeSelector.value();
                    return value && value.title ? value.title : "";
                }),
            });
            this.apiTypeCollector = new ViewModels.ParameterCollector(this._container, {
                supplyInitialData: function () {
                    return _this.apiTypeSelector.value();
                },
                supplyProviderConfig: function () {
                    // provider config for ApiTypePartViewModel
                    return {
                        itemsCategory: _this._apiTypeCategory,
                    };
                },
                receiveResult: function (result) {
                    if (_this.apiTypeSelector.value() == null || _this.apiTypeSelector.value().name != result.name) {
                        _this.apiTypeSelector.value(result);
                        _this.accountSpecSelector.control.value(null);
                    }
                },
                selectable: this.apiTypeSelector.selectable
            });
        };
        CreateBladeViewModel.prototype._initializeAccountSpec = function () {
            var _this = this;
            this.accountSpecSelector = new SpecPicker.Selector(this._container, {
                label: ko.observable(ClientResources.pricingTier),
                form: this,
                pathOrAccessor: this.createEditScopeAccessor(function (data) { return data.spec; }),
                supplyInitialData: function () {
                    return {
                        selectedSpecId: "",
                        entityId: "",
                        recommendedSpecIds: [],
                        selectRecommendedView: true,
                        subscriptionId: _this.subscriptionDropDown.value().subscriptionId,
                        regionId: _this.locationDropDown.value().name,
                        options: {
                            apiType: _this.apiTypeSelector.value().name,
                            subscriptionId: _this.subscriptionDropDown.value().subscriptionId,
                        },
                        disabledSpecs: []
                    };
                },
                validations: ko.observableArray([
                    new MsPortalFx.ViewModels.RequiredValidation(ClientResources.CreateBlade.selectSpec)
                ])
            });
            this.accountSpecSelector.control.displayText = ko.pureComputed(function () {
                var value = _this.accountSpecSelector.control.value();
                return value ? Utilities.getPricingText(value.selectedSpecId) : "";
            });
            this.accountSpecSelector.control.enabled(this.apiTypeSelector.value() != null);
            this.apiTypeSelector.value.subscribe(this._container, function (v) { return _this.accountSpecSelector.control.enabled(v != null); });
        };
        CreateBladeViewModel.prototype._initializeSubscriptions = function () {
            var subscriptionsDropDownOptions = {
                options: ko.observableArray([]),
                form: this,
                accessor: this.createEditScopeAccessor(function (data) {
                    return data.subscription;
                }),
                validations: ko.observableArray([
                    new ViewModels.RequiredValidation(ClientResources.CreateBlade.selectSubscription)
                ])
            };
            this.subscriptionDropDown = new Subscriptions.DropDown(this._container, subscriptionsDropDownOptions);
        };
        CreateBladeViewModel.prototype._initializeResourceGroup = function () {
            var _this = this;
            var resourceGroupsDropDownOptions = {
                options: ko.observableArray([]),
                form: this,
                accessor: this.createEditScopeAccessor(function (data) {
                    return data.resourceGroup;
                }),
                label: ko.observable(ClientResources.resourceGroup),
                subscriptionIdObservable: this.subscriptionDropDown.subscriptionId,
                validations: ko.observableArray([
                    new ViewModels.RequiredValidation(ClientResources.CreateBlade.pleaseSelectResourceGroup)
                ])
            };
            this.resourceGroupDropDown = new ResourceGroups.DropDown(this._container, resourceGroupsDropDownOptions);
            this.resourceGroupDropDown.value.subscribe(this._container, function (resourceGroup) {
                if (_this.locationDropDown) {
                    var locationsDropDown = _this.locationDropDown.control;
                    var resourceGroupLocation = resourceGroup && resourceGroup.value && resourceGroup.value.location;
                    var location = locationsDropDown.items().first(function (item) {
                        return item.value === resourceGroupLocation;
                    });
                    if (location) {
                        locationsDropDown.value(location.text());
                    }
                }
            });
        };
        CreateBladeViewModel.prototype._initializeLocations = function () {
            var locationsDropDownOptions = {
                options: ko.observableArray([]),
                form: this,
                accessor: this.createEditScopeAccessor(function (data) {
                    return data.location;
                }),
                subscriptionIdObservable: this.subscriptionDropDown.subscriptionId,
                resourceTypesObservable: ko.observable([this._resourceFqdn]),
                validations: ko.observableArray([
                    new ViewModels.RequiredValidation(ClientResources.CreateBlade.selectLocation)
                ])
            };
            this.locationDropDown = new Locations.DropDown(this._container, locationsDropDownOptions);
        };
        CreateBladeViewModel.prototype._initializeSection = function () {
            this._initializeSubscriptions();
            this._initializeApiType();
            this._initializeName();
            this._initializeAccountSpec();
            this._initializeResourceGroup();
            this._initializeLocations();
            this._initializeLegal();
            this._initializeSvgContent();
        };
        CreateBladeViewModel.prototype._mapIncomingDataForEditScope = function (incoming) {
            var apiInfo = null;
            var apiSpec = null;
            var parameterConfig = this.parameterProvider.configFromCollector();
            if (parameterConfig && parameterConfig.apitype) {
                console.log("this is api type value from deep link:" + parameterConfig.apitype);
                apiInfo = new ApiInfo();
                apiInfo.name = parameterConfig.apitype;
                apiInfo.title = this._dataContext.createData.apiTypeList().filter(function (value, index, apis) {
                    return String(apis[index].item) == parameterConfig.apitype;
                })[0].title();
                if (parameterConfig.pricingtier) {
                    console.log("this is pricing tier value from deep link:" + parameterConfig.pricingtier);
                    apiSpec = { selectedSpecId: parameterConfig.pricingtier, selectedRecommendedView: true };
                }
            }
            var data = incoming;
            var galleryCreateOptions = this.armProvisioner.armProvisioningConfig
                && this.armProvisioner.armProvisioningConfig.galleryCreateOptions;
            var model = {
                name: ko.observable(),
                apiType: ko.observable(apiInfo),
                subscription: ko.observable(null),
                resourceGroup: ko.observable(null),
                location: ko.observable(null),
                spec: ko.observable(apiSpec),
                legalForm: ko.observable(null)
            };
            return model;
        };
        CreateBladeViewModel.prototype._supplyProvisioningPromise = function (data) {
            var galleryCreateOptions = this.armProvisioner.armProvisioningConfig
                && this.armProvisioner.armProvisioningConfig.galleryCreateOptions;
            var name = data.name();
            var subscriptionId = data.subscription().subscriptionId;
            var resourceGroupCreatorValue = data.resourceGroup();
            var location = data.location();
            var resourceGroupName = resourceGroupCreatorValue.value.name;
            var resourceGroupLocation = resourceGroupCreatorValue.createNew ? location.name : resourceGroupCreatorValue.value.location;
            var resourceIdFormattedString = "/subscriptions/" + subscriptionId + "/resourcegroups/" + resourceGroupName + "/providers/" + this._resourceFqdn + "/" + name;
            var deferred = Q.defer();
            if (data.name()) {
                var parameters = {
                    name: name,
                    location: location.name,
                    apiType: data.apiType().name,
                    sku: data.spec().selectedSpecId
                };
                var templateDeploymentOptions = {
                    subscriptionId: subscriptionId,
                    resourceGroupName: resourceGroupName,
                    resourceGroupLocation: resourceGroupLocation,
                    parameters: parameters,
                    deploymentName: galleryCreateOptions.deploymentName,
                    resourceProviders: [this._resourceNamespace],
                    resourceId: resourceIdFormattedString,
                    templateJson: this._getResourceTemplateJson()
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
                    apiType: { type: "string" },
                    sku: { type: "string" }
                },
                resources: [
                    {
                        apiVersion: "2016-02-01-preview",
                        name: "[parameters('name')]",
                        location: "[parameters('location')]",
                        type: this._resourceFqdn,
                        kind: "[parameters('apiType')]",
                        sku: { "name": "[parameters('sku')]" },
                        properties: {}
                    }
                ]
            });
        };
        CreateBladeViewModel.prototype._constructResourceId = function (subscriptionId, resourceGroupName, resourceName) {
            return "/subscriptions/" + subscriptionId + "/resourceGroups/" + resourceGroupName + "/providers/" + this._resourceFqdn + "/" + resourceName;
        };
        CreateBladeViewModel.prototype._checkResourceNameAvailability = function (resourceName, subscriptionObservable, resourceGroupObservable) {
            var deferred = Q.defer();
            var subscriptionId = subscriptionObservable().subscriptionId;
            var resourceGroup = resourceGroupObservable();
            if (subscriptionId && resourceGroup && !resourceGroup.createNew && resourceGroup.value) {
                var resourceId = this._constructResourceId(subscriptionId, resourceGroup.value.name, resourceName);
                var uri = MsPortalFx.Data.uriFormatter(Constants.ArmServiceEndpoint + "{id}?" + Constants.ArmServiceVersion, false)(resourceId);
                this._getResourceById(resourceId).catch(function (reason) {
                    if (reason.status == 404) {
                        deferred.resolve({ valid: true, message: undefined });
                    }
                }).done(function () { return deferred.resolve({ valid: false, message: ClientResources.CreateBlade.FormValidation.resourceNameAlreadyExists }); });
            }
            else {
                deferred.resolve({ valid: true, message: undefined });
            }
            return deferred.promise;
        };
        CreateBladeViewModel.prototype._getResourceById = function (resourceId) {
            var uri = MsPortalFx.Data.uriFormatter(Constants.ArmServiceEndpoint + "{id}?" + Constants.ArmServiceVersion, false)(resourceId);
            return MsPortalFx.Base.Net.ajax({
                uri: uri,
                type: "GET",
                dataType: "json",
                cache: false,
                traditional: true,
                contentType: "application/json",
                setAuthorizationHeader: true,
            });
        };
        CreateBladeViewModel.prototype._configResourceExistsValidationChain = function () {
            var _this = this;
            this.subscriptionDropDown.value.subscribe(this._container, function (newValue) {
                if (newValue) {
                    if (_this.nameTextBox.value()) {
                        _this.nameTextBox.triggerValidation();
                    }
                    if (_this.accountSpecSelector.control.value() &&
                        _this.accountSpecSelector.control.value().selectedSpecId &&
                        Constants.skusToCheck.indexOf(_this.accountSpecSelector.control.value().selectedSpecId) >= 0) {
                        _this.actionBar.disablePrimaryButton(true);
                        _this.actionBar.valid(false);
                        var promise = _this._resourcesPerSubscriptionView.fetch(newValue.subscriptionId).then(function () {
                            if (_this._resourcesPerSubscriptionView.item()) {
                                var specsToDisable = Utilities.getDisabledSpecsByApiType(_this._resourcesPerSubscriptionView.item(), _this.apiTypeSelector.value().name);
                                if (specsToDisable.filter(function (spec, index, specs) { return specs[index].specId == _this.accountSpecSelector.control.value().selectedSpecId; })[0]) {
                                    _this.accountSpecSelector.control.value(null);
                                }
                            }
                            else {
                                _this.accountSpecSelector.control.value(null);
                            }
                        }).catch(function (reason) {
                            // avoid any potential deployment failure if current promise exception.
                            if (reason && reason.status && reason.status != 404) {
                                _this.accountSpecSelector.control.value(null);
                            }
                        }).finally(function () {
                            _this.actionBar.disablePrimaryButton(false);
                            _this.actionBar.valid(true);
                        });
                    }
                }
            });
            this.resourceGroupDropDown.value.subscribe(this._container, function (newValue) {
                if (!newValue.createNew && newValue.value) {
                    if (_this.nameTextBox.value()) {
                        _this.nameTextBox.triggerValidation();
                    }
                }
            });
            this.apiTypeSelector.value.subscribe(this._container, function (newValue) {
                if (newValue) {
                    if (newValue.name != Constants.ApiTypes.TextAnalytics &&
                        newValue.name != Constants.ApiTypes.Recommendations) {
                        _this.showLegalTerms(true);
                    }
                    else {
                        _this.showLegalTerms(false);
                    }
                }
            });
        };
        CreateBladeViewModel.prototype._initializeSvgContent = function () {
            this.svgLogoContent = new Forms.MultiLineTextBox.ViewModel(this._container, this, "svgLogo", {
                label: ko.observable("SvgLogoContent"),
                emptyValueText: ko.observable("Please Reload this page!"),
                defaultValue: ko.observable(SvgLogo.Content.SVG.sampleLogo.data)
            });
            this.svgContentA = new Forms.MultiLineTextBox.ViewModel(this._container, this, "svg", {
                label: ko.observable("SvgAContent"),
                emptyValueText: ko.observable("Please Reload this page!"),
                defaultValue: ko.observable(Svg.Content.SVG.sampleA.data)
            });
            this.svgContentB = new Forms.MultiLineTextBox.ViewModel(this._container, this, "svg", {
                label: ko.observable("SvgBContent"),
                emptyValueText: ko.observable("Please Reload this page!"),
                defaultValue: ko.observable(Svg.Content.SVG.sampleB.data)
            });
            this.svgContentC = new Forms.MultiLineTextBox.ViewModel(this._container, this, "svg", {
                label: ko.observable("SvgCContent"),
                emptyValueText: ko.observable("Please Reload this page!"),
                defaultValue: ko.observable(Svg.Content.SVG.sampleC.data)
            });
            this.svgContentD = new Forms.MultiLineTextBox.ViewModel(this._container, this, "svg", {
                label: ko.observable("SvgDContent"),
                emptyValueText: ko.observable("Please Reload this page!"),
                defaultValue: ko.observable(Svg.Content.SVG.sampleD.data)
            });
            this.svgContentE = new Forms.MultiLineTextBox.ViewModel(this._container, this, "svg", {
                label: ko.observable("SvgEContent"),
                emptyValueText: ko.observable("Please Reload this page!"),
                defaultValue: ko.observable(Svg.Content.SVG.sampleE.data)
            });
        };
        return CreateBladeViewModel;
    })(Forms.Form.ViewModel);
    exports.CreateBladeViewModel = CreateBladeViewModel;
});
