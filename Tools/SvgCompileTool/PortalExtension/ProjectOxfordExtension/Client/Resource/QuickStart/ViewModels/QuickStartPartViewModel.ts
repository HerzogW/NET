/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */

"use strict";
import ClientResources = require("ClientResources");
import ExtensionDefinition = require("../../../_generated/ExtensionDefinition");
import ResourceArea = require("../../ResourceArea");
import BaseImages = MsPortalFx.Base.Images;
import InfoList = MsPortalFx.ViewModels.Parts.InfoList;
import Constants = require("../../../Shared/Constants");
import Def = ExtensionDefinition.ViewModels.Resource.QuickStartPartViewModel;

export class QuickStartPartViewModel extends MsPortalFx.ViewModels.Parts.InfoList.ViewModel implements Def.Contract {
    private _accountView: MsPortalFx.Data.EntityView<ProjectOxfordExtension.DataModels.Account, any>;
    private _resourceId: KnockoutObservableBase<string>;

    constructor(container: MsPortalFx.ViewModels.PartContainerContract, initialState: any, dataContext: ResourceArea.DataContext) {
        this._accountView = dataContext.resourceData.resourceEntities.createView(container);
        this._resourceId = ko.observable<string>();
        super(initialState);
    }

    public onInputsSet(inputs: Def.InputsContract, settings: any): MsPortalFx.Base.Promise {
        this._resourceId(inputs.id);
        this._accountView.fetch(inputs.id);
        this.populateSections();
        return null;
    }

    private populateSections() {
        var metaData = this.getQuickStartSections();
        for (var i = 0; i < metaData.length; i++) {
            this.addSection(metaData[i].title, metaData[i].description, metaData[i].links, metaData[i].icon);
        }
    }

