using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace mljPodcast
{
    public partial class PodcastContext : DbContext
    {
        //public PodcastContext() : base("DefaultConnection")
        //{
        //    //Database.Initialize(true);
        //}

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Database.SetInitializer(new PodcastInitializer());

            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public virtual DbSet<PodcastCollection> PodcastCollections { get; set; }
        public virtual DbSet<Podcast> Podcasts { get; set; }

        //public class PodcastInitializer : CreateDatabaseIfNotExists<PodcastContext>
        //{
        //    protected override void Seed(PodcastContext context)
        //    {
        //        DBInitializerRomans init = new DBInitializerRomans();
        //        PodcastCollection podcastCollection = init.GetPodcastCollection();
        //        context.PodcastCollections.Add(podcastCollection);
        //        context.SaveChanges();
        //    }
        //}
    }
}