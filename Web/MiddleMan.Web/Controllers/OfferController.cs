namespace MiddleMan.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using MiddleMan.Common;
    using MiddleMan.Data.Common.Repositories;
    using MiddleMan.Data.Models;
    using MiddleMan.Services;
    using MiddleMan.Services.Data;
    using MiddleMan.Services.Interfaces;
    using MiddleMan.Web.ViewModels.Administration.Dashboard.InputModels;
    using MiddleMan.Web.ViewModels.InputModels;
    using MiddleMan.Web.ViewModels.Sell;
    using MiddleMan.Web.ViewModels.ViewModels;
    using MiddleMan.Web.ViewModels.ViewModels.Comment;
    using MiddleMan.Web.ViewModels.ViewModels.Offer;

    public class OfferController : BaseController
    {
        private readonly IOfferService offerService;

        private readonly ICategoryService categoryService;

        private readonly ICloudinaryService cloudinaryService;

        private readonly ICommentService commentService;

        private readonly IUserService userService;

        public OfferController(
            IOfferService offerService,
            ICategoryService categoryService,
            ICloudinaryService cloudinaryService,
            ICommentService commentService,
            IUserService userService)
        {
            this.offerService = offerService;  // todo rename sell to offer or revert
            this.categoryService = categoryService;
            this.cloudinaryService = cloudinaryService;
            this.commentService = commentService;
            this.userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await this.categoryService.GetAllCategoryViewModelsAsync();

            CreateOfferSharedModel viewModel = new CreateOfferSharedModel();
            viewModel.CreateOfferViewModel = new CreateOfferViewModel(categories);
            return this.View(viewModel);
        }

        public async Task<IActionResult> CreateOffer(CreateOfferSharedModel inputModel) // index post requsest for create
        {
            // IFormFile postedPic = this.Request.Form["Photo"];

            var a = inputModel.CreateOfferModel;
            var categoryId = await this.categoryService.GetIdByNameAsync(a.CategotyName);
            a.CategotyName = categoryId;
            a.CreatorId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var imgSize = inputModel.CreateOfferModel.Photo.Length;

            if (imgSize >= 1048576)
                return this.Redirect("/Offer");
            else if (inputModel.CreateOfferModel.Name.Length < 3 || inputModel.CreateOfferModel.Name.Length > 50)
                return this.Redirect("/Offer");
            else if (inputModel.CreateOfferModel.Description.Length < 20 || inputModel.CreateOfferModel.Description.Length > 1000)
                return this.Redirect("/Offer");
            else if (inputModel.CreateOfferModel.Price < 0.01 || inputModel.CreateOfferModel.Price > 2000)
                return this.Redirect("/Offer");

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);


            var photoUrl = await this.cloudinaryService.UploadPhotoAsync(
                a.Photo,
                $"{userId}-{a.Name}",
                GlobalConstants.CloudFolderForRecipePhotos);
            a.PicUrl = photoUrl;

            await this.offerService.CreateOfferAsync(a);

            return this.Redirect("/");
        }

        public async Task<IActionResult> Details(string id)
        {
            var categories = await this.categoryService.GetAllCategoryViewModelsAsync();
            var offer = await this.offerService.GetOfferByIdAsync(id);
            var category = await this.categoryService.GetCategoryNameByIdAsync(offer.CategoryId);
            var comments = await this.commentService.GetOfferCommentsAsync(id);
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var rated = await this.offerService.IsOfferRatedAsync(offer.Id, userId);
            double offerRating = await this.offerService.GetOfferRatingAsync(id);
            string startsStringRating = this.offerService.StartsStringRating(offerRating);

            int? offerRatedByUser = null;
            try
            {
                offerRatedByUser = await this.offerService.GetRateForOffer(offer.Id, userId);
            }
            catch (Exception)
            {
            }


            var offerView = new OfferViewModelDetails()
            {
                CreatorId = offer.CreatorId,
                Description = offer.Description,
                Name = offer.Name,
                PicUrl = offer.PicUrl,
                Price = offer.Price,
                CreatedOn = offer.CreatedOn,
                Id = offer.Id,
                Rated = rated,
                OfferRating = offerRating,
                StartsStringRating = startsStringRating,
            };

            foreach (var comment in comments)
            {
                offerView.Comments.Add(new CommentViewModel()
                {
                    CreatedOn = comment.CreatedOn.ToString("dd/M/yy H:mm"),
                    CreatorName = await this.userService.GetUsernameByIdAsync(comment.CreatorId),
                    Description = comment.Description,
                });
            }

            var detailsModel = new DetailsViewModel()
            {
                CategoryName = category,
                Categories = categories,
                Offer = offerView,
                CategoryId = offer.CategoryId,
                UserRated = offerRatedByUser,
            };

            return this.View(detailsModel);
        }

        public async Task<IActionResult> AddReview(CreateReviewModel inputModel) // index post requsest for create
        {
            try
            {
                inputModel.CreatorId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                await this.commentService.AddReviewToOffer(inputModel);
            }
            catch (Exception)
            {
                return this.Redirect("/");
            }

            return this.Redirect($"/Offer/Details?id={inputModel.Id}");
        }

        public async Task<IActionResult> Edit(string id)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!await this.offerService.IsUserCreatorOfOfferAsync(userId, id))
            {
                return this.Redirect($"/Offer/Details?id={id}");
            }

            var categories = await this.categoryService.GetAllCategoryViewModelsAsync();
            var offer = await this.offerService.GetOfferByIdAsync(id);
            var category = await this.categoryService.GetCategoryNameByIdAsync(offer.CategoryId);
            var comments = await this.commentService.GetOfferCommentsAsync(id);
            var rated = await this.offerService.IsOfferRatedAsync(offer.Id, userId);
            double offerRating = await this.offerService.GetOfferRatingAsync(id);
            string startsStringRating = this.offerService.StartsStringRating(offerRating);

            int? offerRatedByUser = null;
            try
            {
                offerRatedByUser = await this.offerService.GetRateForOffer(offer.Id, userId);
            }
            catch (Exception)
            {
            }


            var offerView = new OfferViewModelDetails()
            {
                CreatorId = offer.CreatorId,
                Description = offer.Description,
                Name = offer.Name,
                PicUrl = offer.PicUrl,
                Price = offer.Price,
                CreatedOn = offer.CreatedOn,
                Id = offer.Id,
                Rated = rated,
                OfferRating = offerRating,
                StartsStringRating = startsStringRating,
            };

            foreach (var comment in comments)
            {
                offerView.Comments.Add(new CommentViewModel()
                {
                    CreatedOn = comment.CreatedOn.ToString("dd/M/yy H:mm"),
                    CreatorName = await this.userService.GetUsernameByIdAsync(comment.CreatorId),
                    Description = comment.Description,
                });
            }

            var detailsModel = new DetailsViewModel()
            {
                CategoryName = category,
                Categories = categories,
                Offer = offerView,
                CategoryId = offer.CategoryId,
                UserRated = offerRatedByUser,
            };

            return this.View(detailsModel);
        }
    }
}
