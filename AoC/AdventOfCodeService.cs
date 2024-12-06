using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace AoC;

public static class AdventOfCodeService
{
    private const string BASE_URL = "https://adventofcode.com";

    // your logged in session cookie (taken from browser)s
    private static readonly string _sessionCookie;

    static AdventOfCodeService()
    {
        _sessionCookie = File.ReadAllText("session.txt");
    }

    public static string GetInput(int year, int day)
    {
        if (File.Exists($"input/{year}/day{day}.txt")) return File.ReadAllText($"input/{year}/day{day}.txt");
        var input = GetInputAsync(year, day).Result;

        if (input != null)
        {
            Directory.CreateDirectory($"input/{year}");
            File.WriteAllText($"input/{year}/day{day}.txt", input);
        }

        return input;
    }

    public static async Task<string> GetInputAsync(int year, int day)
    {
        try
        {
            using var webClient = new HttpClient();
            var url = $"{BASE_URL}/{year}/day/{day}/input";

            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(url)
            };
            request.Headers.Add("cookie", $"session={_sessionCookie}");
            var response = webClient.Send(request);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception($"input request returned: {response.StatusCode} - {response.ReasonPhrase}");

            var res = await response.Content.ReadAsStringAsync();
            return res.Trim();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to retrieve input! error: {e.Message}");
            return null;
        }
    }

    public static string PostAnswer(int year, int day, int part, string value)
    {
        return PostAnswerAsync(year, day, part, value).Result;
    }

    public static void SaveWrongAnswer(int year, int day, int part, string value)
    {
        var wrongAnswers = new List<string>();
        var path = $"wrong/{year}/day{day}-p{part}.txt";
        if (File.Exists(path)) wrongAnswers = File.ReadAllLines(path).ToList();
        wrongAnswers.Add(value);
        Directory.CreateDirectory($"wrong/{year}");
        File.WriteAllLines(path, wrongAnswers);
    }

    public static List<string> GetWrongAnswers(int year, int day, int part)
    {
        var path = $"wrong/{year}/day{day}-p{part}.txt";
        if (File.Exists(path)) return File.ReadAllLines(path).ToList();
        return new List<string>();
    }

    public static void DeleteWrongAnswers(int year, int day, int part)
    {
        var path = $"wrong/{year}/day{day}-p{part}.txt";
        if (File.Exists(path)) File.Delete(path);
    }

    public static async Task<string> PostAnswerAsync(int year, int day, int part, string value)
    {
        try
        {
            using var webClient = new HttpClient();
            var url = $"{BASE_URL}/{year}/day/{day}/answer";

            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Post
            };
            request.Headers.Add("cookie", $"session={_sessionCookie}");

            request.Content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("level", part.ToString()),
                new KeyValuePair<string, string>("answer", value)
            });

            var response = webClient.Send(request);

            var html = await response.Content.ReadAsStringAsync();
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var article = htmlDoc.DocumentNode.SelectSingleNode("//article");
            return article.FirstChild.InnerText;
        }
        catch (Exception e)
        {
            return null;
        }
    }

    public static HtmlNode GetProblemHtml(int year, int day, int part)
    {
        try
        {
            using var webClient = new HttpClient();
            var url = $"{BASE_URL}/{year}/day/{day}";

            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(url)
            };
            request.Headers.Add("cookie", $"session={_sessionCookie}");
            var response = webClient.Send(request);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception($"input request returned: {response.StatusCode} - {response.ReasonPhrase}");

            var content = response.Content.ReadAsStringAsync().Result;
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(content);

            var articles = htmlDoc.DocumentNode.SelectNodes("//article");

            if (articles.Count < part) return null;

            return articles[part - 1];
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to retrieve input! error: {e.Message}");
            return null;
        }
    }

    public static void Print(this HtmlNode html)
    {
        var table = new Table().AddColumn("");

        foreach (var node in html.ChildNodes)
            switch (node.Name)
            {
                case "h2":
                    table.AddRow(new Markup($"[underline invert]{node.InnerText}\n[/]").Centered());
                    table.AddEmptyRow();
                    break;
                case "p":
                    var content = FormatSimple(node);
                    table.AddRow(new Markup(content.ToString()));
                    table.AddEmptyRow();
                    break;
                case "ul":
                    var list = FormatSimple(node);
                    var listPanel = new Panel(new Markup(list.ToString()))
                        .Border(BoxBorder.None)
                        .PadLeft(3);

                    table.AddRow(listPanel);
                    table.AddEmptyRow();
                    break;
                case "pre":
                    var panel = new Panel(new Text(string.Join("\n", node.ChildNodes.Select(x => x.InnerText))));
                    table.AddRow(panel);
                    table.AddEmptyRow();
                    break;
            }

        AnsiConsole.Write(table.HideHeaders().Expand());
    }

    private static StringBuilder FormatSimple(HtmlNode node)
    {
        var content = new StringBuilder();
        foreach (var pNode in node.ChildNodes)
            switch (pNode.Name)
            {
                case "#text":
                    content.Append(pNode.InnerText);
                    break;
                case "code":
                    content.Append($"[plum1 on grey15] {pNode.InnerText} [/]");
                    break;
                case "em":
                    content.Append($"[gold1]{pNode.InnerText}[/]");
                    break;
                case "li":
                case "ni":
                    content.Append($"- {FormatSimple(pNode)}\n");
                    break;
                default:
                    content.Append(pNode.InnerText);
                    break;
            }

        return content;
    }
}