public class Register
{
    private BarcodeScanner _scanner;
    private ICostCalculator _calculator;

    public Register(ICostCalculator calculator)
    {
        _calculator = calculator;
        _scanner = new BarcodeScanner();
        _scanner.OnProductScanned += _calculator.ComputeCost; // Subscribe to the event
    }

    public void OpenRegister()
    {
        Console.WriteLine("Register is open. Please scan products (enter 'done' to finish):");
        string? input;

        while ((input = Console.ReadLine()) != "done")
        {
            if (!string.IsNullOrEmpty(input) && input.Length == 1 && input[0] >= 'A' && input[0] <= 'Z')
            {
                // Instantiate a new Product object based on the scanned code
                Product product = CreateProductFromCode(input[0]);

                // Scan the product
                _scanner.ScanProductAsync(product).Wait(); // Wait to simulate the scan
            }
            else
            {
                Console.WriteLine("Invalid product code. Please scan a valid product.");
            }
        }

        Console.WriteLine("Register closing. Final total displayed above.");
    }

    private Product CreateProductFromCode(char code)
    {
        // Lookup the actual product details such as Group, Price, etc.
        // For now, let's simulate with some dummy data:
        Product product = new Product
        {
            Code = code,
            Group = 1, // This should be set based on your actual product data
            Price = 100M, // Default price, should be replaced with actual price lookup
            CampaignQuantity = 3, // Default campaign settings, should be replaced with actual campaign lookup
            CampaignPrice = 200M, // Default campaign price, should be replaced with actual campaign lookup
            HasDeposit = code == 'P', // Example condition for deposit
            DepositFee = code == 'P' ? 10M : 0M // Example deposit fee
        };

        // Set multipack properties if it's a multipack
        if (code == 'R') // Assuming 'R' is a multipack code
        {
            product.MultipackLinkedProductCode = 'F';
            product.MultipackQuantity = 6; // 'R' represents 6 of 'F'
            // The price of 'R' would be calculated based on the price of 'F'
        }

        return product;
    }
}
