using System.Text.RegularExpressions;

Console.Clear();

Console.Write("Write the congruence(s) or ");
string help = col("--help", "32");
Console.Write($"{help} for documentation.");
string escape = col("ENTER two times in a row", "32");
Console.WriteLine($"Press {escape} to exit.");

string input = take_congruences();

if (parse_input(input) == 1) // Error occurred.
{
    printHelp();
}

/* FUNCTIONS */

string take_congruences()
{
    string input = string.Empty;
    int congruences = 0, systems = 0;

    Console.WriteLine("\x1B[36m"); // Set cyan color for user input.
    while (true)
    {

        if (systems < 1)
        {
            Console.Write($"\x1B[0m({congruences + 1})");
        }
        Console.Write("\t\x1B[36m");

        string? line = Console.ReadLine();

        if (String.IsNullOrEmpty(line) || String.IsNullOrWhiteSpace(line))
        {
            Console.Write(col("EXIT\n", "0"));
            break;
        }

        if (line == "--help")
        {
            Console.Write("\x1B[0m\n");
            return "--help";
        }

        if (line[^1] == ';')
        {
            systems++;
        }
        else
        {
            congruences -= systems; // Keep advancing the counter without considering all the equations now typed.
            systems = 0;
        }

        input += line + '\n';
        congruences++;

    }
    Console.Write("\x1B[0m\n"); // Reset color and put a new line.

    return input;
}

void printHelp()
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

int readModule(string congruence, ref int modIndex)
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

int parse_input(string input)
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

        int? sol = linear_resolution(lOperands[0], rOperands[0], mod);

        if (sol != null)
        {
            Console.Write($"One solution of \x1B[36m({i + 1}) {lOperands[0]}x = {rOperands[0]} (mod {mod})\x1B[0m is ");
            Console.WriteLine($"\x1B[32;1m{sol}\x1B[0m.");
        }
        else {
            Console.Write($"The \x1B[36m({i + 1}) {lOperands[0]}x = {rOperands[0]} (mod {mod})\x1B[0m has \x1B[32;1mNO solutions\x1B[0m.");
        }
    }
    return 0;
}

string col(string text, string color)
{
    return $"\x1B[{color}m{text}\x1B[0m";
}

// Resolve ax=b mod(n)
int? linear_resolution(int ax, int b, int n, int printProcess = 0)
{
    if (b % gcd(ax, n, printProcess) != 0) // Check (a,n) | b
    {
        return null;
    }

    b = b % n;

    int sol = 0;
    while ((ax * sol) % n != b)
    {
        sol++;
    }

    return sol;
}

// Find greates common divisor with Euclide Algorithm
int gcd(int a, int b, int printProcess = 0)
{

    int r2, r;

    if (printProcess > 0)
    {
        int firstA = a, firstB = b;
        // Swap a with b so that a is always >= b.
        if (b > a)
        {
            Console.Write($"a (={a}) and b (={b}) has been swapped so that a >= b. Thus, now it is ");
            a += b;     // a -> a+b
            b = a - b;  // b -> a-b = (a+b)-b = a
            a -= b;     // a -> a-b = (a+b)-(a)= b

        }

        Console.WriteLine($"a = {a}, b = {b}.");

        // a = bd + r

        r = b;

        Console.WriteLine($"Let the second-last reminder be r2 = b = \x1B[1m{b}\x1B[0m.");

        do
        {
            r2 = r;

            Console.Write("\n");

            int d = (int)a / b;
            r = a % b;

            Console.Write($"{a} = \x1B[1m{b}\x1B[0m•{d} + \x1B[1m{r}\x1B[0m");

            a = r2;
            b = r;
        }
        while (r != 0);

        Console.WriteLine($".\n\nSince the last reminder is 0, the process is over and the second-last reminder is \x1B[32;1m{r2} = gcd({firstA}, {firstB})\x1B[0m.");

        return r2;
    }

    // Swap a with b so that a is always >= b.
    if (b > a)
    {
        a += b;     // a -> a+b
        b = a - b;  // b -> a-b = (a+b)-b = a
        a -= b;     // a -> a-b = (a+b)-(a)= b

    }

    // a = bd + r

    r = b;

    do
    {
        r2 = r;

        int d = (int)a / b;
        r = a % b;

        a = r2;
        b = r;
    }
    while (r != 0);

    return r2;

}