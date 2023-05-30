namespace CongruenceSolver;

public class Solver
{
    public static int? linear_resolution(int ax, int b, int n, int printProcess = 0)
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
    public static int gcd(int a, int b, int printProcess = 0)
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
}
