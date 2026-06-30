using ClinicManagement.API.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.API.Application.Extensions
{
    public static class QueryableExtensions
    {
        public static async Task<PagedResult<T>> ToPagedResultAsync<T>(this IQueryable<T> query , int pageNumber , int pageSize)
        {
            var totalcount = await query.CountAsync();
            var data = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedResult<T>
            {
                Data = data,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalcount,
                TotalPages = (int)Math.Ceiling(totalcount / (double)pageSize)
            };
        }
    }
}
