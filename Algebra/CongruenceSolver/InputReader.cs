namespace CongruenceSolver;
using System.Text.RegularExpressions;
using static CongruenceSolver.Solver;

public static class InputReader
{
    private static string operators = "+-";

    public static string col(string text, string color)
    {
        return $"\x1B[{color}m{text}\x1B[0m";
    }
    public static string ReadInput(List<string> congruences, List<string> systems)
    {
        string input = string.Empty;
        string systemInput = string.Empty;
        int countCongr = 0, countSyst = 0;

        while (true)
        {
            if (countSyst < 1)
            {
                input += $"\x1B[0m({countCongr + 1})";
                Console.Write($"\x1B[0m({countCongr + 1})");
            }

            input += "\t\x1B[36m";
            Console.Write("\t\x1B[36m"); // Set cyan color for user input.

            string? line = Console.ReadLine();

            if (String.IsNullOrEmpty(line) || String.IsNullOrWhiteSpace(line))
            {
                input += col("EXIT\n", "0");
                Console.Write(col("EXIT\n", "0"));
                break;
            }

            if (line == "--help")
            {
                Console.Write("\x1B[0m\n");
                return "--help";
            }

            if (line[^1] == ';') // User typed a congruence of a system
            {
                systemInput += line;
                countSyst++;
            }
            else if (countSyst > 0) // User typed the last of congruence of a system
            {
                systems.Add(systemInput);
                systemInput = string.Empty;
                countSyst = 0;
            }
            else // User typed a congrunce.
            {
                congruences.Add(line);
                countCongr++;
            }

            input += line + '\n';
        }
        Console.Write("\x1B[0m\n"); // Reset color and put a new line.

        return input;
    }

    public static void PrintHelp()
    {
        Console.Write("\n");
        Console.WriteLine("USAGE GUIDE");
        Console.WriteLine("----- Linear Congruence -----");
        Console.WriteLine("-\tYou can write numbers in decimal (default), binary (0b) or hexadecimal (0x): 123, 0xA19, 0b-111000");
        Console.WriteLine("-\tUnknowns can be any symbols other than numbers and special operators;");
        Console.WriteLine("-\tMathematical operators are: +, -, /, *, =, mod X;. You can remap them, except for mod X, writing: `remap old:new`");
        Console.WriteLine("-\tYou can write as many white-spaces you whish.");
        Console.Write("\n");
        Console.WriteLine("----- System of Congruence -----");
        Console.WriteLine("-\tSeparate each congruence by a semicolon ;.");
    }

    public static int ReadModule(string congruence, ref int modIndex)
    {
        int mod = 0;

        for (int j = 1; j <= congruence.Length; j++)
        {
            if (!Char.IsDigit(congruence[^j]))
            {
                modIndex = j;
                break;
            }

            mod += (congruence[^j] - '0') * (int)Math.Pow(10, j - 1);
        }

        return mod;
    }

    public static List<int> ParseCongruence(string congruence, ref string error)
    {

        List<int> congVector = new List<int>();

        // Remove all whitespaces.
        string cong = Regex.Replace(congruence, @"\s+", "");

        int j = 0;
        int mod = ReadModule(cong, ref j);

        if (mod == 0)
        {
            error = "Invalid module";
            return congVector;
        }

        // Remove mod from the congruence
        cong = cong[^cong.Length..^(j + 1)];

        string[] operands = cong.Split('=');
        if (operands.Length > 2)
        {
            error = "Too many equals";
            return congVector;
        }
        else if (operands.Length < 2)
        {
            error = "Missing equal";
            return congVector;
        }

        List<int> lOperands = new List<int>();
        j = 0;
        int digit = 0;
        foreach (char c in operands[0])
        {
            if (lOperands.Count < j + 3)
            {
                lOperands.Add(0);
                lOperands.Add('\0');
                lOperands.Add('\0');
            }

            // This operand ended.
            if (operators.Contains(c))
            {
                lOperands[j + 1] = c;
                j++;
                digit = 0;
                // Revert number lOperands[j]
            }
            else if (Char.IsDigit(c))
            {
                lOperands[j] += (c - '0') * (int)Math.Pow(10, digit);
                digit++;
            }
            else
            {
                lOperands[j + 2] = c;
            }

        }
        List<int> rOperands = new List<int>();
        j = 0;
        digit = 0;
        foreach (char c in operands[1])
        {
            if (rOperands.Count < j + 3)
            {
                rOperands.Add(0);
                rOperands.Add('\0');
                rOperands.Add('\0');
            }

            // This operand ended.
            if (operators.Contains(c))
            {
                rOperands[j + 1] = c;
                j++;
                digit = 0;
                // Revert number rOperands[j]
            }
            else if (Char.IsDigit(c))
            {
                rOperands[j] += (c - '0') * (int)Math.Pow(10, digit);
                digit++;
            }
            else
            {
                rOperands[j + 2] = c;
            }
        }

        congVector.Add(lOperands[0]);
        congVector.Add(rOperands[0]);
        congVector.Add(mod);

        return congVector;
    }
}
