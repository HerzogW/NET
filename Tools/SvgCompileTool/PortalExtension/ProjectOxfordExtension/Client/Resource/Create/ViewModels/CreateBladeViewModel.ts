/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */

"use strict";
import ResourceArea = require("../../ResourceArea");
import ClientResources = require("ClientResources");
import ExtensionDefinition = require("../../../_generated/ExtensionDefinition");
import Svg = require("../../../_generated/Svg");
import SvgLogo = require("../../../_generated/SvgLogo");
import CreateModel = require("../Models/CreateModel");
import Constants = require("../../../Shared/Constants");
import Utilities = require("../../../Shared/Utilities");
import ApiTypePart = require("./ApiTypePartViewModel");
import CreateData = require("../../Data/CreateData");

import Azure = MsPortalFx.Azure;
import ViewModels = MsPortalFx.ViewModels;
import Forms = ViewModels.Forms;
import Subscriptions = Azure.Subscriptions;
import Locations = Azure.Locations;
import Arm = Azure.ResourceManager;
import ResourceGroups = Azure.ResourceGroups;
import SpecPicker = Arm.Pickers.Specs;
import CreatorAndSelectorV2 = Forms.CreatorAndSelectorV2;
import ApiInfo = ApiTypePart.ApiInfo;

export abstract class CreateBladeViewModel extends Forms.Form.ViewModel<CreateModel> {

    public title: KnockoutObservable<string>;
    public subtitle: KnockoutObservable<string>;
    public icon: KnockoutObservable<MsPortalFx.Base.Image>;

    public actionBar: ViewModels.ActionBars.CreateActionBar.ViewModel;
    public parameterProvider: ViewModels.ParameterProvider<CreateModel, CreateModel>;
    public armProvisioner: Arm.Provisioner<CreateModel>;
    public generalSection: Forms.Section.ViewModel;

    public svgContent: Forms.MultiLineTextBox.ViewModel;
    public svgContentA: Forms.MultiLineTextBox.ViewModel;
    public svgContentB: Forms.MultiLineTextBox.ViewModel;
    public svgContentC: Forms.MultiLineTextBox.ViewModel;
    public svgContentD: Forms.MultiLineTextBox.ViewModel;
    public svgContentE: Forms.MultiLineTextBox.ViewModel;

    public svgLogoContent: Forms.MultiLineTextBox.ViewModel;

    public nameTextBox: Forms.TextBox.ViewModel;
	public apiTypeSelector: Forms.Selector.ViewModel<ApiInfo>;
	public apiTypeCollector: ViewModels.ParameterCollector<ApiInfo>;
    public accountSpecSelector: SpecPicker.Selector;
    public subscriptionDropDown: Subscriptions.DropDown;
    public resourceGroupDropDown: ResourceGroups.DropDown;
    public locationDropDown: Locations.DropDown;
	public legalSelector: Forms.Selector.ViewModel<CreateData.ApiLegalForm>;
	public legalCollector: ViewModels.ParameterCollector<CreateData.ApiLegalForm>;
	public showLegalTerms = ko.observable(false);

    private _container: MsPortalFx.ViewModels.ContainerContract;
    private _dataContext: ResourceArea.DataContext;
    private _apiTypeCategory: string;
    private _resourceNamespace: string;
    private _resourceFqdn: string;
	private _resourcesPerSubscriptionView: MsPortalFx.Data.EntityView<any, string>;

