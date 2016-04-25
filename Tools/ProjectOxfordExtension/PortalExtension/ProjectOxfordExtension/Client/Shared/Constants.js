/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */
"use strict";
define(["require", "exports", "MsPortalFx/Globalization"], function (require, exports, Globalization) {
    exports.RPNamespace = "Microsoft.ProjectOxford";
    exports.accountsResource = "accounts";
    exports.ProjectOxfordApiTypeCategory = "ProjectOxford";
    exports.CognitiveServicesRPNamespace = "Microsoft.CognitiveServices";
    exports.CognitiveServicesAccountsResourceType = "accounts";
    exports.CognitiveServicesApiTypeCategory = "CognitiveServices"; // wildcard, all APIs are in this category 
    exports.ArmServiceEndpoint = window.fx.environment && window.fx.environment.armEndpoint && window.fx.environment.armEndpoint.replace(/\/$/, "");
    exports.ArmServiceVersion = "api-version=2016-02-01-preview";
    exports.OxfordServiceEndpoint = "https://api.projectoxford.ai/";
    exports.CognitiveServicesServiceEndpoint = "https://westus.api.cognitive.microsoft.com/";
    //export var OxfordServiceEndpoint = "https://oxfordibiza.azure-api.net/"
    exports.LoadTimeout = 3000;
    exports.NameDelayValidationTimeout = 500;
    exports.SaveSelectedSpecTimeout = 2000;
    exports.fwlinks = {
        Home: "http://go.microsoft.com/fwlink/?LinkID=761227&clcid=0x409",
        Doc: "http://go.microsoft.com/fwlink/?LinkID=761228&clcid=0x409",
        Pricing: "http://go.microsoft.com/fwlink/?LinkID=761229&clcid=0x409",
    };
    exports.CurrentLanguage = Globalization.displayLanguage;
    exports.DefaultLanguage = "en";
    exports.createDynamicSvgImage = FxImpl.DefinitionBuilder.createSvgImage;
    exports.ApiConfigRoot = MsPortalFx.Base.Resources.getAppRelativeUri("/apiconfiguration/");
});
