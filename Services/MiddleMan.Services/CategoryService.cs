namespace MiddleMan.Services
{
    using System;
    using System.Threading.Tasks;

    public class CategoryService
    {
        public async Task CreateCategoryAsync(ErrorViewModel inputModel)
        {
            var imageUrls = inputModel.CreateAdDetailInputModel.Images
                .Select(async x =>
                    await cloudinaryService.UploadPictureAsync(x, x.FileName))
                .Select(x => x.Result)
                .ToList();

            var ad = mapper.Map<Ad>(inputModel);
            ad.ActiveTo = DateTime.UtcNow.AddDays(GlobalConstants.AdDuration);
            ad.Images = imageUrls.Select(x => new Image { ImageUrl = x })
                .ToList();
            ad.SellerId = usersService.GetCurrentUserId();

            await context.Ads.AddAsync(ad);
            await context.SaveChangesAsync();
        }
    }
}
