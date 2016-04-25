declare module AzurePortalExtension.DataModels {
    var ResourceBaseType: string;
    interface ResourceBase {
        id: KnockoutObservable<string>;
        name: KnockoutObservable<string>;
        type: KnockoutObservable<string>;
        location: KnockoutObservable<string>;
        etag: KnockoutObservable<string>;
    }
}
declare module AzurePortalExtension.DataModels {
    var RootResourceType: string;
    interface RootResource {
        properties: KnockoutObservable<AzurePortalExtension.DataModels.RootResourceProperties>;
        id: KnockoutObservable<string>;
        name: KnockoutObservable<string>;
        type: KnockoutObservable<string>;
        location: KnockoutObservable<string>;
        etag: KnockoutObservable<string>;
    }
}
declare module AzurePortalExtension.DataModels {
    var RootResourcePropertiesType: string;
    interface RootResourceProperties {
        customProperty: KnockoutObservable<string>;
        provisioningState: KnockoutObservable<string>;
    }
}
