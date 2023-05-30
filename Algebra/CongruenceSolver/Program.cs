using static CongruenceSolver.InputReader;
using static CongruenceSolver.Solver;

Console.Clear();

string help = col("--help", "32");
string escape = col("ENTER two times in a row", "32");
string prompt = $"Write the congruence(s) or {help} for documentation. Press {escape} to exit.";
Console.WriteLine(prompt);

List<string> congruences = new List<string>();
List<string> systems = new List<string>();
List<int> order = new List<int>();
string input = ReadInput(congruences, systems, order);

if (input == "--help")
{
    PrintHelp();
}
else
{
    string solutions = string.Empty;

    int l = 0, s = 0;
    foreach (var ord in order)
    {
        if (ord == 1)
        {
            AddLinear(ref solutions, congruences[l]);
            l++;
        }
        else if (ord == 2)
        {
            AddSystem(ref solutions, systems[s]);
            s++;
        }
        else
        {
            solutions += '\n';
        }
    }

    // Write slutions alongside equations.  
    Console.Clear();
    Console.WriteLine(prompt);

    string[] lines = input.Split('\n');
    string[] solLines = solutions.Split('\n');

    int maxLen = lines.Max(s => s.Length);

    int k;
    for (k = 0; k < solLines.Length; k++)
    {
        int toFill = maxLen - lines[k].Length;
        Console.WriteLine(lines[k] + $"{new String(' ', toFill)}\t\t" + solLines[k]);
    }
    for (int j = k; j < lines.Length; j++)
    {
        Console.WriteLine(lines[j]);
    }
}

void AddLinear(ref string solutions, string congruence)
{
    string error = string.Empty;

    List<int> congVec = ParseCongruence(congruence, ref error);

    if (!String.IsNullOrEmpty(error))
    {
        error = col("error", "41") + col(": " + error + ".", "31");
        solutions += error + '\n';
        return;
    }

    int? sol = SolveLinear(congVec[0], congVec[1], congVec[2]);

    if (sol != null)
    {
        solutions += $"\x1B[0mOne solution is \x1B[32;1m{sol}\x1B[0m.\n";
    }
    else
    {
        solutions += $"\x1B[0mThe are \x1B[32;1mNO solutions\x1B[0m.\n";
    }
}

void AddSystem(ref string solutions, string system)
{
    string error = string.Empty;

    string[] systemCongs = system.Split(';', StringSplitOptions.RemoveEmptyEntries);
    List<int> systemVec = new List<int>();
    // Iterate through systems' congruences.
    for (int j = 0; j < systemCongs.Length; j++)
    {
        string cong = systemCongs[j];
        if (cong[^1] == ';')
        {
            cong = cong.Remove(cong.Length - 1);
        }
        List<int> congVec = ParseCongruence(cong, ref error);

        if (!String.IsNullOrEmpty(error))
        {
            error = col($"error in {j}° congruence", "41") + col(": " + error + ".", "31");
            solutions += error + '\n';
            break;
        }

        systemVec.AddRange(congVec);
    }

    if (!String.IsNullOrEmpty(error))
    {
        return;
    }

    int mod = 0;
    int? sol = SystemSolve(systemVec.ToArray(), ref mod);

    if (sol != null)
    {
        solutions += $"\x1B[32;1m{sol}\x1B[0m is the unique solution module {mod}.\n";
    }
    else
    {
        solutions += $"\x1B[0mThe are \x1B[32;1mNO solutions\x1B[0m.\n";
    }
}