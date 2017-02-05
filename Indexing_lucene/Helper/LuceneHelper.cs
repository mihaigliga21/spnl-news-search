using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using News_Search_DAL;
using Version = Lucene.Net.Util.Version;

namespace Indexing_lucene.Helper
{
    public class LuceneHelper
    {
        public bool CreateIndex(List<LinksTable> adressList)
        {

            if (adressList.Count == 0)
                return false;
            else
            {
                foreach (LinksTable link in adressList)
                {
                    var downloadedPage = DownloadFile(link.LinkAdress);
                    if (downloadedPage != null)
                    {
                        var text4Index = GetTextForLucene(downloadedPage);
                        if (text4Index != null && text4Index.Count > 0)
                        {
                            var status = DoIndex(text4Index);
                            return status;
                        }
                    }
                }
            }

            return false;
        }

        private List<string> GetTextForLucene(string pageContent)
        {
            if (pageContent == null)
                return null;
            else
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(pageContent);
                var textH4 = doc.DocumentNode.SelectNodes("//h4").Select(node => node.InnerText);
                if (textH4.Count() > 0)
                {
                    var list2Return = new List<string>();
                    foreach (string s in textH4)
                    {
                        list2Return.Add(s);
                    }
                    return list2Return;
                }
            }

            return null;
        }

        private string DownloadFile(string adress)
        {
            try
            {
                WebClient client = new WebClient();
                client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

                Stream data = client.OpenRead(adress);
                if (data != null)
                {
                    StreamReader reader = new StreamReader(data);
                    string page = reader.ReadToEnd();

                    return page;
                }
            }
            catch (Exception e)
            {
                return null;
            }
            return null;
        }

        private bool DoIndex(List<string> listContent)
        {
            try
            {
                //state the file location of the index
                Lucene.Net.Store.Directory dir = Lucene.Net.Store.FSDirectory.Open(Properties.Resources.Lucene_Index_Directory);

                //create an analyzer to process the text
                Lucene.Net.Analysis.Analyzer analyzer = new Lucene.Net.Analysis.Standard.StandardAnalyzer(Version.LUCENE_30);

                //create the index writer with the directory and analyzer defined.
                Lucene.Net.Index.IndexWriter indexWriter = new Lucene.Net.Index.IndexWriter(dir, analyzer, IndexWriter.MaxFieldLength.UNLIMITED);

                //create a document, add in a single field
                Lucene.Net.Documents.Document doc = new Lucene.Net.Documents.Document();

                foreach (string s in listContent)
                {
                    Lucene.Net.Documents.Field fldContent = new Lucene.Net.Documents.Field("content",
                        s,
                        Lucene.Net.Documents.Field.Store.YES,
                        Lucene.Net.Documents.Field.Index.ANALYZED,
                        Lucene.Net.Documents.Field.TermVector.YES);

                    doc.Add(fldContent);
                }

                //write the document to the index
                indexWriter.AddDocument(doc);

                //optimize and close the writer
                indexWriter.Optimize();
                indexWriter.Close();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }


            return false;
        }

        public string SearchIndex(string keySearched)
        {
            try
            {
                //state the file location of the index                
                Lucene.Net.Store.Directory dir =
                    Lucene.Net.Store.FSDirectory.Open(Properties.Resources.Lucene_Index_Directory);

                //create an index searcher that will perform the search
                Lucene.Net.Search.IndexSearcher searcher = new Lucene.Net.Search.IndexSearcher(dir);

                //create an analyzer to process the text
                Lucene.Net.Analysis.Analyzer analyzer = new Lucene.Net.Analysis.Standard.StandardAnalyzer(Version.LUCENE_30);

                Query query = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "content", analyzer).Parse(keySearched);

                var mainQuery = new BooleanQuery();

                mainQuery.Add(query, Occur.MUST);

                var resultsSearched = searcher.Search(query, (Filter) null, 10);
                var res = searcher.Doc(resultsSearched.ScoreDocs[0].Doc).fields_ForNUnit.Where(x=>x.StringValue == keySearched);

                return res.FirstOrDefault().StringValue;

            }
            catch (Exception e)
            {
                return e.Message;
            }

            return null;
        }
    }
}
