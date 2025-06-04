using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct
{
    public class CreateProductProfile : Profile
    {
        public CreateProductProfile()
        {
            CreateMap<CreateProductCommand, Product>()
                .ConstructUsing(cmd => new Product(
                    Guid.NewGuid(),
                    cmd.Title,
                    cmd.Price,
                    cmd.Description,
                    cmd.Category,
                    cmd.Image,
                    new Rating(cmd.RatingRate, cmd.RatingCount)
                ));

            CreateMap<Product, CreateProductResult>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
        }
    }
}
