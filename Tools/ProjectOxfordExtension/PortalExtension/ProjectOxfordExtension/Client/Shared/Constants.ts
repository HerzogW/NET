/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */

"use strict";
import Globalization = require("MsPortalFx/Globalization");
export var RPNamespace = "Microsoft.ProjectOxford";
export var accountsResource = "accounts";
export var ProjectOxfordApiTypeCategory = "ProjectOxford";

export var CognitiveServicesRPNamespace = "Microsoft.CognitiveServices";
export var CognitiveServicesAccountsResourceType = "accounts";
export var CognitiveServicesApiTypeCategory = "CognitiveServices"; // wildcard, all APIs are in this category 

export var ArmServiceEndpoint = window.fx.environment && window.fx.environment.armEndpoint && window.fx.environment.armEndpoint.replace(/\/$/, "");
export var ArmServiceVersion = "api-version=2016-02-01-preview";

export var OxfordServiceEndpoint = "https://api.projectoxford.ai/";
export var CognitiveServicesServiceEndpoint = "https://westus.api.cognitive.microsoft.com/";


//export var OxfordServiceEndpoint = "https://oxfordibiza.azure-api.net/"
export var LoadTimeout = 3000;
export var NameDelayValidationTimeout = 500;
export var SaveSelectedSpecTimeout = 2000;

export var fwlinks = {
	Home: "http://go.microsoft.com/fwlink/?LinkID=761227&clcid=0x409",
	Doc: "http://go.microsoft.com/fwlink/?LinkID=761228&clcid=0x409",
	Pricing: "http://go.microsoft.com/fwlink/?LinkID=761229&clcid=0x409",
};

export var CurrentLanguage = Globalization.displayLanguage;
export var DefaultLanguage = "en";
export var createDynamicSvgImage = FxImpl.DefinitionBuilder.createSvgImage;
export var ApiConfigRoot = MsPortalFx.Base.Resources.getAppRelativeUri("/apiconfiguration/");
