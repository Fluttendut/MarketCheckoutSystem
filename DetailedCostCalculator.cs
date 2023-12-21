using System;
using System.Collections.Generic;
using System.Linq;

public class DetailedCostCalculator : ICostCalculator
{
    private List<Product> _products = new List<Product>();

    public void ComputeCost(Product product)
    {
        _products.Add(product); // Add scanned product to the list

        // Group by product category and calculate the total cost per category
        var groupedByCategory = _products
            .GroupBy(p => p.Group)
            .Select(group => new
            {
                Category = group.Key,
                Products = group,
                TotalCost = group.Sum(p => p.CampaignQuantity > 0 && group.Count(g => g.Code == p.Code) % p.CampaignQuantity == 0 ? p.CampaignPrice : p.Price)
            });

        foreach (var category in groupedByCategory)
        {
            Console.WriteLine($"Category {category.Category}:");
            foreach (var prod in category.Products.Distinct())
            {
                Console.WriteLine($"  Product {prod.Code}, Unit Price: {prod.Price:C}, Quantity: {category.Products.Count(g => g.Code == prod.Code)}");
            }
            Console.WriteLine($"  Category Total: {category.TotalCost:C}");
        }
    }
}
