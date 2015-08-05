using System;
using System.Collections.Generic;

namespace mljPodcast
{
    public class Podcast
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string BibleReference { get; set; }
        public string Uri { get; set; }
        public DateTime? PublicationDate { get; set; }
        public double? Length { get; set; }
        public string EpisodeUri { get; set; }
        public int PodcastCollectionid { get; set; }
        public PodcastCollection Collection { get; set; }
    }

    public class PodcastCollection
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Uri { get; set; }
        public string ParentCollectionId { get; set; }

        public virtual PodcastCollection ParentCollection { get; set; }
        public virtual ICollection<PodcastCollection> ChildCollections { get; set; }
        public virtual ICollection<Podcast> Podcasts { get; set; }
    }

    // <item>
    //  <title>411 Item 138 ZedCast with Bruce Murray - Voicemail line 206-666-4357 </title>
    //  <link>http://podcast411.com/forums/viewtopic.php?t=451</link>
    //  <guid>http://media.libsyn.com/media/podcast411/411_060325.mp3</guid>
    //  <description></description>
    //  <enclosure url = "http://media.libsyn.com/media/podcast411/411_060325.mp3" length="11779397" type="audio/mpeg"/>
    //</item>
}