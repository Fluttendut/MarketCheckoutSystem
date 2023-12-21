using System;
using System.Collections.Generic;

public class BasicCostCalculator : ICostCalculator
{
    private decimal _totalCost = 0M;
    private decimal _totalCampaignSavings = 0M; // Variable to keep track of total campaign savings
    private Dictionary<char, int> _productCounts = new Dictionary<char, int>();
    private Dictionary<char, decimal> _productPrices = new Dictionary<char, decimal>
    {
        { 'A', 100M }, // Price for item A
        { 'B', 200M }, // Price for item B
        { 'F', 50M },  // Price for item F
        // Add more products and their prices
    };

    // Dictionary to map multipack codes to their respective single item codes and quantities
    private Dictionary<char, (char itemCode, int quantity)> _multipackMappings = new Dictionary<char, (char, int)>
    {
        { 'R', ('F', 6) } // 'R' is a multipack that represents 6 of item 'F'
        // Add other multipack mappings as necessary
    };

    // Dictionary for campaign rules (should be populated with actual data)
    private Dictionary<char, (int RequiredQuantity, decimal CampaignPrice)> _campaigns = new Dictionary<char, (int, decimal)>
    {
        { 'A', (3, 200M) }, // For item A, buying 3 costs the price of 2 (200M)
        // Add other campaign rules
    };

    public void ComputeCost(Product product)
    {
        // Update product count, handling multipacks and individual items.
        UpdateProductCount(product);

        if (_multipackMappings.TryGetValue(product.Code, out var multipackInfo))
        {
            // Handle multipack pricing
            HandleMultipack(product, multipackInfo);
        }
        else if (product.HasDeposit && !_productPrices.ContainsKey(product.Code))
        {
            // Handle deposit fee products that do not have a standard price
            HandleDeposit(product);
        }
        else
        {
            // Handle individual items and campaigns
            HandleStandardAndCampaignPricing(product);
        }

        // Display the running total
        Console.WriteLine($"Current total: {_totalCost:C}");
    }

    private void HandleMultipack(Product product, (char itemCode, int quantity) multipackInfo)
    {
        char actualItemCode = multipackInfo.itemCode;
        int actualQuantity = multipackInfo.quantity;

        if (!_productCounts.ContainsKey(actualItemCode))
        {
            _productCounts[actualItemCode] = 0;
        }
        _productCounts[actualItemCode] += actualQuantity;

        decimal priceOfActualItem = _productPrices[actualItemCode];
        _totalCost += actualQuantity * priceOfActualItem;

        Console.WriteLine($"Scanned multipack {product.Code}, equivalent to {actualQuantity} of item {actualItemCode}, added {actualQuantity * priceOfActualItem:C}");
    }

    private void HandleDeposit(Product product)
    {
        // Directly add the deposit fee without trying to add a standard price
        _totalCost += product.DepositFee;
        Console.WriteLine($"Added deposit for {product.Code}: {product.DepositFee:C}");
    }

    private void HandleStandardAndCampaignPricing(Product product)
    {
        _productCounts[product.Code] = _productCounts.GetValueOrDefault(product.Code) + 1;
        int productCount = _productCounts[product.Code];

        // Standard item addition
        _totalCost += _productPrices[product.Code];

        // Check if a campaign should be applied
        if (_campaigns.TryGetValue(product.Code, out var campaign) &&
            productCount % campaign.RequiredQuantity == 0)
        {
            // Calculate the savings for this instance of the campaign
            decimal savingsForThisInstance = _productPrices[product.Code]; // The savings for this campaign instance
            _totalCost -= savingsForThisInstance; // Subtract the price of the free item for the campaign
            _totalCampaignSavings += savingsForThisInstance; // Accumulate the total campaign savings

            Console.WriteLine($"Scanned item {product.Code},\n price: {_productPrices[product.Code]:C}"); // Uniform output
            Console.WriteLine($"Applied campaign for {product.Code}: Buy {campaign.RequiredQuantity} for the price of {campaign.RequiredQuantity - 1}. Campaign savings this time: {savingsForThisInstance:C}");

            // Display the total campaign savings only if it's more than the savings of one campaign instance
            if (_totalCampaignSavings > savingsForThisInstance)
            {
                Console.WriteLine($"Total campaign savings so far: {_totalCampaignSavings:C}");
            }
        }
        else
        {
            // Output for non-campaign or first-time campaign scans
            Console.WriteLine($"Scanned item {product.Code},\n price: {_productPrices[product.Code]:C}");
        }
    }






    private void UpdateProductCount(Product product)
    {
        // Increment product count, handling both multipacks and individual items.
        if (product.MultipackLinkedProductCode.HasValue)
        {
            // If this is a multipack, increment the linked product's count
            var linkedCode = product.MultipackLinkedProductCode.Value;
            _productCounts[linkedCode] = _productCounts.GetValueOrDefault(linkedCode) + product.MultipackQuantity;
        }
        else
        {
            // If this is an individual item, simply increment its count
            _productCounts[product.Code] = _productCounts.GetValueOrDefault(product.Code) + 1;
        }
    }
}
