using ExceptionsDemo.Repository;
using ExceptionsDemo.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ExceptionsDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(ProductRepository repo) : Controller
    {
        [HttpGet("{id}")]
        public async Task<IResult> GetProduct(int id)
        {
            //exceptions
            return Results.Ok(await repo.GetProductOrThrow(id));

            //result type
            //var result = await repo.GetProductOrFail(id);
            //return result.Match(okValue => Results.Ok(okValue), errorValue => Results.Problem(title: "NotFound", detail: errorValue, statusCode: StatusCodes.Status404NotFound));
        }

        [HttpPost]
        public async Task<IResult> CreateProduct(CreateProductDto dto)
        {
            //exceptions
            return Results.Ok(await repo.CreateProductOrThrow(dto));

            //result type
            //var result = await repo.CreateProductOrFail(dto);
            //return result.Match(okValue => Results.Ok(okValue), errorValue => Results.Problem(title: "BadRequest", detail: errorValue, statusCode: StatusCodes.Status400BadRequest));
        }
    }
}