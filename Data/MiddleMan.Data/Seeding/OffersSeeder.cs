using Microsoft.EntityFrameworkCore;
using MiddleMan.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MiddleMan.Data.Seeding
{
    public class OffersSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (await dbContext.Categories.AnyAsync())
            {
                return;
            }

            // categories
            await dbContext.Categories.AddAsync(new Category() { Name = "World Of Warcraft", Position = 1, });
            await dbContext.Categories.AddAsync(new Category() { Name = "League Of Legends", Position = 2, });
            await dbContext.Categories.AddAsync(new Category() { Name = "CS:GO", Position = 3, });
            await dbContext.Categories.AddAsync(new Category() { Name = "Valorant", Position = 4, });
            await dbContext.SaveChangesAsync();

            // Admin User
            var adminUser = await dbContext.Users.FirstOrDefaultAsync(x => x.UserName == "admin");
            var adminId = adminUser.Id;

            // User User
            var userUser = await dbContext.Users.FirstOrDefaultAsync(x => x.UserName == "user");
            var userId = userUser.Id;

            // WOW
            var wow = await dbContext.Categories.FirstOrDefaultAsync(x => x.Name == "World Of Warcraft");
            var wowId = wow.Id;
            await dbContext.Offers.AddAsync(new Offer() { Name = "Wow BFA Code", CategoryId = wowId, Price = 40.00, IsApproved = true, IsDeclined = false, PicUrl = "https://i.ytimg.com/vi/8CJMg4LdhdA/maxresdefault.jpg", Description = "A new chapter of the epic Warcraft saga is coming. Discover what the future holds for World of Warcraft including brand-new features, gameplay, story, and more!", BuyContent = "UGKV34G5345TIF", CreatorId = adminId, });
            await dbContext.Offers.AddAsync(new Offer() { Name = "Wow Legion Code", CategoryId = wowId, Price = 20.00, IsApproved = true, IsDeclined = false, PicUrl = "https://www.cjs-cdkeys.com/product_images/v/wow_legion_1024__30444.jpg", Description = "World of Warcraft: Legion is the sixth expansion set in the massively multiplayer online ... There are four raid tiers in Legion, with the first tier being The Emerald", BuyContent = "U23433G5345TIF", CreatorId = adminId, });
            await dbContext.Offers.AddAsync(new Offer() { Name = "Wow BFA Code", CategoryId = wowId, Price = 80.00, IsApproved = true, IsDeclined = false, PicUrl = "https://wow.zamimg.com/uploads/screenshots/normal/721287.jpg", Description = "Complete Collection Heroic Edition. Get started with 30 days of game time, which includes access to World of Warcraft and WoW Classic. This edition also unlocks", BuyContent = "UGKV9678568TIF", CreatorId = adminId, });
            // promo
            await dbContext.Offers.AddAsync(new Offer() { Name = "Wow SL Code", CategoryId = wowId, Price = 35.00, IsApproved = true, IsDeclined = false, PicUrl = "https://cdn.neow.in/news/images/uploaded/2019/11/1572880709_wow-shadowlands-leveling-dexerto.jpg", Description = "Walk the realms beyond in the new WoW expansion, Shadowlands. Join a Covenant and siege an eternal prison to save lost souls—and all of reality.", BuyContent = "UGKV3456245TIF", CreatorId = adminId, IsFeatured = true, });

            // LOL
            var lol = await dbContext.Categories.FirstOrDefaultAsync(x => x.Name == "League Of Legends");
            var lolId = lol.Id;
            await dbContext.Offers.AddAsync(new Offer() { Name = "Lol Account(50lvl)", CategoryId = lolId, Price = 20.00, IsApproved = true, IsDeclined = false, PicUrl = "https://7sport.net/storage/league-of-legends-text4-e1586951649431.jpg", Description = "Whether you're playing Solo or Co-op with friends, League of Legends is a highly competitive, fast paced action-strategy game designed for those who crave a", BuyContent = "account/pass", CreatorId = adminId, });
            await dbContext.Offers.AddAsync(new Offer() { Name = "Lol 52 Account 50+ skins", CategoryId = lolId, Price = 30.00, IsApproved = true, IsDeclined = false, PicUrl = "https://i.pinimg.com/originals/07/03/2c/07032ce211bb172f75a179794f3a0f16.png", Description = "Riot Forge. We're a publisher working with talented and experienced third party developers to bring awesome new League of Legends games to players of all", BuyContent = "account/pass", CreatorId = adminId, });
            await dbContext.Offers.AddAsync(new Offer() { Name = "Lol 10$ RP", CategoryId = lolId, Price = 10.00, IsApproved = true, IsDeclined = false, PicUrl = "https://media.gamestop.com/i/gamestop/10116412/League-of-Legends-$10-Game-Card", Description = "Riot Forge. We're a publisher working with talented and experienced third party developers to bring awesome new League of Legends games to players of all", BuyContent = "UGKV9678568TIF", CreatorId = adminId, });
            // promo
            await dbContext.Offers.AddAsync(new Offer() { Name = "Lol TFT Account(Gold2)", CategoryId = lolId, Price = 35.00, IsApproved = true, IsDeclined = false, PicUrl = "https://images.contentstack.io/v3/assets/blt76b5e73bfd1451ea/blta3ee1b1a1d4697ed/5e5dd1c625d22d7db56a392a/TFT.S3_GALAXIES.ARTICLE-2.jpg", Description = "Riot Forge. We're a publisher working with talented and experienced third party developers to bring awesome new League of Legends games to players of all", BuyContent = "UGKV3456245TIF", CreatorId = adminId, IsFeatured = true, });

            // CS:GO
            var cs = await dbContext.Categories.FirstOrDefaultAsync(x => x.Name == "CS:GO");
            var csId = cs.Id;
            await dbContext.Offers.AddAsync(new Offer() { Name = "CS:GO Account(Gold Nova)", CategoryId = csId, Price = 20.00, IsApproved = true, IsDeclined = false, PicUrl = "https://steamuserimages-a.akamaihd.net/ugc/985611021588931309/89C992F547AAF1ECDDDA90B35BAD3562CA9984EF/", Description = "Whether you're playing Solo or Co-op with friends, League of Legends is a highly competitive, fast paced action-strategy game designed for those who crave a", BuyContent = "account/pass", CreatorId = userId, });
            await dbContext.Offers.AddAsync(new Offer() { Name = "Knife(Carambit)", CategoryId = csId, Price = 300.00, IsApproved = true, IsDeclined = false, PicUrl = "https://i.ytimg.com/vi/wqTT2Qba-EQ/maxresdefault.jpg", Description = "Browse all Karambit CS:GO skins. Check skin market prices, inspect links, rarity levels, case and collection info, plus StatTrak or souvenir drops.", BuyContent = "contact seller", CreatorId = userId, });
            await dbContext.Offers.AddAsync(new Offer() { Name = "M4A4 Desolate", CategoryId = csId, Price = 10.00, IsApproved = true, IsDeclined = false, PicUrl = "https://media.gamestop.com/i/gamestop/10116412/League-of-Legends-$10-Game-Card", Description = "The creator of the next m4a4 pink skin for csgo had provided me exclusive access to showcase it in game. Check out the workshop link", BuyContent = "contact seller", CreatorId = userId, });
            // promo
            await dbContext.Offers.AddAsync(new Offer() { Name = "M4A4 Custom", CategoryId = csId, Price = 305.00, IsApproved = true, IsDeclined = false, PicUrl = "https://steamuserimages-a.akamaihd.net/ugc/28489013034484767/8134F3260F4DBEBA12509602C272A9357278080C/", Description = "The creator of the next m4a4 pink skin for csgo had provided me exclusive access to showcase it in game. Check out the workshop link:", BuyContent = "UGKV3456245TIF", CreatorId = userId, IsFeatured = true, });

            // Valorant
            var val = await dbContext.Categories.FirstOrDefaultAsync(x => x.Name == "Valorant");
            var valId = val.Id;
            await dbContext.Offers.AddAsync(new Offer() { Name = "Valorant Account(40+lvl)", CategoryId = valId, Price = 90.00, IsApproved = true, IsDeclined = false, PicUrl = "https://images.gamewatcherstatic.com/image/file/0/17/106700/Valorant-2.jpg", Description = "Riot Games presents VALORANT: a 5v5 character-based tactical FPS where precise gunplay meets unique agent abilities. Learn about VALORANT and its", BuyContent = "account/pass", CreatorId = adminId, });
            await dbContext.Offers.AddAsync(new Offer() { Name = "Valorant Activate Code", CategoryId = valId, Price = 20.00, IsApproved = true, IsDeclined = false, PicUrl = "https://cdn.mos.cms.futurecdn.net/QCb8dh8TGFfLY2qwKt4vKN.jpg", Description = "Riot Forge. We're a publisher working with talented and experienced third party developers to bring awesome new League of Legends games to players of all", BuyContent = "account/pass", CreatorId = adminId, });
            await dbContext.Offers.AddAsync(new Offer() { Name = "Valorant Skin Pack", CategoryId = valId, Price = 10.00, IsApproved = true, IsDeclined = false, PicUrl = "https://9images.cgames.de/images/gamestar/4/valorant-valorant-points-shop_6097438.jpg", Description = "Riot Forge. We're a publisher working with talented and experienced third party developers to bring awesome new League of Legends games to players of all", BuyContent = "UGKV9678568TIF", CreatorId = adminId, });
            // promo
            await dbContext.Offers.AddAsync(new Offer() { Name = "Valorant Account(20+lvl)", CategoryId = valId, Price = 35.00, IsApproved = true, IsDeclined = false, PicUrl = "https://cdn.mos.cms.futurecdn.net/VGPuAasjJpttMo9d4vGLVT.jpg", Description = "Riot Forge. We're a publisher working with talented and experienced third party developers to bring awesome new League of Legends games to players of all", BuyContent = "account/pass", CreatorId = adminId, IsFeatured = true, });

            await dbContext.SaveChangesAsync();

            // Comments
            var offer = await dbContext.Offers.FirstOrDefaultAsync(x => x.Name == "Valorant Account(20+lvl)");
            var offerId = offer.Id;

            await dbContext.Comments.AddAsync(new Comment() { OfferId = offerId, CreatorId = userId, Description = "Test Comment" });
            await dbContext.OfferUserRates.AddAsync(new OfferUserRate() { OfferId = offerId, UserId = userId, Rate = 5, });

            // Favorites

            await dbContext.UserFavorites.AddAsync(new UserFavorite() { OfferId = offerId, UserId = adminId });
            await dbContext.UserFavorites.AddAsync(new UserFavorite() { OfferId = offerId, UserId = userId });

            // Messages

            await dbContext.Messages.AddAsync(new Message() { OfferId = offerId, SenderId = userId, RecipientId = adminId, Content = "Hi Message", });
            await dbContext.Messages.AddAsync(new Message() { OfferId = offerId, SenderId = adminId, RecipientId = userId, Content = "Hi Message Reply", });

            await dbContext.SaveChangesAsync();
        }
    }
}
