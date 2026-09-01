namespace Basket.Core.Constants;

public static class BasketRouteConstant
{
    private const string PREFIX = "/api";
    private const string V1 = "v1";

    public const string JSON_CONTENT_TYPE = "application/json";
    public const string BASKET_TAG = "Baskets";

    public const string BASKET_ROUTE_V1 = $"{PREFIX}/{V1}/baskets";

    public const string GET_BASKETS_NAME = "GetBaskets";
    public const string GET_BASKETS_DESCRIPTION = "Get Baskets";
}
