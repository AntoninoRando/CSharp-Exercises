namespace CongruenceSolver;
using System.Text.RegularExpressions;
using static CongruenceSolver.Solver;

public class InputReader
{
    public static string col(string text, string color)
    {
        return $"\x1B[{color}m{text}\x1B[0m";
    }
    public static void printHelp()
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
    public static int readModule(string congruence, ref int modIndex)
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

    public static int parse(string input)
    {
        if (input == "--help")
        {
            printHelp();
            return 0;
        }

        string operators = "+-";
        string[] congruences = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < congruences.Length; i++)
        {
            string cong = congruences[i];
            string trim = Regex.Replace(cong, @"\s+", ""); // Remove all whitespaces.

            int j = 0;
            int mod = readModule(trim, ref j);

            if (mod == 0)
            {
                Console.WriteLine(col($"error at ({i + 1})", "41") + col(": Invalid module.", "31"));
                return 1;
            }

            trim = trim[^trim.Length..^(j + 1)]; // Removing modX.;

            string[] operands = trim.Split('=');

            if (operands.Length > 2)
            {
                Console.WriteLine(col($"error at ({i + 1})", "41") + col(": Too many equals.", "31"));
                return 1;
            }
            else if (operands.Length < 2)
            {
                Console.WriteLine(col($"error at ({i + 1})", "41") + col(": Missing equal.", "31"));
                return 1;
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

            int? sol = solveLinear(lOperands[0], rOperands[0], mod);

            if (sol != null)
            {
                Console.Write($"One solution of \x1B[36m({i + 1}) {lOperands[0]}x = {rOperands[0]} (mod {mod})\x1B[0m is ");
                Console.WriteLine($"\x1B[32;1m{sol}\x1B[0m.");
            }
            else
            {
                Console.Write($"The \x1B[36m({i + 1}) {lOperands[0]}x = {rOperands[0]} (mod {mod})\x1B[0m has \x1B[32;1mNO solutions\x1B[0m.");
            }
        }
        return 0;
    }
}
