using AutoMapper;
using Core.Domain.Models;

namespace Host.CamAI.API;

public static class MapExtension
{
    public static PaginationResult<TDes> MapPaginationResult<TDes, TSource>(this IMapper mapper, PaginationResult<TSource> source)
    {
        return new PaginationResult<TDes>
        {
            PageIndex = source.PageIndex,
            PageSize = source.PageSize,
            TotalCount = source.PageSize,
            Values = mapper.Map<IList<TSource>, IList<TDes>>(source.Values)
        };
    }
}
