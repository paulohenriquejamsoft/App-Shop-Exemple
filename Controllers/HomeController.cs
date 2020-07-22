using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shop.Data;
using Shop.Models;

namespace Shop.Controllers
{   
    [Route("v1/categories")]
    public class HomeController:ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<dynamic>> GetTask([FromServices] DataContext context)
        {
            var employee = new User {Id=1, Username="scariodes", Password="1234", Role="employee"};
            var manager = new User {Id=2, Username="manager", Password="1234", Role="manager"};
            var category = new Category {Id = 1, Title="Informática"};
            var product = new Product {Id=1, Category=category, Title="Mouse", Price=299, Description="Mouse gamer muito bom."};

            context.Users.Add(employee);
            context.Users.Add(manager);
            context.Categories.Add(category);
            context.Products.Add(product);
            await context.SaveChangesAsync();

            return Ok(new {
                message = "Dados Configrados"
            });
        }
    }
}