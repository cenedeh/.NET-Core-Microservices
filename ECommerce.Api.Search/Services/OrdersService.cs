using ECommerce.Api.Search.Interfaces;
using ECommerce.Api.Search.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Api.Search.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<OrdersService> _logger;

        public OrdersService(IHttpClientFactory httpClientFactory,
            ILogger<OrdersService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }
        public async Task<(bool IsSuccess, IEnumerable<Order> Orders, string ErrorMessage)> GetOrdersAsync(int customerId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("OrderService");
                var response = await client.GetAsync($"api/orders/{customerId}");
                if (!response.IsSuccessStatusCode) return (false, null, response.ReasonPhrase);
                var content = await response.Content.ReadAsByteArrayAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var result = JsonSerializer.Deserialize<IEnumerable<Order>>(content, options);
                return (true, result, null);

            }
            catch (Exception exception)
            {
                    
                _logger?.LogError(exception.ToString());
                return (false, null, exception.Message);
            }
        }
    }
}