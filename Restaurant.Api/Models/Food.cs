using System;
using System.Collections.Generic;

namespace Restaurant.Api.Models
{
    public class Food
    {
        public Guid Id { get; init; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public List<Review> Reviews { get; set; }

    }
}