namespace Restaurant.Api.Settings
{
    public class MongoDbSettings
    {
        public const string databaseName = "restaurant";
        public string User { get; set; }
        public string Password { get; set; }

        public string ConnectionString
        {
            get
            {
                return $"mongodb+srv://{User}:{Password}@cluster0.vnbo4.mongodb.net/{databaseName}?retryWrites=true&w=majority";
            }
        }

    }
}