    constructor(container: MsPortalFx.ViewModels.ContainerContract, initialState: any, dataContext: ResourceArea.DataContext,
        title: string,
        subtitle: string,
        icon: MsPortalFx.Base.Image,
        apiTypeCategory: string,
        resourceNamespace: string,
        resourceType: string) {

        super(container);

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
        this.parameterProvider = new ViewModels.ParameterProvider<CreateModel, CreateModel>(container, {
			mapIncomingDataForEditScope: this._mapIncomingDataForEditScope.bind(this),
            mapOutgoingDataForCollector: (outgoing: CreateModel) => outgoing
        });

        this.editScope = this.parameterProvider.editScope;
        this._initializeSection();

        this.armProvisioner = new Arm.Provisioner<CreateModel>(container, initialState, {
            supplyTemplateDeploymentOptions: this._supplyProvisioningPromise.bind(this),
            actionBar: this.actionBar,
            parameterProvider: this.parameterProvider
        });

        this.valid = ko.pureComputed(() => {
            return this.nameTextBox.valid()
                && this.apiTypeSelector.valid()
                && this.accountSpecSelector.control.valid()
                && this.subscriptionDropDown.valid()
                && this.resourceGroupDropDown.valid()
                && this.locationDropDown.valid()
				&& this.legalSelector.valid()
        });
        this.valid.subscribe(container, this.actionBar.valid);

		this._configResourceExistsValidationChain();
    }

    public onInputsSet(inputs: any) {
        return Q();
    }

    private _initializeName(): void {
        this.nameTextBox = new Forms.TextBox.ViewModel(this._container, this, "name", {
            label: ko.observable(ClientResources.resourceName),
            emptyValueText: ko.observable(ClientResources.CreateBlade.enterTheName),
            validations: ko.observableArray([
                new ViewModels.RequiredValidation(ClientResources.CreateBlade.resourceNameRequired),
                new ViewModels.LengthRangeValidation(2, 64, ClientResources.CreateBlade.resourceNameLengthValidationMessage),
                new ViewModels.RegExMatchValidation("^(?!.*_$)(?!.*-$)[a-zA-Z0-9_-]*$", ClientResources.CreateBlade.resourceNameInvalidMessage),
				new MsPortalFx.ViewModels.CustomValidation("", (value: any) => {
					return this._checkResourceNameAvailability(
						value,
						this.subscriptionDropDown.value,
						this.resourceGroupDropDown.value);
				})
            ])
        });

        this.nameTextBox.delayValidationTimeout(Constants.NameDelayValidationTimeout);
        this.nameTextBox.valueUpdateTrigger = ViewModels.Controls.ValueUpdateTrigger.AfterKeyDown;
    }

	private _initializeLegal(): void {
		this.legalSelector = new ViewModels.Forms.Selector.ViewModel<CreateData.ApiLegalForm>(this._container, this,
			this.createEditScopeAccessor((data) => data.legalForm),
            {
                label: ko.observable<string>(ClientResources.CreateBlade.LegalTerm.title),
                validations: ko.observableArray<ViewModels.Validation>([new ViewModels.RequiredValidation(ClientResources.CreateBlade.LegalTerm.title)]),
                displayText: ko.pureComputed<string>(() => {
                    var value = this.legalSelector.value();
					if (value && value.agreed) {
						return ClientResources.CreateBlade.LegalTerm.agreed;
					}
					else {
						return ClientResources.CreateBlade.LegalTerm.description;
					}
				}),
            });

		this.legalCollector = new ViewModels.ParameterCollector<CreateData.ApiLegalForm>(this._container,
            {
				supplyInitialData: () => {
					return {
						agreed: false
					}
                },
				receiveResult: (result: CreateData.ApiLegalForm) => {
					this.legalSelector.value(result);
                },
                selectable: this.legalSelector.selectable
            });
	}

	private _initializeApiType(): void {
		this.apiTypeSelector = new ViewModels.Forms.Selector.ViewModel<ApiInfo>(this._container, this,
			this.createEditScopeAccessor((data) => data.apiType),
            {
                label: ko.observable<string>(ClientResources.apiType),
                validations: ko.observableArray<ViewModels.Validation>([new ViewModels.RequiredValidation(ClientResources.CreateBlade.selectApiType)]),
                displayText: ko.pureComputed<string>(() => {
                    var value = this.apiTypeSelector.value();
					return value && value.title ? value.title : "";
				}),
            });

		this.apiTypeCollector = new ViewModels.ParameterCollector<ApiInfo>(this._container,
            {
                supplyInitialData: () => {
					return this.apiTypeSelector.value();
                },
                supplyProviderConfig: () => {
                    // provider config for ApiTypePartViewModel
                    return {
                        itemsCategory: this._apiTypeCategory,
                    };
                },
				receiveResult: (result: ApiInfo) => {
					if (this.apiTypeSelector.value() == null || this.apiTypeSelector.value().name != result.name) {
						this.apiTypeSelector.value(result);
						this.accountSpecSelector.control.value(null);
                    }
                },
                selectable: this.apiTypeSelector.selectable
            });
	}

