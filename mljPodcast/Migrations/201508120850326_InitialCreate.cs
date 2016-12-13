namespace mljPodcast.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PodcastCollection",
                c => new
                    {
                        PodcastCollectionId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        Uri = c.String(),
                        ParentCollectionId = c.Int(),
                        ParentCollection_PodcastCollectionId = c.Int(),
                    })
                .PrimaryKey(t => t.PodcastCollectionId)
                .ForeignKey("dbo.PodcastCollection", t => t.ParentCollection_PodcastCollectionId)
                .Index(t => t.ParentCollection_PodcastCollectionId);
            
            CreateTable(
                "dbo.Podcast",
                c => new
                    {
                        PodcastId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        BibleReference = c.String(),
                        Uri = c.String(),
                        PublicationDate = c.DateTime(),
                        Length = c.Double(),
                        EpisodeUri = c.String(),
                        PodcastCollectionId = c.Int(nullable: false),
                        Collection_PodcastCollectionId = c.Int(),
                        ParentCollection_PodcastCollectionId = c.Int(),
                        PodcastCollection_PodcastCollectionId = c.Int(),
                    })
                .PrimaryKey(t => t.PodcastId)
                .ForeignKey("dbo.PodcastCollection", t => t.Collection_PodcastCollectionId)
                .ForeignKey("dbo.PodcastCollection", t => t.ParentCollection_PodcastCollectionId)
                .ForeignKey("dbo.PodcastCollection", t => t.PodcastCollection_PodcastCollectionId)
                .Index(t => t.Collection_PodcastCollectionId)
                .Index(t => t.ParentCollection_PodcastCollectionId)
                .Index(t => t.PodcastCollection_PodcastCollectionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Podcast", "PodcastCollection_PodcastCollectionId", "dbo.PodcastCollection");
            DropForeignKey("dbo.Podcast", "ParentCollection_PodcastCollectionId", "dbo.PodcastCollection");
            DropForeignKey("dbo.Podcast", "Collection_PodcastCollectionId", "dbo.PodcastCollection");
            DropForeignKey("dbo.PodcastCollection", "ParentCollection_PodcastCollectionId", "dbo.PodcastCollection");
            DropIndex("dbo.Podcast", new[] { "PodcastCollection_PodcastCollectionId" });
            DropIndex("dbo.Podcast", new[] { "ParentCollection_PodcastCollectionId" });
            DropIndex("dbo.Podcast", new[] { "Collection_PodcastCollectionId" });
            DropIndex("dbo.PodcastCollection", new[] { "ParentCollection_PodcastCollectionId" });
            DropTable("dbo.Podcast");
            DropTable("dbo.PodcastCollection");
        }
    }
}
