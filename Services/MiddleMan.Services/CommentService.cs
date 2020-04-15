using Microsoft.EntityFrameworkCore;
using MiddleMan.Data;
using MiddleMan.Data.Models;
using MiddleMan.Services.Interfaces;
using MiddleMan.Web.ViewModels.Administration.Dashboard.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiddleMan.Services
{
    public class CommentService : ICommentService
    {
        private readonly ApplicationDbContext context;
        private readonly IOfferService offerService;

        public CommentService(ApplicationDbContext context, IOfferService offerService)
        {
            this.context = context;
            this.offerService = offerService;
        }

        public async Task AddReviewToOffer(CreateReviewModel inputModel)
        {
            var offer = await this.context.Offers.FirstOrDefaultAsync(x => x.Id == inputModel.Id);
            if (offer == null)
            {
                return;
            }

            var userComments = this.GetOfferCommentsAsync(inputModel.Id).Result.Where(x => x.CreatorId == inputModel.CreatorId).ToList();

            var comment = new Comment()
            {
                Description = inputModel.Review,
                OfferId = inputModel.Id,
                CreatorId = inputModel.CreatorId,
            };

            var rated = await this.offerService.IsOfferRatedAsync(offer.Id, inputModel.CreatorId);
            int? offerRatedByUser = 0;

            offerRatedByUser = await this.offerService.GetRateForOffer(offer.Id, inputModel.CreatorId);

            if (userComments.Any())
            {
                var offerUserRate = await this.context.OfferUserRates
                    .FirstOrDefaultAsync(x => x.UserId == inputModel.CreatorId && x.OfferId == inputModel.Id);
                offerUserRate.Rate = int.Parse(inputModel.Rating);
            }

            OfferUserRate offerRate = null;

            if (offerRatedByUser == 0)
            {
                offerRate = new OfferUserRate()
                {
                    OfferId = offer.Id,
                    UserId = inputModel.CreatorId,
                    Rate = int.Parse(inputModel.Rating),
                };

                if (offerRatedByUser == 0)
                {
                    this.context.OfferUserRates.Add(offerRate);
                }
            }

            if (comment.Description != null)
            {
                offer.Comments.Add(comment);
            }

            await this.context.SaveChangesAsync();
        }

        public async Task<List<Comment>> GetOfferCommentsAsync(string id)
        {
            return await this.context.Comments
                .Where(x => x.OfferId == id)
                .OrderByDescending(x => x.CreatedOn)
                .ToListAsync();
        }
    }
}
