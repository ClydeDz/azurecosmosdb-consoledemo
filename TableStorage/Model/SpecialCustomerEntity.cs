namespace TableStorage.Model
{
    public class SpecialCustomerEntity : CustomerEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpecialCustomerEntity"/> class.
        /// Your entity type must expose a parameter-less constructor
        /// </summary>
        public SpecialCustomerEntity()
        {
        }

        public SpecialCustomerEntity(string lastName, string firstName)
            :base(lastName, firstName)
        {
        }

        public string LoyaltyNumber { get; set; }

        public int LoyaltyPoints { get; set; }

    }
}
