

using App.Entities.Entities;
using App.Entities.Helper;
using App.Managers.EntityDtos;
using AutoMapper;

namespace App.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Customer mappings
            CreateMap<Customer, CustomerDto>();

            CreateMap<CreateCustomerDto, Customer>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsDelete, opt => opt.Ignore());

            CreateMap<UpdateCustomerDto, Customer>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsDelete, opt => opt.Ignore());

            // PagedList mapping
            CreateMap(typeof(PagedList<>), typeof(PagedResultDto<>))
                .ConvertUsing(typeof(PagedListConverter<,>));
        }
    }

    // Custom converter for PagedList to PagedResultDto
    public class PagedListConverter<TSource, TDestination> : ITypeConverter<PagedList<TSource>, PagedResultDto<TDestination>>
    {
        public PagedResultDto<TDestination> Convert(PagedList<TSource> source, PagedResultDto<TDestination> destination, ResolutionContext context)
        {
            var items = context.Mapper.Map<List<TDestination>>(source.Items);

            return new PagedResultDto<TDestination>
            {
                Items = items,
                CurrentPage = source.CurrentPage,
                TotalPages = source.TotalPages,
                PageSize = source.PageSize,
                TotalCount = source.TotalCount,
                HasPrevious = source.HasPrevious,
                HasNext = source.HasNext
            };
        }
    }
}
