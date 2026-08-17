namespace Catalog.API.Constants;

public static class ProductConstants
{
    private const string PREFIX = "/api";

    public const string JSON_CONTENT_TYPE = "application/json";
    public const string PRODUCT_TAG = "Products";

    public const string PRODUCT_ROUTE = $"{PREFIX}/products";
    public const string CREATE_PRODUCT_NAME = "CreateProduct";
    public const string CREATE_PRODUCT_DESCRIPTION = "Create Product";

    public const string GET_PRODUCTS_NAME = "GetProducts";
    public const string GET_PRODUCTS_DESCRIPTION = "Get Products";

    public const string GET_PRODUCT_BY_ID_NAME = "GetProductById";
    public const string GET_PRODUCT_BY_ID_DESCRIPTION = "Get Product By Id";

    public const string GET_PRODUCT_BY_CATEGORY_ROUTE = $"{PREFIX}/products/category";
    public const string GET_PRODUCT_BY_CATEGORY_NAME = "GetProductByCategory";
    public const string GET_PRODUCT_BY_CATEGORY_DESCRIPTION = "Get Product By Category";
}
