namespace PinkPanther.BlueCrocodile.Seed
{
    internal class Program
    {
        private const string ConnectionString = "mongodb://localhost/pinkpanther";
        private const string Database = "pinkpanther";

        private static void Main(string[] args)
        {
            var seeder = new Seeder(ConnectionString);
            seeder.SeedAsync().Wait();
        }
    }
}