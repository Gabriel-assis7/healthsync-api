using System.ComponentModel.DataAnnotations;

namespace HealthSync.BuildingBlocks.MongoDB.Options
{
    public class MongoSettings
    {
        public bool Enabled { get; set; } = true;

        [Required]
        public string ConnectionString { get; set; } = null!;

        [Required]
        public string DatabaseName { get; set; } = null!;

        [Range(1, 300)]
        public int ConnectTimeoutSeconds { get; set; } = 30;

        public int MaxRetryAttempts { get; set; } = 3;

        public int MaxConnectionIdleTimeSeconds { get; set; } = 300;

        public int MaxPoolSize { get; set; } = 100;

        public int MinPoolSize { get; set; } = 3;
    }
}
