using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace WpfMapAPI
{
    static class KakaoAPI
    {
        // https://developers.kakao.com/docs/latest/ko/local/dev-guide#address-coord
        const string RestKey = "";
        const string BaseUrl = @"https://dapi.kakao.com/v2/local/search/keyword.json";
        internal async static Task<List<MyLocale>> Search(string query)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"KakaoAK {RestKey}");
            var ret = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}?query={query}"));
            var content = await ret.Content.ReadAsStringAsync();
            JObject job = JsonConvert.DeserializeObject<JObject>(content);
            JArray docs = (JArray)job["documents"];

            var results = new List<MyLocale>();
            foreach(var token in docs)
            {
                var locale = new MyLocale(name: (string)token["place_name"], lat: double.Parse((string)token["y"]), lng: double.Parse((string)token["x"]));
                results.Add(locale);
            }
            return results;
        }
    }
}
