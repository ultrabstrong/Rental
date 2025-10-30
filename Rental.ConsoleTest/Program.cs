using System;

namespace Rental.ConsoleTest;

partial class Program
{
#pragma warning disable IDE0052 // Remove unread private members
    static readonly string DesktopLoc =
        Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\";
    static readonly string DownloadsLoc =
        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads\\";
#pragma warning restore IDE0052 // Remove unread private members

    static void Main()
    {
        try { }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception caught:{Environment.NewLine}{ex}");
        }
        Console.WriteLine("Program finished. Press any key to exit");
        Console.ReadKey();
    }
}
