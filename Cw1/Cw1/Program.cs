using System;
using System.Collections;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace cw1
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var url = args.Length > 0 ? args[0] : "https://www.pja.edu.pl";

            bool result = Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);


            if (result)
            {
                await GetHttpASyncAsync(url);
            }
            else
            {
                throw new ArgumentException("Zły adres URL!!");
            }
            

        }

        private static async Task GetHttpASyncAsync(string url)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(url);
                
                    if (response.IsSuccessStatusCode)
                    {
                        var htmlContent = await response.Content.ReadAsStringAsync();
                        var regex = new Regex("[a-z]+[a-z0-9]*@[a-z0-9]+\\.[a-z]+", RegexOptions.IgnoreCase);

                        var matches = regex.Matches(htmlContent);

                        if (matches.Count == 0)
                        {
                            Console.WriteLine("Nie znaleziono żadnych adresów e-mail");
                        }
                        else
                        {
                            Console.WriteLine("Znalezione adresyy: " + matches.Count);
                            Hashtable hash = new Hashtable();
                            foreach (var match in matches)
                            {
                                var nextString = match.ToString();
                                if (hash.Contains(nextString) == false)
                                {
                                    hash.Add(nextString, string.Empty);
                                }

                            }
                            foreach (DictionaryEntry found in hash)
                            {
                                Console.WriteLine(found.Key);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Błąd pobierania strony");
                    }

                // prawidłowe zwalnianie zasobów
                httpClient.Dispose();
            }

        }
    }
}