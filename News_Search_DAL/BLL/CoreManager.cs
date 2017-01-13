using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using News_Search_DAL;

namespace News_Search_DAL.BLL
{
    public class CoreManager
    {
        private static readonly SearchNewsCrawlerDataEntities Ctx = new SearchNewsCrawlerDataEntities();

        public static List<LinksTable> GeLinks()
        {
            var responseItems = new List<LinksTable>();
            try
            {
                responseItems = Ctx.LinksTables.Where(x => x.HasIndexed == null).ToList();
            }
            catch (Exception)
            {
            }
            return responseItems;
        }       
    }
}
