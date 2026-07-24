namespace Catalog.API.Constants;

public static class ProductConstants
{
    private const string PREFIX = "/api";
    private const string PRODUCT_ROUTE = "products";

    public const string JSON_CONTENT_TYPE = "application/json";
    public const string PRODUCT_TAG = "Products";

    public const string CREATE_PRODUCT_ROUTE = $"{PREFIX}/{PRODUCT_ROUTE}";
    public const string CREATE_PRODUCT_NAME = "CreateProduct";
    public const string CREATE_PRODUCT_DESCRIPTION = "Create Product";

    public const string GET_PRODUCTS_ROUTE = $"{PREFIX}/{PRODUCT_ROUTE}";
    public const string GET_PRODUCTS_NAME = "GetProducts";
    public const string GET_PRODUCTS_DESCRIPTION = "Get Products";
}
