/// <reference path="../../TypeReferences.d.ts" />
define(["require", "exports", "ClientResources"], function (require, exports, ClientResources) {
    /**
     * Represents the asset type information for the Resource asset type.
     */
    var AssetTypeViewModel = (function () {
        /**
         * Initializes a new instance of the AssetTypeViewModel view model class.
         *
         * @param container Object representing the container in the shell.
         * @param initialState Bag of properties saved to user settings via viewState.
         * @param dataContext Long lived data access object passed into all view models in the current area.
         */
        function AssetTypeViewModel(container, initialState, dataContext) {
        }
        /**
         * Gets the browse config.
         *
         * @return A promise which will be resolved with the browse config.
         */
        AssetTypeViewModel.prototype.getBrowseConfig = function () {
            return Q.resolve({
                columns: [
                    // Column for the model using a custom column.
                    {
                        id: "status",
                        name: ko.observable(ClientResources.status),
                        itemKey: "status"
                    },
                ],
                // This following are the default columns.
                defaultColumns: [
                    "status",
                    "location"
                ]
            });
        };
        /**
         * Gets the supplemental data for the array of resource IDs.
         *
         * @param resourceIds The array of resource IDs for the supplemental data.
         * @param properties The array of supplemental properties that are required.
         * @return A promise which will be resolved when data is ready to be streamed.
         */
        AssetTypeViewModel.prototype.getSupplementalData = function (resourceIds, properties) {
            return null;
        };
        return AssetTypeViewModel;
    })();
    exports.AssetTypeViewModel = AssetTypeViewModel;
});
