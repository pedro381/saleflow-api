﻿namespace SaleFlow.Repository.Configurations
{
    public class MongoDbOptions
    {
        public string ConnectionString { get; set; } = default!;
        public string DatabaseName { get; set; } = default!;
    }
}
