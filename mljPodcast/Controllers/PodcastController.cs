using mljPodcast.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mljPodcast.Controllers
{
    public class PodcastController : Controller
    {
        // GET: Podcast
        public ActionResult Index(int chapter = 0)
        {
            return View();
        }

        public ActionResult RomansStatic()
        {
            return View();
        }

        public ActionResult Romans(int id = 0)
        {
            List<Podcast> podcastModelForView = new List<Podcast>();
            PodcastDataService podcastDataService = new PodcastDataService();

            podcastModelForView = podcastDataService.RetrieveAllPodcasts("Romans", new List<PodcastCollection>(), podcastModelForView);            

            return View(podcastModelForView);
        }
    }
}