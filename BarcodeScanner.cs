using System;
using System.Threading.Tasks;

public class BarcodeScanner
{
    public event Action<Product> OnProductScanned = delegate { }; // Empty delegate

    public async Task ScanProductAsync(Product product)
    {
        await Task.Delay(500); // Simulates scanning delay
        OnProductScanned?.Invoke(product);
    }
}
