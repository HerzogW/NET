/// <amd-bundling root="true" />
define(["require", "exports"], function (require, exports) {
    var AzurePortalExtensionStudy;
    (function (AzurePortalExtensionStudy) {
        "use strict";
        var repeatingFragment = " class='msportalfx-svg-placeholder' role='img' xmlns:svg='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink'><title></title>";
        var _commonViewBox50 = "0 0 50 50";
        var _commonViewBox16 = "0 0 16 16";
        var _msportalfxSvgColorClass = " class='msportalfx-svg-c";
        var _enableBackgroundNew = " enable-background='new '";
        var _polygonPoint = "<polygon points='";
        var createSvgImage = FxImpl.DefinitionBuilder.createSvgImage;
        function _setData(data, resources) {
            resources.forEach(function (v) {
                data[v[0]] = createSvgImage(v[1], v[2]);
            });
        }
        function _svgViewBox(viewbox, content) {
            return "<svg viewBox='" + viewbox + "'" + repeatingFragment + content + "</svg>";
        }
        function _widthHeightAttribs(width, height) {
            return " width='" + width + "' height='" + height + "'";
        }
        function _rectXY(x, y) {
            return "<rect x='" + x + "' y='" + y + "'";
        }
        function _pathOpacity(x) {
            return "<path opacity='" + x + "'";
        }
        function _circleCxCyR(cx, cy, r) {
            return "<circle cx='" + cx + "' cy='" + cy + "' r='" + r + "'";
        }
        var Content;
        (function (Content) {
            var SVG;
            (function (SVG_1) {
                var data = [
                    ["sample", _svgViewBox(_commonViewBox50, "<path d='M43.339,18.659c0.195-0.911,0.291-1.779,0.291-2.643C43.63,9.39,38.232,4,31.595,4 c-4.631,0-8.8,2.622-10.81,6.736c-1.619-1.61-3.809-2.523-6.114-2.523c-4.763,0-8.638,3.875-8.638,8.637 c0,0.646,0.072,1.298,0.218,1.968C2.328,20.416,0,23.731,0,27.763c0,5.437,4.443,9.696,10.113,9.696h0.646 c0.11,4.968,4.18,8.975,9.164,8.975c3.355,0,6.393-1.809,7.996-4.681c1.206,1.045,2.748,1.628,4.36,1.628 c3.42,0,6.249-2.591,6.65-5.922h0.961c5.67,0,10.11-4.259,10.11-9.696C50,23.564,47.456,20.109,43.339,18.659'" + _msportalfxSvgColorClass + "05'/><path d='M39.437,23.192l0.736-1.743l-0.213-0.184l-1.607-1.396l0.008-1.411l1.812-1.62l-0.713-1.753l-0.279,0.019 l-2.125,0.152l-0.992-1.006l0.135-2.428l-1.742-0.735l-0.184,0.212l-1.397,1.609l-1.412-0.01l-1.621-1.81l-1.752,0.711l0.02,0.281 l0.15,2.125l-1.004,0.99l-2.428-0.135l-0.736,1.743l0.213,0.184l1.608,1.397l-0.009,1.412l-1.81,1.621l0.711,1.752l0.281-0.021 l2.123-0.149l0.992,1.005l-0.135,2.426l1.743,0.736l0.184-0.213l1.396-1.608l1.412,0.01l1.621,1.812l1.752-0.712l-0.02-0.282 l-0.15-2.124l1.006-0.992L39.437,23.192z M30.628,22.693c-1.971-0.831-2.894-3.102-2.062-5.071c0.833-1.972,3.102-2.894,5.072-2.063 c1.97,0.832,2.893,3.103,2.062,5.073C34.868,22.602,32.599,23.525,30.628,22.693'" + _msportalfxSvgColorClass + "01'/><path d='M28.357,31.948v-1.893l-0.266-0.086l-2.025-0.663l-0.539-1.304l1.04-2.198l-1.336-1.337l-0.251,0.127 l-1.899,0.962l-1.305-0.541l-0.818-2.289l-1.891-0.002l-0.087,0.269l-0.663,2.023l-1.304,0.539l-2.198-1.039l-1.337,1.337 l0.127,0.251l0.962,1.9l-0.54,1.302l-2.29,0.819l-0.001,1.891l0.267,0.088l2.025,0.661l0.539,1.306l-1.04,2.196l1.336,1.34 l0.251-0.128l1.901-0.964l1.302,0.541l0.819,2.289l1.891,0.001l0.087-0.266l0.663-2.025l1.305-0.539l2.196,1.04l1.339-1.336 l-0.127-0.251l-0.964-1.899l0.542-1.305L28.357,31.948z M20.044,34.908c-2.138-0.001-3.869-1.736-3.869-3.874 c0.002-2.139,1.736-3.87,3.875-3.869c2.139,0.001,3.871,1.735,3.869,3.874C23.918,33.177,22.183,34.909,20.044,34.908'" + _msportalfxSvgColorClass + "01'/><path d='M34.449,20.103c-0.539,1.279-2.014,1.878-3.293,1.338c-1.279-0.54-1.878-2.014-1.338-3.293 c0.54-1.277,2.014-1.878,3.293-1.338C34.389,17.35,34.988,18.824,34.449,20.103'" + _msportalfxSvgColorClass + "13'/><path d='M22.561,31.037c-0.001,1.389-1.128,2.514-2.515,2.513c-1.388-0.001-2.512-1.127-2.511-2.516 c0.001-1.388,1.127-2.513,2.513-2.512C21.437,28.523,22.562,29.649,22.561,31.037'" + _msportalfxSvgColorClass + "13'/>")]
                ];
                _setData(SVG, data);
            })(SVG = Content.SVG || (Content.SVG = {}));
        })(Content = AzurePortalExtensionStudy.Content || (AzurePortalExtensionStudy.Content = {}));
    })(AzurePortalExtensionStudy || (AzurePortalExtensionStudy = {}));
    return AzurePortalExtensionStudy;
});
