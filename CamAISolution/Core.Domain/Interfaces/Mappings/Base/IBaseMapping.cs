using Core.Domain.Models;

namespace Core.Domain.Interfaces.Mappings;

public interface IBaseMapping
{
    /// <summary>
    /// Map object with 2 generic types TSource, TDes
    /// </summary>
    /// <typeparam name="TSource">TSrouce: input type</typeparam>
    /// <typeparam name="TDes">TDes: destination type</typeparam>
    /// <param name="source">TSrouce input object</param>
    /// <returns></returns>
    TDes Map<TSource, TDes>(TSource source);

    TDes Map<TSource, TDes>(TSource source, TDes des);

    PaginationResult<TDes> Map<TSource, TDes>(PaginationResult<TSource> source);
}
