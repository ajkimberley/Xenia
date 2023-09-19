param appServiceName string
param location string
param hostingPlanName string
param alwaysOn bool
param ftpsState string
param hostPlanSku string
param hostPlanSkuCode string
param linuxFxVersion string
param sqlServerName string
param sqlServerAdminType string
param sqlServerAdminLogin string
param sqlServerAdminSid string
param sqlDbSkuName string
param sqlDbMaxSizeBytes int
param sqlDbZoneRedundant bool
param sqlDbAutoPauseDelay int
param sqlDbRequestedBackupStorageRedundancy string
param sqlDbMinCapacity string
param sqlDbIsLedgerOn bool
param sqlDbAvailabilityZone string

resource xeniaApiHostingPlan 'Microsoft.Web/serverfarms@2022-09-01' = {
  name: '${hostingPlanName}_${uniqueString(resourceGroup().id)}'
  location: location
  kind: 'linux'
  properties: {
    targetWorkerSizeId: 0
    reserved: true
    zoneRedundant: false
  }
  sku: {
    tier: hostPlanSku
    name: hostPlanSkuCode
  }
  dependsOn: []
}

resource xeniaApiAppService 'Microsoft.Web/sites@2018-11-01' = {
  name: appServiceName
  location: location
  tags: {}
  properties: {
    siteConfig: {
      appSettings: []
      linuxFxVersion: linuxFxVersion
      alwaysOn: alwaysOn
      ftpsState: ftpsState
    }
    serverFarmId: xeniaApiHostingPlan.id
    clientAffinityEnabled: false
    httpsOnly: true
  }
}

resource xeniaSqlServer 'Microsoft.Sql/servers@2022-05-01-preview' = {
  name: sqlServerName
  location: location
}

resource xeniaSqlServerAdministrator 'Microsoft.Sql/servers/administrators@2023-02-01-preview' = {
  parent: xeniaSqlServer
  name: 'SqlServerAdmin'
  properties: {
    administratorType: sqlServerAdminType
    login: sqlServerAdminLogin
    sid: sqlServerAdminSid
  }
}

resource servers_screen_media_xenia_dev_name_Xenia 'Microsoft.Sql/servers/databases@2023-02-01-preview' = {
  parent: xeniaSqlServer
  name: 'Xenia'
  location: location
  sku: {
    name: sqlDbSkuName
  }
  properties: {
    maxSizeBytes: sqlDbMaxSizeBytes
    zoneRedundant: sqlDbZoneRedundant
    autoPauseDelay: sqlDbAutoPauseDelay
    requestedBackupStorageRedundancy: sqlDbRequestedBackupStorageRedundancy
    minCapacity: sqlDbMinCapacity
    isLedgerOn: sqlDbIsLedgerOn
    availabilityZone: sqlDbAvailabilityZone
  }
}
