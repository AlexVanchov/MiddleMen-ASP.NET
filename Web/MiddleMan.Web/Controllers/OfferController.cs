namespace MiddleMan.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
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
    using MiddleMan.Web.ViewModels.InputModels.Offer;
    using MiddleMan.Web.ViewModels.Search;
    using MiddleMan.Web.ViewModels.ViewModels;
    using MiddleMan.Web.ViewModels.ViewModels.Comment;
    using MiddleMan.Web.ViewModels.ViewModels.Offer;
    using MiddleMan.Web.ViewModels.ViewModels.Search;

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

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var categories = await this.categoryService.GetAllCategoryViewModelsAsync();

            CreateOfferSharedModel viewModel = new CreateOfferSharedModel();
            viewModel.CreateOfferViewModel = new CreateOfferViewModel(categories);
            return this.View(viewModel);
        }

        public async Task<IActionResult> CreateOffer(CreateOfferSharedModel inputModel) // index post requsest for create
        {
            if (!this.ModelState.IsValid)
            {
                return this.Redirect("/Offer");
            }

            var imgSize = inputModel.CreateOfferModel.Photo.Length;

            if (imgSize >= 1048576)
            {
                return this.Redirect("/Offer");
            }

            var offerInput = inputModel.CreateOfferModel;
            var categoryId = await this.categoryService.GetIdByNameAsync(offerInput.CategotyName);
            offerInput.CategotyName = categoryId;
            offerInput.CreatorId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var photoUrl = await this.cloudinaryService.UploadPhotoAsync(
                offerInput.Photo,
                $"{userId}-{offerInput.Name}",
                GlobalConstants.CloudFolderForOfferPhotos);
            offerInput.PicUrl = photoUrl;

            await this.offerService.CreateOfferAsync(offerInput);

            return this.Redirect("/");
        }

        public async Task<IActionResult> Details(string id)
        {
            var offer = await this.offerService.GetOfferByIdAsync(id);

            if (offer != null)
            {
                var categories = await this.categoryService.GetAllCategoryViewModelsAsync();
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
                    CreatorName = this.userService.GetUserByIdAsync(offer.CreatorId).Result.UserName,
                    Description = offer.Description,
                    Name = offer.Name,
                    PicUrl = offer.PicUrl,
                    Price = offer.Price,
                    CreatedOn = offer.CreatedOn,
                    Id = offer.Id,
                    Rated = rated,
                    OfferRating = offerRating,
                    StartsStringRating = startsStringRating,
                    IsFavoritedByUser = await this.userService.IsOfferFavoritedByUserAsync(offer.Id, userId),
                };

                foreach (var comment in comments)
                {
                    offerView.Comments.Add(new CommentViewModel()
                    {
                        CreatedOn = comment.CreatedOn.ToString("dd/M/yy H:mm"),
                        CreatorName = await this.userService.GetUsernameByIdAsync(comment.CreatorId),
                        Description = comment.Description,
                        CreatorId = comment.CreatorId,
                    });
                }

                var detailsModel = new DetailsViewModel()
                {
                    CategoryName = category,
                    Categories = categories,
                    Offer = offerView,
                    CategoryId = offer.CategoryId,
                    UserRated = offerRatedByUser,
                    IsOwner = offer.CreatorId == userId ? true : false,
                };

                return this.View(detailsModel);
            }

            return this.NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddReview(CreateReviewModel commentInputModel, string id) // index post requsest for create
        {
            if (!this.ModelState.IsValid)
            {
                return this.Redirect($"/Offer/Edit?id={id}");
            }

            commentInputModel.CreatorId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            commentInputModel.Id = id;

            await this.commentService.AddReviewToOffer(commentInputModel);

            return this.Redirect($"/Offer/Details?id={commentInputModel.Id}");
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
                BuyContent = offer.BuyContent,
            };

            foreach (var comment in comments)
            {
                offerView.Comments.Add(new CommentViewModel()
                {
                    CreatedOn = comment.CreatedOn.ToString("dd/M/yy H:mm"),
                    CreatorName = await this.userService.GetUsernameByIdAsync(comment.CreatorId),
                    Description = comment.Description,
                    CreatorId = comment.CreatorId,
                });
            }

            var detailsModel = new EditViewModel()
            {
                CategoryName = category,
                Categories = categories,
                Offer = offerView,
                CategoryId = offer.CategoryId,
                BuyContent = offer.BuyContent,
            };

            return this.View(detailsModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, EditOfferModel inputModel) // index post requsest for create
        {
            if (!this.ModelState.IsValid)
            {
                return this.Redirect($"/Offer/Edit?id={id}");
            }

            var offerInput = inputModel;
            var imgSize = offerInput.Photo != null ? inputModel.Photo.Length : 1;

            if (imgSize >= 1048576)
            {
                return this.Redirect($"/Offer/Edit?id={id}");
            }

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!await this.offerService.IsUserCreatorOfOfferAsync(userId, id))
            {
                return this.Redirect($"/Offer/Details?id={id}");
            }

            var categoryId = await this.categoryService.GetIdByNameAsync(offerInput.CategoryName);
            offerInput.CategoryId = categoryId;

            if (imgSize >= 1048576)
            {
                return this.Redirect($"/Offer/Edit?id={id}");
            }

            await this.offerService.EditOfferAsync(offerInput, id);

            return this.Redirect("/");
        }

        public async Task<IActionResult> Search(SearchInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Redirect("/Home/Search");
            }

            var offers = await this.offerService.GetOffersBySearchAsync(input.SearchWord);

            var searchModel = new SearchViewModel();
            searchModel.SearchWord = input.SearchWord;

            foreach (var offer in offers)
            {
                var categoryName = await this.categoryService.GetCategoryNameByIdAsync(offer.CategoryId);
                searchModel.Offers.Add(new OfferViewModelDetails()
                {
                    Name = offer.Name,
                    CreatedOn = offer.CreatedOn,
                    CreatorId = offer.CreatorId,
                    Description = offer.Description.Length >= 65 ? offer.Description.Substring(0, 65) : offer.Description,
                    PicUrl = offer.PicUrl,
                    Price = offer.Price,
                    Id = offer.Id,
                    CategoryName = categoryName,
                    IsFeatured = offer.IsFeatured,
                });
            }

            return this.View(searchModel);
        }
    }
}
