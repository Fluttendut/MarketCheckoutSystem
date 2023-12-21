public class Product
{
    public char Code { get; set; }
    public int Group { get; set; }
    public decimal Price { get; set; }
    public int MultipackQuantity { get; set; } = 1; // Default to 1 for individual items
    public decimal CampaignPrice { get; set; }
    public int CampaignQuantity { get; set; }
    public bool HasDeposit { get; set; }
    public decimal DepositFee { get; set; } = 10m; // Add this line for deposit fee
    public char? MultipackLinkedProductCode { get; set; }

}
