using Microsoft.Azure.CosmosDB.Table;

namespace TableStorage.Model
{
    /// <summary>
    /// Define a Customer entity for demonstrating the Table Service. For the purposes of the sample we use the 
    /// customer's first name as the row key and last name as the partition key. In reality this would not be a good
    /// partition key (PK) and row key (RK) combination as it would likely not be guaranteed to be unique which is one of the requirements for an entity. 
    /// </summary>
    public class CustomerEntity : TableEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerEntity"/> class.
        /// Your entity type must expose a parameter-less constructor
        /// </summary>
        public CustomerEntity()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerEntity"/> class.
        /// Defines the partition key (PK) and row key (RK).
        /// </summary>
        /// <param name="lastName">The last name.</param>
        /// <param name="firstName">The first name.</param>
        public CustomerEntity(string lastName, string firstName)
        {
            PartitionKey = lastName;
            RowKey = firstName;
        }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }
        
    }
}
