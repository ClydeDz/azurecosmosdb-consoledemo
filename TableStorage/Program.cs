// ***********************************************README*******************************************************************
// Instructions: 
// This sample can be run using either the Azure Storage Emulator
// OR by updating the App.Config file with your AccountName and Key from your Azure subscription. 
//
// Downloads:
// Storage Emulator: https://rebrand.ly/azure-storage-emulator
// Storage Explorer: https://rebrand.ly/azure-storage-explorer
// 
// Steps for scenarios:
// To run the sample using the Storage Emulator + Storage Explorer (default option)
//      1. Start the Azure Storage Emulator (once only) by pressing the Start button or the Windows key and searching for it
//         by typing "Azure Storage Emulator". Select it from the list of applications to start it.
//      2. Run the project and optionall debug using F10.
//      3. Run Azure Storage Explorer. While developing locally, find the table created by this program as follows: 
//              (local and attached) -> Storage accounts -> Development -> Tables -> Table name
// 
// To run the sample against Azure Storage from your Azure subscription
//      1. Open the app.config file and comment out the connection string for the emulator (UseDevelopmentStorage=True) and
//         uncomment the connection string for the storage service (AccountName=[]...)
//      2. Create a Storage Account through the Azure Portal and provide your [AccountName] and [AccountKey] in 
//         the App.Config file. See https://rebrand.ly/tablestorage-dotnet for more information
//      3. Run the app as you did earlier. Now you'll be able to see the same data in Azure as well.
//
// To run the sample against Azure Cosmos db Table Storage from your Azure subscription
//      1. Open the app.config file and comment out the connection string for the emulator (UseDevelopmentStorage=True) and
//         uncomment the connection string for the storage service (AccountName=[]...)
//      2. Create a Cosmos db through the Azure Portal as mentioned here https://rebrand.ly/azurecosmos-tableapi-dotnet.
//      3. Grab the connection string from the portal and paste in the App.config file as the value for the key 'StorageConnectionString'.
//      4. Run the app as you did earlier. Now you'll be able to see the same data in Azure as well.
// 
// Forked from https://rebrand.ly/azuretablestorage-dotnet-sample
// For demo only
// 
// *************************************************************************************************************************

namespace TableStorage
{
    using Microsoft.Azure;
    using Microsoft.Azure.CosmosDB.Table;
    using Microsoft.Azure.Storage;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TableStorage.Model;

    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Azure Table Storage - Getting Started Samples\n");

            #region INITIALIZE

            // Parse the connection string and return a reference to the storage account.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            
            // Create the table client
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Retrieve a reference to the table.
            CloudTable table = tableClient.GetTableReference("people");

            // Create the table if it doesn't exist.
            table.CreateIfNotExists();

            #endregion

            #region OPERATIONS

            int step = 5;
            switch (step)
            {
                case 0:
                    // Demonstrate how to insert a single entity
                    SingleInsert(table).Wait();
                    break;
                case 1:
                    // Get the value we just inserted
                    SingleGet(table).Wait();
                    break;
                case 2:
                    // Now lets see how to insert bulk entries
                    BatchInsert(table).Wait();
                    break;
                case 3:
                    // Alright, prepared to get these bulk entries back?
                    BatchGet(table);
                    break;
                case 4:
                    // Feeling confident? Lets insert a special customer into the same table
                    // Note the schema change
                    SpecialSingleInsert(table).Wait();
                    break;
                case 5:
                    // Lets display the special custom back on screen
                    SpecialSingleGet(table).Wait();
                    break;
                default:
                    SingleGet(table).Wait();
                    break;
            }
                        
            #endregion

