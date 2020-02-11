using MiddleMan.Web.ViewModels.InputModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MiddleMan.Services.Interfaces
{
    public interface IOfferService
    {
        Task CreateOfferAsync(CreateOfferModel inputModel);  // TODO model
    }
}
