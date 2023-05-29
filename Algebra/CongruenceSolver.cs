using System;

Console.WriteLine("Write the congruence(s) or --help for documentation. Press double ENTER to exit.");

string input = string.Empty;
string line = string.Empty;
ConsoleKeyInfo keyInfo;
Boolean enter_pressed = false;

while (true)
{
    keyInfo = Console.ReadKey();

    if (keyInfo.Key == ConsoleKey.Enter)
    {
        if (enter_pressed)
        {
            break;
        }

        Console.WriteLine(line);
        line = String.Empty;
        enter_pressed = true;
    }
    else
    {
        input += keyInfo.KeyChar;
        line += keyInfo.KeyChar;
        enter_pressed = false;
    }
}

if (input == "--help")
{
    print_help();
}

void print_help()
{
    Console.WriteLine("\n\n----- Linear Congruence -----");
    Console.WriteLine("-\tYou can write numbers in decimal (default), binary (0b) or hexadecimal (0x): 123, 0xA19, 0b-111000");
    Console.WriteLine("-\tUnknowns can be any symbols other than numbers and special operators;");
    Console.WriteLine("-\tMathematical operators are: +, -, /, *, =, mod( X). You can remap them, except for mod( X), writing: `remap old:new`");
    Console.WriteLine("-\tYou can write as many white-spaces you whish.");
    Console.Write("\n");
    Console.WriteLine("----- System of Congruence -----");
    Console.WriteLine("-\tSimply type a new congruence after the end of mod ( X).");
    Console.WriteLine("-\tYou can use ENTER to start a new line and better organize your work.");
}