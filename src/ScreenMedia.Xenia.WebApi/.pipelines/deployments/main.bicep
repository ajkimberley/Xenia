param name string
param location string
param hostingPlanName string
param alwaysOn bool
param ftpsState string
param sku string
param skuCode string
param workerSize string
param workerSizeId string
param numberOfWorkers string
param linuxFxVersion string

resource xeniaApiAppService 'Microsoft.Web/sites@2018-11-01' = {
  name: name
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

#disable-next-line BCP081
resource xeniaApiHostingPlan 'Microsoft.Web/serverfarms@2018-11-01' = {
  name: hostingPlanName
  location: location
  kind: 'linux'
  tags: {}
  properties: {
    name: hostingPlanName
    workerSize: workerSize
    workerSizeId: workerSizeId
    numberOfWorkers: numberOfWorkers
    reserved: true
    zoneRedundant: false
  }
  sku: {
    tier: sku
    name: skuCode
  }
  dependsOn: []
}
