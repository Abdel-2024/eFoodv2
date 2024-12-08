using AutoMapper;
using eFood.Application.DTOs;
using eFood.Domain.Entities;
using System.Collections.Generic;


namespace eFood.Application.Mappings
{
    public class AutoMapperCreator : Profile
    {
        public AutoMapperCreator()
        {
            CreateMap<CategoryProduct, CategoryProductCreateDTO>().ReverseMap();
            CreateMap<CategoryProduct, CategoryProductDTO>();

            CreateMap<Category, CategoryCreateDTO>().ReverseMap();
            CreateMap<Category, CategoryDTO>();
            CreateMap<Category, CategorySummaryDTO>();

            CreateMap<TypeOption, TypeOptionCreateDTO>().ReverseMap();
            CreateMap<TypeOption, TypeOptionDTO>();
            CreateMap<TypeOption, TypeOptionSummaryDTO>();

            CreateMap<Options, OptionsCreateDTO>().ReverseMap();
            CreateMap<Options, OptionsDTO>();

            CreateMap<Products, ProductsCreateDTO>().ReverseMap();
            CreateMap<Products, ProductsDTO>()
                            .ForMember(x => x.Options, o => o.MapFrom(ToOptions));

            CreateMap<AppsResult<Category>, AppsResult<CategoryDTO>>();
            CreateMap<AppsListResult<Category>, AppsListResult<CategoryDTO>>();

            CreateMap<AppsResult<TypeOption>, AppsResult<TypeOptionDTO>>();
            CreateMap<AppsListResult<TypeOption>, AppsListResult<TypeOptionDTO>>();

            CreateMap<AppsResult<Options>, AppsResult<OptionsDTO>>();
            CreateMap<AppsListResult<Options>, AppsListResult<OptionsDTO>>();

            CreateMap<AppsResult<Products>, AppsResult<ProductsDTO>>();
            CreateMap<AppsListResult<Products>, AppsListResult<ProductsDTO>>();

            CreateMap<AppsListResult<CategoryProduct>, AppsListResult<CategoryProductDTO>>();
            
        }

        private List<Options> ToOptions(Products products, ProductsDTO productsDTO)
        {
            var listToReturn = new List<Options>();

            if (object.Equals(products.ProductsOptions,null))
            {
                return listToReturn;
            }

            foreach (var item in products.ProductsOptions)
            {
                listToReturn.Add(item.Options);
            }

            return listToReturn;
        }
    }
}
