declare module AzurePortalExtensionStudy.DataModels {
    var ResourceBaseType: string;
    interface ResourceBase {
        id: KnockoutObservable<string>;
        name: KnockoutObservable<string>;
        type: KnockoutObservable<string>;
        location: KnockoutObservable<string>;
        etag: KnockoutObservable<string>;
    }
}
declare module AzurePortalExtensionStudy.DataModels {
    var RootResourceType: string;
    interface RootResource {
        properties: KnockoutObservable<AzurePortalExtensionStudy.DataModels.RootResourceProperties>;
        id: KnockoutObservable<string>;
        name: KnockoutObservable<string>;
        type: KnockoutObservable<string>;
        location: KnockoutObservable<string>;
        etag: KnockoutObservable<string>;
    }
}
declare module AzurePortalExtensionStudy.DataModels {
    var RootResourcePropertiesType: string;
    interface RootResourceProperties {
        customProperty: KnockoutObservable<string>;
        provisioningState: KnockoutObservable<string>;
    }
}
