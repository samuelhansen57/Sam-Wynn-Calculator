using System;

class Program
{
    static int num1, num2, num3, num4;

    static void Main()
    {
        Numbers();
        Calculate();
    }

    static void Numbers()
    {
        num1 = 1;
        num2 = 1;
        num3 = 1;
        num4 = 1;
        // etc, etc...
    }

    static void Calculate()
    {
        Console.WriteLine("===================");
        Console.WriteLine($"{num1} + {num2} + {num3} + {num4}");
        Console.WriteLine("===================");
    }
}
