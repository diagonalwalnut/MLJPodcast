using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace mljPodcast.Models
{
    public class PodcastDataService
    {
        public void StoreCollection(string bookForPath, JsonSerializer serializer, List<PodcastCollection> collection)
        {
            string path = CreateFilePath(bookForPath);

            serializer.NullValueHandling = NullValueHandling.Ignore;

            using (StreamWriter sw = new StreamWriter(path))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, collection);
            }
        }

        public List<PodcastCollection> RetrieveCollection(string bookForPath, List<PodcastCollection> collection)
        {
            string path = CreateFilePath(bookForPath);

            if (File.Exists(path))
            { 
                string jsonText = File.ReadAllText(path);

                collection = JsonConvert.DeserializeObject<List<PodcastCollection>>(jsonText);
            }

            return collection;
        }

        public List<Podcast> RetrieveAllPodcasts(string bookForPath, List<PodcastCollection> collection, List<Podcast> podcastList)
        {
            collection = RetrieveCollection(bookForPath, collection);

            foreach (PodcastCollection pCollection in collection)
            {
                foreach (Podcast podcast in pCollection.Podcasts)
                {
                    podcastList.Add(podcast);
                }
            }

            return podcastList;
        }

        private string CreateFilePath(string bookForPath)
        {
            string folderName = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/");
            string path = System.IO.Path.Combine(folderName, bookForPath + ".json");

            return path;
        }
    }
}