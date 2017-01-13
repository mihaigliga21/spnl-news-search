using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

                foreach (var linksTable in item)
                {
                    Console.WriteLine(linksTable.LinkAdress + Environment.NewLine);
                }
            }
            catch (Exception)
            {

                throw;
            }

            Console.ReadLine();

        }
    }
}
