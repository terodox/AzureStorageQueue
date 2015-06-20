namespace Monitoring.Entities.Migrations
{
	using System;
	using System.Data.Entity;
	using System.Data.Entity.Migrations;
	using System.Linq;

	internal sealed class Configuration : DbMigrationsConfiguration<Monitoring.Entities.Context.MonitoringContext>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = false;
			ContextKey = "Monitoring.Entities.Context.MonitoringContext";
		}

		protected override void Seed(Monitoring.Entities.Context.MonitoringContext context)
		{
		}
	}
}
