using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace AdventOfCode2022
{
    class Utilities
    {
        private static HttpClientHandler _Handler = new HttpClientHandler()
        {
            UseCookies = true,
            CookieContainer = GetCookieContainer()
        };

        private static HttpClient _HttpClient = new HttpClient(_Handler)
        {
            BaseAddress = new Uri("https://adventofcode.com/")
        };

        public static IEnumerable<string> GetInputForDay(int day)
        {
            var task = _HttpClient.GetStreamAsync($"2022/day/{day}/input");
            task.Wait();
            var stream = task.Result;
            using (StreamReader sr = new StreamReader(stream))
            {
                while (sr.EndOfStream == false)
                {
                    yield return sr.ReadLine();
                }
            }
        }

        public static IEnumerable<string> GetInputFromFile(int day, bool example = true, int part = 1)
        {
            using (FileStream fs = System.IO.File.OpenRead($@".\Day{day}{(part == 2 ? "Part2" : String.Empty)}{(example ? "Ex" : String.Empty)}.txt"))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    while (!sr.EndOfStream)
                    {
                        yield return sr.ReadLine();
                    }
                }
            }
        }

        private static CookieContainer GetCookieContainer()
        {
            var cookieContainer = new CookieContainer();
            cookieContainer.Add(new Cookie()
            {
                Name = "session",
                Domain = ".adventofcode.com",
                Value = ConfigurationManager.AppSettings["SessionCookie"]
            });

            return cookieContainer;
        }
    }
}