using System;

class Program
{
    static void Main(string[] args)
    {
        var calculator = new BasicCostCalculator(); // Switch to DetailedCostCalculator if needed
        var register = new Register(calculator);


        // Open the register to start the checkout process
        register.OpenRegister();

    }
}
