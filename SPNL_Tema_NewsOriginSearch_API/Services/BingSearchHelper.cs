using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using SPNL_Tema_NewsOriginSearch_API.Models;

namespace SPNL_Tema_NewsOriginSearch_API.Services
{
    public class BingSearchHelper
    {
        private static string ImageSearchEndPoint = "https://api.cognitive.microsoft.com/bing/v5.0/images/search";
        private static string AutoSuggestionEndPoint = "https://api.cognitive.microsoft.com/bing/v5.0/suggestions";
        private static string NewsSearchEndPoint = "https://api.cognitive.microsoft.com/bing/v5.0/news/search";

        private static HttpClient AutoSuggestionClient { get; set; }
        private static HttpClient SearchClient { get; set; }

        private static string _autoSuggestionApiKey;
        public static string AutoSuggestionApiKey
        {
            get { return _autoSuggestionApiKey; }
            set
            {
                var changed = _autoSuggestionApiKey != value;
                _autoSuggestionApiKey = value;
                if (changed)
                {
                    InitializeBingClients();
                }
            }
        }

        private static string _searchApiKey;
        public static string SearchApiKey
        {
            get { return _searchApiKey; }
            set
            {
                var changed = _searchApiKey != value;
                _searchApiKey = value;
                if (changed)
                {
                    InitializeBingClients();
                }
            }
        }

        private static void InitializeBingClients()
        {
            AutoSuggestionClient = new HttpClient();
            AutoSuggestionClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", AutoSuggestionApiKey);

            SearchClient = new HttpClient();
            SearchClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", SearchApiKey);
        }

        public static async Task<IEnumerable<string>> GetImageSearchResults(string query, string imageContent = "Face", int count = 20, int offset = 0)
        {
            List<string> urls = new List<string>();

            var result = await SearchClient.GetAsync(string.Format("{0}?q={1}&safeSearch=Strict&imageType=Photo&color=ColorOnly&count={2}&offset={3}{4}", ImageSearchEndPoint, WebUtility.UrlEncode(query), count, offset, string.IsNullOrEmpty(imageContent) ? "" : "&imageContent=" + imageContent));
            result.EnsureSuccessStatusCode();
            var json = await result.Content.ReadAsStringAsync();
            dynamic data = JObject.Parse(json);
            if (data.value != null && data.value.Count > 0)
            {
                for (int i = 0; i < data.value.Count; i++)
                {
                    urls.Add(data.value[i].contentUrl.Value);
                }
            }

            return urls;
        }

        public static async Task<IEnumerable<string>> GetAutoSuggestResults(string query, string market = "en-US")
        {
            List<string> suggestions = new List<string>();

            var result = await AutoSuggestionClient.GetAsync(string.Format("{0}/?q={1}&mkt={2}", AutoSuggestionEndPoint, WebUtility.UrlEncode(query), market));
            result.EnsureSuccessStatusCode();
            var json = await result.Content.ReadAsStringAsync();
            dynamic data = JObject.Parse(json);
            if (data.suggestionGroups != null && data.suggestionGroups.Count > 0 &&
                data.suggestionGroups[0].searchSuggestions != null)
            {
                for (int i = 0; i < data.suggestionGroups[0].searchSuggestions.Count; i++)
                {
                    suggestions.Add(data.suggestionGroups[0].searchSuggestions[i].displayText.Value);
                }
            }

            return suggestions;
        }


        public static async Task<IEnumerable<NewsArticle>> GetNewsSearchResults(string query, int count = 20, int offset = 0, string market = "en-US")
        {
            List<NewsArticle> articles = new List<NewsArticle>();

            var result = await SearchClient.GetAsync(string.Format("{0}/?q={1}&count={2}&offset={3}&mkt={4}", NewsSearchEndPoint, WebUtility.UrlEncode(query), count, offset, market));
            result.EnsureSuccessStatusCode();
            var json = await result.Content.ReadAsStringAsync();
            dynamic data = JObject.Parse(json);

            if (data.value != null && data.value.Count > 0)
            {
                for (int i = 0; i < data.value.Count; i++)
                {
                    if (data.value[i] != null)
                    {
                        articles.Add(new NewsArticle
                        {
                            Title = data.value[i].name != null ? data.value[i].name : "no title",
                            Url = data.value[i].url != null ? data.value[i].url : "no url",
                            Description = data.value[i].description != null ? data.value[i].description : "no description",
                            //ThumbnailUrl = data.value[i].image != null ? data.value[i].image.thumbnail.contentUrl : "no thumbnailUrl",
                            Provider = data.value[i].provider[0] != null ? data.value[i].provider[0].name : "no provider",
                            DatePublished = data.value[i].datePublished != null ? data.value[i].datePublished : "no datePublished",
                            Category = data.value[i].category != null ? data.value[i].category : "no category"
                        });
                    }
                }
            }
            return articles;
        }
    }
}