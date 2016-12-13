﻿using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web.Mvc;
using System.Windows;
using System.Windows.Media;

namespace mljPodcast.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Podcast()
        {
            string uri = "http://www.mljtrust.org/collections/book-of-romans/";

            List<PodcastCollection> podcastCollection = new List<PodcastCollection>();
            HtmlDocument htmlDoc = GetHtmlDoc(uri);

            podcastCollection = GetCollections(htmlDoc, "chapter-list", uri);

            ViewBag.PodcastList = podcastCollection;

            return View();
        }

        private List<Podcast> ProcessPodcastSubcollection(string uri)
        {
            HtmlDocument htmlDoc = GetHtmlDoc(uri);
            List<string> pages = GetSubCollections(htmlDoc, "pagination");
            int currentPage = 1;

            List<Podcast> podcastList = new List<Podcast>();

            for (int i = 0; i < (pages.Count + 1); i++)
            {
                htmlDoc = GetHtmlDoc(uri + "?page=" + i);
                podcastList.AddRange(GetPodcasts(htmlDoc));

                currentPage += 1;
            }

            return podcastList;
        }

        private List<PodcastCollection> GetCollections(HtmlDocument htmldoc, string xpath, string uri)
        {
            List<PodcastCollection> collectionList = new List<PodcastCollection>();

            Uri siteuri = new Uri(uri);
            string baseUri = siteuri.Scheme + "://" + siteuri.Authority;

            HtmlNode bodyNode = htmldoc.DocumentNode.SelectSingleNode("//body");

            //bodyNode.SelectSingleNode("//ul[@class='" + xpath + "']").SelectSingleNode("//span");
            //collectionList.Add(new PodcastCollection { Title = "Chapter 1", Podcasts = ProcessPodcastSubcollection(baseUri) });

            if (bodyNode != null)
            {
                IEnumerable<HtmlNode> tempCollectionList = bodyNode.SelectSingleNode("//ul[@class='" + xpath + "']").Descendants("a");

                foreach (var item in tempCollectionList)
                {
                    try
                    {
                        collectionList.Add(new PodcastCollection { Uri = item.Attributes["href"].Value.ToString(), Title = item.Attributes["title"].Value.ToString(), Podcasts = ProcessPodcastSubcollection(baseUri + item.Attributes["href"].Value.ToString()) });
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
            WebRequest req = HttpWebRequest.Create(uri);
            req.Method = "GET";

            string source;
            using (StreamReader reader = new StreamReader(req.GetResponse().GetResponseStream()))
            {
                source = reader.ReadToEnd();
            }

            HtmlWeb hw = new HtmlWeb();
            HtmlDocument htmlDoc = hw.Load(uri);

            htmlDoc.OptionFixNestedTags = true;

            return htmlDoc;
        }

        private List<Podcast> GetPodcasts(HtmlDocument htmldoc)
        {
            string xPathString = "//div[@class='span8 sermon']";
            HtmlNode bodyNode = htmldoc.DocumentNode.SelectSingleNode("//body");

            List<Podcast> returnPodcastList = new List<Podcast>();
            foreach (HtmlNode childNode in bodyNode.SelectNodes(xPathString))
            {
                returnPodcastList.Add(GetPodcastData(childNode));
            }

            return returnPodcastList;
        }

        private Podcast GetPodcastData(HtmlNode node)
        {
            Podcast returnPodcast = new Podcast();

            try { returnPodcast.Title = GetTitle(node.ChildNodes.FindFirst("h3")); }
            catch (Exception) { }

            try { returnPodcast.EpisodeUri = GetEpisodeUri(node.ChildNodes.FindFirst("h3")); }
            catch (Exception) { }

            try { returnPodcast.Uri = GetUri(node.ChildNodes.FindFirst("audio")); }
            catch (Exception) { }

            try { returnPodcast.BibleReference = GetBibleReference(node.SelectSingleNode("//p[@class='metadata']")); }
            catch (Exception) { }

            try
            {
                returnPodcast.Description = GetDescription(node,
                                              returnPodcast.BibleReference,
                                              node.SelectSingleNode("//p[@class='metadata']").ChildNodes[0].InnerText);
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
                description = metaData + bibleReference + "<br>" + node.SelectSingleNode("//p[@class='description']").InnerText;
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

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}