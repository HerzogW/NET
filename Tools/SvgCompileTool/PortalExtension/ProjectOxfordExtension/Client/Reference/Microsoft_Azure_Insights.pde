{
  "extension": "Microsoft_Azure_Insights",
  "version": "999.999.999.999",
  "sdkVersion": "5.0.1.244 (release#3cb4076.150306-1340)",
  "schemaVersion": "1.0.0.0",
  "assetTypes": [
    {
      "name": "AzureDiagnostics",
      "permissions": [ ]
    }
  ],
  "parts": [
    {
      "name": "EventsSummaryPart",
      "inputs": [
        "resourceId",
        "options"
      ],
      "initialSize": 2,
      "largeInitialSize": null
    },
    {
      "name": "AlertsSummaryPart",
      "inputs": [ ],
      "initialSize": 2,
      "largeInitialSize": null
    },
    {
      "name": "MetricsSummaryPart",
      "inputs": [
        "metricQuerySetting",
        "options"
      ],
      "initialSize": 10,
      "largeInitialSize": null
    },
    {
      "name": "UsageGaugePart",
      "inputs": [
        "resourceId",
        "usageMetricName",
        "options"
      ],
      "initialSize": null,
      "largeInitialSize": null
    },
    {
      "name": "ApplicationsSummaryPart",
      "inputs": [
        "resourceId"
      ],
      "initialSize": 2,
      "largeInitialSize": null
    },
    {
      "name": "ScaleSummaryPart",
      "inputs": [ ],
      "initialSize": 2,
      "largeInitialSize": null
    }
  ],
  "blades": [
    {
      "name": "DiagnosticsConfigurationUpdateBlade",
      "keyParameters": [ ],
      "inputs": [
        "resourceId",
        "options"
      ],
      "outputs": [
        "metricQuerySetting"
      ]
    },
    {
      "name": "ScaleSettingBlade",
      "keyParameters": [
        "resourceId"
      ],
      "inputs": [
        "resourceId",
        "options"
      ],
      "outputs": [ ]
    }
  ]
}