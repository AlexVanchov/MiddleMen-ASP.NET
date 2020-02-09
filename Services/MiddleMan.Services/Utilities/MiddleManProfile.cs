namespace SellMe.Services.Utilities
{
    using AutoMapper;
    using global::MiddleMan.Data.Models;
    using global::MiddleMan.Web.ViewModels.ViewModels;

    public class MiddleManProfile : Profile
    {
        public MiddleManProfile()
        {
            // Create Map
            this.CreateMap<Category, CategoryViewModel>()
                .ForMember(x => x.Name, cfg => cfg.MapFrom(x => x.Name));
        }
    }
}
