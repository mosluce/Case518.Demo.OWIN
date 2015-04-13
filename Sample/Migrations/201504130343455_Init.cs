namespace Sample.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApplicationUsers",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ProviderKey = c.String(),
                        Name = c.String(),
                        Email = c.String(),
                        Password = c.String(),
                        Captcha = c.String(),
                        Subscription = c.Boolean(nullable: false),
                        Gender = c.Int(nullable: false),
                        Activated = c.Boolean(nullable: false),
                        ActivationCode = c.String(),
                        ActivatedTime = c.DateTime(nullable: false),
                        RegisterTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ApplicationUsers");
        }
    }
}