    private _initializeAccountSpec(): void {
        this.accountSpecSelector = new SpecPicker.Selector(this._container, {
            label: ko.observable<string>(ClientResources.pricingTier),
            form: this,
			pathOrAccessor: this.createEditScopeAccessor((data) => data.spec),
            supplyInitialData: () => {
                return {
                    selectedSpecId: "",
                    entityId: "",
                    recommendedSpecIds: [],
                    selectRecommendedView: true,
                    subscriptionId: this.subscriptionDropDown.value().subscriptionId,
                    regionId: this.locationDropDown.value().name,
                    options: {
						apiType: this.apiTypeSelector.value().name,
						subscriptionId: this.subscriptionDropDown.value().subscriptionId,
					},
                    disabledSpecs: []
                };
            },
			validations: ko.observableArray<MsPortalFx.ViewModels.Validation>([
				new MsPortalFx.ViewModels.RequiredValidation(ClientResources.CreateBlade.selectSpec)
			])
        });

        this.accountSpecSelector.control.displayText = ko.pureComputed<string>(() => {
            var value = this.accountSpecSelector.control.value();
            return value ? Utilities.getPricingText(value.selectedSpecId) : "";
        });

		this.accountSpecSelector.control.enabled(this.apiTypeSelector.value() != null);
		this.apiTypeSelector.value.subscribe(this._container, v => this.accountSpecSelector.control.enabled(v != null));
    }

    private _initializeSubscriptions(): void {
        var subscriptionsDropDownOptions: Subscriptions.DropDown.Options = {
            options: ko.observableArray([]),
            form: this,
            accessor: this.createEditScopeAccessor((data) => {
                return data.subscription;
            }),
            validations: ko.observableArray<ViewModels.Validation>([
                new ViewModels.RequiredValidation(ClientResources.CreateBlade.selectSubscription)
            ])
        };

        this.subscriptionDropDown = new Subscriptions.DropDown(this._container, subscriptionsDropDownOptions);
    }

    private _initializeResourceGroup(): void {
        var resourceGroupsDropDownOptions: ResourceGroups.DropDown.Options = {
            options: ko.observableArray([]),
            form: this,
            accessor: this.createEditScopeAccessor((data) => {
                return data.resourceGroup;
            }),
            label: ko.observable<string>(ClientResources.resourceGroup),
            subscriptionIdObservable: this.subscriptionDropDown.subscriptionId,
            validations: ko.observableArray<MsPortalFx.ViewModels.Validation>([
                new ViewModels.RequiredValidation(ClientResources.CreateBlade.pleaseSelectResourceGroup)
            ])
        };

        this.resourceGroupDropDown = new ResourceGroups.DropDown(this._container, resourceGroupsDropDownOptions);

        this.resourceGroupDropDown.value.subscribe(this._container, (resourceGroup) => {
            if (this.locationDropDown) {
                var locationsDropDown = <Forms.FilterComboBox.ViewModel>this.locationDropDown.control;
                var resourceGroupLocation = resourceGroup && resourceGroup.value && resourceGroup.value.location;
                var location = locationsDropDown.items().first((item) => {
                    return item.value === resourceGroupLocation;
                });

                if (location) {
                    locationsDropDown.value(location.text());
                }
            }
        });
    }

    private _initializeLocations(): void {
        var locationsDropDownOptions = <Locations.DropDown.Options>{
            options: ko.observableArray([]),
            form: this,
            accessor: this.createEditScopeAccessor((data) => {
                return data.location;
            }),
            subscriptionIdObservable: this.subscriptionDropDown.subscriptionId,
            resourceTypesObservable: ko.observable([this._resourceFqdn]),
            validations: ko.observableArray<ViewModels.Validation>([
                new ViewModels.RequiredValidation(ClientResources.CreateBlade.selectLocation)
            ])
        };

        this.locationDropDown = new Locations.DropDown(this._container, locationsDropDownOptions);
    }

