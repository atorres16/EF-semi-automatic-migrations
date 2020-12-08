namespace EF_semi_automatic_migrations.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class property1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Things", "Property1", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Things", "Property1");
        }
    }
}
