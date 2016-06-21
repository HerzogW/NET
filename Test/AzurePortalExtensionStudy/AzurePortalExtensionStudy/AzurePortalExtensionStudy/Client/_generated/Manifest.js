define(["require", "exports", "../Shared/Icons", "ClientResources"], function (require, exports, Icons, ClientResources) {
    "use strict";
    var ExtensionDefinition;
    (function (ExtensionDefinition) {
        "use strict";
        var untypedManifest = {
            "name": "AzurePortalExtensionStudy",
            "version": "1.0",
            "schemaVersion": "1.0.0.2",
            "sdkVersion": "5.0.302.329 (production_sdk#b7c08d1.160414-1450)",
            "notifications": [],
            "assetTypes": [
                {
                    "name": "MyResource",
                    "singularDisplayName": ClientResources.AssetTypeNames.Resource.singular,
                    "pluralDisplayName": ClientResources.AssetTypeNames.Resource.plural,
                    "lowerSingularDisplayName": ClientResources.AssetTypeNames.Resource.lowerSingular,
                    "lowerPluralDisplayName": ClientResources.AssetTypeNames.Resource.lowerPlural,
                    "viewModel": "Browse$AssetTypeViewModel",
                    "partName": "ResourcePart",
                    "bladeName": "ResourceBlade",
                    "browseType": 1,
                    "resourceType": {
                        "resourceTypeName": "Microsoft.PortalSdk/rootResources",
                        "apiVersion": null
                    },
                    "icon": Icons.Icons.cloudService
                }
            ],
            "notifications2": [],
            "startBoardParts": [],
            "blades": [
                {
                    "name": "PropertiesBlade",
                    "width": 0
                },
                {
                    "name": "ResourceBlade"
                },
                {
                    "name": "QuickStartBlade",
                    "width": 1
                },
                {
                    "name": "SettingsBlade",
                    "width": 0
                },
                {
                    "name": "CreateBlade",
                    "width": 0
                }
            ],
            "galleryParts": []
        };
        untypedManifest.pageVersion = window.fx.environment.pageVersion;
        ExtensionDefinition.manifest = untypedManifest;
        MsPortalFx.Extension.registerAmd(ExtensionDefinition.manifest, "Program", require, "../_generated/Blades/");
    })(ExtensionDefinition || (ExtensionDefinition = {}));
    return ExtensionDefinition;
});
