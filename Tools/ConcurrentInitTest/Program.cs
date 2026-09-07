using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;

class Program
{
    static async Task Main(string[] args)
    {
        string baseUrl = args.Length > 0 ? args[0] : "http://localhost/DebugInit.aspx";
        int concurrency = args.Length > 1 ? int.Parse(args[1]) : 50;

        using var http = new HttpClient();
        http.Timeout = TimeSpan.FromSeconds(30);
        var tasks = Enumerable.Range(0, concurrency).Select(i => CallOnce(http, baseUrl, i));
        await Task.WhenAll(tasks);
        Console.WriteLine("Done");
    }

    static async Task CallOnce(HttpClient http, string url, int id)
    {
        try
        {
            var s = await http.GetStringAsync(url);
            Console.WriteLine($"[{id}] OK: {s}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[{id}] ERROR: {ex.GetBaseException().Message}");
        }
    }
}
