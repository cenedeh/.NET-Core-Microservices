using ECommerce.Api.Search.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Search.Services
{
    public class SearchService : ISearchService
    {
        private readonly IOrdersService _ordersService;
        private readonly IProductsService _productsService;
        private readonly ICustomersService _customersService;

        public SearchService(IOrdersService ordersService, IProductsService productsService,
            ICustomersService customersService)
        {
            _ordersService = ordersService;
            _productsService = productsService;
            _customersService = customersService;
        }
        public async Task<(bool IsSuccess, dynamic SearchResults)> SearchAsync(int customerId)
        {
            var customersResult = await _customersService.GetCustomerAsync(customerId);
            var ordersResult = await _ordersService.GetOrdersAsync(customerId);
            var productsResult = await _productsService.GetProductsAsync();
            if (!ordersResult.IsSuccess) return (false, null);

            foreach (var order in ordersResult.Orders)
            {
                foreach (var item in order.Items)
                {
                    item.ProductName = productsResult.IsSuccess
                        ? productsResult.Products?.FirstOrDefault(p => p.Id == item.ProductId)?.Name
                        : "Product information is not available";
                }
            }
            var result = new
            {
                Customer = customersResult.IsSuccess ? 
                    customersResult.Customer :
                    new { Name = "Customer information is not available"},
                ordersResult.Orders
            };
            return (true, result);

        }
    }
}