namespace CongruenceSolver;
using System.Text.RegularExpressions;
using static CongruenceSolver.Solver;

public static class InputReader
{
    // private static string operators = "+-";

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

    private static int ReadModule(string congruence, ref int modIndex)
    {
        if (!congruence.Contains("mod"))
        {
            return 0;
        }


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

    private static int ReadOperand(string operand, ref bool unknown)
    {
        if (operand.Length == 1 && !Char.IsDigit(operand[0]))
        {
            unknown = true;
            return 1;
        }

        int unk = 1;
        int num = 0;

        for (int i = 1; i <= operand.Length; i++)
        {
            char chr = operand[^i];
            
            if (!Char.IsDigit(chr))
            {
                unk++;
                continue;
            }

            num += (chr - '0') * (int)Math.Pow(10, i - unk);

        }

        unknown = unk == 2 ? true : false;
        return num;
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

        bool unknown = false;
        int unkCount = 0;
        int ax = ReadOperand(operands[0], ref unknown);
        unkCount += unknown? 1 : 0;
        int b = ReadOperand(operands[1], ref unknown);
        unkCount += unknown? 1 : 0;

        if (unkCount == 0)
        {
            error = "Unknown is missing";
            return congVector;
        }

        if (unknown)
        {
            // Swap ax with b if b is unknown
            ax += b;     // ax -> ax+b
            b = ax - b;  // b -> ax-b = (ax+b)-b = ax
            ax -= b;     // ax -> ax-b = (ax+b)-(a)= b
        }

        congVector.Add(ax);
        congVector.Add(b);
        congVector.Add(mod);

        return congVector;
    }
}
