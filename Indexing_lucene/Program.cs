using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Indexing_lucene.Helper;
using News_Search_DAL.BLL;

namespace Indexing_lucene
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var item = CoreManager.GeLinks();
                LuceneHelper helper = new LuceneHelper();

                //create index
                /*var status = helper.CreateIndex(item);
                Console.WriteLine(status);
                 */

                //search
                var res = helper.SearchIndex("MP4 Converter 3.17.21");
                Console.WriteLine(res);
            }
            catch (Exception)
            {

                throw;
            }

            Console.ReadLine();

        }
    }
}
