using Shop.Domain.Localization;

namespace Shop.Application.Infrastructure.Data.Extensions
{
    public static class ModelBuilderExtension
    {
        public static void SeedData(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Language>().HasData(
                new Language()
                {
                    Id = 1,
                    Name = "English",
                    Code = "en",
                    Culture = "en-US",
                    DisplayOrder = 0,
                    IsRtl = false,
                    IsActive = true
                },
                new Language()
                {
                    Id = 2,
                    Name = "VietNamese",
                    Code = "vi",
                    Culture = "vi-VN",
                    DisplayOrder = 0,
                    IsRtl = false,
                    IsActive = true
                });
        }
    }
}
