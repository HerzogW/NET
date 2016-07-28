/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */
"use strict";
define(["require", "exports"], function (require, exports) {
    var CreateData = (function () {
        function CreateData() {
            var _this = this;
            this.apiTypeList = ko.pureComputed(function () {
                var list = _this._apiTypeList();
                return list || [];
            });
            this._apiTypeList = ko.observableArray([]);
        }
        CreateData.prototype.getSpecData = function (apiType) {
            //     switch (apiType) {
            //         case (Constants.ApiTypes.Face):
            //             return <SpecPicker.SpecData>{
            //                 "specType": "Microsoft.ProjectOxford/faceApi",
            //                 "specs": [
            //                     {
            //                         "id": "F0",
            //                         "colorScheme": "mediumBlue",
            //                         "title": "Free",
            //                         "specCode": "F0",
            //                         "promotedFeatures": [
            //                             {
            //                                 "value": "20",
            //                                 "unitDescription": "Calls per minute"
            //                             },
            //                             {
            //                                 "value": "30K",
            //                                 "unitDescription": "Calls per month"
            //                             }
            //                         ],
            //                         "features": [
            //                             {
            //                                 "id": "faceDetection",
            //                             },
            //                             {
            //                                 "id": "faceVerification",
            //                             },
            //                             {
            //                                 "id": "faceIdentification",
            //                             },
            //                             {
            //                                 "id": "faceGrouping",
            //                             },
            //                             {
            //                                 "id": "similaFaceSearching",
            //                             },
            //                         ],
            //                         "cost": {
            //                             "amount": 0.0,
            //                             "currencyCode": "USD",
            //                             "caption": "{0} per call (Estimated)"
            //                         }
            //                     },
            //			{
            //                         "id": "S0",
            //                         "colorScheme": "mediumBlue",
            //                         "title": "Standard",
            //                         "specCode": "S0",
            //                         "promotedFeatures": [
            //                             {
            //                                 "value": "10",
            //                                 "unitDescription": "Calls per second"
            //                             }
            //                             //{
            //                             //    "value": "25M",
            //                             //    "unitDescription": "Calls per month"
            //                             //}
            //                         ],
            //                         "features": [
            //					{
            //                                 "id": "faceDetection",
            //                             },
            //                             {
            //                                 "id": "faceVerification",
            //                             },
            //                             {
            //                                 "id": "faceIdentification",
            //                             },
            //                             {
            //                                 "id": "faceGrouping",
            //                             },
            //                             {
            //                                 "id": "similaFaceSearching",
            //                             },
            //					//{
            //					//                            "id": "sla",
            //					//	"displayValue": "99%"
            //					//                        },
            //                         ],
            //                         "cost": {
            //                             "caption": "{0}/1000 calls (Estimated)"
            //                         }
            //                     }
            //                 ],
            //                 "features": [
            //			{
            //				"id": "faceDetection",
            //				"displayName": "Face Detection",
            //				"iconSvgData": SvgContent.faceDetection.data
            //			},
            //			{
            //				"id": "faceVerification",
            //				"displayName": "Face Verification",
            //				"iconSvgData": SvgContent.faceVerification.data
            //			},
            //			{
            //				"id": "faceIdentification",
            //				"displayName": "Face Identification",
            //				"iconSvgData": SvgContent.faceIdentification.data
            //			},
            //			{
            //				"id": "faceGrouping",
            //				"displayName": "Face Grouping",
            //				"iconSvgData": SvgContent.faceGrouping.data
            //			},
            //			{
            //				"id": "similaFaceSearching",
            //				"displayName": "Similar Face Searching",
            //				"iconSvgData": SvgContent.similarFaceSearching.data
            //			},
            //			{
            //				"id": "faceApi",
            //				"displayName": "Face API",
            //				"iconName": "PersonWithFriend"
            //			}
            //		],
            //                 "resourceMap": {
            //			"default": [
            //				//{
            //				//	"id": "F0",
            //				//	"firstParty": [
            //				//		{
            //				//			"resourceId": null,
            //				//			"quantity": 1.0
            //				//		}
            //				//	]
            //				//},
            //				{
            //					"id": "S0",
            //					"firstParty": [
            //						{
            //							"resourceId": "y8fx4nayuif995rumisrmegon3b3z6c6czax4nayrti71j3x5pdwtc7y4imyqeyy6a",
            //							"quantity": 1.0
            //						}
            //					]
            //				}
            //			]
            //		},
            //                 "specsToAllowZeroCost": [
            //                     "F0"
            //                 ]
            //             };
            //         case (Constants.ApiTypes.ComputerVision):
            //             return <SpecPicker.SpecData>{
            //                 "specType": "Microsoft.ProjectOxford/computerVisionApi",
            //                 "specs": [
            //                     {
            //                         "id": "F0",
            //                         "colorScheme": "mediumBlue",
            //                         "title": "Free",
            //                         "specCode": "F0",
            //                         "promotedFeatures": [
            //                             {
            //                                 "value": "20",
            //                                 "unitDescription": "Calls per minute"
            //                             },
            //                             {
            //                                 "value": "5K",
            //                                 "unitDescription": "Calls per month"
            //                             }
            //                         ],
            //                         "features": [
            //					{
            //						"id": "visionFeatureAnalysis"
            //					},
            //					{
            //						"id": "visionOCR"
            //					},
            //					{
            //						"id": "visionThumbnail"
            //					}
            //                         ],
            //                         "cost": {
            //                             "amount": 0.0,
            //                             "currencyCode": "USD",
            //                             "caption": "{0} per call (Estimated)"
            //                         }
            //                     },	
            //			{
            //                         "id": "S0",
            //                         "colorScheme": "mediumBlue",
            //                         "title": "Standard",
            //                         "specCode": "S0",
            //                         "promotedFeatures": [
            //                             {
            //                                 "value": "10",
            //                                 "unitDescription": "Calls per second"
            //                             }
            //                             //{
            //                             //    "value": "25M",
            //                             //    "unitDescription": "Calls per month"
            //                             //}
            //                         ],
            //                         "features": [
            //					//{
            //					//                            "id": "sla",
            //					//	"displayValue": "99%"
            //					//                        },
            //					{
            //						"id": "visionFeatureAnalysis"
            //					},
            //					{
            //						"id": "visionOCR"
            //					},
            //					{
            //						"id": "visionThumbnail"
            //                             }
            //                         ],
            //                         "cost": {
            //                             "caption": "{0}/1000 calls (Estimated)"
            //                         }
            //                     }					
            //                 ],
            //                 "features": [
            //			{
            //				"id": "visionApi",
            //				"displayName": "Computer Vision API",
            //				"iconName": "Tools"
            //			},
            //			{
            //				"id": "visionFeatureAnalysis",
            //				"displayName": "Feature Analysis",
            //				"iconSvgData": SvgContent.visionFeatureAnalysis.data
            //			},
            //			{
            //				"id": "visionOCR",
            //				"displayName": "OCR",
            //				"iconSvgData": SvgContent.visionOCR.data
            //			},
            //			{
            //				"id": "visionThumbnail",
            //				"displayName": "Thumbnail",
            //				"iconSvgData": SvgContent.visionThumbnail.data
            //			},
            //		],
            //		"resourceMap": {
            //			"default": [
            //				//{
            //				//	"id": "F0",
            //				//	"firstParty": [
            //				//		{
            //				//			"resourceId": null,
            //				//			"quantity": 1.0
            //				//		}
            //				//	]
            //				//},
            //				{
            //					"id": "S0",
            //					"firstParty": [
            //						{
            //							"resourceId": "y8fx4nayc76ezjs4a6nr9g17kjme4jjsouax4nayrti71j3x5pdwtc7y4imyqeyy6a",
            //							"quantity": 1.0
            //						}
            //					]
            //				}
            //			]
            //		},
            //                 "specsToAllowZeroCost": [
            //                     "F0"
            //                 ]
            //             };
            //         case (Constants.ApiTypes.Emotion):
            //             return <SpecPicker.SpecData>{
            //                 "specType": "Microsoft.ProjectOxford/emotionApi",
            //                 "specs": [
            //                     {
            //                         "id": "F0",
            //                         "colorScheme": "mediumBlue",
            //                         "title": "Free",
            //                         "specCode": "F0",
            //                         "promotedFeatures": [
            //                             {
            //                                 "value": "20",
            //                                 "unitDescription": "Calls per minute"
            //                             },
            //                             {
            //                                 "value": "30K",
            //                                 "unitDescription": "Calls per month"
            //                             }
            //                         ],
            //                         "features": [
            //					{
            //						"id": "emotionRecognition"
            //					}
            //                         ],
            //                         "cost": {
            //                             "amount": 0.0,
            //                             "currencyCode": "USD",
            //                             "caption": "{0} per call (Estimated)"
            //                         }
            //                     },
            //			{
            //                         "id": "S0",
            //                         "colorScheme": "mediumBlue",
            //                         "title": "Basic / Standard",
            //                         "specCode": "S0",
            //                         "promotedFeatures": [
            //                             {
            //                                 "value": "10",
            //                                 "unitDescription": "Calls per second"
            //                             }
            //                             //{
            //                             //    "value": "25M",
            //                             //    "unitDescription": "Calls per month"
            //                             //}
            //                         ],
            //                         "features": [
            //					//{
            //					//                            "id": "sla",
            //					//	"displayValue": "99%"
            //					//                        },
            //					{
            //						"id": "emotionRecognition"
            //                             }
            //                         ],
            //                         "cost": {
            //                             "caption": "Starting {0}/1000 calls (Estimated)"
            //                         }
            //                     }
            //                 ],
            //                 "features": [
            //			{
            //				"id": "emotionApi",
            //				"displayName": "Emotion API",
            //				"iconName": "Person"
            //			},
            //			{
            //				"id": "emotionRecognition",
            //				"displayName": "Recognition",
            //				"iconSvgData": SvgContent.emotionRecognition.data
            //			}
            //		],
            //		"resourceMap": {
            //			"default": [
            //				//{
            //				//	"id": "F0",
            //				//	"firstParty": [
            //				//		{
            //				//			"resourceId": null,
            //				//			"quantity": 1.0
            //				//		}
            //				//	]
            //				//},
            //				{
            //					"id": "S0",
            //					"firstParty": [
            //						{
            //							"resourceId": "y8fx4nayo1qgebq1qc8rjd83pcjs8g6od8ax4nayrti71j3x5pdwtc7y4imyqeyy6a",
            //							"quantity": 1.0
            //						},
            //						//{
            //						//	"resourceId": "y8fx4nay67yf5znpehirtpd67qhueidigzax4nayrti71j3x5pdwtc7y4imyqeyy6a",
            //						//	"quantity": 1.0
            //						//}
            //					]
            //				}
            //			]
            //		},
            //                 "specsToAllowZeroCost": [
            //                     "F0"
            //                 ]
            //             };
            //         case (Constants.ApiTypes.Speech):
            //             return <SpecPicker.SpecData>{
            //                 "specType": "Microsoft.ProjectOxford/speechApi",
            //                 "specs": [
            //                     {
            //                         "id": "F0",
            //                         "colorScheme": "mediumBlue",
            //                         "title": "Free",
            //                         "specCode": "F0",
            //                         "promotedFeatures": [
            //                             {
            //                                 "value": "5",
            //                                 "unitDescription": "Calls per second"
            //                             },
            //                             {
            //                                 "value": "5K",
            //                                 "unitDescription": "Calls per month"
            //                             }
            //                         ],
            //                         "features": [
            //					{
            //						"id": "speechConversion"
            //					},
            //					{
            //						"id": "speechRecognition"
            //					},
            //					{
            //						"id": "speechIntentRecognition"
            //					}
            //                         ],
            //                         "cost": {
            //                             "amount": 0.0,
            //                             "currencyCode": "USD",
            //                             "caption": "{0} per call (Estimated)"
            //                         }
            //                     },
            //			{
            //                         "id": "S0",
            //                         "colorScheme": "mediumBlue",
            //                         "title": "Standard",
            //                         "specCode": "S0",
            //                         "promotedFeatures": [
            //                             {
            //                                 "value": "20",
            //                                 "unitDescription": "Calls per second"
            //                             },
            //                         //    {
            //                         //        "value": "43K",
            //                         //        "unitDescription": "Calls per month"
            //                         //}
            //				],
            //                         "features": [
            //					//{
            //					//                            "id": "sla",
            //					//	"displayValue": "99%"
            //					//                        }
            //					{
            //						"id": "speechConversion"
            //					},
            //					{
            //						"id": "speechRecognition"
            //                     },
            //					{
            //						"id": "speechIntentRecognition"
            //					}
            //                 ],
            //                         "cost": {
            //                             "caption": "{0}/1000 calls (Estimated)"
            //                         }
            //                     }
            //                 ],
            //                 "features": [
            //			{
            //				"id": "speechConversion",
            //				"displayName": "Text to Speech Conversion",
            //				"iconSvgData": SvgContent.textToSpeechConversion.data
            //			},
            //			{
            //				"id": "speechRecognition",
            //				"displayName": "Recognition",
            //				"iconSvgData": SvgContent.speechRecognition.data
            //			},
            //			{
            //				"id": "speechIntentRecognition",
            //				"displayName": "Intent Recognition",
            //				"iconSvgData": SvgContent.speechIntentRecognition.data
            //			}
            //		],
            //		"resourceMap": {
            //			"default": [
            //				//{
            //				//	"id": "F0",
            //				//	"firstParty": [
            //				//		{
            //				//			"resourceId": null,
            //				//			"quantity": 1.0
            //				//		}
            //				//	]
            //				//},
            //				{
            //					"id": "S0",
            //					"firstParty": [
            //						{
            //							"resourceId": "y8fx4nayd1ize3g4gepwmmajgt6r8ppui8ax4nayrti71j3x5pdwtc7y4imyqeyy6a",
            //							"quantity": 1.0
            //						}
            //					]
            //				}
            //			]
            //		},
            //                 "specsToAllowZeroCost": [
            //                     "F0"
            //                 ]
            //             };
            //         case (Constants.ApiTypes.SpellCheck):
            //             return <SpecPicker.SpecData>{
            //                 "specType": "Microsoft.ProjectOxford/spellCheckApi",
            //                 "specs": [
            //                     {
            //                         "id": "F0",
            //                         "colorScheme": "mediumBlue",
            //                         "title": "Free",
            //                         "specCode": "F0",
            //                         "promotedFeatures": [
            //                             {
            //                                 "value": "7",
            //                                 "unitDescription": "Calls per minute"
            //                             },
            //                             {
            //                                 "value": "5K",
            //                                 "unitDescription": "Calls per month"
            //                             }
            //                         ],
            //                         "features": [
            //                         ],
            //                         "cost": {
            //                             "amount": 0.0,
            //                             "currencyCode": "USD",
            //                             "caption": "{0}/Month (Estimated)"
            //                         }
            //                     },
            //                 ],
            //                 "features": [],
            //                 "resourceMap": null,
            //                 "specsToAllowZeroCost": [
            //                     "F0"
            //                 ]
            //             };
            //         case (Constants.ApiTypes.LUIS):
            //             return <SpecPicker.SpecData>{
            //                 "specType": "Microsoft.ProjectOxford/luis",
            //                 "specs": [
            //                     {
            //                         "id": "F0",
            //                         "colorScheme": "mediumBlue",
            //                         "title": "Free",
            //                         "specCode": "F0",
            //                         "promotedFeatures": [
            //                             {
            //                                 "value": "5",
            //                                 "unitDescription": "Calls per second"
            //                             },
            //                             {
            //                                 "value": "100K",
            //                                 "unitDescription": "Calls per month"
            //                             }
            //                         ],
            //                         "features": [
            //					{
            //						"id": "luisUnderstanding"
            //                             }
            //                         ],
            //                         "cost": {
            //                             "amount": 0.0,
            //                             "currencyCode": "USD",
            //                             "caption": "{0} per call (Estimated)"
            //                         }
            //                     },
            //			{
            //                         "id": "S0",
            //                         "colorScheme": "mediumBlue",
            //                         "title": "Standard",
            //                         "specCode": "S0",
            //                         "promotedFeatures": [
            //                             {
            //                                 "value": "10",
            //                                 "unitDescription": "Calls per second"
            //                             }
            //                             //{
            //                             //    "value": "12M",
            //                             //    "unitDescription": "Calls per month"
            //                             //}
            //                         ],
            //                         "features": [
            //					{
            //						"id": "luisUnderstanding"
            //					}
            //                         ],
            //                         "cost": {
            //                             "caption": "{0}/1000 calls (Estimated)"
            //                         }
            //                     }
            //                 ],
            //                 "features": [
            //			{
            //				"id": "luisUnderstanding",
            //				"displayName": "Language understanding",
            //				"iconSvgData": SvgContent.languageUnderstanding.data
            //			}
            //		],
            //                 "resourceMap": {
            //			"default": [
            //				//{
            //				//	"id": "F0",
            //				//	"firstParty": [
            //				//		{
            //				//			"resourceId": null,
            //				//			"quantity": 1.0
            //				//		}
            //				//	]
            //				//},
            //				{
            //					"id": "S0",
            //					"firstParty": [
            //						{
            //							"resourceId": "y8fx4nayj3mpjyt7nqcwpcf3o8hnid387uax4nayrti71j3x5pdwtc7y4imyqeyy6a",
            //							"quantity": 1.0
            //						}
            //					]
            //				}
            //			]
            //		},
            //                 "specsToAllowZeroCost": [
            //                     "F0"
            //                 ]
            //             };
            //         case (Constants.ApiTypes.Video.Stabilization):
            //             return <SpecPicker.SpecData>{
            //                 "specType": "Microsoft.ProjectOxford/videoStabilizationApi",
            //                 "specs": [
            //                     {
            //                         "id": "F0",
            //                         "colorScheme": "mediumBlue",
            //                         "title": "Free",
            //                         "specCode": "F0",
            //                         "promotedFeatures": [
            //                             {
            //                                 "value": "0.33",
            //                                 "unitDescription": "Calls per minute"
            //                             },
            //                             {
            //                                 "value": "100",
            //                                 "unitDescription": "Calls per month"
            //                             }
            //                         ],
            //                         "features": [
            //                         ],
            //                         "cost": {
            //                             "amount": 0.0,
            //                             "currencyCode": "USD",
            //                             "caption": "{0}/Month (Estimated)"
            //                         }
            //                     },
            //                 ],
            //                 "features": [],
            //                 "resourceMap": null,
            //                 "specsToAllowZeroCost": [
            //                     "F0"
            //                 ]
            //             };
            //         case (Constants.ApiTypes.Video.FaceDetection):
            //             return <SpecPicker.SpecData>{
            //                 "specType": "Microsoft.ProjectOxford/videoFaceDetectionApi",
            //                 "specs": [
            //                     {
            //                         "id": "F0",
            //                         "colorScheme": "mediumBlue",
            //                         "title": "Free",
            //                         "specCode": "F0",
            //                         "promotedFeatures": [
            //                             {
            //                                 "value": "0.33",
            //                                 "unitDescription": "Calls per minute"
            //                             },
            //                             {
            //                                 "value": "100",
            //                                 "unitDescription": "Calls per month"
            //                             }
            //                         ],
            //                         "features": [
            //                         ],
            //                         "cost": {
            //                             "amount": 0.0,
            //                             "currencyCode": "USD",
            //                             "caption": "{0}/Month (Estimated)"
            //                         }
            //                     },
            //                 ],
            //                 "features": [],
            //                 "resourceMap": null,
            //                 "specsToAllowZeroCost": [
            //                     "F0"
            //                 ]
            //             };
            //         case (Constants.ApiTypes.Video.MotionDetection):
            //             return <SpecPicker.SpecData>{
            //                 "specType": "Microsoft.ProjectOxford/videoMotionDetectionApi",
            //                 "specs": [
            //                     {
            //                         "id": "F0",
            //                         "colorScheme": "mediumBlue",
            //                         "title": "Free",
            //                         "specCode": "F0",
            //                         "promotedFeatures": [
            //                             {
            //                                 "value": "0.33",
            //                                 "unitDescription": "Calls per minute"
            //                             },
            //                             {
            //                                 "value": "100",
            //                                 "unitDescription": "Calls per month"
            //                             }
            //                         ],
            //                         "features": [
            //                         ],
            //                         "cost": {
            //                             "amount": 0.0,
            //                             "currencyCode": "USD",
            //                             "caption": "{0}/Month (Estimated)"
            //                         }
            //                     },
            //                 ],
            //                 "features": [],
            //                 "resourceMap": null,
            //                 "specsToAllowZeroCost": [
            //                     "F0"
            //                 ]
            //             };
            //         case (Constants.ApiTypes.TextAnalytics):
            //             return <SpecPicker.SpecData>{
            //                 "specType": "Microsoft.CognitiveServices/textAnalyticsApi",
            //                 "specs": [
            //                     {
            //                         "id": "F0",
            //                         "colorScheme": "mediumBlue",
            //                         "title": ClientResources.PricingTier.free,
            //                         "specCode": "F0",
            //                         "promotedFeatures": [
            //                             {
            //                                 "value": "5K",
            //                                 "unitDescription": "Calls per 30 days"
            //                             }
            //                         ],
            //                         "features": [
            //                             { "id": "sentiment" },
            //                             { "id": "keyPhrases" },
            //                             { "id": "topics" },
            //                             { "id": "language" },
            //                         ],
            //                         "cost": {
            //                             "amount": 0.0,
            //                             "currencyCode": "USD",
            //                             "caption": "{0} per 30 days"
            //                         }
            //                     },
            //                     {
            //                         "id": "S1",
            //                         "colorScheme": "mediumBlue",
            //                         "title": ClientResources.PricingTier.standard,
            //                         "specCode": "S1",
            //                         "promotedFeatures": [
            //                             {
            //                                 "value": "100K",
            //                                 "unitDescription": "Calls per 30 days"
            //                             }
            //                         ],
            //                         "features": [
            //                             { "id": "sentiment" },
            //                             { "id": "keyPhrases" },
            //                             { "id": "topics" },
            //                             { "id": "language" },
            //                         ],
            //                         "cost": {
            //                             "amount": 150.0,
            //                             "currencyCode": "USD",
            //                             "caption": "{0} per 30 days"
            //                         }
            //                     },
            //                     {
            //                         "id": "S2",
            //                         "colorScheme": "mediumBlue",
            //                         "title": ClientResources.PricingTier.standard,
            //                         "specCode": "S2",
            //                         "promotedFeatures": [
            //                             {
            //                                 "value": "500K",
            //                                 "unitDescription": "Calls per 30 days"
            //                             }
            //                         ],
            //                         "features": [
            //                             { "id": "sentiment" },
            //                             { "id": "keyPhrases" },
            //                             { "id": "topics" },
            //                             { "id": "language" },
            //                         ],
            //                         "cost": {
            //                             "amount": 500.0,
            //                             "currencyCode": "USD",
            //                             "caption": "{0} per 30 days"
            //                         }
            //                     },
            //                     {
            //                         "id": "S3",
            //                         "colorScheme": "mediumBlue",
            //                         "title": ClientResources.PricingTier.standard,
            //                         "specCode": "S3",
            //                         "promotedFeatures": [
            //                             {
            //                                 "value": "2.5M",
            //                                 "unitDescription": "Calls per 30 days"
            //                             }
            //                         ],
            //                         "features": [
            //                             { "id": "sentiment" },
            //                             { "id": "keyPhrases" },
            //                             { "id": "topics" },
            //                             { "id": "language" },
            //                         ],
            //                         "cost": {
            //                             "amount": 1250.0,
            //                             "currencyCode": "USD",
            //                             "caption": "{0} per 30 days"
            //                         }
            //                     },
            //                     {
            //                         "id": "S4",
            //                         "colorScheme": "mediumBlue",
            //                         "title": ClientResources.PricingTier.standard,
            //                         "specCode": "S4",
            //                         "promotedFeatures": [
            //                             {
            //                                 "value": "10M",
            //                                 "unitDescription": "Calls per 30 days"
            //                             }
            //                         ],
            //                         "features": [
            //                             { "id": "sentiment" },
            //                             { "id": "keyPhrases" },
            //                             { "id": "topics" },
            //                             { "id": "language" },
            //                         ],
            //                         "cost": {
            //                             "amount": 2500.0,
            //                             "currencyCode": "USD",
            //                             "caption": "{0} per 30 days"
            //                         }
            //                     },
            //                 ],
            //                 "features": [
            //                     {
            //                         "id": "sentiment",
            //                         "displayName": ClientResources.ApiType.TextAnalytics.Features.sentiment,
            //                         "iconSvgData": SvgContent.sentimentAnalysis.data
            //                     },
            //                     {
            //                         "id": "keyPhrases",
            //                         "displayName": ClientResources.ApiType.TextAnalytics.Features.keyPhrase,
            //                         "iconSvgData": SvgContent.keyPhrasesExtraction.data
            //                     },
            //                     {
            //                         "id": "topics",
            //                         "displayName": ClientResources.ApiType.TextAnalytics.Features.topicDetection,
            //                         "iconSvgData": SvgContent.topicDetection.data
            //                     },
            //                     {
            //                         "id": "language",
            //                         "displayName": ClientResources.ApiType.TextAnalytics.Features.languageDetection,
            //                         "iconSvgData": SvgContent.languageDetection.data
            //                     },
            //                 ],
            //                 "resourceMap": {
            //                     "default": [
            //                         {
            //                             "id": "F0",
            //                             "firstParty": [
            //                                 {
            //                                     "resourceId": "y8fx4nayicqgm8d3xzfrjj9pr74m7k1immax4nayrti71j3x5pdwtc7y4imyqeyy6a",
            //                                     "quantity": 1.0
            //                                 }
            //                             ]
            //                         },
            //                         {
            //                             "id": "S1",
            //                             "firstParty": [
            //                                 {
            //                                     "resourceId": "y8fx4naypeswds3wpuorxj1zaosy993e39ax4nayrti71j3x5pdwtc7y4imyqeyy6a",
            //                                     "quantity": 1.0
            //                                 }
            //                             ]
            //                         },
            //                         {
            //                             "id": "S2",
            //                             "firstParty": [
            //                                 {
            //                                     "resourceId": "y8fx4nayydnmdcj7rtrwjeqt5p5fd8i1ixax4nayrti71j3x5pdwtc7y4imyqeyy6a",
            //                                     "quantity": 1.0
            //                                 }
            //                             ]
            //                         },
            //                         {
            //                             "id": "S3",
            //                             "firstParty": [
            //                                 {
            //                                     "resourceId": "y8fx4nayof5tquob8wmw7dgjmtnh5181o5ax4nayrti71j3x5pdwtc7y4imyqeyy6a",
            //                                     "quantity": 1.0
            //                                 }
            //                             ]
            //                         },
            //                         {
            //                             "id": "S4",
            //                             "firstParty": [
            //                                 {
            //                                     "resourceId": "y8fx4nayedtcht5izkrw8fcp8sacr5jyf5ax4nayrti71j3x5pdwtc7y4imyqeyy6a",
            //                                     "quantity": 1.0
            //                                 }
            //                             ]
            //                         },
            //                     ]
            //                 },
            //                 "specsToAllowZeroCost": [
            //                     "F0"
            //                 ]
            //             };
            //         case (Constants.ApiTypes.Recommendations):
            //             return <SpecPicker.SpecData>{
            //                 "specType": "Microsoft.CognitiveServices/recommendationsApi", //TODO: what is spec type
            //                 "specs": [
            //                     {
            //                         "id": "F0",
            //                         "colorScheme": "mediumBlue",
            //                         "title": ClientResources.PricingTier.free,
            //                         "specCode": "F0",
            //                         "promotedFeatures": [
            //                             {
            //                                 "value": "10K",
            //                                 "unitDescription": "Calls per 30 days"
            //                             }
            //                         ],
            //                         "features": [
            //                         ],
            //                         "cost": {
            //                             "amount": 0.0,
            //                             "currencyCode": "USD",
            //                             "caption": "{0} per 30 days"
            //                         }
            //                     },
            //                     {
            //                         "id": "S1",
            //                         "colorScheme": "mediumBlue",
            //                         "title": ClientResources.PricingTier.standard,
            //                         "specCode": "S1",
            //                         "promotedFeatures": [
            //                             {
            //                                 "value": "100K",
            //                                 "unitDescription": "Calls per 30 days"
            //                             }
            //                         ],
            //                         "features": [
            //                         ],
            //                         "cost": {
            //                             "amount": 150.0,
            //                             "currencyCode": "USD",
            //                             "caption": "{0} per 30 days"
            //                         }
            //                     },
            //                     {
            //                         "id": "S2",
            //                         "colorScheme": "mediumBlue",
            //                         "title": ClientResources.PricingTier.standard,
            //                         "specCode": "S2",
            //                         "promotedFeatures": [
            //                             {
            //                                 "value": "1M",
            //                                 "unitDescription": "Calls per 30 days"
            //                             }
            //                         ],
            //                         "features": [
            //                         ],
            //                         "cost": {
            //                             "amount": 500.0,
            //                             "currencyCode": "USD",
            //                             "caption": "{0} per 30 days"
            //                         }
            //                     },
            //                     {
            //                         "id": "S3",
            //                         "colorScheme": "mediumBlue",
            //                         "title": ClientResources.PricingTier.standard,
            //                         "specCode": "S3",
            //                         "promotedFeatures": [
            //                             {
            //                                 "value": "10M",
            //                                 "unitDescription": "Calls per 30 days"
            //                             }
            //                         ],
            //                         "features": [
            //                         ],
            //                         "cost": {
            //                             "amount": 2500.0,
            //                             "currencyCode": "USD",
            //                             "caption": "{0} per 30 days"
            //                         }
            //                     },
            //                     {
            //                         "id": "S4",
            //                         "colorScheme": "mediumBlue",
            //                         "title": ClientResources.PricingTier.standard,
            //                         "specCode": "S4",
            //                         "promotedFeatures": [
            //                             {
            //                                 "value": "50M",
            //                                 "unitDescription": "Calls per 30 days"
            //                             }
            //                         ],
            //                         "features": [
            //                         ],
            //                         "cost": {
            //                             "amount": 5000.0,
            //                             "currencyCode": "USD",
            //                             "caption": "{0} per 30 days"
            //                         }
            //			}
            //                 ],
            //                 "features": [],
            //                 "resourceMap": {
            //                     "default": [
            //                         {
            //                             "id": "F0",
            //                             "firstParty": [
            //                                 {
            //                                     "resourceId": "y8fx4nayskcnt69gputwp8s6j4bmwt19zdax4nayrti71j3x5pdwtc7y4imyqeyy6a",
            //                                     "quantity": 1.0
            //                                 }
            //                             ]
            //                         },
            //                         {
            //                             "id": "S1",
            //                             "firstParty": [
            //                                 {
            //                                     "resourceId": "y8fx4nayqifzhih8fx1r58ueyb4s1jkf59ax4nayrti71j3x5pdwtc7y4imyqeyy6a",
            //                                     "quantity": 1.0
            //                                 }
            //                             ]
            //                         },
            //                         {
            //                             "id": "S2",
            //                             "firstParty": [
            //                                 {
            //                                     "resourceId": "y8fx4naytfpymoekpnerdes5ko9b9ja5fxax4nayrti71j3x5pdwtc7y4imyqeyy6a",
            //                                     "quantity": 1.0
            //                                 }
            //                             ]
            //                         },
            //                         {
            //                             "id": "S3",
            //                             "firstParty": [
            //                                 {
            //                                     "resourceId": "y8fx4nayznwk55zetoiwbpfbki8z7xrq15ax4nayrti71j3x5pdwtc7y4imyqeyy6a",
            //                                     "quantity": 1.0
            //                                 }
            //                             ]
            //                         },
            //                         {
            //                             "id": "S4",
            //                             "firstParty": [
            //                                 {
            //                                     "resourceId": "y8fx4naycomak887nauwipknm7ih1cx478ax4nayrti71j3x5pdwtc7y4imyqeyy6a",
            //                                     "quantity": 1.0
            //                                 }
            //                             ]
            //                         },
            //                     ]
            //                 },
            //                 "specsToAllowZeroCost": [
            //                     "F0"
            //                 ]
            //             };
            //case (Constants.ApiTypes.WebLM):
            //             return <SpecPicker.SpecData>{
            //		"specType": "Microsoft.ProjectOxford/webLMApi",
            //		"specs": [
            //			{
            //				"id": "F0",
            //				"colorScheme": "mediumBlue",
            //				"title": "Free",
            //				"specCode": "F0",
            //				"promotedFeatures": [
            //					{
            //						"value": "1000",
            //						"unitDescription": "Calls per minute"
            //					},
            //					{
            //						"value": "100K",
            //						"unitDescription": "Calls per month"
            //					}
            //				],
            //				"features": [
            //					{
            //						"id": "webLanguageModel"
            //					}
            //				],
            //				"cost": {
            //					"amount": 0.0,
            //					"currencyCode": "USD",
            //					"caption": "{0} per call (Estimated)"
            //				}
            //                     },
            //			{
            //				"id": "S0",
            //				"colorScheme": "mediumBlue",
            //				"title": "Standard",
            //				"specCode": "S0",
            //				"promotedFeatures": [
            //					{
            //						"value": "1000",
            //						"unitDescription": "Calls per second"
            //					}
            //					//{
            //					//	"value": "100K",
            //					//	"unitDescription": "Calls per month"
            //					//}
            //                 ],
            //				"features": [
            //					{
            //						"id": "webLanguageModel"
            //					}
            //				],
            //				"cost": {
            //					//"amount": 0.1,
            //					//"currencyCode": "USD",
            //					"caption": "{0}/1000 calls (Estimated)"
            //				}
            //			}
            //		],
            //                 "features": [
            //			{
            //				"id": "webLanguageModel",
            //				"displayName": "Language Model",
            //				"iconSvgData": SvgContent.webLanguageModel.data
            //			}
            //		],
            //		"resourceMap": {
            //			"default": [
            //				//{
            //				//	"id": "F0",
            //				//	"firstParty": [
            //				//		{
            //				//			"resourceId": null,
            //				//			"quantity": 1.0
            //				//		}
            //				//	]
            //				//},
            //				{
            //					"id": "S0",
            //					"firstParty": [
            //						{
            //							"resourceId": "y8fx4nay5ddirhjw7dhwdkkzy9jeirky88ax4nayrti71j3x5pdwtc7y4imyqeyy6a",
            //							"quantity": 1.0
            //						}
            //					]
            //				}
            //			]
            //		},
            //                 "specsToAllowZeroCost": [
            //                     "F0"
            //                 ]
            //             };
            //     }
            return null;
        };
        return CreateData;
    })();
    exports.CreateData = CreateData;
});