    private _initializeSection(): void {
		this._initializeSubscriptions();
		this._initializeApiType();
        this._initializeName();
        this._initializeAccountSpec();
        this._initializeResourceGroup();
        this._initializeLocations();
        this._initializeLegal();

        this._initializeSvgContent();
    }

	private _mapIncomingDataForEditScope(incoming: CreateModel): CreateModel {
		var apiInfo: ApiInfo = null;
		var apiSpec: MsPortalFx.Azure.ResourceManager.Pickers.Specs.Result = null;
		let parameterConfig = this.parameterProvider.configFromCollector();
		if (parameterConfig && parameterConfig.apitype) {
			console.log("this is api type value from deep link:" + parameterConfig.apitype);

			apiInfo = new ApiInfo();
			apiInfo.name = parameterConfig.apitype;
			apiInfo.title = this._dataContext.createData.apiTypeList().filter((value: CreateData.ApiItem, index: number, apis: CreateData.ApiItem[]) => {
				return String(apis[index].item) == parameterConfig.apitype;
			})[0].title();

			if (parameterConfig.pricingtier) {
				console.log("this is pricing tier value from deep link:" + parameterConfig.pricingtier);

				apiSpec = { selectedSpecId: parameterConfig.pricingtier, selectedRecommendedView: true };
			}
        }

        var data: any = incoming;

        var galleryCreateOptions = this.armProvisioner.armProvisioningConfig
            && this.armProvisioner.armProvisioningConfig.galleryCreateOptions;

        var model: CreateModel = {
            name: ko.observable<string>(),
			apiType: ko.observable<ApiInfo>(apiInfo),
            subscription: ko.observable(null),
            resourceGroup: ko.observable(null),
            location: ko.observable(null),
            spec: ko.observable(apiSpec),
			legalForm: ko.observable(null)
        };

		return model;
    }