            Console.WriteLine();
            Console.WriteLine("Press any key to exit");
            Console.Read();
        }
        
        /// <summary>
        /// Demonstrates the insertion of a single entity
        /// </summary>
        /// <param name="table">The instance of the cloud table</param>
        public static async Task SingleInsert(CloudTable table)
        {
            // Create an instance of a customer entity. 
            CustomerEntity customer = new CustomerEntity("Lewisz", "Waalter")
            {
                Email = "lewis.walter@contoso.com",
                PhoneNumber = "425-555-0101"
            };
            
            // Create the InsertOrReplace table operation
            TableOperation insertOrMergeOperation = TableOperation.InsertOrMerge(customer);

            // Execute the operation.
            TableResult result = await table.ExecuteAsync(insertOrMergeOperation); 

            if(result.Result != null)
            {
                Console.WriteLine("Entity inserted");
            }
            else
            {
                Console.WriteLine("Could not insert entity");
            }
        }

        /// <summary>
        /// Demonstrates the retreivale of a single entity
        /// </summary>
        /// <param name="table">The instance of the cloud table</param>
        public static async Task SingleGet(CloudTable table)
        {
            // Create a retrieve operation that takes a customer entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<CustomerEntity>("Lewis", "Walter");

            // Execute the retrieve operation.
            TableResult retrievedResult = await table.ExecuteAsync(retrieveOperation);

            if (retrievedResult.Result != null)
            {
                Console.WriteLine(string.Format("Email: {0}",((CustomerEntity)retrievedResult.Result).Email));
            }
        }

        /// <summary>
        /// Demonstrates the batch insert process
        /// </summary>
        /// <param name="table">The instance of the cloud table</param>
        public static async Task BatchInsert(CloudTable table)
        {
            // Create the batch operation. 
            TableBatchOperation batchOperation = new TableBatchOperation();
            
            // The following code generates test data for use during the query samples.  
            for (int i = 0; i < 10; i++)
            {
                batchOperation.InsertOrMerge(new CustomerEntity("Smith", string.Format("{0}", i.ToString("D4")))
                {
                    Email = string.Format("{0}.{1}@contoso.com", i.ToString("D4"), "Smith"),
                    PhoneNumber = string.Format("425-555-{0}", i.ToString("D4"))
                });
            }

            // Execute the batch operation.
            IList<TableResult> retrievedResult = await table.ExecuteBatchAsync(batchOperation);

            if (retrievedResult.Count > 0)
            {
                Console.WriteLine("Batch inserted");
            }
            else
            {
                Console.WriteLine("Batch not inserted");
            }
        }

        /// <summary>
        /// Demonstrates the retreival of the batch of entities where the partition key is 'Smith'
        /// </summary>
        /// <param name="table">The instance of the cloud table</param>
        public static void BatchGet(CloudTable table)
        {
            // Construct the query operation for all customer entities where PartitionKey="Smith".
            TableQuery<CustomerEntity> query = new TableQuery<CustomerEntity>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Smith"));

            // Print the fields for each customer.
            foreach (CustomerEntity entity in table.ExecuteQuery(query))
            {
                Console.WriteLine("{0}, {1}\t{2}\t{3}", entity.PartitionKey, entity.RowKey,
                    entity.Email, entity.PhoneNumber);
            }
        }

        /// <summary>
        /// Demonstrates the insertion of a single entity.
        /// Similar to the SingleInsert() method but this adds additional properties to the same table.
        /// </summary>
        /// <param name="table">The instance of the cloud table</param>
        public static async Task SpecialSingleInsert(CloudTable table)
        {
            // Create an instance of a customer entity. 
            SpecialCustomerEntity customer = new SpecialCustomerEntity("Thorne", "Bella")
            {
                Email = "thorne.bella@contoso.com",
                PhoneNumber = "425-999-0101",
                LoyaltyNumber = "ABC-1234-XYZ",
                LoyaltyPoints = 100
            };

            // Create the InsertOrReplace table operation
            TableOperation insertOrMergeOperation = TableOperation.InsertOrMerge(customer);

            // Execute the operation.
            TableResult result = await table.ExecuteAsync(insertOrMergeOperation);

            if (result.Result != null)
            {
                Console.WriteLine("Special entity inserted");
            }
            else
            {
                Console.WriteLine("Could not insert special entity");
            }
        }

        /// <summary>
        /// Demonstrates the retreival of the newly added special entity.
        /// Similar to the SingleGet() method just added additional properties to the output
        /// </summary>
        /// <param name="table">The instance of the cloud table</param>
        public static async Task SpecialSingleGet(CloudTable table)
        {
            // Create a retrieve operation that takes a customer entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<SpecialCustomerEntity>("Thorne", "Bella");

            // Execute the retrieve operation.
            TableResult retrievedResult = await table.ExecuteAsync(retrieveOperation);

            if (retrievedResult.Result != null)
            {
                var _specialCustomer = ((SpecialCustomerEntity)retrievedResult.Result);
                Console.WriteLine(string.Format("Email: {0}\tLoyalty Number:{1}\tLoyalty points:{2}", 
                    _specialCustomer.Email,
                    _specialCustomer.LoyaltyNumber,
                    _specialCustomer.LoyaltyPoints));
            }
        }

    }
}
