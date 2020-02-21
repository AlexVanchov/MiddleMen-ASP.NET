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

        public CommentService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task AddReviewToOffer(CreateReviewModel inputModel)
        {
            var offer = await this.context.Offers.FirstOrDefaultAsync(x => x.Id == inputModel.Id);
            if (offer == null)
                throw new ArgumentNullException("Invalid Data");
            else if (int.Parse(inputModel.Rating) < 1 || int.Parse(inputModel.Rating) > 5)
                throw new ArgumentNullException("Invalid Data");
            else if (inputModel.Review == null) 
                throw new ArgumentNullException("Invalid Data");

            var comment = new Comment()
            {
                Description = inputModel.Review,
                OfferId = inputModel.Id,
                RatingGiven = int.Parse(inputModel.Rating),
                CreatorId = inputModel.CreatorId,
            };

            offer.Comments.Add(comment);
            await this.context.SaveChangesAsync();
        }

        public async Task<List<Comment>> GetOfferComments(string id)
        {
            return await this.context.Comments.Where(x => x.OfferId == id).ToListAsync();
        }
    }
}
