using Ambev.DeveloperEvaluation.Application.Products.Queries.GetProductsList;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Products.Queries.GetProductsByCategory
{
    public class GetProductsByCategoryProfile : Profile
    {
        public GetProductsByCategoryProfile()
        {
            CreateMap<Product, ProductListItem>();
        }
    }
}
