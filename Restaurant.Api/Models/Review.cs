using System;

namespace Restaurant.Api.Models
{
    public class Review
    {
        public Guid Id { get; init; }
        public int Star { get; set; }
        public string User { get; set; }
        public string Description { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset LatestEditedDate { get; set; }
    }
}