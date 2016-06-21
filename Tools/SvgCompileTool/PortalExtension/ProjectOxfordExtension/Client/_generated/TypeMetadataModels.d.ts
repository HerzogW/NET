declare module ProjectOxfordExtension.DataModels {
    var KeysType: string;
    interface Keys {
        key1: KnockoutObservable<string>;
        key2: KnockoutObservable<string>;
    }
}
declare module ProjectOxfordExtension.DataModels {
    var ResourceBaseType: string;
    interface ResourceBase {
        id: KnockoutObservable<string>;
        name: KnockoutObservable<string>;
        type: KnockoutObservable<string>;
        location: KnockoutObservable<string>;
        kind: KnockoutObservable<string>;
    }
}
declare module ProjectOxfordExtension.DataModels {
    var AccountType: string;
    interface Account {
        properties: KnockoutObservable<ProjectOxfordExtension.DataModels.AccountProperties>;
        sku: KnockoutObservable<ProjectOxfordExtension.DataModels.Sku>;
        id: KnockoutObservable<string>;
        name: KnockoutObservable<string>;
        type: KnockoutObservable<string>;
        location: KnockoutObservable<string>;
        kind: KnockoutObservable<string>;
    }
}
declare module ProjectOxfordExtension.DataModels {
    var AccountPropertiesType: string;
    interface AccountProperties {
        provisioningState: KnockoutObservable<string>;
        endpoint: KnockoutObservable<string>;
    }
}
declare module ProjectOxfordExtension.DataModels {
    var SkuType: string;
    interface Sku {
        name: KnockoutObservable<string>;
    }
}
