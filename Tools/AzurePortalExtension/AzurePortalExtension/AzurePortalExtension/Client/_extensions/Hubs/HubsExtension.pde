{
  "extension": "HubsExtension",
  "version": "1.0",
  "sdkVersion": "5.0.302.329 (production_sdk#b7c08d1.160414-1449)",
  "schemaVersion": "1.0.0.2",
  "assetTypes": [
    {
      "name": "ArmExplorer",
      "permissions": []
    },
    {
      "name": "BrowseAll",
      "permissions": []
    },
    {
      "name": "BrowseAllWithType",
      "permissions": []
    },
    {
      "name": "BrowseService",
      "permissions": []
    },
    {
      "name": "BrowseDynamicAsset",
      "permissions": []
    },
    {
      "name": "BrowseDynamicResource",
      "permissions": []
    },
    {
      "name": "BrowseInstanceLink",
      "permissions": []
    },
    {
      "name": "BrowseResource",
      "permissions": []
    },
    {
      "name": "BrowseAllResources",
      "permissions": []
    },
    {
      "name": "BrowseRecentResources",
      "permissions": []
    },
    {
      "name": "BrowseResourceGroup",
      "permissions": []
    },
    {
      "name": "ActiveDirectoryPlaceholder",
      "permissions": []
    },
    {
      "name": "ActiveDirectoryMfaPlaceholder",
      "permissions": []
    },
    {
      "name": "ActiveDirectoryRmsPlaceholder",
      "permissions": []
    },
    {
      "name": "ApiManagementPlaceholder",
      "permissions": []
    },
    {
      "name": "BackupPlaceholder",
      "permissions": []
    },
    {
      "name": "BizTalkServicePlaceholder",
      "permissions": []
    },
    {
      "name": "CdnPlaceholder",
      "permissions": []
    },
    {
      "name": "EventHubPlaceholder",
      "permissions": []
    },
    {
      "name": "MachineLearningPlaceholder",
      "permissions": []
    },
    {
      "name": "MarketplaceAddOnPlaceholder",
      "permissions": []
    },
    {
      "name": "MediaServicePlaceholder",
      "permissions": []
    },
    {
      "name": "MobileEngagementPlaceholder",
      "permissions": []
    },
    {
      "name": "MobileServicePlaceholder",
      "permissions": []
    },
    {
      "name": "RemoteAppPlaceholder",
      "permissions": []
    },
    {
      "name": "ServiceBusPlaceholder",
      "permissions": []
    },
    {
      "name": "SiteRecoveryPlaceholder",
      "permissions": []
    },
    {
      "name": "StorSimplePlaceholder",
      "permissions": []
    },
    {
      "name": "StreamAnalyticsPlaceholder",
      "permissions": []
    },
    {
      "name": "TrafficManagerPlaceholder",
      "permissions": []
    },
    {
      "name": "WhatsNew",
      "permissions": []
    },
    {
      "name": "Lock",
      "permissions": []
    },
    {
      "name": "Deployments",
      "permissions": []
    },
    {
      "name": "ResourceGroups",
      "permissions": [
        {
          "Name": "read",
          "Action": "Microsoft.Resources/subscriptions/resourceGroups/read"
        },
        {
          "Name": "deleteObject",
          "Action": "Microsoft.Resources/subscriptions/resourceGroups/delete"
        },
        {
          "Name": "write",
          "Action": "Microsoft.Resources/subscriptions/resourceGroups/write"
        },
        {
          "Name": "writeDeployments",
          "Action": "Microsoft.Resources/subscriptions/resourceGroups/deployments/write"
        },
        {
          "Name": "readDeployments",
          "Action": "Microsoft.Resources/subscriptions/resourceGroups/deployments/read"
        },
        {
          "Name": "readEvents",
          "Action": "Microsoft.Insights/events/read"
        }
      ]
    },
    {
      "name": "ResourceGroupEvents",
      "permissions": []
    },
    {
      "name": "ServicesHealth",
      "permissions": []
    },
    {
      "name": "Settings",
      "permissions": []
    },
    {
      "name": "Tag",
      "permissions": []
    }
  ],
  "parts": [
    {
      "name": "BrowseServicePart",
      "inputs": [
        "assetTypeId"
      ],
      "commandBindings": [],
      "initialSize": 2,
      "largeInitialSize": null
    },
    {
      "name": "BrowseServiceListPart",
      "inputs": [
        "assetTypeId"
      ],
      "commandBindings": [],
      "initialSize": 8,
      "largeInitialSize": null
    },
    {
      "name": "BrowseServiceListPartWithCookie",
      "inputs": [
        "assetTypeId",
        "cookie"
      ],
      "commandBindings": [],
      "initialSize": 8,
      "largeInitialSize": null
    },
    {
      "name": "BrowseResourceListPart",
      "inputs": [
        "bladeId",
        "resourceType",
        "selectedSubscriptions",
        "subscriptionsFiltered",
        "filter"
      ],
      "commandBindings": [],
      "initialSize": 8,
      "largeInitialSize": null
    },
    {
      "name": "BrowseResourceListPartWithCookie",
      "inputs": [
        "bladeId",
        "resourceType",
        "cookie",
        "selectedSubscriptions",
        "subscriptionsFiltered",
        "filter"
      ],
      "commandBindings": [],
      "initialSize": 8,
      "largeInitialSize": null
    },
    {
      "name": "ResourceGroupMapPart",
      "inputs": [
        "resourceGroup"
      ],
      "commandBindings": [],
      "initialSize": 5,
      "largeInitialSize": null
    },
    {
      "name": "ResourceMapPart",
      "inputs": [
        "assetOwner",
        "assetType",
        "assetId"
      ],
      "commandBindings": [],
      "initialSize": 5,
      "largeInitialSize": null
    },
    {
      "name": "MapResourceGroupListPart",
      "inputs": [
        "id",
        "bladeId",
        "resourceType",
        "selectedSubscriptions",
        "subscriptionsFiltered",
        "filter"
      ],
      "commandBindings": [],
      "initialSize": 5,
      "largeInitialSize": null
    },
    {
      "name": "DiagnosticsTile",
      "inputs": [],
      "commandBindings": [],
      "initialSize": 1,
      "largeInitialSize": null
    },
    {
      "name": "WhatsNewTile",
      "inputs": [],
      "commandBindings": [],
      "initialSize": 2,
      "largeInitialSize": null
    },
    {
      "name": "FeedbackTile",
      "inputs": [],
      "commandBindings": [],
      "initialSize": 0,
      "largeInitialSize": null
    },
    {
      "name": "ServicesHealthPart",
      "inputs": [],
      "commandBindings": [],
      "initialSize": 5,
      "largeInitialSize": null
    },
    {
      "name": "SpecPickerListViewPart",
      "inputs": [],
      "commandBindings": [],
      "initialSize": 9,
      "largeInitialSize": null
    },
    {
      "name": "PricingTierLauncher",
      "inputs": [
        "entityId"
      ],
      "commandBindings": [],
      "initialSize": 3,
      "largeInitialSize": null
    },
    {
      "name": "SpecComparisonPart",
      "inputs": [],
      "commandBindings": [],
      "initialSize": 8,
      "largeInitialSize": null
    },
    {
      "name": "SpecPickerListViewPartV3",
      "inputs": [],
      "commandBindings": [],
      "initialSize": 9,
      "largeInitialSize": null,
      "parameterProvider": true
    },
    {
      "name": "PricingTierLauncherV3",
      "inputs": [
        "entityId"
      ],
      "commandBindings": [],
      "initialSize": 3,
      "largeInitialSize": null
    },
    {
      "name": "ResourceFilterPart",
      "inputs": [],
      "commandBindings": [],
      "initialSize": 8,
      "largeInitialSize": null
    },
    {
      "name": "ResourceTagsPart",
      "inputs": [
        "resourceId"
      ],
      "commandBindings": [],
      "initialSize": null,
      "largeInitialSize": null
    }
  ],
  "blades": [
    {
      "name": "UnauthorizedAssetBlade",
      "keyParameters": [],
      "inputs": [],
      "optionalInputs": [],
      "outputs": []
    },
    {
      "name": "NotFoundAssetBlade",
      "keyParameters": [],
      "inputs": [],
      "optionalInputs": [],
      "outputs": []
    },
    {
      "name": "UnavailableAssetBlade",
      "keyParameters": [],
      "inputs": [],
      "optionalInputs": [],
      "outputs": []
    },
    {
      "name": "BrowseResourceBlade",
      "keyParameters": [],
      "inputs": [
        "resourceType"
      ],
      "optionalInputs": [
        "filter",
        "scope",
        "kind"
      ],
      "outputs": []
    },
    {
      "name": "BrowseAllResourcesBlade",
      "keyParameters": [],
      "inputs": [],
      "optionalInputs": [
        "scope"
      ],
      "outputs": []
    },
    {
      "name": "BrowseAllFilteredResourcesBlade",
      "keyParameters": [],
      "inputs": [
        "filter"
      ],
      "optionalInputs": [
        "scope"
      ],
      "outputs": []
    },
    {
      "name": "BrowseResourceGroupBlade",
      "keyParameters": [],
      "inputs": [
        "resourceType"
      ],
      "optionalInputs": [
        "scope"
      ],
      "outputs": []
    },
    {
      "name": "MapResourceGroupBlade",
      "keyParameters": [],
      "inputs": [
        "id"
      ],
      "optionalInputs": [],
      "outputs": []
    },
    {
      "name": "ResourceGroupPickerV3Blade",
      "keyParameters": [],
      "inputs": [],
      "optionalInputs": [],
      "outputs": [],
      "parameterProvider": true
    },
    {
      "name": "DeployFromTemplateBlade",
      "keyParameters": [],
      "inputs": [
        "internal_bladeCallerParams"
      ],
      "optionalInputs": [],
      "outputs": [],
      "parameterProvider": true
    },
    {
      "name": "ParametersEditorBlade",
      "keyParameters": [],
      "inputs": [],
      "optionalInputs": [],
      "outputs": [],
      "parameterProvider": true
    },
    {
      "name": "TemplateEditorBlade",
      "keyParameters": [],
      "inputs": [],
      "optionalInputs": [],
      "outputs": []
    },
    {
      "name": "LocationPickerV3Blade",
      "keyParameters": [],
      "inputs": [],
      "optionalInputs": [],
      "outputs": [],
      "parameterProvider": true
    },
    {
      "name": "DeploymentDetailsBlade",
      "keyParameters": [
        "id"
      ],
      "inputs": [
        "id"
      ],
      "optionalInputs": [
        "referrerInfo"
      ],
      "outputs": []
    },
    {
      "name": "ResourceGroupMapBlade",
      "keyParameters": [
        "id"
      ],
      "inputs": [
        "id"
      ],
      "optionalInputs": [],
      "outputs": []
    },
    {
      "name": "SettingsBlade",
      "keyParameters": [],
      "inputs": [],
      "optionalInputs": [],
      "outputs": []
    },
    {
      "name": "SubscriptionPickerV3Blade",
      "keyParameters": [],
      "inputs": [],
      "optionalInputs": [],
      "outputs": [],
      "parameterProvider": true
    }
  ],
  "commands": [
    {
      "name": "MoveResourceCommand",
      "inputs": [
        "resourceId"
      ]
    }
  ]
}