namespace MiddleMan.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using MiddleMan.Data.Models;
    using MiddleMan.Services.Data;
    using MiddleMan.Services.Interfaces;
    using MiddleMan.Web.ViewModels.Administration.Dashboard.InputModels;
    using MiddleMan.Web.ViewModels.InputModels;
    using MiddleMan.Web.ViewModels.InputModels.Offer;
    using MiddleMan.Web.ViewModels.ViewModels;
    using MiddleMan.Web.ViewModels.ViewModels.Offer;
    using Moq;
    using Xunit;

    public class CategoryServiceTests
    {
        private ICategoryService categoryService;
        private IUserService userService;
        private IOfferService offerService;
        private ICommentService commentService;

        public CategoryServiceTests()
        {
        }

        [Fact]
        public async Task CreateCategoryAsync_WithValidData_ShouldCreateNewCategory()
        {
            var expected = 1;
            var guid = Guid.NewGuid().ToString();

            var moqHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var moqCategoriesService = new Mock<ICategoryService>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();

            var context = InitializeContext.CreateContextForInMemory();
            this.userService = new UserService(context, moqHttpContextAccessor.Object);
            this.categoryService = new CategoryService(context, this.userService);

            var user = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
            };

            var createOfferInputModel = new CreateOfferModel()
            {
                Name = "Wow Account",
                CategotyName = "Wow",
                CreatorId = guid,
                Description = "Some Test Description",
                Price = 10.00,
                PicUrl = "link",
            };

            this.offerService = new OfferService(context, moqCategoriesService.Object, moqCloudinaryService.Object);

            var offer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            await this.offerService.ApproveOfferAsync(offer.Id);

            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Assert

            await this.categoryService.CreateCategoryAsync(new CreateCategoryModel()
            {
                Name = "Wow",
            });

            var category = await context.Categories.FirstOrDefaultAsync(x => x.Name == "Wow");
            Assert.NotNull(category);
        }

        [Fact]
        public async Task GetAllCategoryViewModelsAsync_WithValidData_ShouldReturnAllCategoriesVModels()
        {
            var expected = new List<CategoryViewModel>() {
            new CategoryViewModel()
                {
                Name = "Wow",
                Position = 1,
                OffersCount = 1,
                },
            };
            var guid = Guid.NewGuid().ToString();

            var moqHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var moqCategoriesService = new Mock<ICategoryService>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();

            var context = InitializeContext.CreateContextForInMemory();
            this.userService = new UserService(context, moqHttpContextAccessor.Object);
            this.categoryService = new CategoryService(context, this.userService);

            var user = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
            };

            var category = await this.categoryService.CreateCategoryAsync(new CreateCategoryModel()
            {
                Name = "Wow",
            });

            expected[0].Id = category.Id;

            var createOfferInputModel = new CreateOfferModel()
            {
                Name = "Wow Account",
                CategotyName = category.Id,
                CreatorId = guid,
                Description = "Some Test Description",
                Price = 10.00,
                PicUrl = "link",
            };

            this.offerService = new OfferService(context, moqCategoriesService.Object, moqCloudinaryService.Object);

            var offer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            await this.offerService.ApproveOfferAsync(offer.Id);

            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Assert
            var categories = await this.categoryService.GetAllCategoryViewModelsAsync();
            Assert.Equal(expected.Count, categories.Count);
            Assert.Equal(expected[0].Id, categories[0].Id);
            Assert.Equal(expected[0].OffersCount, categories[0].OffersCount);
        }

        [Fact]
        public async Task GetAllOffersFromCategoryViewModelsAsync_WithValidData_ShouldReturnAllOfferCategoriesVModels()
        {
            var guid = Guid.NewGuid().ToString();

            var moqHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var moqCategoriesService = new Mock<ICategoryService>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();

            var context = InitializeContext.CreateContextForInMemory();
            this.userService = new UserService(context, moqHttpContextAccessor.Object);
            this.categoryService = new CategoryService(context, this.userService);

            var user = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
            };

            var category = await this.categoryService.CreateCategoryAsync(new CreateCategoryModel()
            {
                Name = "Wow",
            });

            var createOfferInputModel = new CreateOfferModel()
            {
                Name = "Wow Account",
                CategotyName = category.Id,
                CreatorId = guid,
                Description = "Some Test Description",
                Price = 10.00,
                PicUrl = "link",
            };

            this.offerService = new OfferService(context, moqCategoriesService.Object, moqCloudinaryService.Object);

            var offer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            await this.offerService.ApproveOfferAsync(offer.Id);
            await context.Offers.FirstOrDefaultAsync(x => x.CategoryId == category.Id);

            var offerRating = await this.categoryService.GetOfferRatingAsync(offer.Id);
            var isFavoritedByUser = await this.userService.IsOfferFavoritedByUserAsync(offer.Id, guid);

            var expected = new List<OfferViewModel>() {
            new OfferViewModel()
                {
                    Id = offer.Id,
                    Name = offer.Name,
                    Description = offer.Description.Length >= 65 ? offer.Description.Substring(0, 65) : offer.Description,
                    Price = offer.Price,
                    PicUrl = offer.PicUrl,
                    ClickUrl = $"/Offer/Details?id={offer.Id}",
                    ReadMore = offer.Description.Length >= 65 ? true : false,
                    IsFavoritedByUser = isFavoritedByUser,
                    StartsStringRating = this.categoryService.StartsStringRating(offerRating),
                },
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Assert
            var offers = await this.categoryService.GetAllOffersFromCategoryViewModelsAsync(category.Id, guid, 0);
            Assert.Equal(expected[0].Id, offers[0].Id);
            Assert.Equal(expected[0].IsFavoritedByUser, offers[0].IsFavoritedByUser);
            Assert.Equal(expected[0].ReadMore, offers[0].ReadMore);
            Assert.Equal(expected[0].ClickUrl, offers[0].ClickUrl);
            Assert.Equal(expected[0].StartsStringRating, offers[0].StartsStringRating);
        }

        [Fact]
        public async Task GetAllOffersFromCategoryViewModelsAsync_WithInValidData_ShouldReturnEmptyList()
        {
            var guid = Guid.NewGuid().ToString();

            var moqHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var moqCategoriesService = new Mock<ICategoryService>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();

            var context = InitializeContext.CreateContextForInMemory();
            this.userService = new UserService(context, moqHttpContextAccessor.Object);
            this.categoryService = new CategoryService(context, this.userService);

            var user = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
            };

            var category = await this.categoryService.CreateCategoryAsync(new CreateCategoryModel()
            {
                Name = "Wow",
            });

            var createOfferInputModel = new CreateOfferModel()
            {
                Name = "Wow Account",
                CategotyName = category.Id,
                CreatorId = guid,
                Description = "Some Test Description",
                Price = 10.00,
                PicUrl = "link",
            };

            this.offerService = new OfferService(context, moqCategoriesService.Object, moqCloudinaryService.Object);

            var offer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            await this.offerService.ApproveOfferAsync(offer.Id);
            await context.Offers.FirstOrDefaultAsync(x => x.CategoryId == category.Id);

            var offerRating = await this.categoryService.GetOfferRatingAsync(offer.Id);
            var isFavoritedByUser = await this.userService.IsOfferFavoritedByUserAsync(offer.Id, guid);

            var expected = new List<OfferViewModel>();

            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Assert
            var offers = await this.categoryService.GetAllOffersFromCategoryViewModelsAsync("InvalidID", guid, 1);
            Assert.Equal(expected, offers);
        }

        [Fact]
        public async Task GetIdByNameAsync_WithValidData_ShouldReturnId()
        {
            string expected;
            var guid = Guid.NewGuid().ToString();

            var moqHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var moqCategoriesService = new Mock<ICategoryService>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();

            var context = InitializeContext.CreateContextForInMemory();
            this.userService = new UserService(context, moqHttpContextAccessor.Object);
            this.categoryService = new CategoryService(context, this.userService);

            var user = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
            };

            var category = await this.categoryService.CreateCategoryAsync(new CreateCategoryModel()
            {
                Name = "Wow",
            });

            expected = category.Id;

            var createOfferInputModel = new CreateOfferModel()
            {
                Name = "Wow Account",
                CategotyName = category.Id,
                CreatorId = guid,
                Description = "Some Test Description",
                Price = 10.00,
                PicUrl = "link",
            };

            this.offerService = new OfferService(context, moqCategoriesService.Object, moqCloudinaryService.Object);

            var offer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            await this.offerService.ApproveOfferAsync(offer.Id);

            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Assert
            var categories = await this.categoryService.GetIdByNameAsync(category.Name);
            Assert.Equal(expected, categories);
        }

        [Fact]
        public async Task GetIdByNameAsync_WithInValidData_ShouldReturnNull()
        {
            string expected;
            var guid = Guid.NewGuid().ToString();

            var moqHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var moqCategoriesService = new Mock<ICategoryService>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();

            var context = InitializeContext.CreateContextForInMemory();
            this.userService = new UserService(context, moqHttpContextAccessor.Object);
            this.categoryService = new CategoryService(context, this.userService);

            var user = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
            };

            var category = await this.categoryService.CreateCategoryAsync(new CreateCategoryModel()
            {
                Name = "Wow",
            });

            var createOfferInputModel = new CreateOfferModel()
            {
                Name = "Wow Account",
                CategotyName = category.Id,
                CreatorId = guid,
                Description = "Some Test Description",
                Price = 10.00,
                PicUrl = "link",
            };

            this.offerService = new OfferService(context, moqCategoriesService.Object, moqCloudinaryService.Object);

            var offer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            await this.offerService.ApproveOfferAsync(offer.Id);

            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Assert
            var categories = await this.categoryService.GetIdByNameAsync("InvalidName");
            Assert.Null(categories);
        }

        [Fact]
        public async Task HasNextPage_WithValidData_ShouldReturnFalse()
        {
            string expected;
            var guid = Guid.NewGuid().ToString();

            var moqHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var moqCategoriesService = new Mock<ICategoryService>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();

            var context = InitializeContext.CreateContextForInMemory();
            this.userService = new UserService(context, moqHttpContextAccessor.Object);
            this.categoryService = new CategoryService(context, this.userService);

            var user = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
            };

            var category = await this.categoryService.CreateCategoryAsync(new CreateCategoryModel()
            {
                Name = "Wow",
            });

            var createOfferInputModel = new CreateOfferModel()
            {
                Name = "Wow Account",
                CategotyName = category.Id,
                CreatorId = guid,
                Description = "Some Test Description",
                Price = 10.00,
                PicUrl = "link",
            };

            this.offerService = new OfferService(context, moqCategoriesService.Object, moqCloudinaryService.Object);

            var offer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            await this.offerService.ApproveOfferAsync(offer.Id);

            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Assert
            var categories = await this.categoryService.HasNextPage(category.Id, 1);
            Assert.False(categories);
        }

        [Fact]
        public async Task GetCategoryNameByIdAsync_WithValidData_ShouldReturnName()
        {
            string expected;
            var guid = Guid.NewGuid().ToString();

            var moqHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var moqCategoriesService = new Mock<ICategoryService>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();

            var context = InitializeContext.CreateContextForInMemory();
            this.userService = new UserService(context, moqHttpContextAccessor.Object);
            this.categoryService = new CategoryService(context, this.userService);

            var user = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
            };

            var category = await this.categoryService.CreateCategoryAsync(new CreateCategoryModel()
            {
                Name = "Wow",
            });

            expected = category.Name;

            var createOfferInputModel = new CreateOfferModel()
            {
                Name = "Wow Account",
                CategotyName = category.Id,
                CreatorId = guid,
                Description = "Some Test Description",
                Price = 10.00,
                PicUrl = "link",
            };

            this.offerService = new OfferService(context, moqCategoriesService.Object, moqCloudinaryService.Object);

            var offer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            await this.offerService.ApproveOfferAsync(offer.Id);

            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Assert
            var categories = await this.categoryService.GetCategoryNameByIdAsync(category.Id);
            Assert.Equal(expected, categories);
        }

        [Fact]
        public async Task GetCategoryNameByIdAsync_WithInValidData_ShouldReturnNull()
        {
            string expected;
            var guid = Guid.NewGuid().ToString();

            var moqHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var moqCategoriesService = new Mock<ICategoryService>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();

            var context = InitializeContext.CreateContextForInMemory();
            this.userService = new UserService(context, moqHttpContextAccessor.Object);
            this.categoryService = new CategoryService(context, this.userService);

            var user = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
            };

            var category = await this.categoryService.CreateCategoryAsync(new CreateCategoryModel()
            {
                Name = "Wow",
            });

            expected = category.Name;

            var createOfferInputModel = new CreateOfferModel()
            {
                Name = "Wow Account",
                CategotyName = category.Id,
                CreatorId = guid,
                Description = "Some Test Description",
                Price = 10.00,
                PicUrl = "link",
            };

            this.offerService = new OfferService(context, moqCategoriesService.Object, moqCloudinaryService.Object);

            var offer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            await this.offerService.ApproveOfferAsync(offer.Id);

            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Assert
            var categories = await this.categoryService.GetCategoryNameByIdAsync("InvalidID");
            Assert.Null(categories);
        }

        [Fact]
        public async Task GetOffersCountInCategoryByIdAsync_WithValidData_ShouldReturnCount()
        {
            int expected;
            var guid = Guid.NewGuid().ToString();

            var moqHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var moqCategoriesService = new Mock<ICategoryService>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();

            var context = InitializeContext.CreateContextForInMemory();
            this.userService = new UserService(context, moqHttpContextAccessor.Object);
            this.categoryService = new CategoryService(context, this.userService);

            var user = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
            };

            var category = await this.categoryService.CreateCategoryAsync(new CreateCategoryModel()
            {
                Name = "Wow",
            });

            expected = 1;

            var createOfferInputModel = new CreateOfferModel()
            {
                Name = "Wow Account",
                CategotyName = category.Id,
                CreatorId = guid,
                Description = "Some Test Description",
                Price = 10.00,
                PicUrl = "link",
            };

            this.offerService = new OfferService(context, moqCategoriesService.Object, moqCloudinaryService.Object);

            var offer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            await this.offerService.ApproveOfferAsync(offer.Id);

            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Assert
            var categories = await this.categoryService.GetOffersCountInCategoryByIdAsync(category.Id);
            Assert.Equal(expected, categories);
        }

        [Fact]
        public async Task GetCategoryIdByNameAsync_WithValidData_ShouldReturnId()
        {
            string expected;
            var guid = Guid.NewGuid().ToString();

            var moqHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var moqCategoriesService = new Mock<ICategoryService>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();

            var context = InitializeContext.CreateContextForInMemory();
            this.userService = new UserService(context, moqHttpContextAccessor.Object);
            this.categoryService = new CategoryService(context, this.userService);

            var user = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
            };

            var category = await this.categoryService.CreateCategoryAsync(new CreateCategoryModel()
            {
                Name = "Wow",
            });

            expected = category.Id;

            var createOfferInputModel = new CreateOfferModel()
            {
                Name = "Wow Account",
                CategotyName = category.Id,
                CreatorId = guid,
                Description = "Some Test Description",
                Price = 10.00,
                PicUrl = "link",
            };

            this.offerService = new OfferService(context, moqCategoriesService.Object, moqCloudinaryService.Object);

            var offer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            await this.offerService.ApproveOfferAsync(offer.Id);

            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Assert
            var categories = await this.categoryService.GetCategoryIdByNameAsync(category.Name);
            Assert.Equal(expected, categories);
        }

        [Fact]
        public async Task GetCategoryIdByNameAsync_WithInValidData_ShouldReturnNull()
        {
            string expected;
            var guid = Guid.NewGuid().ToString();

            var moqHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var moqCategoriesService = new Mock<ICategoryService>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();

            var context = InitializeContext.CreateContextForInMemory();
            this.userService = new UserService(context, moqHttpContextAccessor.Object);
            this.categoryService = new CategoryService(context, this.userService);

            var user = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
            };

            var category = await this.categoryService.CreateCategoryAsync(new CreateCategoryModel()
            {
                Name = "Wow",
            });

            expected = category.Id;

            var createOfferInputModel = new CreateOfferModel()
            {
                Name = "Wow Account",
                CategotyName = category.Id,
                CreatorId = guid,
                Description = "Some Test Description",
                Price = 10.00,
                PicUrl = "link",
            };

            this.offerService = new OfferService(context, moqCategoriesService.Object, moqCloudinaryService.Object);

            var offer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            await this.offerService.ApproveOfferAsync(offer.Id);

            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Assert
            var categories = await this.categoryService.GetCategoryIdByNameAsync("InvalidNAME");
            Assert.Equal(expected, categories);
        }
    }
}
