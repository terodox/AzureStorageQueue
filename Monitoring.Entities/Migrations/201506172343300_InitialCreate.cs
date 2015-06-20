namespace Monitoring.Entities.Migrations
{
	using System;
	using System.Data.Entity.Migrations;
	
	public partial class InitialCreate : DbMigration
	{
		public override void Up()
		{
			CreateTable(
				"dbo.Queues",
				c => new
					{
						Id = c.Int(nullable: false, identity: true),
						Name = c.String(nullable: false, defaultValue: ""),
						Created = c.DateTime(nullable: false, defaultValue: DateTime.Now),
						Updated = c.DateTime(nullable: false, defaultValue: DateTime.Now),
					})
				.PrimaryKey(t => t.Id);
		}
		
		public override void Down()
		{
			DropTable("dbo.Queues");
		}
	}
}
