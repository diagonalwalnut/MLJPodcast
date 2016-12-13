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

        public ActionResult Romans(int id = 0)
        {
            PodcastViewModel vm = new PodcastViewModel(id);

            return View(vm.PodcastCollection.AsEnumerable());
        }

        public ActionResult ViewPodcastData()
        {
            DBInitializerRomans data = new DBInitializerRomans();

            PodcastCollection p = data.GetPodcastCollection();

            return View();
        }

        public ActionResult UpdateData()
        {
            DBInitializerRomans init = new DBInitializerRomans();
            PodcastCollection podcastCollection = init.GetPodcastCollection();

            //PodcastContext dbcontext = new PodcastContext();
            //dbcontext.PodcastCollections.Add(podcastCollection);
            //dbcontext.SaveChanges();

            return View(podcastCollection.ChildCollections);
        }
    }
}