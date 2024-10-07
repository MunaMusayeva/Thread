using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

public class Program
{
    static List<string> carList = new List<string>(); 
    static CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

    public static void Main()
    {
        Console.WriteLine("Mode seçin: ");
        Console.WriteLine("1: Single Mode");
        Console.WriteLine("2: Multi Mode");
        string modeInput = Console.ReadLine();
        bool isMultiMode = modeInput == "2"; 
        List<string> jsonFiles = new List<string>
        {
            "{ \"Model\": \"Toyota\", \"Vendor\": \"Toyota\", \"Year\": \"2015\", \"ImagePath\": \"img1\" }",
            "{ \"Model\": \"Honda\", \"Vendor\": \"Honda\", \"Year\": \"2018\", \"ImagePath\": \"img2\" }",
            "{ \"Model\": \"BMW\", \"Vendor\": \"BMW\", \"Year\": \"2020\", \"ImagePath\": \"img3\" }",
            "{ \"Model\": \"Audi\", \"Vendor\": \"Audi\", \"Year\": \"2017\", \"ImagePath\": \"img4\" }",
            "{ \"Model\": \"Mercedes\", \"Vendor\": \"Mercedes\", \"Year\": \"2019\", \"ImagePath\": \"img5\" }"
        };
        Console.WriteLine("Starting in " + (isMultiMode ? "MultiMode" : "SingleMode"));
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        LoadJsonFiles(jsonFiles, isMultiMode);
        stopwatch.Stop();
        Console.WriteLine("Time: " + stopwatch.Elapsed.ToString(@"hh\:mm\:ss\:fff"));
        ShowResults();
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
    public static void LoadJsonFiles(List<string> jsonFiles, bool isMultiMode)
    {
        if (isMultiMode)
        {
            MultiModeLoad(jsonFiles);
        }
        else
        {
            SingleModeLoad(jsonFiles);
        }
    }
    private static void SingleModeLoad(List<string> jsonFiles)
    {
        foreach (var jsonData in jsonFiles)
        {
            if (cancellationTokenSource.Token.IsCancellationRequested)
                break;

            carList.Add(jsonData); 
        }
    }
    private static void MultiModeLoad(List<string> jsonFiles)
    {
        var tasks = new List<Task>();

        foreach (var jsonData in jsonFiles)
        {
            if (cancellationTokenSource.Token.IsCancellationRequested)
                break;

            tasks.Add(Task.Run(() =>
            {
                carList.Add(jsonData); 
            }, cancellationTokenSource.Token));
        }

        Task.WhenAll(tasks).Wait(); 
    }
    private static void ShowResults()
    {
        Console.WriteLine("\nYüklenmiş avtomobiller:");
        foreach (var car in carList)
        {
            Console.WriteLine(car);
        }
    }
    public static void CancelLoading()
    {
        cancellationTokenSource.Cancel();
    }
}
