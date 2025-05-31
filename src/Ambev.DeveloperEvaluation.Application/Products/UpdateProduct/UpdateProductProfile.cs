using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct
{
    public class UpdateProductProfile : Profile
    {
        public UpdateProductProfile()
        {
            CreateMap<UpdateProductCommand, Product>()
                .ConstructUsing(cmd => new Product(
                    cmd.Id,
                    cmd.Title,
                    cmd.Price,
                    cmd.Description,
                    cmd.Category,
                    cmd.Image,
                    new Rating(cmd.RatingRate, cmd.RatingCount)
                ));

            CreateMap<Product, UpdateProductResult>()
                .ForMember(dest => dest.Success, opt => opt.MapFrom(src => true));
        }
    }
}
