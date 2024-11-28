namespace BusinessLayer.Modal.Types;

public record ProductItem(
    string productName,
    int quantity,
    int price
);

public record CreatePaymentLinkRequest(
    List<ProductItem> products, // Danh sách sản phẩm
    string description,
    string returnUrl,
    string cancelUrl
);