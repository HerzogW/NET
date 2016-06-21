/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */

"use strict";
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
export var ApiTypes = {
    ComputerVision: "ComputerVision",
    Emotion: "Emotion",
    Face: "Face",
    LUIS: "LUIS",
    Speech: "Speech",
    SpellCheck: "SpellCheck",
    Stabilization: "Stabilization",
    FaceDetection: "FaceDetection",
    MotionDetection: "MotionDetection",
    TextAnalytics: "TextAnalytics",
    Recommendations: "Recommendations",
	WebLM: "WebLM",
    Video: {
        Stabilization: "Stabilization",
        FaceDetection: "FaceDetection",
        MotionDetection: "MotionDetection",
    }
}

export var skusToCheck: string[] = ["F0"];

export var skusToCheckQuota: { [key: string]: number; } = { ["F0"]: 1 };

export var fwlinks = {
	Home: "http://go.microsoft.com/fwlink/?LinkID=761227&clcid=0x409",
	Doc: "http://go.microsoft.com/fwlink/?LinkID=761228&clcid=0x409",
	Pricing: "http://go.microsoft.com/fwlink/?LinkID=761229&clcid=0x409",
	API: {
		CodeOfConduct: "http://go.microsoft.com/fwlink/?LinkId=698895",
		Vision: {
			Detail: "http://go.microsoft.com/fwlink/?LinkID=761183&clcid=0x409",
			Doc: "http://go.microsoft.com/fwlink/?LinkID=761184&clcid=0x409",
			Sdk: "http://go.microsoft.com/fwlink/?LinkID=761185&clcid=0x409"
		},
		Emotion: {
			Detail: "http://go.microsoft.com/fwlink/?LinkID=761188&clcid=0x409",
			Doc: "http://go.microsoft.com/fwlink/?LinkID=761187&clcid=0x409",
			Sdk: "http://go.microsoft.com/fwlink/?LinkID=761186&clcid=0x409"
		},
		Face: {
			Detail: "http://go.microsoft.com/fwlink/?LinkID=761189&clcid=0x409",
			Doc: "http://go.microsoft.com/fwlink/?LinkID=761190&clcid=0x409",
			Sdk: "http://go.microsoft.com/fwlink/?LinkID=761209&clcid=0x409"
		},
		LUIS: {
			Detail: "http://go.microsoft.com/fwlink/?LinkID=761192&clcid=0x409",
			Doc: "http://go.microsoft.com/fwlink/?LinkID=761191&clcid=0x409",
			Sdk: "http://go.microsoft.com/fwlink/?LinkID=761207&clcid=0x409"
		},
		Speech: {
			Detail: "http://go.microsoft.com/fwlink/?LinkID=761193&clcid=0x409",
			Doc: "http://go.microsoft.com/fwlink/?LinkID=761194&clcid=0x409",
			Sdk: "http://go.microsoft.com/fwlink/?LinkID=761205&clcid=0x409"
		},
		WebLM: {
			Detail: "http://go.microsoft.com/fwlink/?LinkID=761200&clcid=0x409",
			Doc: "http://go.microsoft.com/fwlink/?LinkID=761199&clcid=0x409",
			Sdk: "http://go.microsoft.com/fwlink/?LinkID=761201&clcid=0x409"
		},
	},
};
