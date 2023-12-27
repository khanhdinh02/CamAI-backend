using AutoMapper;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Models;

namespace Infrastructure.Mapping.Base;

public class BaseMapping(IMapper mapper) : IBaseMapping
{
    public TDes Map<TSource, TDes>(TSource source)
    {
        return mapper.Map<TSource, TDes>(source);
    }

    public TDes Map<TSource, TDes>(TSource source, TDes des)
    {
        return mapper.Map(source, des);
    }

    public PaginationResult<TDes> Map<TSource, TDes>(PaginationResult<TSource> source)
    {
        return mapper.Map<PaginationResult<TDes>>(source);
    }
}
