namespace TestApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.News",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        ShortDescrpition = c.String(),
                        Created = c.DateTime(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NewsFile_Mapping",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        News_Id = c.Int(nullable: false),
                        FileId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.File", t => t.FileId, cascadeDelete: true)
                .ForeignKey("dbo.News", t => t.News_Id, cascadeDelete: true)
                .Index(t => t.News_Id)
                .Index(t => t.FileId);
            
            CreateTable(
                "dbo.File",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BinaryData = c.Binary(),
                        ContentType = c.String(),
                        FileName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PlayListFile_Mapping",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PlayList_Id = c.Int(nullable: false),
                        FileId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.File", t => t.FileId, cascadeDelete: true)
                .ForeignKey("dbo.Playlist", t => t.PlayList_Id, cascadeDelete: true)
                .Index(t => t.PlayList_Id)
                .Index(t => t.FileId);
            
            CreateTable(
                "dbo.Playlist",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        CreatedBy = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PlayListTag_Mapping",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PlayList_Id = c.Int(nullable: false),
                        TagId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Playlist", t => t.PlayList_Id, cascadeDelete: true)
                .ForeignKey("dbo.Tag", t => t.TagId, cascadeDelete: true)
                .Index(t => t.PlayList_Id)
                .Index(t => t.TagId);
            
            CreateTable(
                "dbo.Tag",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NewsTag_Mapping",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        News_Id = c.Int(nullable: false),
                        TagId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.News", t => t.News_Id, cascadeDelete: true)
                .ForeignKey("dbo.Tag", t => t.TagId, cascadeDelete: true)
                .Index(t => t.News_Id)
                .Index(t => t.TagId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NewsFile_Mapping", "News_Id", "dbo.News");
            DropForeignKey("dbo.PlayListFile_Mapping", "PlayList_Id", "dbo.Playlist");
            DropForeignKey("dbo.PlayListTag_Mapping", "TagId", "dbo.Tag");
            DropForeignKey("dbo.NewsTag_Mapping", "TagId", "dbo.Tag");
            DropForeignKey("dbo.NewsTag_Mapping", "News_Id", "dbo.News");
            DropForeignKey("dbo.PlayListTag_Mapping", "PlayList_Id", "dbo.Playlist");
            DropForeignKey("dbo.PlayListFile_Mapping", "FileId", "dbo.File");
            DropForeignKey("dbo.NewsFile_Mapping", "FileId", "dbo.File");
            DropIndex("dbo.NewsTag_Mapping", new[] { "TagId" });
            DropIndex("dbo.NewsTag_Mapping", new[] { "News_Id" });
            DropIndex("dbo.PlayListTag_Mapping", new[] { "TagId" });
            DropIndex("dbo.PlayListTag_Mapping", new[] { "PlayList_Id" });
            DropIndex("dbo.PlayListFile_Mapping", new[] { "FileId" });
            DropIndex("dbo.PlayListFile_Mapping", new[] { "PlayList_Id" });
            DropIndex("dbo.NewsFile_Mapping", new[] { "FileId" });
            DropIndex("dbo.NewsFile_Mapping", new[] { "News_Id" });
            DropTable("dbo.NewsTag_Mapping");
            DropTable("dbo.Tag");
            DropTable("dbo.PlayListTag_Mapping");
            DropTable("dbo.Playlist");
            DropTable("dbo.PlayListFile_Mapping");
            DropTable("dbo.File");
            DropTable("dbo.NewsFile_Mapping");
            DropTable("dbo.News");
        }
    }
}
