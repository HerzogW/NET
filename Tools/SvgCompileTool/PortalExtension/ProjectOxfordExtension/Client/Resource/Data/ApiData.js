/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */
"use strict";
define(["require", "exports", "../../Shared/Constants"], function (require, exports, Constants) {
    var ApiData = (function () {
        function ApiData() {
        }
        ApiData.prototype.getEndPoint = function (apiType) {
            switch (apiType) {
                case (Constants.ApiTypes.Face):
                    return MsPortalFx.Data.uriFormatter(Constants.OxfordServiceEndpoint + "face/v1.0", false)("");
                case (Constants.ApiTypes.ComputerVision):
                    return MsPortalFx.Data.uriFormatter(Constants.OxfordServiceEndpoint + "vision/v1", false)("");
                case (Constants.ApiTypes.Emotion):
                    return MsPortalFx.Data.uriFormatter(Constants.OxfordServiceEndpoint + "emotion/v1.0", false)("");
                case (Constants.ApiTypes.LUIS):
                    return MsPortalFx.Data.uriFormatter(Constants.OxfordServiceEndpoint + "luis/v1", false)("");
                case (Constants.ApiTypes.Speech):
                    return MsPortalFx.Data.uriFormatter(Constants.OxfordServiceEndpoint + "speech/v0", false)("");
                case (Constants.ApiTypes.SpellCheck):
                    return MsPortalFx.Data.uriFormatter(Constants.OxfordServiceEndpoint + "text/v1.0/spellcheck", false)("");
                case (Constants.ApiTypes.Video.FaceDetection):
                    return MsPortalFx.Data.uriFormatter(Constants.OxfordServiceEndpoint + "video/v1.0", false)("");
                case (Constants.ApiTypes.Video.MotionDetection):
                    return MsPortalFx.Data.uriFormatter(Constants.OxfordServiceEndpoint + "video/v1.0", false)("");
                case (Constants.ApiTypes.Video.Stabilization):
                    return MsPortalFx.Data.uriFormatter(Constants.OxfordServiceEndpoint + "video/v1.0", false)("");
                case (Constants.ApiTypes.TextAnalytics):
                    return MsPortalFx.Data.uriFormatter(Constants.CognitiveServicesServiceEndpoint + "textAnalytics/v1.0", false)("");
                case (Constants.ApiTypes.Recommendations):
                    return MsPortalFx.Data.uriFormatter(Constants.CognitiveServicesServiceEndpoint + "recommendations/v1.0", false)("");
                case (Constants.ApiTypes.WebLM):
                    return MsPortalFx.Data.uriFormatter(Constants.OxfordServiceEndpoint + "text/weblm/v1.0", false)("");
            }
        };
        return ApiData;
    })();
    exports.ApiData = ApiData;
});
