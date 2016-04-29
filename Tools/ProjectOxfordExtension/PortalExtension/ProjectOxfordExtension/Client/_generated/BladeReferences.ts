﻿/**
 * @file Source code generated by PDL compiler.
 * @version 1.0
 * @sdkversion 5.0.302.319 (production_sdk#058d65f.160330-1230)
 * @schemaversion 1.0.0.2
 */
/// <reference path="../TypeReferences.d.ts" />

export = BladeDefinitions;
module BladeDefinitions {
    export class CreateBladeReference extends MsPortalFx.Composition.PdlBladeReference<void> {
        public constructor() {
            super("CreateBlade", undefined);
        }
    }
    export class CreationLegalBladeReference extends MsPortalFx.Composition.PdlBladeReference<void> {
        public constructor() {
            super("CreationLegalBlade", undefined);
        }
    }
    export class CognitiveServicesApiTypeReference extends MsPortalFx.Composition.PdlBladeReference<void> {
        public constructor() {
            super("CognitiveServicesApiType", undefined);
        }
    }
    export interface PropertiesBladeInputs {
        id: any;
    }
    export class PropertiesBladeReference extends MsPortalFx.Composition.PdlBladeReference<PropertiesBladeInputs> {
        public constructor(inputs: PropertiesBladeInputs) {
            super("PropertiesBlade", inputs);
        }
    }
    export class ApiAccountSpecPickerReference extends MsPortalFx.Composition.PdlBladeReference<void> {
        public constructor() {
            super("ApiAccountSpecPicker", undefined);
        }
    }
    export interface KeySettingBladeInputs {
        id: any;
    }
    export class KeySettingBladeReference extends MsPortalFx.Composition.PdlBladeReference<KeySettingBladeInputs> {
        public constructor(inputs: KeySettingBladeInputs) {
            super("KeySettingBlade", inputs);
        }
    }
    export interface ResourceBladeInputs {
        id: any;
    }
    export class ResourceBladeReference extends MsPortalFx.Composition.PdlBladeReference<ResourceBladeInputs> {
        public constructor(inputs: ResourceBladeInputs) {
            super("ResourceBlade", inputs);
        }
    }
    export interface QuickStartBladeInputs {
        id: any;
    }
    export class QuickStartBladeReference extends MsPortalFx.Composition.PdlBladeReference<QuickStartBladeInputs> {
        public constructor(inputs: QuickStartBladeInputs) {
            super("QuickStartBlade", inputs);
        }
    }
    export interface SettingsBladeInputs {
        id: any;
    }
    export class SettingsBladeReference extends MsPortalFx.Composition.PdlBladeReference<SettingsBladeInputs> {
        public constructor(inputs: SettingsBladeInputs) {
            super("SettingsBlade", inputs);
        }
    }
    export interface BlankBladeInputs {
        id: any;
    }
    export class BlankBladeReference extends MsPortalFx.Composition.PdlBladeReference<BlankBladeInputs> {
        public constructor(inputs: BlankBladeInputs) {
            super("BlankBlade", inputs);
        }
    }
    export class OxfordCreateBladeReference extends MsPortalFx.Composition.PdlBladeReference<void> {
        public constructor() {
            super("OxfordCreateBlade", undefined);
        }
    }
    export class ProjectOxfordApiTypeReference extends MsPortalFx.Composition.PdlBladeReference<void> {
        public constructor() {
            super("ProjectOxfordApiType", undefined);
        }
    }
}
