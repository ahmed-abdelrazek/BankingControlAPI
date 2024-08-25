using Microsoft.EntityFrameworkCore;

namespace BankingControlAPI.Domain.Responses
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; init; }
        public int TotalPages { get; init; }
        public int PageSize { get; init; }
        public int TotalCount { get; init; }

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            AddRange(items);
        }

        public static async Task<PagedList<T>> ToPagedListAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