    private _supplyProvisioningPromise(data: CreateModel): MsPortalFx.Base.PromiseV<Arm.TemplateDeploymentOptions> {
        var galleryCreateOptions = this.armProvisioner.armProvisioningConfig
            && this.armProvisioner.armProvisioningConfig.galleryCreateOptions;

        var name = data.name();
        var subscriptionId = data.subscription().subscriptionId;
        var resourceGroupCreatorValue = data.resourceGroup();
        var location = data.location();

        var resourceGroupName = resourceGroupCreatorValue.value.name;
        var resourceGroupLocation = resourceGroupCreatorValue.createNew ? location.name : resourceGroupCreatorValue.value.location;

        var resourceIdFormattedString =
            `/subscriptions/${subscriptionId}/resourcegroups/${resourceGroupName}/providers/${this._resourceFqdn}/${name}`;

        var deferred = Q.defer<Arm.TemplateDeploymentOptions>();

        if (data.name()) {
            var parameters: StringMap<string> = {
                name: name,
                location: location.name,
				apiType: data.apiType().name,
				sku: data.spec().selectedSpecId
            };

            var templateDeploymentOptions: Arm.TemplateDeploymentOptions = {
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
        } else {
            deferred.reject();
        }

        return deferred.promise;
    }

    private _getResourceTemplateJson(): string {
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
    }

	private _constructResourceId(subscriptionId: string, resourceGroupName: string, resourceName: string): string {
		return "/subscriptions/" + subscriptionId + "/resourceGroups/" + resourceGroupName + "/providers/" + this._resourceFqdn + "/" + resourceName;
	}

	private _checkResourceNameAvailability(
		resourceName: string,
		subscriptionObservable: KnockoutObservableBase<MsPortalFx.Azure.Subscription>,
		resourceGroupObservable: KnockoutObservableBase<MsPortalFx.Azure.CreatorAndDropdDownValue<MsPortalFx.Azure.ResourceGroup>>)
		: MsPortalFx.Base.PromiseV<MsPortalFx.ViewModels.ValidationResult> {

		var deferred = Q.defer<MsPortalFx.ViewModels.ValidationResult>();
		var subscriptionId = subscriptionObservable().subscriptionId;
		var resourceGroup = resourceGroupObservable();

		if (subscriptionId && resourceGroup && !resourceGroup.createNew && resourceGroup.value) {
			var resourceId = this._constructResourceId(subscriptionId, resourceGroup.value.name, resourceName);
			var uri: string = MsPortalFx.Data.uriFormatter(Constants.ArmServiceEndpoint + "{id}?" + Constants.ArmServiceVersion, false)(resourceId);

			this._getResourceById(resourceId).catch((reason: any) => {
				if (reason.status == 404) {
					deferred.resolve({ valid: true, message: undefined });
				}
			}).done(() => deferred.resolve({ valid: false, message: ClientResources.CreateBlade.FormValidation.resourceNameAlreadyExists }));
		} else {
			deferred.resolve({ valid: true, message: undefined });
		}

		return deferred.promise;
	}

	private _getResourceById(resourceId: string): MsPortalFx.Base.Net.JQueryPromiseXhr<any> {
		var uri: string = MsPortalFx.Data.uriFormatter(Constants.ArmServiceEndpoint + "{id}?" + Constants.ArmServiceVersion, false)(resourceId);
		return MsPortalFx.Base.Net.ajax({
			uri: uri,
			type: "GET",
			dataType: "json",
			cache: false,
			traditional: true,
			contentType: "application/json",
			setAuthorizationHeader: true,
		});
	}

	private _configResourceExistsValidationChain(): void {
		this.subscriptionDropDown.value.subscribe(this._container, (newValue: MsPortalFx.Azure.Subscription) => {

			if (newValue) {
				if (this.nameTextBox.value()) {
					this.nameTextBox.triggerValidation();
				}

				if (this.accountSpecSelector.control.value() &&
					this.accountSpecSelector.control.value().selectedSpecId &&
					Constants.skusToCheck.indexOf(this.accountSpecSelector.control.value().selectedSpecId) >= 0) {
					this.actionBar.disablePrimaryButton(true);
					this.actionBar.valid(false);
					var promise = this._resourcesPerSubscriptionView.fetch(newValue.subscriptionId).then(() => {
						if (this._resourcesPerSubscriptionView.item()) {
							var specsToDisable = Utilities.getDisabledSpecsByApiType(this._resourcesPerSubscriptionView.item(), this.apiTypeSelector.value().name);

							if (specsToDisable.filter((spec: any, index: number, specs: any) => { return specs[index].specId == this.accountSpecSelector.control.value().selectedSpecId })[0]) {
								this.accountSpecSelector.control.value(null);
							}
						}
						else {
							this.accountSpecSelector.control.value(null);
						}
					}).catch((reason: any) => {
						// avoid any potential deployment failure if current promise exception.
						if (reason && reason.status && reason.status != 404) { //404 means current subscription not registered, it's safe to let user select free tier in this case.
							this.accountSpecSelector.control.value(null);
						}
					}).finally(() => {
						this.actionBar.disablePrimaryButton(false);
						this.actionBar.valid(true);
					});
				}
			}
		});

		this.resourceGroupDropDown.value.subscribe(this._container, (newValue: MsPortalFx.Azure.CreatorAndDropdDownValue<MsPortalFx.Azure.ResourceGroup>) => {
			if (!newValue.createNew && newValue.value) {
				if (this.nameTextBox.value()) {
					this.nameTextBox.triggerValidation();
				}
			}
		});

		this.apiTypeSelector.value.subscribe(this._container, (newValue: ApiTypePart.ApiInfo) => {
			if (newValue) {
				if (newValue.name != Constants.ApiTypes.TextAnalytics &&
					newValue.name != Constants.ApiTypes.Recommendations) {
					this.showLegalTerms(true);
				}
				else {
					this.showLegalTerms(false);
				}
			}
		});
    }

    private _initializeSvgContent(): void {

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
    }
}