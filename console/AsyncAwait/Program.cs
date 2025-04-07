namespace AsyncAwait
{

    class Program
    {

        static async Task Main(string[] args)
        {
            Task sleep = new(() =>{
                Thread.Sleep(1000);
                Console.WriteLine("Sleep");
            });

            sleep.Start();

            var res = await GetWeather();
            var result = await res.Content.ReadAsStringAsync();
            Console.WriteLine(result);

            await sleep;

        }

        static Task<HttpResponseMessage> GetWeather()
        {
            HttpClient client = new();
            HttpRequestMessage request = new();
            request.RequestUri = new Uri("https://api.openweathermap.org/data/2.5/weather?q=palta&units=metric&cnt=2&appid=85bc5965ba13541bc2399b781b2251f7");
            request.Method = HttpMethod.Get;

            return client.SendAsync(request);
        }
    }
}