﻿using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace AoC;

public class PostAnswerBody
{
    public int Level { get; set; }
    public string Answer { get; set; }
}

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
        using var webClient = new HttpClient();
        var url = $"{BASE_URL}/{year}/day/{day}/input";

        var request = new HttpRequestMessage
        {
            RequestUri = new Uri(url),
        };
        request.Headers.Add("cookie", $"session={_sessionCookie}");
        var response = webClient.Send(request);

        if (response.StatusCode != HttpStatusCode.OK)
        {
            throw new Exception($"input request returned: {response.StatusCode} - {response.ReasonPhrase}");
        }

        return response.Content.ReadAsStringAsync().Result.Trim();
    }

    public static string PostAnswer(int year, int day, int part, string value)
    {
        using var webClient = new HttpClient();
        var url = $"{BASE_URL}/{year}/day/{day}/answer";

        var request = new HttpRequestMessage
        {
            RequestUri = new Uri(url),
            Method = HttpMethod.Post
        };
        request.Headers.Add("cookie", $"session={_sessionCookie}");

        var body = new PostAnswerBody
        {
            Level = part,
            Answer = value
        };
        request.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

        var response = webClient.Send(request);

        var html = response.Content.ReadAsStringAsync().Result;
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var article = htmlDoc.DocumentNode.SelectSingleNode("//article");
        return article.FirstChild.InnerText;
    }
}