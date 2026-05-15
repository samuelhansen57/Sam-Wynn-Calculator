using System;
using System.Collections.Generic;
using System.Globalization;

class Program
{
    static void Main()
    {
        Console.WriteLine("Calculator REPL - type 'help' for commands, 'exit' to quit.");
        Console.WriteLine("Enter expressions with any amount of numbers, operators, and parentheses.");

        while (true)
        {
            Console.Write("Expression> ");
            string? input = Console.ReadLine();
            if (input == null || input.Trim().ToLower() == "exit")
                break;

            if (input.Trim().ToLower() == "help")
            {
                PrintHelp();
                continue;
            }

            if (string.IsNullOrWhiteSpace(input))
                continue;

            try
            {
                double result = EvaluateExpression(input);
                Console.WriteLine($"Result: {result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    static void PrintHelp()
    {
        Console.WriteLine("Commands:");
        Console.WriteLine("  help        Show this help");
        Console.WriteLine("  exit        Quit the program");
        Console.WriteLine("Usage:");
        Console.WriteLine("  Use any number of values and operators in one expression.");
        Console.WriteLine("  Parentheses are supported: ( )");
        Console.WriteLine("  Supported operators: +, -, *, /, %, ^");
        Console.WriteLine("  Supported function: sqrt(x)");
        Console.WriteLine("Examples:");
        Console.WriteLine("  1 + 2 * 3");
        Console.WriteLine("  (1 + 2) * 3");
        Console.WriteLine("  2 ^ 3 ^ 2");
        Console.WriteLine("  sqrt(16) + 5");
    }

    static double EvaluateExpression(string input)
    {
        var tokens = Tokenize(input);
        var rpn = ConvertToRpn(tokens);
        return EvaluateRpn(rpn);
    }

    enum TokenType
    {
        Number,
        Operator,
        LeftParen,
        RightParen,
        Function,
    }

    class Token
    {
        public TokenType Type { get; }
        public string Value { get; }

        public Token(TokenType type, string value)
        {
            Type = type;
            Value = value;
        }
    }

    static List<Token> Tokenize(string input)
    {
        var tokens = new List<Token>();
        int i = 0;

        while (i < input.Length)
        {
            char c = input[i];

            if (char.IsWhiteSpace(c))
            {
                i++;
                continue;
            }

            if (char.IsDigit(c) || c == '.')
            {
                int start = i;
                while (i < input.Length && (char.IsDigit(input[i]) || input[i] == '.'))
                    i++;
                string number = input[start..i];
                tokens.Add(new Token(TokenType.Number, number));
                continue;
            }

            if (char.IsLetter(c))
            {
                int start = i;
                while (i < input.Length && char.IsLetter(input[i]))
                    i++;
                string identifier = input[start..i].ToLowerInvariant();
                tokens.Add(new Token(TokenType.Function, identifier));
                continue;
            }

            switch (c)
            {
                case '+':
                case '-':
                case '*':
                case '/':
                case '%':
                case '^':
                    tokens.Add(new Token(TokenType.Operator, c.ToString()));
                    i++;
                    break;
                case '(':
                    tokens.Add(new Token(TokenType.LeftParen, c.ToString()));
                    i++;
                    break;
                case ')':
                    tokens.Add(new Token(TokenType.RightParen, c.ToString()));
                    i++;
                    break;
                default:
                    throw new InvalidOperationException($"Unexpected character '{c}'.");
            }
        }

        return tokens;
    }

    static List<Token> ConvertToRpn(List<Token> tokens)
    {
        var output = new List<Token>();
        var operators = new Stack<Token>();

        for (int i = 0; i < tokens.Count; i++)
        {
            Token token = tokens[i];

            switch (token.Type)
            {
                case TokenType.Number:
                    output.Add(token);
                    break;
                case TokenType.Function:
                    operators.Push(token);
                    break;
                case TokenType.Operator:
                    string op = token.Value;
                    bool isUnary = IsUnaryOperator(token, i == 0 ? null : tokens[i - 1]);
                    if (isUnary)
                        op = op == "-" ? "u-" : "u+";

                    while (operators.Count > 0 && operators.Peek().Type != TokenType.LeftParen &&
                           ((IsLeftAssociative(op) && Precedence(op) <= Precedence(operators.Peek().Value)) ||
                            (!IsLeftAssociative(op) && Precedence(op) < Precedence(operators.Peek().Value))))
                    {
                        output.Add(operators.Pop());
                    }

                    operators.Push(new Token(TokenType.Operator, op));
                    break;
                case TokenType.LeftParen:
                    operators.Push(token);
                    break;
                case TokenType.RightParen:
                    while (operators.Count > 0 && operators.Peek().Type != TokenType.LeftParen)
                        output.Add(operators.Pop());

                    if (operators.Count == 0 || operators.Peek().Type != TokenType.LeftParen)
                        throw new InvalidOperationException("Mismatched parentheses.");

                    operators.Pop();

                    if (operators.Count > 0 && operators.Peek().Type == TokenType.Function)
                        output.Add(operators.Pop());
                    break;
            }
        }

        while (operators.Count > 0)
        {
            var op = operators.Pop();
            if (op.Type == TokenType.LeftParen || op.Type == TokenType.RightParen)
                throw new InvalidOperationException("Mismatched parentheses.");
            output.Add(op);
        }

        return output;
    }

    static bool IsUnaryOperator(Token token, Token? previous)
    {
        if (token.Type != TokenType.Operator)
            return false;

        if (token.Value != "+" && token.Value != "-")
            return false;

        if (previous == null)
            return true;

        if (previous.Type == TokenType.Operator || previous.Type == TokenType.LeftParen || previous.Type == TokenType.Function)
            return true;

        return false;
    }

    static int Precedence(string op) => op switch
    {
        "u+" => 4,
        "u-" => 4,
        "^" => 3,
        "*" => 2,
        "/" => 2,
        "%" => 2,
        "+" => 1,
        "-" => 1,
        _ => 0,
    };

    static bool IsLeftAssociative(string op) => op switch
    {
        "^" => false,
        "u+" => false,
        "u-" => false,
        _ => true,
    };

    static double EvaluateRpn(List<Token> rpn)
    {
        var stack = new Stack<double>();

        foreach (var token in rpn)
        {
            switch (token.Type)
            {
                case TokenType.Number:
                    stack.Push(double.Parse(token.Value, CultureInfo.InvariantCulture));
                    break;
                case TokenType.Function:
                    if (token.Value == "sqrt")
                    {
                        if (stack.Count < 1)
                            throw new InvalidOperationException("Function 'sqrt' requires one argument.");
                        double value = stack.Pop();
                        if (value < 0)
                            throw new InvalidOperationException("Cannot calculate sqrt of a negative number.");
                        stack.Push(Math.Sqrt(value));
                    }
                    else
                    {
                        throw new InvalidOperationException($"Unknown function '{token.Value}'.");
                    }
                    break;
                case TokenType.Operator:
                    switch (token.Value)
                    {
                        case "+":
                            PushBinary(stack, (a, b) => a + b);
                            break;
                        case "-":
                            PushBinary(stack, (a, b) => a - b);
                            break;
                        case "*":
                            PushBinary(stack, (a, b) => a * b);
                            break;
                        case "/":
                            PushBinary(stack, (a, b) =>
                            {
                                if (b == 0)
                                    throw new InvalidOperationException("Division by zero.");
                                return a / b;
                            });
                            break;
                        case "%":
                            PushBinary(stack, (a, b) =>
                            {
                                if (b == 0)
                                    throw new InvalidOperationException("Modulus by zero.");
                                return a % b;
                            });
                            break;
                        case "^":
                            PushBinary(stack, (a, b) => Math.Pow(a, b));
                            break;
                        case "u+":
                            PushUnary(stack, a => a);
                            break;
                        case "u-":
                            PushUnary(stack, a => -a);
                            break;
                        default:
                            throw new InvalidOperationException($"Unknown operator '{token.Value}'.");
                    }
                    break;
            }
        }

        if (stack.Count != 1)
            throw new InvalidOperationException("Invalid expression.");

        return stack.Pop();
    }

    static void PushUnary(Stack<double> stack, Func<double, double> operation)
    {
        if (stack.Count < 1)
            throw new InvalidOperationException("Missing operand for unary operator.");
        double value = stack.Pop();
        stack.Push(operation(value));
    }

    static void PushBinary(Stack<double> stack, Func<double, double, double> operation)
    {
        if (stack.Count < 2)
            throw new InvalidOperationException("Missing operands for binary operator.");
        double right = stack.Pop();
        double left = stack.Pop();
        stack.Push(operation(left, right));
    }
}
