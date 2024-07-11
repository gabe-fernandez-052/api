using Cache.Database;
using Cache.Entities;
using Cache.Extensions;
using Cache.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace Cache.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(ApplicationDbContext dbContext, IDistributedCache cache) : Controller
    {
        [HttpGet("{id}")]
        public async Task<IResult> GetProduct(int id)
        {
            var key = $"products-{id}";
            var product = await cache.GetOrCreateAsync(key, async token =>
            {
                var product = await dbContext.Products.FindAsync(id);
                return product;
            });

            if (product is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(product);
        }

        [HttpPost]
        public async Task<IResult> CreateProduct(CreateProductDto dto)
        {
            var product = new Product()
            {
                Id = dto.Id,
                Name = dto.Name,
                Price = dto.Price
            };

            dbContext.Products.Add(product);

            await dbContext.SaveChangesAsync();

            return Results.Ok(product.Id);
        }
    }
}