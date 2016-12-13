using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace mljPodcast
{
    class PodcastInitial
    {
        public void Seed()
        {
            PodcastContext context = new PodcastContext();
            DBInitializerRomans init = new DBInitializerRomans();
            PodcastCollection podcastCollection = init.GetPodcastCollection();
            context.PodcastCollections.Add(podcastCollection);
            context.SaveChanges();
        }
    }
}