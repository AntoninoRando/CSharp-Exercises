namespace CongruenceSolver;

public class Solver
{
    public static int? solveLinear(int ax, int b, int n, int? gcd = null, bool printProcess = false)
    {
        if (gcd == null)
        {
            gcd = GCD(ax, n, printProcess);
        }

        if (b % gcd != 0) // Check (a,n) | b
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

    // Transforms ax = b mod n into x = b' mod n' (equivalent)
    public static bool MakeTheoSuitable(ref int ax, ref int b, ref int n)
    {
        int gcd = GCD(ax, n);
        int? sol = solveLinear(ax, b, n, gcd);

        if (sol == null)
        {
            return false;
        }

        ax = 1;
        b = (int)sol;
        n = (int)n / gcd;
        return true;
    }

    public static int? SystemSolve(int[] m, ref int mod)
    {
        // Check if the array is given is a suitable form:
        // [ax1, b1, n1, ax2, b2, n2, ..., axn, bn, nn] i.e. [unknown, equal, module] * each congruence
        if (m.Length % 3 != 0)
        {
            return null;
        }

        // Transform each congruence in a form suitable for the Chinese Remainder Theorem
        for (int i = 0; i < m.Length; i+=3)
        {
            // If at least one congruence has no solution
            if (!MakeTheoSuitable(ref m[i], ref m[i+1], ref m[i+2]))
            {
                return null;
            }
        }

        // Check if the modules are two by two coprime and calculate R = mod1 x mod2 x ... x modn
        List<int> Ri = new List<int>();
        Ri.Add(1);
        for (int i = 2; i < m.Length; i += 3)
        {
            Ri[0] *= m[i];
            for (int j = i + 3; j < m.Length; j += 3)
            {
                if (GCD(m[i], m[j]) != 1)
                {
                    return null;
                }
            }
        }

        mod = Ri[0];

        // Calculate R1 = mod2 x ... x modn, R2 = mod1 x mod3 x ... x modn, ... Rn = mod1 x mod2 x ... x mod(n-1)
        for (int i = 2; i < m.Length; i += 3)
        {
            Ri.Add(Ri[0] / m[i]); // mod1 x ... x mod(i-1) x mod(i+1) x ... x modn = R/modi
        }

        // Solve each congruence RiXi = Bi modi
        List<int> Xi = new List<int>(); 
        for (int i = 1; i < Ri.Count; i++)
        {
            // At this point, since we already check the ipothesis of the theorem are respected,
            // the systema has a solution, so we avoid solution-check.
            int RiXi = Ri[i];
            int Bi = m[1 + (3*(i-1))];
            int ni = m[2 + (3*(i-1))];
            Xi.Add(Ri[i] * (int) solveLinear(RiXi, Bi, ni)); // Final solution is X = R1X1 + R2X2 + ... + RnXn 
        }

        return Xi.Sum() % mod;
    }

    // Find greatest common divisor with Euclide Algorithm
    public static int GCD(int a, int b)
    {
        // Swap a with b so that a is always >= b.
        if (b > a) 
        {
            a += b;     // a -> a+b
            b = a - b;  // b -> a-b = (a+b)-b = a
            a -= b;     // a -> a-b = (a+b)-(a)= b

        }

        // a = bd + r

        int r2, r = b;

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

    public static int GCD(int a, int b, bool printProcess)
    {
        if (!printProcess)
        {
            return GCD(a, b);
        }

        int firstA = a, firstB = b;

        if (b > a)
        {
            Console.Write($"a (={a}) and b (={b}) has been swapped so that a >= b. Thus, now it is ");
            a += b;
            b = a - b;
            a -= b;

        }

        Console.WriteLine($"a = {a}, b = {b}.");


        int r2, r = b;

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
}
