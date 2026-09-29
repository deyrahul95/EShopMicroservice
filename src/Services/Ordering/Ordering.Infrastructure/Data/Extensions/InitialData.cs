using Ordering.Domain.Enums;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Infrastructure.Data.Extensions;

internal static class InitialData
{
    public static IEnumerable<Customer> Customers => [
        Customer.Create(id: CustomerId.Of(new Guid("b9068f61-5da7-46f0-984f-f3fb8c21b9ee")), name: "Admin", email: "admin@shop.com"),
        Customer.Create(id: CustomerId.Of(new Guid("a338e1b0-ebd1-48c3-a111-b41dea13d72d")), name: "Developer", email: "developer@shop.com"),
        Customer.Create(id: CustomerId.Of(new Guid("db0e08a6-aa21-4419-bf8d-44c22d38b5f3")), name: "Tester", email: "tester@shop.com"),
    ];

    public static IEnumerable<Product> Products => [
        Product.Create(id: ProductId.Of(new Guid("cf840ee1-5060-4995-8e60-1741a250d442")), name: "Samsung Galaxy S25 Ultra", price: 91950.00m),
        Product.Create(id: ProductId.Of(new Guid("79a22fb1-01be-4ab7-8fc3-6ba6b0b43632")), name: "Samsung Galaxy S26 Ultra", price: 116900.00m),
        Product.Create(id: ProductId.Of(new Guid("c6705e4a-02d5-49f6-8bd7-59fa6065f154")), name: "Apple Duo", price: 320000.00m),
        Product.Create(id: ProductId.Of(new Guid("7170fa86-1847-419a-9408-7a06262bcf9b")), name: "Samsung Galaxy z fold", price: 149500.00m),
        Product.Create(id: ProductId.Of(new Guid("4da19d5f-8fa9-42de-ab73-b604a19310f5")), name: "Apple 18 PRO MAX", price: 149950.00m)
    ];

    public static IEnumerable<Order> OrdersWithItems
    {
        get
        {
            var address1 = Address.Of(
                firstName: "Admin",
                lastName: "Sol",
                emailAddress: "admin@shop.com",
                addressLine: "127/1, Admin Para, Bootstrap",
                country: "Bootstrap",
                state: "Startup",
                zipCode: "12701");
            var address2 = Address.Of(
                firstName: "Developer",
                lastName: "Net",
                emailAddress: "Developer@shop.com",
                addressLine: "127/0, Developer Para, Bootstrap",
                country: "Bootstrap",
                state: "Startup",
                zipCode: "12700");

            var payment1 = Payment.Of(
                cardName: "MasterCard",
                cardNumber: "2441139244113924",
                expiration: "12/2031",
                cvv: "244",
                paymentMethod: PaymentMethod.CreditCard);
            var payment2 = Payment.Of(
                cardName: "VisaCard",
                cardNumber: "2441139230045671",
                expiration: "05/2029",
                cvv: "369",
                paymentMethod: PaymentMethod.DebitCard);

            var order1 = Order.Create(
                id: OrderId.Of(new Guid("2ffab7ee-f8ad-4cf2-8a50-8867d007c0f6")),
                customerId: CustomerId.Of(new Guid("b9068f61-5da7-46f0-984f-f3fb8c21b9ee")),
                orderName: OrderName.Of("Samsung Smartphone"),
                shippingAddress: address1,
                billingAddress: address1,
                payment: payment2);

            order1.Add(productId: ProductId.Of(new Guid("79a22fb1-01be-4ab7-8fc3-6ba6b0b43632")), quantity: 2, price: 116900.00m);
            order1.Add(productId: ProductId.Of(new Guid("7170fa86-1847-419a-9408-7a06262bcf9b")), quantity: 1, price: 149500.00m);

            var order2 = Order.Create(
                id: OrderId.Of(new Guid("5fee2208-cf54-457b-a008-0b268106a2ea")),
                customerId: CustomerId.Of(new Guid("a338e1b0-ebd1-48c3-a111-b41dea13d72d")),
                orderName: OrderName.Of("Apple Iphone"),
                shippingAddress: address2,
                billingAddress: address2,
                payment: payment1);

            order2.Add(productId: ProductId.Of(new Guid("c6705e4a-02d5-49f6-8bd7-59fa6065f154")), quantity: 2, price: 320000.00m);
            order2.Add(productId: ProductId.Of(new Guid("4da19d5f-8fa9-42de-ab73-b604a19310f5")), quantity: 1, price: 149950.00m);

            return [order1, order2];
        }
    }
}
