using AutoMapper;
using Core.Domain.Interfaces.Mappings;

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
}
