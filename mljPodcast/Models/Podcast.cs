using System;
using System.Collections.Generic;

namespace mljPodcast
{
    public class Podcast
    {
        public int PodcastId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string BibleReference { get; set; }
        public string Uri { get; set; }
        public DateTime? PublicationDate { get; set; }
        public double? Length { get; set; }
        public string EpisodeUri { get; set; }
        public int PodcastCollectionId { get; set; }
        public int Order { get; set; }

        public virtual PodcastCollection PodcastCollection { get; set; }
    }

    public class PodcastCollection
    {
        public int PodcastCollectionId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Uri { get; set; }
        public int ParentCollectionId { get; set; }
        public int Order { get; set; }

        public virtual PodcastCollection ParentCollection { get; set; }
        public virtual List<PodcastCollection> ChildCollections { get; set; }
        public virtual List<Podcast> Podcasts { get; set; }
    }
}