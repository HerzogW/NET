/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */

"use strict";
import ExtensionDefinition = require("../_generated/ExtensionDefinition");
import ApiData = require("./Data/ApiData");
import CreateData = require("./Data/CreateData");
import ResourceData = require("./Data/ResourceData");
import SubscriptionsData = require("./Data/SubscriptionsData");
import KeyData = require("./Data/KeyData");
import Constants = require("../Shared/Constants");

export class DataContext {
    public subscriptions = new SubscriptionsData.SubscriptionsData();
    public keyData = new KeyData.KeyData();
    public apiData = new ApiData.ApiData();
    public createData = new CreateData.CreateData();
    public resourceData = new ResourceData.ResourceData(); 
}
