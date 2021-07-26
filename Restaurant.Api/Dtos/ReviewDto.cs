using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Restaurant.Api.Models;

namespace Restaurant.Api.Dtos
{
    public record ReviewDto(Guid Id, int Star, string User, string Description, DateTimeOffset CreatedDate, DateTimeOffset LatestEditedDate);
    public record CreateReviewDto([Required] int Star, string Description);
    public record UpdateReviewDto([Required] int Star, string Description);
}