{
  "extension": "HubsExtension",
  "version": "1.0",
  "sdkVersion": "5.0.302.107 (production#df1584a.150918-1143)",
  "schemaVersion": "1.0.0.2",
  "assetTypes": [
    {
      "name": "Tag",
      "permissions": []
    },
    {
      "name": "Gallery",
      "permissions": []
    },
    {
      "name": "StoreGallery",
      "permissions": []
    },
    {
      "name": "Settings",
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
      "name": "WhatsNew",
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
      "name": "ActiveDirectoryPlaceholder",
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
      "name": "OperationalInsightsPlaceholder",
      "permissions": []
    },
    {
      "name": "SchedulerPlaceholder",
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
      "name": "BrowseResourceGroup",
      "permissions": []
    },
    {
      "name": "ArmExplorer",
      "permissions": []
    },
    {
      "name": "BrowseInstanceLink",
      "permissions": []
    },
    {
      "name": "Notification",
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
    }
  ],
  "parts": [
    {
      "name": "ResourceTagsPart",
      "inputs": [
        "resourceId"
      ],
      "commandBindings": [],
      "initialSize": 1,
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
      "name": "PricingTierLauncherV3",
      "inputs": [
        "entityId"
      ],
      "commandBindings": [],
      "initialSize": 3,
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
      "name": "PricingTierLauncher",
      "inputs": [
        "entityId"
      ],
      "commandBindings": [],
      "initialSize": 3,
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
      "name": "AzureServiceHealthPart",
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
      "name": "DiagnosticsTile",
      "inputs": [],
      "commandBindings": [],
      "initialSize": 1,
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
      "name": "WhatsNewTile",
      "inputs": [],
      "commandBindings": [],
      "initialSize": 2,
      "largeInitialSize": null
    },
    {
      "name": "TourTile",
      "inputs": [],
      "commandBindings": [],
      "initialSize": 2,
      "largeInitialSize": null
    },
    {
      "name": "LegalTermsSubscriptionProgrammaticAccessTilePart",
      "inputs": [
        "subscriptionId"
      ],
      "commandBindings": [],
      "initialSize": 2,
      "largeInitialSize": null
    },
    {
      "name": "GalleryLauncherPart",
      "inputs": [
        "galleryItemId",
        "provisioningOptions"
      ],
      "commandBindings": [],
      "initialSize": 2,
      "largeInitialSize": null
    },
    {
      "name": "BrowseResourcePinnedPart",
      "inputs": [
        "resourceType"
      ],
      "commandBindings": [],
      "initialSize": 2,
      "largeInitialSize": null
    },
    {
      "name": "BrowseAllResourcesPinnedPart",
      "inputs": [
        "resourceType"
      ],
      "commandBindings": [],
      "initialSize": 2,
      "largeInitialSize": null
    },
    {
      "name": "BrowseAllFilteredResourcesPinnedPart",
      "inputs": [
        "filter",
        "resourceType"
      ],
      "commandBindings": [],
      "initialSize": 2,
      "largeInitialSize": null
    },
    {
      "name": "BrowseRecentResourcesPinnedPart",
      "inputs": [
        "resourceType"
      ],
      "commandBindings": [],
      "initialSize": 2,
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
        "subscriptionsFiltered"
      ],
      "commandBindings": [],
      "initialSize": 8,
      "largeInitialSize": null
    },
    {
      "name": "BrowseRecentResourceListPart",
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
      "name": "BrowseResourceGroupPinnedPart",
      "inputs": [
        "resourceType"
      ],
      "commandBindings": [],
      "initialSize": 2,
      "largeInitialSize": null
    },
    {
      "name": "MapResourceGroupCollectionPart",
      "inputs": [
        "id",
        "resourceType"
      ],
      "commandBindings": [],
      "initialSize": 5,
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
      "name": "BrowseInstanceLinkPinnedPart",
      "inputs": [
        "resourceType"
      ],
      "commandBindings": [],
      "initialSize": 2,
      "largeInitialSize": null
    },
    {
      "name": "BrowseInstanceLinkListPart",
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
      "name": "BrowseServicePart",
      "inputs": [
        "assetTypeId"
      ],
      "commandBindings": [],
      "initialSize": 2,
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
    }
  ],
  "blades": [
    {
      "name": "DeployFromTemplateBlade",
      "keyParameters": [],
      "inputs": [
        "internal_bladeCallerParams"
      ],
      "outputs": [],
      "parameterProvider": true
    },
    {
      "name": "TemplateEditorBlade",
      "keyParameters": [],
      "inputs": [
        "internal_bladeCallerParams"
      ],
      "outputs": [],
      "parameterProvider": true
    },
    {
      "name": "ParametersEditorBlade",
      "keyParameters": [],
      "inputs": [],
      "outputs": [],
      "parameterProvider": true
    },
    {
      "name": "CreateEmptyResourceGroupBlade",
      "keyParameters": [],
      "inputs": [],
      "outputs": [],
      "parameterProvider": true
    },
    {
      "name": "ResourceGroupPickerV3Blade",
      "keyParameters": [],
      "inputs": [],
      "outputs": [],
      "parameterProvider": true
    },
    {
      "name": "SubscriptionPickerV3Blade",
      "keyParameters": [],
      "inputs": [],
      "outputs": [],
      "parameterProvider": true
    },
    {
      "name": "LocationPickerV3Blade",
      "keyParameters": [],
      "inputs": [],
      "outputs": [],
      "parameterProvider": true
    },
    {
      "name": "SettingsBlade",
      "keyParameters": [],
      "inputs": [],
      "outputs": []
    },
    {
      "name": "DeploymentDetailsBlade",
      "keyParameters": [
        "id"
      ],
      "inputs": [
        "id"
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
      "outputs": []
    },
    {
      "name": "BrowseResourceBlade",
      "keyParameters": [],
      "inputs": [
        "resourceType"
      ],
      "outputs": []
    },
    {
      "name": "BrowseAllResourcesBlade",
      "keyParameters": [],
      "inputs": [],
      "outputs": []
    },
    {
      "name": "BrowseAllFilteredResourcesBlade",
      "keyParameters": [],
      "inputs": [
        "filter"
      ],
      "outputs": []
    },
    {
      "name": "BrowseRecentResourcesBlade",
      "keyParameters": [],
      "inputs": [],
      "outputs": []
    },
    {
      "name": "BrowseResourceGroupBlade",
      "keyParameters": [],
      "inputs": [
        "resourceType"
      ],
      "outputs": []
    },
    {
      "name": "MapResourceGroupBlade",
      "keyParameters": [],
      "inputs": [
        "id"
      ],
      "outputs": []
    },
    {
      "name": "BrowseInstanceLinkBlade",
      "keyParameters": [],
      "inputs": [
        "resourceType"
      ],
      "outputs": []
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