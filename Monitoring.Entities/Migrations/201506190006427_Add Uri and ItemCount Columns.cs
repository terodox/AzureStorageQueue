namespace Monitoring.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUriandItemCountColumns : DbMigration
    {
        public override void Up()
        {
			AddColumn("dbo.Queues", "ItemCount", c => c.Int(defaultValue: 0, nullable: false));
			AddColumn("dbo.Queues", "Uri", c => c.String(defaultValue: "", nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Queues", "Uri");
        }
    }
}
