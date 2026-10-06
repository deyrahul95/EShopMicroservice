namespace Ordering.API.Constants;

public static class OrderRouteConstant
{
    private const string PREFIX = "/api";
    private const string VERSION = "v1";

    public const string JSON_CONTENT_TYPE = "application/json";
    public const string ORDER_TAG = "Orders";

    public const string ORDER_ROUTE = $"{PREFIX}/{VERSION}/orders";

    public const string CREATE_ORDER_NAME = "CreateOrder";
    public const string CREATE_ORDER_DESCRIPTION = "Create a new order";

    public const string UPDATE_ORDER_NAME = "UpdateOrder";
    public const string UPDATE_ORDER_DESCRIPTION = "Update an existing order";

    public const string DELETE_ORDER_ROUTE = $"{PREFIX}/{VERSION}/orders/" + "{id}";
    public const string DELETE_ORDER_NAME = "DeleteOrder";
    public const string DELETE_ORDER_DESCRIPTION = "Delete an existing order";

    public const string GET_ORDERS_BY_NAME_ROUTE = $"{PREFIX}/{VERSION}/orders/" + "{orderName}";
    public const string GET_ORDERS_BY_NAME_ROUTE_NAME = "GetOrdersByOrderName";
    public const string GET_ORDERS_BY_NAME_DESCRIPTION = "Fetch orders by order name";

    public const string GET_ORDERS_BY_CUSTOMER_ROUTE = $"{PREFIX}/{VERSION}/orders/customer/" + "{customerId}";
    public const string GET_ORDERS_BY_CUSTOMER_NAME = "GetOrdersByCustomerId";
    public const string GET_ORDERS_BY_CUSTOMER_DESCRIPTION = "Fetch orders by customer id";

    public const string GET_ORDERS_NAME = "GetOrders";
    public const string GET_ORDERS_DESCRIPTION = "Fetch all orders";
}
