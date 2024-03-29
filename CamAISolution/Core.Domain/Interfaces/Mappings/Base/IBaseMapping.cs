using Core.Domain.Models;

namespace Core.Domain.Interfaces.Mappings;

public interface IBaseMapping
{
    TDes Map<TSource, TDes>(TSource source);

    TDes Map<TSource, TDes>(TSource source, TDes des);

    PaginationResult<TDes> Map<TSource, TDes>(PaginationResult<TSource> source);
}