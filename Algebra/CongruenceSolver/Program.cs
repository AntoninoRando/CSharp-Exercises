using static CongruenceSolver.InputReader;
using static CongruenceSolver.Solver;
using System.Text.RegularExpressions;

//Console.Clear();

Console.Write("Write the congruence(s) or ");
string help = col("--help", "32");
Console.Write($"{help} for documentation.");
string escape = col("ENTER two times in a row", "32");
Console.WriteLine($"Press {escape} to exit.");

int mod = 0;
int? sol = SystemSolve(new int[9] {1025, 5312065, 8, 36, 322, 5, 4, 7, 3}, ref mod);
Console.WriteLine($"The solution is {sol} module {mod}");
// string input = take_congruences();

// if (parse(input) == 1) // Error occurred.
// {
//     printHelp();
// }

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