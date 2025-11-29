namespace Ordering.Domain.Models
{
    // Why we are using Customer and Product entities in the Ordering microservices. 
    // Microservices can duplicate data like product from catalog microservices in order to reduce interservice communication.
    // But when we duplicate data from other microservices, this time it needs to be updated in eventual consistency way. 
    public class Product : Entity<ProductId>
    {
        public string Name { get; private set; } = default!;

        public decimal Price { get; private set; } = default!;

        public static Product Create(ProductId productId, string name, decimal price) {
            // Add any necessary validation logic here
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

            var product = new Product {
                Id = productId,
                Name = name,
                Price = price
            };

            return product;
        }
    }
}
