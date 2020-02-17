using Microsoft.EntityFrameworkCore;
using MiddleMan.Data;
using MiddleMan.Data.Models;
using MiddleMan.Services.Interfaces;
using MiddleMan.Web.ViewModels.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MiddleMan.Services
{
    public class OfferService : IOfferService
    {

        private readonly ApplicationDbContext context;

        public OfferService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task CreateOfferAsync(CreateOfferModel inputModel)
        {
            var offer = new Offer()
            {
                Name = inputModel.Name,
                Description = inputModel.Description,
                Price = inputModel.Price,
                PicUrl = inputModel.PicUrl,
                CategoryId = inputModel.CategotyName,
                CreatorId = inputModel.CreatorId,
            };

            await this.context.Offers.AddAsync(offer);
            await this.context.SaveChangesAsync();
        }

        public async Task<List<Offer>> GetAllNotAprovedOffersAsync()
        {
            var offers = await this.context.Offers.Where(x => x.IsApproved == false && x.IsDeleted == false).ToListAsync();
            return offers;
        }

        public async Task<List<Offer>> GetAllAprovedOffersAsync()
        {
            var offers = await this.context.Offers.Where(x => x.IsApproved == true && x.IsDeleted == false).ToListAsync();
            return offers;
        }

        public async Task<Offer> GetOfferByIdAsync(string id)
        {
            var offer = await this.context.Offers.FirstOrDefaultAsync(x => x.Id == id);
            return offer;
        }

        public async Task ApproveOfferAsync(string id)
        {
            var offer = await this.context.Offers.FirstOrDefaultAsync(x => x.Id == id);
            offer.IsApproved = true;
            await this.context.SaveChangesAsync();
        }

        public async Task RemoveOffer(string id)
        {
            var offer = await this.context.Offers.FirstOrDefaultAsync(x => x.Id == id);
            offer.IsDeleted = true;
            await this.context.SaveChangesAsync();
        }
    }
}
