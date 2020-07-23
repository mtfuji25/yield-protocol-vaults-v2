using System.Data.Entity.Migrations;

namespace DataManager.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<DataManager.Models.DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Models.DataContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
