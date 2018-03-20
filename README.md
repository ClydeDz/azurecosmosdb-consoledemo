**Azure Cosmos DB - Basic Console Demo**

### Instructions: 
----
This sample can be run using either the Azure Storage Emulator OR by updating the App.Config file with your AccountName and Key from your Azure subscription. 

### Downloads:
----
Storage Emulator: https://rebrand.ly/azure-storage-emulator    
Storage Explorer: https://rebrand.ly/azure-storage-explorer    

### Steps for scenarios:
----
To run the sample using the Storage Emulator + Storage Explorer (default option).
1. Start the Azure Storage Emulator (once only) by pressing the Start button or the Windows key and searching for it by typing "Azure Storage Emulator". Select it from the list of applications to start it.
2. Run the project and optionall debug using F10.
3. Run Azure Storage Explorer. While developing locally, find the table created by this program as follows: `(local and attached) -> Storage accounts -> Development -> Tables -> Table name`

To run the sample against Azure Storage from your Azure subscription
1. Open the app.config file and comment out the connection string for the emulator (`UseDevelopmentStorage=True`) and uncomment the connection string for the storage service (`AccountName=[]...`)
2. Create a Storage Account through the Azure Portal and provide your `[AccountName]` and `[AccountKey]` in the App.Config file. See https://rebrand.ly/tablestorage-dotnet for more information
3. Run the app as you did earlier. Now you'll be able to see the same data in Azure as well.

To run the sample against Azure Cosmos db Table Storage from your Azure subscription
1. Open the app.config file and comment out the connection string for the emulator (UseDevelopmentStorage=True) and uncomment the connection string for the storage service (`AccountName=[]...`)
2. Create a Cosmos db through the Azure Portal as mentioned here https://rebrand.ly/azurecosmos-tableapi-dotnet.
3. Grab the connection string from the portal and paste in the App.config file as the value for the key `StorageConnectionString`.
4. Run the app as you did earlier. Now you'll be able to see the same data in Azure as well.

Forked from https://rebrand.ly/azuretablestorage-dotnet-sample    
For demo only
