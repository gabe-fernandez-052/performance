using ExceptionsDemo.Database;
using ExceptionsDemo.Entities;
using ExceptionsDemo.Exceptions;
using ExceptionsDemo.Requests;

namespace ExceptionsDemo.Repository
{
    public class ProductRepository(ApplicationDbContext dbContext)
    {
        public async Task<Product> GetProductOrThrow(int id)
        {
            var product = await dbContext.Products.FindAsync(id);

            if (product is null)
            {
                throw new ProductNotFoundException("Product does not exist");
            }

            return product;
        }

        public async Task<Result<Product, string>> GetProductOrFail(int id)
        {
            var product = await dbContext.Products.FindAsync(id);

            if (product is null)
            {
                return new Error<Product, string>("Product does not exist");
            }

            return new Ok<Product, string>(product);
        }

        public async Task<int> CreateProductOrThrow(CreateProductDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                throw new ProductNotValidException("Product is not valid");
            }

            var product = new Product()
            {
                Id = dto.Id,
                Name = dto.Name,
                Price = dto.Price
            };

            dbContext.Products.Add(product);

            await dbContext.SaveChangesAsync();

            return product.Id;
        }

        public async Task<Result<int, string>> CreateProductOrFail(CreateProductDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return new Error<int, string>("Product is not valid");
            }

            var product = new Product()
            {
                Id = dto.Id,
                Name = dto.Name,
                Price = dto.Price
            };

            dbContext.Products.Add(product);

            await dbContext.SaveChangesAsync();

            return new Ok<int, string>(product.Id);
        }
    }
}