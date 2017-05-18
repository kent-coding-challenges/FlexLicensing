namespace FlexLicensing.Calculator.Migrations
{
    using FlexLicensing.Calculator.Database;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<FlexDbContext>
    {
        public Configuration()
        {
            // Options for development, ideall both should be disabled on production environment.
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }
    }
}
