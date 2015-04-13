namespace Sample.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mod0001 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationUsers", "Birthday", c => c.DateTime());
            AlterColumn("dbo.ApplicationUsers", "ActivatedTime", c => c.DateTime());
            AlterColumn("dbo.ApplicationUsers", "RegisterTime", c => c.DateTime());
            DropColumn("dbo.ApplicationUsers", "ProviderKey");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ApplicationUsers", "ProviderKey", c => c.String());
            AlterColumn("dbo.ApplicationUsers", "RegisterTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ApplicationUsers", "ActivatedTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.ApplicationUsers", "Birthday");
        }
    }
}
