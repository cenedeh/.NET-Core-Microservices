using ECommerce.Api.Products.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.Api.Products.Interfaces
{
    public interface IProductsProvider
    {
        Task<(bool IsSuccess, IEnumerable<Product>, string ErrorMessage)> GetProductsAsync();
        Task<(bool IsSuccess, Product, string ErrorMessage)> GetProductAsync(int id);
    }
}
