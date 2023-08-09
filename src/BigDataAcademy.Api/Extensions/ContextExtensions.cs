using BigDataAcademy.Api.Utilities;
using BigDataAcademy.Model;
using Microsoft.EntityFrameworkCore;

namespace BigDataAcademy.Api.Extensions;

public static class ContextExtensions
{
    public static async Task<PagedResult<T>> FindPaged<T>(
        this IQueryable<T> set,
        PagedCriteria pagedCriteria,
        CancellationToken cancellationToken)
        where T : EntityBase
    {
        var count = await set.LongCountAsync(cancellationToken);
        var maxPage = (int)Math.Ceiling((decimal)count / pagedCriteria.PageSize);
        var page = pagedCriteria.Page <= maxPage ? pagedCriteria.Page : maxPage;

        return new PagedResult<T>
        {
            PagedCriteria = new PagedCriteria
            {
                Page = page,
                PageSize = pagedCriteria.PageSize,
            },
            TotalPages = maxPage,
            Total = await set.LongCountAsync(cancellationToken),
            Items = await set
                .OrderBy(o => o.IngestionTime)
                .Skip((page - 1) * pagedCriteria.PageSize)
                .Take(pagedCriteria.PageSize)
                .ToArrayAsync(cancellationToken),
        };
    }
}
