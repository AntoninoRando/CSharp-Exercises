using static CongruenceSolver.InputReader;
using static CongruenceSolver.Solver;
using System.Text.RegularExpressions;

Console.Clear();

string help = col("--help", "32");
string escape = col("ENTER two times in a row", "32");
string prompt = $"Write the congruence(s) or {help} for documentation. Press {escape} to exit.";
Console.WriteLine(prompt);

List<string> congruences = new List<string>();
List<string> systems = new List<string>();
string input = ReadInput(congruences, systems);

if (input == "--help")
{
    PrintHelp();
}
else
{
    List<string> solutions = new List<string>();

    for (int i = 0; i < congruences.Count; i++)
    {
        string error = string.Empty;

        List<int> congVec = ParseCongruence(congruences[i], ref error);
        if (!String.IsNullOrEmpty(error))
        {
            error = col($"error", "41") + col(": " + error + ".", "31");
            solutions.Add(error);
        }
        else
        {
            int? sol = SolveLinear(congVec[0], congVec[1], congVec[2]);

            if (sol != null)
            {
                solutions.Add($"\x1B[0mOne solution is \x1B[32;1m{sol}\x1B[0m.");
            }
            else
            {
                solutions.Add($"\x1B[0mThe are \x1B[32;1mNO solutions\x1B[0m.");
            }
        }
    }

    // Write slutions alongside equations.  
    Console.Clear();
    Console.WriteLine(prompt);
    string[] lines = input.Split('\n');

    int maxLen = lines.Max(s => s.Length);

    int k;
    for (k = 0; k < solutions.Count; k++)
    {
        int toFill = maxLen - lines[k].Length;
        Console.WriteLine(lines[k] + $"{new String(' ', toFill)}\t\t" + solutions[k]);
    }
    for (int j = k; j < lines.Length; j++)
    {
        Console.WriteLine(lines[j]);
    }
}