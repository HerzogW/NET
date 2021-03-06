define(["require", "exports"], function (require, exports) {
    "use strict";
    var ExtensionDefinition;
    (function (ExtensionDefinition) {
        var Internal;
        (function (Internal) {
            var untypedDefinition = {
                "commandsCatalog": [],
                "name": "AzurePortalExtensionStudy",
                "version": "1.0",
                "hash": "47DEQpj8HBSa+/TImW+5JCeuQeRkm5NMpJWZG3hSuFU=",
                "schemaVersion": "1.0.0.2",
                "sdkVersion": "5.0.302.329 (production_sdk#b7c08d1.160414-1450)",
                "partsCatalog": [
                    {
                        "name": "QuickStartPart",
                        "viewModel": "QuickStart$QuickStartPartViewModel",
                        "partKind": 21,
                        "canUseOldInputVersions": false,
                        "inputs": [],
                        "bindings": [],
                        "details": [
                            {
                                "blade": "QuickStartBlade"
                            }
                        ]
                    },
                    {
                        "name": "ResourcePart",
                        "viewModel": "Browse$ResourcePartViewModel",
                        "partKind": 22,
                        "canUseOldInputVersions": false,
                        "inputs": [
                            "id"
                        ],
                        "bindings": [
                            {
                                "property": "id",
                                "valuesFrom": [
                                    {
                                        "referenceType": 1,
                                        "property": "id"
                                    }
                                ]
                            }
                        ],
                        "details": [
                            {
                                "blade": "ResourceBlade",
                                "selectableBindings": [
                                    {
                                        "property": "id",
                                        "valuesFrom": [
                                            {
                                                "referenceType": 0,
                                                "property": "content.assetId"
                                            }
                                        ]
                                    }
                                ]
                            }
                        ],
                        "assetType": "MyResource",
                        "assetIdInputProperty": "id"
                    }
                ],
                "commandGroups": [
                    {
                        "name": "MoveResource",
                        "commands": [
                            {
                                "name": "MoveResourceCommand",
                                "reference": {
                                    "commandType": "MoveResourceCommand",
                                    "extension": "HubsExtension"
                                },
                                "bindings": [
                                    {
                                        "property": "resourceId",
                                        "valuesFrom": [
                                            {
                                                "referenceType": 5,
                                                "property": "constant_overrideInBlade",
                                                "constantValue": "overrideInBlade"
                                            }
                                        ]
                                    }
                                ]
                            }
                        ]
                    }
                ],
                "htmlTemplates": {
                    "pdca8cab7e7be8c4bb1bac2cc195434587b": {
                        "file": "Client/Resource/Settings/SettingsBlade.pdl",
                        "content": "<div data-bind=\"pcGrid: settingList\"></div>"
                    },
                    "pdcd5fbc43220924c6c80cff00331ff832b": {
                        "file": "Create.html",
                        "content": "<div class=\"msportalfx-form\" data-bind=\"formElement: generalSection\"></div>"
                    }
                },
                "styleSheets": []
            };
            Internal.definition = untypedDefinition;
        })(Internal || (Internal = {}));
        ExtensionDefinition.definitionName = "AzurePortalExtensionStudy";
        function getDefinition() {
            if (Internal.definition) {
                var def = Internal.definition;
                Internal.definition = null;
                return def;
            }
            throw new Error("Extension definition is no longer available.");
        }
        ExtensionDefinition.getDefinition = getDefinition;
        var External;
        (function (External) {
            var HubsExtension;
            (function (HubsExtension) {
                HubsExtension.name = "HubsExtension";
                var Blades;
                (function (Blades) {
                    var UnauthorizedAssetBlade;
                    (function (UnauthorizedAssetBlade) {
                        UnauthorizedAssetBlade.name = "UnauthorizedAssetBlade";
                    })(UnauthorizedAssetBlade = Blades.UnauthorizedAssetBlade || (Blades.UnauthorizedAssetBlade = {}));
                    var NotFoundAssetBlade;
                    (function (NotFoundAssetBlade) {
                        NotFoundAssetBlade.name = "NotFoundAssetBlade";
                    })(NotFoundAssetBlade = Blades.NotFoundAssetBlade || (Blades.NotFoundAssetBlade = {}));
                    var UnavailableAssetBlade;
                    (function (UnavailableAssetBlade) {
                        UnavailableAssetBlade.name = "UnavailableAssetBlade";
                    })(UnavailableAssetBlade = Blades.UnavailableAssetBlade || (Blades.UnavailableAssetBlade = {}));
                    var BrowseResourceBlade;
                    (function (BrowseResourceBlade) {
                        BrowseResourceBlade.name = "BrowseResourceBlade";
                        var Inputs;
                        (function (Inputs) {
                            Inputs.resourceType = "resourceType";
                        })(Inputs = BrowseResourceBlade.Inputs || (BrowseResourceBlade.Inputs = {}));
                    })(BrowseResourceBlade = Blades.BrowseResourceBlade || (Blades.BrowseResourceBlade = {}));
                    var BrowseAllResourcesBlade;
                    (function (BrowseAllResourcesBlade) {
                        BrowseAllResourcesBlade.name = "BrowseAllResourcesBlade";
                    })(BrowseAllResourcesBlade = Blades.BrowseAllResourcesBlade || (Blades.BrowseAllResourcesBlade = {}));
                    var BrowseAllFilteredResourcesBlade;
                    (function (BrowseAllFilteredResourcesBlade) {
                        BrowseAllFilteredResourcesBlade.name = "BrowseAllFilteredResourcesBlade";
                        var Inputs;
                        (function (Inputs) {
                            Inputs.filter = "filter";
                        })(Inputs = BrowseAllFilteredResourcesBlade.Inputs || (BrowseAllFilteredResourcesBlade.Inputs = {}));
                    })(BrowseAllFilteredResourcesBlade = Blades.BrowseAllFilteredResourcesBlade || (Blades.BrowseAllFilteredResourcesBlade = {}));
                    var BrowseResourceGroupBlade;
                    (function (BrowseResourceGroupBlade) {
                        BrowseResourceGroupBlade.name = "BrowseResourceGroupBlade";
                        var Inputs;
                        (function (Inputs) {
                            Inputs.resourceType = "resourceType";
                        })(Inputs = BrowseResourceGroupBlade.Inputs || (BrowseResourceGroupBlade.Inputs = {}));
                    })(BrowseResourceGroupBlade = Blades.BrowseResourceGroupBlade || (Blades.BrowseResourceGroupBlade = {}));
                    var MapResourceGroupBlade;
                    (function (MapResourceGroupBlade) {
                        MapResourceGroupBlade.name = "MapResourceGroupBlade";
                        var Inputs;
                        (function (Inputs) {
                            Inputs.id = "id";
                        })(Inputs = MapResourceGroupBlade.Inputs || (MapResourceGroupBlade.Inputs = {}));
                    })(MapResourceGroupBlade = Blades.MapResourceGroupBlade || (Blades.MapResourceGroupBlade = {}));
                    var ResourceGroupPickerV3Blade;
                    (function (ResourceGroupPickerV3Blade) {
                        ResourceGroupPickerV3Blade.name = "ResourceGroupPickerV3Blade";
                    })(ResourceGroupPickerV3Blade = Blades.ResourceGroupPickerV3Blade || (Blades.ResourceGroupPickerV3Blade = {}));
                    var DeployFromTemplateBlade;
                    (function (DeployFromTemplateBlade) {
                        DeployFromTemplateBlade.name = "DeployFromTemplateBlade";
                        var Inputs;
                        (function (Inputs) {
                            Inputs.internal_bladeCallerParams = "internal_bladeCallerParams";
                        })(Inputs = DeployFromTemplateBlade.Inputs || (DeployFromTemplateBlade.Inputs = {}));
                    })(DeployFromTemplateBlade = Blades.DeployFromTemplateBlade || (Blades.DeployFromTemplateBlade = {}));
                    var ParametersEditorBlade;
                    (function (ParametersEditorBlade) {
                        ParametersEditorBlade.name = "ParametersEditorBlade";
                    })(ParametersEditorBlade = Blades.ParametersEditorBlade || (Blades.ParametersEditorBlade = {}));
                    var TemplateEditorBlade;
                    (function (TemplateEditorBlade) {
                        TemplateEditorBlade.name = "TemplateEditorBlade";
                    })(TemplateEditorBlade = Blades.TemplateEditorBlade || (Blades.TemplateEditorBlade = {}));
                    var LocationPickerV3Blade;
                    (function (LocationPickerV3Blade) {
                        LocationPickerV3Blade.name = "LocationPickerV3Blade";
                    })(LocationPickerV3Blade = Blades.LocationPickerV3Blade || (Blades.LocationPickerV3Blade = {}));
                    var DeploymentDetailsBlade;
                    (function (DeploymentDetailsBlade) {
                        DeploymentDetailsBlade.name = "DeploymentDetailsBlade";
                        var Inputs;
                        (function (Inputs) {
                            Inputs.id = "id";
                        })(Inputs = DeploymentDetailsBlade.Inputs || (DeploymentDetailsBlade.Inputs = {}));
                    })(DeploymentDetailsBlade = Blades.DeploymentDetailsBlade || (Blades.DeploymentDetailsBlade = {}));
                    var ResourceGroupMapBlade;
                    (function (ResourceGroupMapBlade) {
                        ResourceGroupMapBlade.name = "ResourceGroupMapBlade";
                        var Inputs;
                        (function (Inputs) {
                            Inputs.id = "id";
                        })(Inputs = ResourceGroupMapBlade.Inputs || (ResourceGroupMapBlade.Inputs = {}));
                    })(ResourceGroupMapBlade = Blades.ResourceGroupMapBlade || (Blades.ResourceGroupMapBlade = {}));
                    var SettingsBlade;
                    (function (SettingsBlade) {
                        SettingsBlade.name = "SettingsBlade";
                    })(SettingsBlade = Blades.SettingsBlade || (Blades.SettingsBlade = {}));
                    var SubscriptionPickerV3Blade;
                    (function (SubscriptionPickerV3Blade) {
                        SubscriptionPickerV3Blade.name = "SubscriptionPickerV3Blade";
                    })(SubscriptionPickerV3Blade = Blades.SubscriptionPickerV3Blade || (Blades.SubscriptionPickerV3Blade = {}));
                })(Blades = HubsExtension.Blades || (HubsExtension.Blades = {}));
            })(HubsExtension = External.HubsExtension || (External.HubsExtension = {}));
            var Microsoft_Azure_AD;
            (function (Microsoft_Azure_AD) {
                Microsoft_Azure_AD.name = "Microsoft_Azure_AD";
                var Blades;
                (function (Blades) {
                    var SelectMember;
                    (function (SelectMember) {
                        SelectMember.name = "SelectMember";
                        var Inputs;
                        (function (Inputs) {
                            Inputs.collectorBindingInternals_inputs = "collectorBindingInternals-inputs";
                            Inputs.collectorBindingInternals_errors = "collectorBindingInternals-errors";
                            Inputs.stepInput = "stepInput";
                        })(Inputs = SelectMember.Inputs || (SelectMember.Inputs = {}));
                        var Outputs;
                        (function (Outputs) {
                            Outputs.collectorBindingInternals_outputs = "collectorBindingInternals-outputs";
                            Outputs.collectorBindingInternals_commit = "collectorBindingInternals-commit";
                            Outputs.stepOutput = "stepOutput";
                        })(Outputs = SelectMember.Outputs || (SelectMember.Outputs = {}));
                    })(SelectMember = Blades.SelectMember || (Blades.SelectMember = {}));
                    var SelectMemberV3;
                    (function (SelectMemberV3) {
                        SelectMemberV3.name = "SelectMemberV3";
                        var Inputs;
                        (function (Inputs) {
                            Inputs.title = "title";
                            Inputs.subtitle = "subtitle";
                        })(Inputs = SelectMemberV3.Inputs || (SelectMemberV3.Inputs = {}));
                    })(SelectMemberV3 = Blades.SelectMemberV3 || (Blades.SelectMemberV3 = {}));
                    var RolesListBlade;
                    (function (RolesListBlade) {
                        RolesListBlade.name = "RolesListBlade";
                        var Inputs;
                        (function (Inputs) {
                            Inputs.scope = "scope";
                        })(Inputs = RolesListBlade.Inputs || (RolesListBlade.Inputs = {}));
                    })(RolesListBlade = Blades.RolesListBlade || (Blades.RolesListBlade = {}));
                    var UserAssignmentsBlade;
                    (function (UserAssignmentsBlade) {
                        UserAssignmentsBlade.name = "UserAssignmentsBlade";
                        var Inputs;
                        (function (Inputs) {
                            Inputs.scope = "scope";
                        })(Inputs = UserAssignmentsBlade.Inputs || (UserAssignmentsBlade.Inputs = {}));
                    })(UserAssignmentsBlade = Blades.UserAssignmentsBlade || (Blades.UserAssignmentsBlade = {}));
                })(Blades = Microsoft_Azure_AD.Blades || (Microsoft_Azure_AD.Blades = {}));
            })(Microsoft_Azure_AD = External.Microsoft_Azure_AD || (External.Microsoft_Azure_AD = {}));
        })(External = ExtensionDefinition.External || (ExtensionDefinition.External = {}));
        var AssetTypes;
        (function (AssetTypes) {
            var MyResource;
            (function (MyResource) {
                MyResource.name = "MyResource";
            })(MyResource = AssetTypes.MyResource || (AssetTypes.MyResource = {}));
        })(AssetTypes = ExtensionDefinition.AssetTypes || (ExtensionDefinition.AssetTypes = {}));
        var AssetTypeNames;
        (function (AssetTypeNames) {
            AssetTypeNames.myResource = AssetTypes.MyResource.name;
        })(AssetTypeNames = ExtensionDefinition.AssetTypeNames || (ExtensionDefinition.AssetTypeNames = {}));
        var BladeNames;
        (function (BladeNames) {
            BladeNames.propertiesBlade = "PropertiesBlade";
            BladeNames.resourceBlade = "ResourceBlade";
            BladeNames.quickStartBlade = "QuickStartBlade";
            BladeNames.settingsBlade = "SettingsBlade";
            BladeNames.createBlade = "CreateBlade";
        })(BladeNames = ExtensionDefinition.BladeNames || (ExtensionDefinition.BladeNames = {}));
        var CommandGroupNames;
        (function (CommandGroupNames) {
            CommandGroupNames.moveResource = "MoveResource";
        })(CommandGroupNames = ExtensionDefinition.CommandGroupNames || (ExtensionDefinition.CommandGroupNames = {}));
    })(ExtensionDefinition || (ExtensionDefinition = {}));
    return ExtensionDefinition;
});
