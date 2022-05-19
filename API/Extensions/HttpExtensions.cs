using Microsoft.AspNetCore.Http;
using API.Helpers;
using System.Text.Json;
namespace API.Extensions
{
    public static class HttpExtensions
    {
        public static void AddPaginationHeader(this HttpResponse response, int currenctPage, int itemPerpage, int totalItems, int totalPages){

            var paginationHeader = new PaginationHeader(currenctPage, totalItems, itemPerpage, totalPages);

            var options = new JsonSerializerOptions{
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationHeader, options));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");


        }
    }
}