namespace Ordering.Application.Extensions
{
    public static class OrderExtensions
    {
        public static IEnumerable<OrderDto> ToOrderDtoList(this IEnumerable<Order> orders)
        {
            return orders.Select(order => new OrderDto(
                Id: order.Id.Value,
                CustomerId: order.CustomerId.Value,
                OrderName: order.OrderName.Value,
                ShippingAddress: new AddressDto
                (
                    order.ShippingAddress.FirstName,
                    order.ShippingAddress.LastName,
                    order.ShippingAddress.EmailAddress,
                    order.ShippingAddress.AddressLine,
                    order.ShippingAddress.ZipCode,
                    order.ShippingAddress.Country,
                    order.ShippingAddress.State
                ),
                BillingAddress: new AddressDto
                (
                    order.BillingAddress.FirstName,
                    order.BillingAddress.LastName,
                    order.BillingAddress.EmailAddress,
                    order.BillingAddress.AddressLine,
                    order.BillingAddress.ZipCode,
                    order.BillingAddress.Country,
                    order.BillingAddress.State
                ),
                Payment: new PaymentDto
                (
                    order.Payment.CardName,
                    order.Payment.CardNumber,
                    order.Payment.Expiration,
                    order.Payment.CVV,
                    order.Payment.PaymentMethod
                ),
                Status: order.Status,
                OrderItems: order.OrderItems.Select(item => new OrderItemDto
                (
                    OrderId: item.Id.Value,
                    ProductId: item.ProductId.Value,
                    Quantity: item.Quantity,
                    Price: item.Price
                )).ToList()
            ));
        }
    }
}
