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
    print_help();
}

string take_congruences()
{
    string input = string.Empty;
    string line = string.Empty;
    ConsoleKeyInfo keyInfo;
    int enter_pressed = 0, congruences = 0;

    Console.Write($"\n({congruences + 1})\t\x1B[36m"); // Setting cyan color for user input.
    while (true)
    {
        keyInfo = Console.ReadKey();

        if (keyInfo.Key == ConsoleKey.Enter)
        {
            if (enter_pressed == 1 || line == string.Empty)
            {
                Console.Write(col("   \tEXIT", "0"));
                break;
            }

            congruences++;
            Console.Write($"\n\x1B[0m({congruences + 1})\t\x1B[36m");
            line = String.Empty;
            enter_pressed = 1;
        }
        else
        {
            input += keyInfo.KeyChar;
            line += keyInfo.KeyChar;
            enter_pressed = 0;
        }
    }
    Console.WriteLine("\x1B[0m"); // Reset color and put a new line.

    return input;
}

void print_help()
{
    Console.WriteLine("\nUSAGE GUIDE");
    Console.WriteLine("----- Linear Congruence -----");
    Console.WriteLine("-\tYou can write numbers in decimal (default), binary (0b) or hexadecimal (0x): 123, 0xA19, 0b-111000");
    Console.WriteLine("-\tUnknowns can be any symbols other than numbers and special operators;");
    Console.WriteLine("-\tMathematical operators are: +, -, /, *, =, mod (X);. You can remap them, except for mod( X), writing: `remap old:new`");
    Console.WriteLine("-\tYou can write as many white-spaces you whish.");
    Console.Write("\n");
    Console.WriteLine("----- System of Congruence -----");
    Console.WriteLine("-\tSimply type a new congruence after the end of mod (X);.");
    Console.WriteLine("-\tYou can use ENTER to start a new line and better organize your work.");
}

int parse_input(string input)
{
    if (input == "--help")
    {
        print_help();
        return 0;
    }

    string[] congruences = input.Split(";");
    foreach (string cong in congruences)
    {
        string trim = Regex.Replace(cong, @"\s+", ""); // Remove all whitespaces.

        int mod = 0, i;
        for (i = 2; i <= trim.Length; i++) // At the end ^1 is ) then ^2 is a digit.
        {
            if (!Char.IsDigit(trim[^i]))
            {
                break;
            }

            mod += (trim[^i] - '0') * (int)Math.Pow(10, i - 2);
        }

        if (mod == 0)
        {
            Console.WriteLine(col("error", "41") + col(": Invalid module.", "31"));
            return 1;
        }

        trim = trim[^trim.Length..^(i + 3)]; // Removing mod(X).;

        string[] parts = trim.Split('=');

        if (parts.Length > 2)
        {
            Console.WriteLine(col("error", "41") + col(": Too many equals.", "31"));
            return 1;
        }
        else if (parts.Length < 2)
        {
            Console.WriteLine(col("error", "41") + col(": Missing equal.", "31"));
            return 1;
        }

        string[] lOperands = parts[0].Split('+', '-', '*');
        string[] rOperands = parts[1].Split('+', '-', '*');

        int ax = 3522, b = 1818;
        int sol = linear_resolution(ax, b, mod, 1);
        Console.WriteLine($"The solution to {ax}x = {b} (mod {mod}) IS {sol}.");
    }

    return 0;
}

string col(string text, string color)
{
    return $"\x1B[{color}m{text}\x1B[0m";
}

// Resolve ax=b mod(n)
int linear_resolution(int ax, int b, int n, int printProcess = 0)
{
    if (b % gcd(ax, n, printProcess) != 0) // Check (a,n) | b
    {
        return -1;
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

            Console.Write($"{a} = \x1B[1m{r2}\x1B[0m•{d} + \x1B[1m{r}\x1B[0m");

            a = d;
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

        a = d;
        b = r;
    }
    while (r != 0);

    return r2;

}