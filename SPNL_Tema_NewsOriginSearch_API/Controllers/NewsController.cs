using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using SPNL_Tema_NewsOriginSearch_API.Models;
using SPNL_Tema_NewsOriginSearch_API.Services;

namespace SPNL_Tema_NewsOriginSearch_API.Controllers
{
    [System.Web.Http.RoutePrefix("api/News")]
    public class NewsController : ApiController
    {
        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private List<NewsArticle> _articleListResponse = new List<NewsArticle>();

        // GET api/News/SearchNews?qyery=query
        [System.Web.Http.HttpGet]
        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.Route("searchnews")]
        public async Task<List<NewsArticle>> SearchNews(string query, string market = "en-US")
        {
            BingSearchHelper.SearchApiKey = "";

            BingSearchHelper.SearchApiKey = "API Key";
            try
            {
                var bingSearch = await BingSearchHelper.GetNewsSearchResults(query, 1, 0, market);

                foreach (var article in bingSearch)
                {
                    var queryResponse = new NewsArticle()
                    {
                        Title = article.Title,
                        Description = article.Description,
                        Provider = article.Provider,
                        //ThumbnailUrl = article.ThumbnailUrl,
                        Url = article.Url,
                        DatePublished = article.DatePublished,
                        Category = article.Category
                    };
                    _articleListResponse.Add(queryResponse);
                }

                return _articleListResponse;
            }
            catch (Exception exception)
            {
                var mes = new NewsArticle()
                {
                    Title = exception.Message
                };
                _articleListResponse.Add(mes);

                return _articleListResponse;
            }
        }

        // GET api/News/GetMarkets
        [System.Web.Http.HttpGet]
        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.Route("getmarkets")]
        public IEnumerable<Markets> GetMarkets()
        {
            var marketlist = new List<Markets>();

            string marketStrings = "Argentina : es-AR, Australia : en-AU, Austria : de-AT, Belgium_Nl : nl-BE, Belgium_Fr : fr-BE, Brazil : pt-BR, Canada_Eng : en-CA, Canada_Fr : fr-CA, Chile : es-CL, Denmark : da-DK, Finland : fi-FI, France : fr-FR, Germany : de-DE, Hong Kong SAR : zh-HK, India : en-IN, Indonesia : en-ID, Ireland : en-IE, Italy:  it-IT, Japan : ja-JP, Korea : ko-KR, Malaysia : en-MY, Mexico : es-MX, Netherlands : nl-NL, New Zealand : en-NZ, Norway : no-NO, Chinese : zh-CN, Poland : pl-PL, Portugal : pt-PT, Philippines : en-PH, Russia : ru-RU, Saudi Arabia : ar-SA, South Africa : en-ZA, Spain : es-ES, Sweden : sv-SE, Switzerland_Fr : fr-CH, Switzerland_De : de-CH, Taiwan : zh-TW, Turkey : tr-TR, United Kingdom : en-GB, United States_Eng : en-US, United States_Es : es-US";

            foreach (var marketString in marketStrings.Split(','))
            {
                var item = new Markets()
                {
                    Name = marketString.Split(':')[0].Trim(),
                    Code = marketString.Split(':')[1].Trim()
                };
                marketlist.Add(item);
            }
            return marketlist;
        }
    }
}
