namespace MiddleMan.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using MiddleMan.Data.Models;
    using MiddleMan.Web.ViewModels.Administration.Dashboard.InputModels;

    public interface ICommentService
    {
        Task AddReviewToOffer(CreateReviewModel inputModel);

        Task<List<Comment>> GetOfferComments(string id);
    }
}
