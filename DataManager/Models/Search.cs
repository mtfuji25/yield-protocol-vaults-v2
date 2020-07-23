using Analogueweb.Mvc.Utilities;
using DataManager.Models.CarraraSQL;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using SpellChecker.Net.Search.Spell;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text.RegularExpressions;
using System.Web.Hosting;

namespace DataManager.Models
{
    public class Search
    {
        #region Fields
        public static string IndexDirectory = HostingEnvironment.MapPath("~/App_Data/Lucene/");
        #endregion

        #region Methods
        private static Results<T> GetResults<T>(string query, string type)
        {
            string didYouMean;
            TopDocs topDocs;
            query = query.Trim().ToLower().Replace("'", "").Replace("-", " ");
            Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29);
            Directory directory = FSDirectory.Open(new System.IO.DirectoryInfo(IndexDirectory + type));
            string[] fields = new string[] { "Search" };
            MultiFieldQueryParser parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_29, fields, analyzer);
            BooleanQuery booleanQuery = new BooleanQuery();
            string[] searchWords = query.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            string[] didYouMeans = new string[searchWords.Length];
            didYouMean = string.Empty;
            foreach (string word in searchWords)
            {
                if (word != "&")
                {
                    booleanQuery.Add(parser.Parse(QueryParser.Escape(word).Replace("~", "") + "~"), Occur.MUST);
                }
            }
            IndexReader reader = IndexReader.Open(directory, true);
            Searcher searcher = new IndexSearcher(reader);
            topDocs = searcher.Search(booleanQuery, reader.MaxDoc);
            ScoreDoc[] scoreDocs = topDocs.ScoreDocs;
            bool exactMatch = false;
            List<string> results = new List<string>();
            for (int i = 0; i < scoreDocs.Length; i++)
            {
                int doc = scoreDocs[i].Doc;
                float score = scoreDocs[i].Score;
                DateTime date = DateTime.Now;
                Document document = searcher.Doc(doc);
                results.Add(document.Get("Id"));
                if (!exactMatch)
                {
                    string input = document.Get("Search").ToLower();
                    bool match = true;
                    for (int j = 0; j < searchWords.Length; j++)
                    {
                        string word = searchWords[j];
                        if (Regex.IsMatch(input, @"(^|\s|\b)" + Regex.Escape(word) + @"(\b|\s|$)", RegexOptions.IgnoreCase))
                        {
                            if (didYouMeans[j] != word)
                            {
                                didYouMeans[j] = word;
                            }
                        }
                        else
                        {
                            match = false;
                            if (string.IsNullOrEmpty(didYouMeans[j]))
                            {
                                didYouMeans[j] = SuggestSimilar(word, type);
                            }
                        }
                    }
                    exactMatch = match;
                }
            }
            query = string.Join(" ", searchWords);
            if (!exactMatch)
            {
                didYouMean = string.Join(" ", didYouMeans);
            }
            query = string.Join(" ", searchWords);
            if (!exactMatch)
            {
                didYouMean = string.Join(" ", didYouMeans);
            }
            Results<T> model = new Results<T>
            {
                DidYouMean = didYouMean,
                Ids = results.ToList(),
                TotalHits = topDocs.TotalHits
            };
            reader.Dispose();
            searcher.Dispose();
            directory.Dispose();
            analyzer.Dispose();
            return model;
        }

        private static string SuggestSimilar(string term, string type)
        {
            string[] terms = new SpellChecker.Net.Search.Spell.SpellChecker(FSDirectory.Open(new System.IO.DirectoryInfo(IndexDirectory + type))).SuggestSimilar(term, 1);
            if (terms.Length > 0)
            {
                return terms[0];
            }
            return term;
        }

        public static Results<Contact> Contacts(bool? active, CarraraSQL.CarraraSQL context, string query)
        {
            Results<Contact> results = GetResults<Contact>(query, "Contacts");
            int[] ids = results.Ids.Select(int.Parse).ToArray();
            if (active.HasValue)
            {
                results.List = context.Contacts.AsNoTracking().Where(x => ids.Contains(x.ContactID) && x.ContactTypeID != 14).ToList();
            }
            else
            {
                results.List = context.Contacts.AsNoTracking().Where(x => ids.Contains(x.ContactID)).ToList();
            }
            results.List = results.List.OrderBy(d => results.Ids.IndexOf(d.ContactID.ToString())).ToList();
            results.TotalHits = results.List.Count();
            return results;
        }

        public static Results<Employee> Employees(CarraraSQL.CarraraSQL context, string query)
        {
            Results<Employee> results = GetResults<Employee>(query, "Employees");
            int[] ids = results.Ids.Select(int.Parse).ToArray();
            results.List = context.Employees.AsNoTracking().Where(x => ids.Contains(x.EmployeeID)).OrderBy(d => results.Ids.IndexOf(d.EmployeeID.ToString())).ToList();
            results.TotalHits = results.List.Count();
            return results;
        }

        public static Results<Job> Jobs(bool? active, CarraraSQL.CarraraSQL context, string query)
        {
            Results<Job> results = GetResults<Job>(query, "Jobs");
            int[] ids = results.Ids.Select(int.Parse).ToArray();
            if (active.HasValue && active.Value)
            {
                results.List = context.Jobs.AsNoTracking().Where(x => ids.Contains(x.JobID) && x.JobStatusID.HasValue && x.JobStatusID.Value == 1).ToList();
            }
            else
            {
                results.List = context.Jobs.AsNoTracking().Where(x => ids.Contains(x.JobID)).ToList();
            }
            results.List = results.List.OrderBy(d => results.Ids.IndexOf(d.JobID.ToString())).ToList();
            results.TotalHits = results.List.Count();
            return results;
        }

        public static Results<Vehicle> Vehicles(bool? active, CarraraSQL.CarraraSQL context, string query)
        {
            Results<Vehicle> results = GetResults<Vehicle>(query, "Vehicles");
            int[] ids = results.Ids.Select(int.Parse).ToArray();
            if (active.HasValue)
            {
                results.List = context.Vehicles.AsNoTracking().Where(x => ids.Contains(x.VehicleID) && x.IsActive == active.Value).ToList();
            }
            else
            {
                results.List = context.Vehicles.AsNoTracking().Where(x => ids.Contains(x.VehicleID)).ToList();
            }
            results.List = results.List.OrderBy(d => results.Ids.IndexOf(d.VehicleID.ToString())).ToList();
            results.TotalHits = results.List.Count();
            return results;
        }
        #endregion

        #region Nested Types
        public class Index
        {
            #region Delegates
            private delegate void AsyncMethodCaller();
            #endregion

            #region Methods
            public static void All()
            {
                // Add objects to index here
                ContactsAsync();
                EmployeesAsync();
                JobsAsync();
                VehiclesAsync();
            }

            private static void AsyncCallback(IAsyncResult ar)
            {
                try
                {
                    AsyncResult result = (AsyncResult)ar;
                    ((AsyncMethodCaller)result.AsyncDelegate).EndInvoke(ar);
                }
                catch (Exception exception)
                {
                    ExceptionLog.Write(exception);
                }
            }

            private static void Contacts()
            {
                Stopwatch stopWatch = new Stopwatch();
                using (CarraraSQL.CarraraSQL carraraSql = new CarraraSQL.CarraraSQL())
                {
                    stopWatch.Start();
                    string result = "Ok";
                    try
                    {
                        // Get list of items, but only load the fields we need to maximize query speed
                        var items = (from item in carraraSql.Contacts
                                     select new
                                     {
                                         item.Company,
                                         item.EMail,
                                         item.FirstName,
                                         item.ContactID,
                                         item.LastName
                                     }).ToList();
                        List<Document> list = new List<Document>();
                        foreach (var item in items)
                        {
                            // Create a new search document for this item
                            Document document = new Document();
                            string company = string.IsNullOrEmpty(item.Company) ? string.Empty : item.Company;
                            string email = string.IsNullOrEmpty(item.EMail) ? string.Empty : item.EMail;
                            string name = string.Join(" ", new string[] { item.FirstName, item.LastName });
                            string search = string.Join(" ", new string[] { email, name, company });
                            document.Add(new Field("Id", item.ContactID.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                            // Apply a boost to the name field so it recieves priority over other fields
                            document.Add(new Field("Name", name, Field.Store.YES, Field.Index.ANALYZED)
                            {
                                Boost = 2f
                            });
                            document.Add(new Field("Search", search, Field.Store.YES, Field.Index.ANALYZED));
                            list.Add(document);
                        }
                        // Write the index to disk
                        Directory directory = FSDirectory.Open(IndexDirectory + "Contacts");
                        Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29, CharArraySet.EMPTY_SET);
                        using (IndexWriter writer = new IndexWriter(directory, analyzer, true, IndexWriter.MaxFieldLength.LIMITED))
                        {
                            foreach (Document document in list)
                            {
                                writer.AddDocument(document);
                            }
                            writer.Optimize();
                        }
                        // Create a sepearate index for spell checking
                        using (IndexReader reader = IndexReader.Open(directory, true))
                        {
                            new SpellChecker.Net.Search.Spell.SpellChecker(directory).IndexDictionary(new LuceneDictionary(reader, "Search"));
                        }
                    }
                    catch (Exception ex)
                    {
                        result = ex.Message;
                    }
                    stopWatch.Stop();
                    using (DataContext dataContext = new DataContext())
                    {
                        dataContext.TaskLogs.Add(new TaskLog
                        {
                            Date = DateTime.Now,
                            ElapsedTicks = stopWatch.ElapsedMilliseconds,
                            Result = result,
                            Task = "Rebuild Search Indexes: Contacts"
                        });
                        dataContext.SaveChanges();
                    }
                }
            }

            public static void ContactsAsync()
            {
                AsyncMethodCaller caller = new AsyncMethodCaller(Contacts);
                AsyncCallback callback = new AsyncCallback(AsyncCallback);
                caller.BeginInvoke(callback, null);
            }

            private static void Employees()
            {
                Stopwatch stopWatch = new Stopwatch();
                using (CarraraSQL.CarraraSQL carraraSQL = new CarraraSQL.CarraraSQL())
                {
                    stopWatch.Start();
                    string result = "Ok";
                    try
                    {
                        // Get list of items, but only load the fields we need to maximize query speed
                        var items = (from item in carraraSQL.Employees
                                     select new
                                     {
                                         item.FirstName,
                                         item.EmployeeID,
                                         item.EmployeeNumber,
                                         item.LastName
                                     }).ToList();
                        List<Document> list = new List<Document>();
                        foreach (var item in items)
                        {
                            // Create a new search document for this item
                            Document document = new Document();
                            string name = string.Join(" ", new string[] { item.FirstName, item.LastName });
                            string number = item.EmployeeNumber;
                            string search = string.Join(" ", new string[] { name, number });
                            document.Add(new Field("Id", item.EmployeeID.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                            // Apply a boost to the name field so it recieves priority over other fields
                            document.Add(new Field("Name", name, Field.Store.YES, Field.Index.ANALYZED)
                            {
                                Boost = 2f
                            });
                            document.Add(new Field("Search", search, Field.Store.YES, Field.Index.ANALYZED));
                            list.Add(document);
                        }
                        // Write the index to disk
                        Directory directory = FSDirectory.Open(IndexDirectory + "Employees");
                        Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29, CharArraySet.EMPTY_SET);
                        using (IndexWriter writer = new IndexWriter(directory, analyzer, true, IndexWriter.MaxFieldLength.LIMITED))
                        {
                            foreach (Document document in list)
                            {
                                writer.AddDocument(document);
                            }
                            writer.Optimize();
                        }
                        // Create a sepearate index for spell checking
                        using (IndexReader reader = IndexReader.Open(directory, true))
                        {
                            new SpellChecker.Net.Search.Spell.SpellChecker(directory).IndexDictionary(new LuceneDictionary(reader, "Search"));
                        }
                    }
                    catch (Exception ex)
                    {
                        result = ex.Message;
                    }
                    stopWatch.Stop();
                    using (DataContext dataContext = new DataContext())
                    {
                        dataContext.TaskLogs.Add(new TaskLog
                        {
                            Date = DateTime.Now,
                            ElapsedTicks = stopWatch.ElapsedMilliseconds,
                            Result = result,
                            Task = "Rebuild Search Indexes: Employees"
                        });
                        dataContext.SaveChanges();

                    }
                }
            }

            public static void EmployeesAsync()
            {
                AsyncMethodCaller caller = new AsyncMethodCaller(Employees);
                AsyncCallback callback = new AsyncCallback(AsyncCallback);
                caller.BeginInvoke(callback, null);
            }

            private static void Jobs()
            {
                Stopwatch stopWatch = new Stopwatch();
                using (CarraraSQL.CarraraSQL carraraSQL = new CarraraSQL.CarraraSQL())
                {
                    stopWatch.Start();
                    string result = "Ok";
                    try
                    {
                        // Get list of items, but only load the fields we need to maximize query speed
                        var items = (from item in carraraSQL.Jobs
                                     select new
                                     {
                                         item.JobName,
                                         item.JobNumber,
                                         item.GeneralContractor
                                     }).ToList();
                        List<Document> list = new List<Document>();
                        foreach (var item in items)
                        {
                            // Create a new search document for this item
                            Document document = new Document();
                            string contractor = item.GeneralContractor;
                            string name = item.JobName;
                            string number = item.JobNumber;
                            string search = string.Join(" ", new string[] { name, number, contractor });
                            document.Add(new Field("Id", item.JobNumber.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                            // Apply a boost to the name field so it recieves priority over other fields
                            document.Add(new Field("Name", name, Field.Store.YES, Field.Index.ANALYZED)
                            {
                                Boost = 2f
                            });
                            document.Add(new Field("Search", search, Field.Store.YES, Field.Index.ANALYZED));
                            list.Add(document);
                        }
                        // Write the index to disk
                        Directory directory = FSDirectory.Open(IndexDirectory + "Jobs");
                        Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29, CharArraySet.EMPTY_SET);
                        using (IndexWriter writer = new IndexWriter(directory, analyzer, true, IndexWriter.MaxFieldLength.LIMITED))
                        {
                            foreach (Document document in list)
                            {
                                writer.AddDocument(document);
                            }
                            writer.Optimize();
                        }
                        // Create a sepearate index for spell checking
                        using (IndexReader reader = IndexReader.Open(directory, true))
                        {
                            new SpellChecker.Net.Search.Spell.SpellChecker(directory).IndexDictionary(new LuceneDictionary(reader, "Search"));
                        }
                    }
                    catch (Exception ex)
                    {
                        result = ex.Message;
                    }
                    stopWatch.Stop();
                    using (DataContext dataContext = new DataContext())
                    {
                        dataContext.TaskLogs.Add(new TaskLog
                        {
                            Date = DateTime.Now,
                            ElapsedTicks = stopWatch.ElapsedMilliseconds,
                            Result = result,
                            Task = "Rebuild Search Indexes: Jobs"
                        });
                        dataContext.SaveChanges();
                    }
                }
            }

            public static void JobsAsync()
            {
                AsyncMethodCaller caller = new AsyncMethodCaller(Jobs);
                AsyncCallback callback = new AsyncCallback(AsyncCallback);
                caller.BeginInvoke(callback, null);
            }

            private static void Vehicles()
            {
                Stopwatch stopWatch = new Stopwatch();
                using (CarraraSQL.CarraraSQL carraraSQL = new CarraraSQL.CarraraSQL())
                {
                    stopWatch.Start();
                    string result = "Ok";
                    try
                    {
                        // Get list of items, but only load the fields we need to maximize query speed
                        var items = (from item in carraraSQL.Vehicles
                                     select new
                                     {
                                         item.Make,
                                         item.VehicleName,
                                         item.VehicleCode,
                                         item.VehicleTypeID,
                                         item.VIN
                                     }).ToList();
                        List<Document> list = new List<Document>();
                        foreach (var item in items)
                        {
                            // Create a new search document for this item
                            Document document = new Document();
                            string code = string.IsNullOrEmpty(item.VehicleCode) ? string.Empty : item.VehicleCode;
                            string make = string.IsNullOrEmpty(item.Make) ? string.Empty : item.Make;
                            string name = string.IsNullOrEmpty(item.VehicleName) ? string.Empty : item.VehicleName;
                            string vin = string.IsNullOrEmpty(item.VIN) ? string.Empty : item.VIN;
                            string search = string.Join(" ", new string[] { code, make, name, vin });
                            document.Add(new Field("Id", item.VehicleTypeID.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                            // Apply a boost to the name field so it recieves priority over other fields
                            document.Add(new Field("Name", name, Field.Store.YES, Field.Index.ANALYZED)
                            {
                                Boost = 2f
                            });
                            document.Add(new Field("Search", search, Field.Store.YES, Field.Index.ANALYZED));
                            list.Add(document);
                        }
                        // Write the index to disk
                        Directory directory = FSDirectory.Open(IndexDirectory + "Vehicles");
                        Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29, CharArraySet.EMPTY_SET);
                        using (IndexWriter writer = new IndexWriter(directory, analyzer, true, IndexWriter.MaxFieldLength.LIMITED))
                        {
                            foreach (Document document in list)
                            {
                                writer.AddDocument(document);
                            }
                            writer.Optimize();
                        }
                        // Create a sepearate index for spell checking
                        using (IndexReader reader = IndexReader.Open(directory, true))
                        {
                            new SpellChecker.Net.Search.Spell.SpellChecker(directory).IndexDictionary(new LuceneDictionary(reader, "Search"));
                        }
                    }
                    catch (Exception ex)
                    {
                        result = ex.Message;
                    }
                    stopWatch.Stop();
                    using (DataContext dataContext = new DataContext())
                    {
                        dataContext.TaskLogs.Add(new TaskLog
                        {
                            Date = DateTime.Now,
                            ElapsedTicks = stopWatch.ElapsedMilliseconds,
                            Result = result,
                            Task = "Rebuild Search Indexes: Contacts"
                        });
                        dataContext.SaveChanges();
                    }
                }
            }

            public static void VehiclesAsync()
            {
                AsyncMethodCaller caller = new AsyncMethodCaller(Vehicles);
                AsyncCallback callback = new AsyncCallback(AsyncCallback);
                caller.BeginInvoke(callback, null);
            }
            #endregion
        }

        public class Results<T>
        {
            #region Properties
            public string DidYouMean { get; set; }

            public List<string> Ids { get; set; }

            public List<T> List { get; set; }

            public int TotalHits { get; set; }
            #endregion
        }
        #endregion
    }
}