define(["require", "exports", "ClientResources"], function (require, exports, ClientResources) {
    "use strict";
    var AssetTypeViewModel = (function () {
        function AssetTypeViewModel(container, initialState, dataContext) {
        }
        AssetTypeViewModel.prototype.getBrowseConfig = function () {
            return Q.resolve({
                columns: [
                    {
                        id: "status",
                        name: ko.observable(ClientResources.status),
                        itemKey: "status"
                    },
                ],
                defaultColumns: [
                    "status",
                    "location"
                ]
            });
        };
        AssetTypeViewModel.prototype.getSupplementalData = function (resourceIds, properties) {
            return null;
        };
        return AssetTypeViewModel;
    }());
    exports.AssetTypeViewModel = AssetTypeViewModel;
});
