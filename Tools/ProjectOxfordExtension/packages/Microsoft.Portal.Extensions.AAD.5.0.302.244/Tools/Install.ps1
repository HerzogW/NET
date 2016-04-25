param($installPath, $toolsPath, $package, $project)

# set the build action for the pde to ExtensionResource
$item = $project.ProjectItems.Item("Client").ProjectItems.Item("_extensions").ProjectItems.Item("AAD").ProjectItems.Item("Microsoft_Azure_AD.pde")
$item.Properties.Item("ItemType").Value = "ExtensionReference"
