param($installPath, $toolsPath, $package, $project)

# set the build action for the pde to ExtensionResource
$item = $project.ProjectItems.Item("Client").ProjectItems.Item("_extensions").ProjectItems.Item("Hubs").ProjectItems.Item("HubsExtension.pde") 
$item.Properties.Item("ItemType").Value = "ExtensionReference"
$DTE.ItemOperations.Navigate("http://go.microsoft.com/fwlink/?LinkId=691929")