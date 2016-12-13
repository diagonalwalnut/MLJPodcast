using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace mljPodcast
{
    class DBInitializerRomans
    {
        public DBInitializerRomans()
        {

        }

        public PodcastCollection GetPodcastCollection(int chapternumber = 0)
        {
            PodcastCollection podcastCollection = new PodcastCollection();
            podcastCollection.Title = "Book of Romans";
            podcastCollection.Uri = "http://www.mljtrust.org/collections/book-of-romans/" + chapternumber;
            podcastCollection.Description = "Sermons By Dr. Lloyd-Jones on the book of Romans.";

            HtmlDocument htmlDoc = GetHtmlDoc(podcastCollection.Uri);

            podcastCollection.ChildCollections = GetCollections(htmlDoc, "chapter-list", podcastCollection.Uri);

            return podcastCollection;
        }

        private List<Podcast> ProcessPodcastSubcollection(string uri)
        {
            HtmlDocument htmlDoc = GetHtmlDoc(uri);
            List<string> pages = GetSubCollections(htmlDoc, "pagination");

            List<Podcast> podcastList = new List<Podcast>();

            for (int i = 1; i < (pages.Count + 2); i++)
            {
                htmlDoc = GetHtmlDoc(uri + "?page=" + i);
                podcastList.AddRange(GetPodcasts(htmlDoc));
            }

            return podcastList;
        }

        private List<PodcastCollection> GetCollections(HtmlDocument htmldoc, string xpath, string uri)
        {
            List<PodcastCollection> collectionList = new List<PodcastCollection>();

            Uri siteuri = new Uri(uri);
            string baseUri = siteuri.Scheme + "://" + siteuri.Authority;

            HtmlNode bodyNode = htmldoc.DocumentNode.SelectSingleNode("//body");

            if (bodyNode != null)
            {
                IEnumerable<HtmlNode> tempCollectionList = bodyNode.SelectSingleNode("//ul[@class='" + xpath + "']").Descendants("a");

                int ordercount = 0;
                foreach (var item in tempCollectionList)
                {
                    try
                    {
                        collectionList.Add(new PodcastCollection
                        {
                            Uri = baseUri + item.Attributes["href"].Value.ToString(),
                            Title = item.Attributes["title"].Value.ToString(),
                            Podcasts = ProcessPodcastSubcollection(baseUri + item.Attributes["href"].Value.ToString()),
                            Order = ordercount
                        });
                    }
                    catch (Exception e) { }
                }
            }

            return collectionList;
        }

        private List<string> GetSubCollections(HtmlDocument htmldoc, string xpath)
        {
            List<string> pageList = new List<string>();

            HtmlNode bodyNode = htmldoc.DocumentNode.SelectSingleNode("//body");

            if (bodyNode != null)
            {
                HtmlNodeCollection test = bodyNode.SelectSingleNode("//div[@class='" + xpath + "']").ChildNodes;

                foreach (var item in test)
                {
                    try
                    {
                        if (item.OriginalName.Equals("a"))
                        {
                            string pagingLink = item.Attributes["href"].Value.ToString();
                            pageList.Add(pagingLink);
                        }
                    }
                    catch (Exception e) { }
                }
            }

            return pageList;
        }

        private HtmlDocument GetHtmlDoc(string uri)
        {
            HtmlWeb hw = new HtmlWeb();
            HtmlDocument htmlDoc = hw.Load(uri);

            htmlDoc.OptionFixNestedTags = true;

            return htmlDoc;
        }

        private List<Podcast> GetPodcasts(HtmlDocument htmldoc)
        {
            string xPathString = "//div[@class='span8 sermon']";
            HtmlNode bodyNode = htmldoc.DocumentNode.SelectSingleNode("//body");

            HtmlNodeCollection testnodes = bodyNode.SelectNodes(xPathString);
            int testCount = testnodes.Count;

            List<Podcast> returnPodcastList = new List<Podcast>();
            int ordercount = 0;
            foreach (HtmlNode childNode in testnodes)
            {
                returnPodcastList.Add(GetPodcastData(childNode, ordercount));
            }

            return returnPodcastList;
        }

        private Podcast GetPodcastData(HtmlNode node, int ordercount)
        {
            Podcast returnPodcast = new Podcast();

            returnPodcast.Order = ordercount;

            try { returnPodcast.Title = GetTitle(node.ChildNodes.FindFirst("h3")); }
            catch (Exception) { }

            try { returnPodcast.EpisodeUri = GetEpisodeUri(node.ChildNodes.FindFirst("h3")); }
            catch (Exception) { }

            try { returnPodcast.Uri = GetUri(node.ChildNodes.FindFirst("audio")); }
            catch (Exception) { }

            try { returnPodcast.BibleReference = GetBibleReference(node.SelectSingleNode("./p[@class='metadata']")); }
            catch (Exception) { }

            try
            {
                returnPodcast.Description = GetDescription(node,
                                              returnPodcast.BibleReference,
                                              node.SelectSingleNode("./p[@class='metadata']").ChildNodes[0].InnerText);
            }
            catch (Exception) { }

            //returnPodcast.Length = GetPodcastLength(returnPodcast.Uri);

            return returnPodcast;
        }

        private string GetTitle(HtmlNode node)
        {
            string title = node.InnerText;

            return title;
        }

        private string GetEpisodeUri(HtmlNode node)
        {
            string episodeUri = node.FirstChild.Attributes["href"].Value.ToString();

            return episodeUri;
        }

        private string GetDescription(HtmlNode node, string bibleReference, string metaData)
        {
            string description = String.Empty;

            try
            {
                description = metaData + bibleReference + "<br>" + node.SelectSingleNode("./p[@class='description']").InnerText;
            }
            catch (Exception) { }

            return description;
        }

        private string GetBibleReference(HtmlNode node)
        {
            string bibleReference = String.Empty;

            try
            {
                bibleReference = node.ChildNodes[1].InnerText;
            }
            catch (Exception) { }

            return bibleReference;
        }

        private string GetUri(HtmlNode node)
        {
            string uri = String.Empty;

            try { uri = node.Attributes["src"].Value.ToString(); }
            catch (Exception) { }

            return uri;
        }

        static double GetPodcastLength(string uri)
        {
            MediaPlayer player = new MediaPlayer();
            //Something is wrong. It dies here.
            player.Open(new Uri(uri, UriKind.Absolute));


            double returnLength = 0;

            if (player.NaturalDuration.HasTimeSpan)
            {
                returnLength = player.NaturalDuration.TimeSpan.TotalSeconds;
            }

            player.Close();

            return returnLength;
        }

        //private void CreateRSSFile(List<PodcastCollection> podcastCollection)
        //{
        //    string podcastPageText = "";
        //    int counter = 0;
        //    foreach (PodcastCollection podcastcollection in podcastCollection)
        //    {
        //        foreach (Podcast podcast in podcastcollection.Podcasts)
        //        {
        //            podcastPageText = podcastPageText + "<item><title>" + podcast.Title + "</title>";
        //            podcastPageText = podcastPageText + "<link>" + podcast.EpisodeUri + "</link>";
        //            podcastPageText = podcastPageText + "<description>" + podcast.Description + "</description>";
        //            podcastPageText = podcastPageText + "<guid>" + podcast.EpisodeUri + "</guid>";
        //            podcastPageText = podcastPageText + "<enclosure url='" + podcast.Uri + "' type='audio/mpeg'>" + podcast.EpisodeUri + "</enclosure>";
        //            podcastPageText = podcastPageText + "</item>";

        //            counter += 1;
        //        }
        //    }

        //    string appDataPath = Server.MapPath("/Models/PodcastTemplate.txt");

        //    using (StreamReader sr = new StreamReader(Server.MapPath("/Models/PodcastTemplate.txt")))
        //    {
        //        string contents = sr.ReadToEnd();

        //        contents = contents.Replace("###ITEMS###", podcastPageText);

        //        string fileName = Server.MapPath("/Models/Romans.rss");
        //        if (System.IO.File.Exists(fileName))
        //        {
        //            System.IO.File.Delete(fileName);
        //        }

        //        using (FileStream fs = new FileStream(fileName, FileMode.Create))
        //        {
        //            using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
        //            {
        //                w.WriteLine(contents);
        //            }
        //        }
        //    }
        //}
    }
}
