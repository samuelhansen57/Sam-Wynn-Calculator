using System;

class Program
{
    static double num1, num2;
    static string operation;

    static void Main()
    {
        InputNumbers();
        ChooseOperation();
        Calculate();
    }

    static void InputNumbers()
    {
        // Default values
        num1 = 10;
        num2 = 5;
    }

    static void ChooseOperation()
    {
        // Default operation to addition
        opration = "+";

        // Here user could ideally input operation if we were taking runtime input,
        // but since you asked for a default setup, you can change 'operation' manually.
        // Operations: +, -, *, /
    }

    static void Calculate()
    {
        double result = 0;
        bool validOperation = true;

        switch(operation)
        {
            case "+":
                result = num1 + num2;
                break;

            case "-":
                result = num1 - num2;
                break;

            case "*":
                result = num1 * num2;
                break;

            case "/":
                if (num2 != 0)
                {
                    result = num1 / num2;
                }
                else
                {
                    Console.WriteLine("Error: Division by zero!");
                    validOperation = false;
                }
                break;

            default:
                Console.WriteLine("Invalid operation chosen.");
                validOperation = false;
                break;
        }

        if (validOperation)
        {
            Console.WriteLine("===================");
            Console.WriteLine($"{num1} {operation} {num2} = {result}");
            Console.WriteLine("===================");
        }
    }
}
