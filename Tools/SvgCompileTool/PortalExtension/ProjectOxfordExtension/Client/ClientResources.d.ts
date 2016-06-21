declare module "ClientResources" {
    export = ClientResources;

    var ClientResources: {
        ResourceBlade: {
            subtitle: string;
            monitoringLensTitle: string;
            DeleteCommand: {
                resourceKey: string;
                confirmation: string;
                title: string;
                progress: string;
            };
            SettingsCommand: {
                resourceKey: string;
            };
            endPoint: string;
            eventsInThePastWeekLauncherPartTitle: string;
            lensOperationsTitle: string;
            lensUsageTitle: string;
            pricingTierPartTitle: string;
        };
        QuickStartBlade: {
            title: string;
            Face: {
                description1: string;
                description2: string;
                description3: string;
            };
            Common: {
                title1: string;
                title2: string;
                title3: string;
            };
            apiReference: string;
            developerCodeOfConduct: string;
            liveDemos: string;
            CognitiveServices: {
                about: string;
            };
            ComputerVision: {
                description1: string;
                description2: string;
                description3: string;
            };
            Emotion: {
                description1: string;
                description2: string;
                description3: string;
            };
            LUIS: {
                description1: string;
                description2: string;
                description3: string;
            };
            Speech: {
                description1: string;
                description2: string;
                description3: string;
            };
            SpellCheck: {
                description1: string;
                description2: string;
                description3: string;
            };
            Video: {
                FaceDetection: {
                    description1: string;
                    description2: string;
                    description3: string;
                };
                MotionDetection: {
                    description1: string;
                    description2: string;
                    description3: string;
                };
                Stabilization: {
                    description1: string;
                    description2: string;
                    description3: string;
                };
            };
            TextAnalytics: {
                description1: string;
                description2: string;
                getStarted: string;
                sampleCode: string;
                tryItNow: string;
            };
            Recommendations: {
                description1: string;
                description2: string;
                getStarted: string;
                sampleCode: string;
            };
            getStarted: string;
            WebLM: {
                description2: string;
                description3: string;
                description1: string;
            };
            apiDoc: string;
            apiSdk: string;
        };
        PropertiesBlade: {
            title: string;
            subscriptionId: string;
            subscriptionName: string;
            resourceId: string;
        };
        loadingText: string;
        KeyManagementBlade: {
            RegenerateKeysCommand: {
                regeneratePrimary: string;
                regenerateSecondary: string;
                regeneratingKeyInProgressTitle: string;
                failureNotificationTitle: string;
                successNotificationTitle: string;
                regeneratePrimaryKeyConfirmMessage: string;
                regenerateSecondaryKeyConfirmMessage: string;
                title: string;
            };
            title: string;
            genKeyStateGenerateing: string;
            genKeyStateSuccess: string;
            key1Title: string;
            key2Title: string;
            MsgBox: {
                genKey1Des: string;
                genKey1Title: string;
                genKey2Des: string;
                genKey2Title: string;
            };
        };
        location: string;
        AssetTypeNames: {
            Resource: {
                plural: string;
                singular: string;
                lowerPlural: string;
                lowerSingular: string;
            };
        };
        extensionName: string;
        region: string;
        subscription: string;
        status: string;
        SettingsBlade: {
            title: string;
            alert: string;
            audit: string;
            diagnostics: string;
            Group: {
                help: string;
                manage: string;
                monitor: string;
                support: string;
            };
            keys: string;
            quickStart: string;
            support: string;
            troubleshooting: string;
            PricingTier: {
                UpdateFail: {
                    message: string;
                    title: string;
                };
                UpdateProgress: {
                    message: string;
                    title: string;
                };
                UpdateSuccess: {
                    message: string;
                    title: string;
                };
            };
        };
        resourceName: string;
        CreateBlade: {
            pleaseSelectLocation: string;
            pleaseSelectResourceGroup: string;
            pleaseSelectSubscription: string;
            createNewResourceGroup: string;
            resourceNameInvalidMessage: string;
            resourceNameLengthValidationMessage: string;
            selectResourceGroup: string;
            selectSubscription: string;
            enterTheName: string;
            selectLocation: string;
            resourceNameRequired: string;
            selectApiType: string;
            selectSpec: string;
            title: string;
            subtitle: string;
            FormValidation: {
                resourceNameAlreadyExists: string;
            };
            LegalTerm: {
                agreed: string;
                bladeTitle: string;
                buttonText: string;
                description: string;
                title: string;
            };
        };
        resourceGroup: string;
        apiType: string;
        SpecPickerBlade: {
            subtitle: string;
            title: string;
        };
        Status: {
            active: string;
            provisionCanceled: string;
            provisionFailed: string;
            suspended: string;
        };
        ApiTypeBlade: {
            description: string;
        };
        ApiType: {
            ComputerVision: {
                description: string;
                title: string;
            };
            Emotion: {
                description: string;
                title: string;
            };
            Face: {
                description: string;
                title: string;
            };
            LUIS: {
                description: string;
                title: string;
            };
            Speech: {
                description: string;
                title: string;
            };
            SpellCheck: {
                description: string;
                title: string;
            };
            Video: {
                Stabilization: {
                    description: string;
                    title: string;
                };
                FaceDetection: {
                    description: string;
                    title: string;
                };
                MotionDetection: {
                    description: string;
                    title: string;
                };
            };
            TextAnalytics: {
                subtitle: string;
                title: string;
                Features: {
                    sentiment: string;
                    keyPhrase: string;
                    topicDetection: string;
                    languageDetection: string;
                };
            };
            Recommendations: {
                subtitle: string;
                title: string;
            };
            WebLM: {
                description: string;
                title: string;
            };
        };
        pricingTier: string;
        projectOxford: string;
        PricingTier: {
            basic: string;
            free: string;
            premium: string;
            standard: string;
        };
        CognitiveServices: {
            ApiTypeBlade: {
                description: string;
            };
            CreateBlade: {
                subtitle: string;
                title: string;
            };
            AssetTypeNames: {
                Resource: {
                    plural: string;
                    singular: string;
                    lowerPlural: string;
                    lowerSingular: string;
                };
            };
            ResourceBlade: {
                subtitle: string;
            };
        };
        cognitiveServices: string;
        SpecCard: {
            disabledMessage: string;
            disabledHelpMessage: string;
        };
        BrowseBlade: {
            DeleteResourceFail: {
                message: string;
                title: string;
            };
            DeleteResourceInProgress: {
                message: string;
                title: string;
            };
            DeleteResourceSuccess: {
                message: string;
                title: string;
            };
        };
    };
}