    private getQuickStartSections() {
        
        switch (this._accountView.item().kind()) {
            case (Constants.ApiTypes.ComputerVision):
                return [{
                    title: ClientResources.QuickStartBlade.Common.title1,
                    description: ClientResources.QuickStartBlade.ComputerVision.description1,
                    links: [new InfoList.Link(ClientResources.QuickStartBlade.CognitiveServices.about, Constants.fwlinks.Home),
                        new InfoList.Link(ClientResources.ApiType.ComputerVision.title, Constants.fwlinks.API.Vision.Detail)],
                    icon: BaseImages.Info()
                },
                    {
                        title: ClientResources.QuickStartBlade.Common.title2,
                        description: ClientResources.QuickStartBlade.ComputerVision.description2,
                        links: [new InfoList.Link(ClientResources.QuickStartBlade.apiDoc, Constants.fwlinks.API.Vision.Doc),
                            new InfoList.Link(ClientResources.QuickStartBlade.developerCodeOfConduct, Constants.fwlinks.API.CodeOfConduct)],
                        icon: BaseImages.Publish()
                    },
                    {
                        title: ClientResources.QuickStartBlade.Common.title3,
                        description: ClientResources.QuickStartBlade.ComputerVision.description3,
                        links: [new InfoList.Link(ClientResources.QuickStartBlade.apiSdk, Constants.fwlinks.API.Vision.Sdk)],
                        icon: BaseImages.Tools()
                    }];
            case (Constants.ApiTypes.Emotion):
                return [{
                    title: ClientResources.QuickStartBlade.Common.title1,
                    description: ClientResources.QuickStartBlade.Emotion.description1,
                    links: [new InfoList.Link(ClientResources.QuickStartBlade.CognitiveServices.about, Constants.fwlinks.Home),
                        new InfoList.Link(ClientResources.ApiType.Emotion.title, Constants.fwlinks.API.Emotion.Detail)],
                    icon: BaseImages.Info()
                },
                    {
                        title: ClientResources.QuickStartBlade.Common.title2,
                        description: ClientResources.QuickStartBlade.Emotion.description2,
                        links: [
                            new InfoList.Link(ClientResources.QuickStartBlade.apiDoc, Constants.fwlinks.API.Emotion.Doc),
                            new InfoList.Link(ClientResources.QuickStartBlade.developerCodeOfConduct, Constants.fwlinks.API.CodeOfConduct)],
                        icon: BaseImages.Publish()
                    },
                    {
                        title: ClientResources.QuickStartBlade.Common.title3,
                        description: ClientResources.QuickStartBlade.Emotion.description3,
                        links: [new InfoList.Link(ClientResources.QuickStartBlade.apiSdk, Constants.fwlinks.API.Emotion.Sdk)
                        ],
                        icon: BaseImages.Tools()
                    }];
            case (Constants.ApiTypes.Face):
                return [{
                    title: ClientResources.QuickStartBlade.Common.title1,
                    description: ClientResources.QuickStartBlade.Face.description1,
                    links: [new InfoList.Link(ClientResources.QuickStartBlade.CognitiveServices.about, Constants.fwlinks.Home),
                        new InfoList.Link(ClientResources.ApiType.Face.title, Constants.fwlinks.API.Face.Detail)],
                    icon: BaseImages.Info()
                },
                    {
                        title: ClientResources.QuickStartBlade.Common.title2,
                        description: ClientResources.QuickStartBlade.Face.description2,
                        links: [
                            new InfoList.Link(ClientResources.QuickStartBlade.apiDoc, Constants.fwlinks.API.Face.Doc),
                            new InfoList.Link(ClientResources.QuickStartBlade.developerCodeOfConduct, Constants.fwlinks.API.CodeOfConduct)],
                        icon: BaseImages.Publish()
                    },
                    {
                        title: ClientResources.QuickStartBlade.Common.title3,
                        description: ClientResources.QuickStartBlade.Face.description3,
                        links: [new InfoList.Link(ClientResources.QuickStartBlade.apiSdk, Constants.fwlinks.API.Face.Sdk)],
                        icon: BaseImages.Tools()
                    }];
            case (Constants.ApiTypes.LUIS):
                return [{
                    title: ClientResources.QuickStartBlade.Common.title1,
                    description: ClientResources.QuickStartBlade.LUIS.description1,
                    links: [new InfoList.Link(ClientResources.QuickStartBlade.CognitiveServices.about, Constants.fwlinks.Home),
                        new InfoList.Link(ClientResources.ApiType.LUIS.title, Constants.fwlinks.API.LUIS.Detail)
                    ],
                    icon: BaseImages.Info()
                },
                    {
                        title: ClientResources.QuickStartBlade.Common.title2,
                        description: ClientResources.QuickStartBlade.LUIS.description2,
                        links: [new InfoList.Link(ClientResources.QuickStartBlade.apiDoc, Constants.fwlinks.API.LUIS.Doc)],
                        icon: BaseImages.Publish()
                    }];
            case (Constants.ApiTypes.Speech):
                return [{
                    title: ClientResources.QuickStartBlade.Common.title1,
                    description: ClientResources.QuickStartBlade.Speech.description1,
                    links: [new InfoList.Link(ClientResources.QuickStartBlade.CognitiveServices.about, Constants.fwlinks.Home),
                        new InfoList.Link(ClientResources.ApiType.Speech.title, Constants.fwlinks.API.Speech.Detail)],
                    icon: BaseImages.Info()
                },
                    {
                        title: ClientResources.QuickStartBlade.Common.title2,
                        description: ClientResources.QuickStartBlade.Speech.description2,
                        links: [
                            new InfoList.Link(ClientResources.QuickStartBlade.apiDoc, Constants.fwlinks.API.Speech.Doc)],
                        icon: BaseImages.Publish()
                    },
                    {
                        title: ClientResources.QuickStartBlade.Common.title3,
                        description: ClientResources.QuickStartBlade.Speech.description3,
                        links: [new InfoList.Link(ClientResources.QuickStartBlade.apiSdk, Constants.fwlinks.API.Speech.Sdk)],
                        icon: BaseImages.Tools()
                    }];
            case (Constants.ApiTypes.SpellCheck):
                return [{
                    title: ClientResources.QuickStartBlade.Common.title1,
                    description: ClientResources.QuickStartBlade.SpellCheck.description1,
                    links: [new InfoList.Link(ClientResources.QuickStartBlade.CognitiveServices.about, "https://www.projectoxford.ai"),
                        new InfoList.Link(ClientResources.ApiType.SpellCheck.title, "https://www.projectoxford.ai/spellcheck"),
                        new InfoList.Link(ClientResources.QuickStartBlade.liveDemos, "https://www.projectoxford.ai/demo/spellcheck")],
                    icon: BaseImages.Info()
                },
                    {
                        title: ClientResources.QuickStartBlade.Common.title2,
                        description: ClientResources.QuickStartBlade.SpellCheck.description2,
                        links: [
                            new InfoList.Link(ClientResources.QuickStartBlade.getStarted, "https://msdn.microsoft.com/en-us/library/mt634663.aspx"),
                            new InfoList.Link(ClientResources.QuickStartBlade.apiReference, "https://dev.projectoxford.ai/docs/services/552ca72149c3f714647273f4")],
                        icon: BaseImages.Publish()
                    },
                    {
                        title: ClientResources.QuickStartBlade.Common.title3,
                        description: ClientResources.QuickStartBlade.SpellCheck.description3,
                        links: [new InfoList.Link("Windows", "https://www.projectoxford.ai/SDK/GetFile?path=text/Text-SpellCheck-SDK-CSharp.zip")
                        ],
                        icon: BaseImages.Tools()
                    }];
            case (Constants.ApiTypes.Video.FaceDetection):
                return [{
                    title: ClientResources.QuickStartBlade.Common.title1,
                    description: ClientResources.QuickStartBlade.Video.FaceDetection.description1,
                    links: [new InfoList.Link(ClientResources.QuickStartBlade.CognitiveServices.about, "https://www.projectoxford.ai"),
                        new InfoList.Link(ClientResources.ApiType.Video.FaceDetection.title, "https://www.projectoxford.ai/video"),
                        new InfoList.Link(ClientResources.QuickStartBlade.liveDemos, "https://www.projectoxford.ai/demo/video#faceDetection")],
                    icon: BaseImages.Info()
                },
                    {
                        title: ClientResources.QuickStartBlade.Common.title2,
                        description: ClientResources.QuickStartBlade.Video.FaceDetection.description2,
                        links: [new InfoList.Link(ClientResources.QuickStartBlade.apiReference, "https://dev.projectoxford.ai/docs/services/565d6516778daf15800928d5")],
                        icon: BaseImages.Publish()
                    },
                    {
                        title: ClientResources.QuickStartBlade.Common.title3,
                        description: ClientResources.QuickStartBlade.Video.FaceDetection.description3,
                        links: [new InfoList.Link("Windows", "https://www.projectoxford.ai/SDK/GetFile?path=video/Video-SDK-Windows.zip"),
                        ],
                        icon: BaseImages.Tools()
                    }];
            case (Constants.ApiTypes.Video.MotionDetection):
                return [{
                    title: ClientResources.QuickStartBlade.Common.title1,
                    description: ClientResources.QuickStartBlade.Video.MotionDetection.description1,
                    links: [new InfoList.Link(ClientResources.QuickStartBlade.CognitiveServices.about, "https://www.projectoxford.ai"),
                        new InfoList.Link(ClientResources.ApiType.Video.MotionDetection.title, "https://www.projectoxford.ai/video"),
                        new InfoList.Link(ClientResources.QuickStartBlade.liveDemos, "https://www.projectoxford.ai/demo/video#motionDetection")],
                    icon: BaseImages.Info()
                },
                    {
                        title: ClientResources.QuickStartBlade.Common.title2,
                        description: ClientResources.QuickStartBlade.Video.MotionDetection.description2,
                        links: [new InfoList.Link(ClientResources.QuickStartBlade.apiReference, "https://dev.projectoxford.ai/docs/services/565d6516778daf15800928d5")],
                        icon: BaseImages.Publish()
                    },
                    {
                        title: ClientResources.QuickStartBlade.Common.title3,
                        description: ClientResources.QuickStartBlade.Video.MotionDetection.description3,
                        links: [new InfoList.Link("Windows", "https://www.projectoxford.ai/SDK/GetFile?path=video/Video-SDK-Windows.zip"),
                        ],
                        icon: BaseImages.Tools()
                    }];
            case (Constants.ApiTypes.Video.Stabilization):
                return [{
                    title: ClientResources.QuickStartBlade.Common.title1,
                    description: ClientResources.QuickStartBlade.Video.Stabilization.description1,
                    links: [new InfoList.Link(ClientResources.QuickStartBlade.CognitiveServices.about, "https://www.projectoxford.ai"),
                        new InfoList.Link(ClientResources.ApiType.Video.Stabilization.title, "https://www.projectoxford.ai/video"),
                        new InfoList.Link(ClientResources.QuickStartBlade.liveDemos, "https://www.projectoxford.ai/demo/video#stabilization")],
                    icon: BaseImages.Info()
                },
                    {
                        title: ClientResources.QuickStartBlade.Common.title2,
                        description: ClientResources.QuickStartBlade.Video.Stabilization.description2,
                        links: [new InfoList.Link(ClientResources.QuickStartBlade.apiReference, "https://dev.projectoxford.ai/docs/services/565d6516778daf15800928d5")],
                        icon: BaseImages.Publish()
                    },
                    {
                        title: ClientResources.QuickStartBlade.Common.title3,
                        description: ClientResources.QuickStartBlade.Video.Stabilization.description3,
                        links: [new InfoList.Link("Windows", "https://www.projectoxford.ai/SDK/GetFile?path=video/Video-SDK-Windows.zip"),
                        ],
                        icon: BaseImages.Tools()
                    }];
            case (Constants.ApiTypes.TextAnalytics):
                return [{
                        title: ClientResources.QuickStartBlade.Common.title1,
                        description: ClientResources.QuickStartBlade.TextAnalytics.description1,
                        links: [
                            new InfoList.Link(ClientResources.QuickStartBlade.CognitiveServices.about, "http://go.microsoft.com/fwlink/?LinkId=759709"),
                            new InfoList.Link(ClientResources.ApiType.TextAnalytics.title, "http://go.microsoft.com/fwlink/?LinkId=759711"),
                            new InfoList.Link(ClientResources.QuickStartBlade.TextAnalytics.tryItNow, "http://go.microsoft.com/fwlink/?LinkId=759712"),
                        ],
                        icon: BaseImages.Info()
                    },
                    {
                        title: ClientResources.QuickStartBlade.Common.title2, 
                        description: ClientResources.QuickStartBlade.TextAnalytics.description2,
                        links: [
                            new InfoList.Link(ClientResources.QuickStartBlade.TextAnalytics.getStarted, "http://go.microsoft.com/fwlink/?LinkId=760860"),
                            new InfoList.Link(ClientResources.QuickStartBlade.apiReference, "http://go.microsoft.com/fwlink/?LinkID=759346"),
                            new InfoList.Link(ClientResources.QuickStartBlade.TextAnalytics.sampleCode, "http://go.microsoft.com/fwlink/?LinkId=759345"), 
                        ],
                        icon: BaseImages.Publish()
                    },
                    ];
            case (Constants.ApiTypes.Recommendations):
                return [{
                        title: ClientResources.QuickStartBlade.Common.title1,
                        description: ClientResources.QuickStartBlade.Recommendations.description1,
                        links: [
                            new InfoList.Link(ClientResources.QuickStartBlade.CognitiveServices.about, "http://go.microsoft.com/fwlink/?LinkId=759709"),
                            new InfoList.Link(ClientResources.ApiType.Recommendations.title, "http://go.microsoft.com/fwlink/?LinkId=759710"),
                        ],
                        icon: BaseImages.Info()
                    },
                    {
                        title: ClientResources.QuickStartBlade.Common.title2,
                        description: ClientResources.QuickStartBlade.Recommendations.description2,
                        links: [
                            new InfoList.Link(ClientResources.QuickStartBlade.Recommendations.getStarted, "http://go.microsoft.com/fwlink/?LinkId=759328"),
                            new InfoList.Link(ClientResources.QuickStartBlade.apiReference, "http://go.microsoft.com/fwlink/?LinkID=759348"),
                            new InfoList.Link(ClientResources.QuickStartBlade.Recommendations.sampleCode, "http://go.microsoft.com/fwlink/?LinkId=759344"), 
                        ],
                        icon: BaseImages.Publish()
                    },
                    ];
			case (Constants.ApiTypes.WebLM):
                return [{
                    title: ClientResources.QuickStartBlade.Common.title1,
                    description: ClientResources.QuickStartBlade.WebLM.description1,
                    links: [new InfoList.Link(ClientResources.QuickStartBlade.CognitiveServices.about, Constants.fwlinks.Home),
                        new InfoList.Link(ClientResources.ApiType.WebLM.title, Constants.fwlinks.API.WebLM.Detail)],
                    icon: BaseImages.Info()
                },
                    {
                        title: ClientResources.QuickStartBlade.Common.title2,
                        description: ClientResources.QuickStartBlade.WebLM.description2,
                        links: [
                            new InfoList.Link(ClientResources.QuickStartBlade.apiDoc, Constants.fwlinks.API.WebLM.Doc),
                            new InfoList.Link(ClientResources.QuickStartBlade.developerCodeOfConduct, Constants.fwlinks.API.CodeOfConduct)],
                        icon: BaseImages.Publish()
                    },
                    {
                        title: ClientResources.QuickStartBlade.Common.title3,
                        description: ClientResources.QuickStartBlade.WebLM.description3,
                        links: [new InfoList.Link(ClientResources.QuickStartBlade.apiSdk, Constants.fwlinks.API.WebLM.Sdk)
                        ],
                        icon: BaseImages.Tools()
                    }];
            default:
                {
                    MsPortalFx.Base.Diagnostics.Log.writeEntry(
                        MsPortalFx.Base.Diagnostics.LogEntryLevel.Error,
                        "QuickStartBalde",
                        "Invalid Api Type: {0}".format(ko.toJSON(this._accountView.item().kind())));
                    return null;
                }
        }
    }
}