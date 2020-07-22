using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;
using Shop.Services;

namespace Shop.Controllers
{
    [Route("v1/users")]
    public class UserController:ControllerBase
    {
       [HttpGet]
       [Authorize(Roles = "manager")]
       public async Task<ActionResult<List<User>>> GetAction(
           [FromServices] DataContext context)
        {
           var users = await context.Users.AsNoTracking().ToListAsync();
           return Ok(users);
        }

       [HttpPost]
       public async Task<ActionResult<User>> Post(
           [FromServices] DataContext context,
           [FromBody] User model
       ){
           if(!ModelState.IsValid){
               return BadRequest(ModelState);
           }

           try{

               model.Role = "employee";
               context.Users.Add(model);
               await context.SaveChangesAsync();

               model.Password = "";
               return Ok(model);
           }catch(Exception){
               return BadRequest(new { message = "Não foi possível criar o usuário"});
           }
       }

        [AllowAnonymous]    
        [HttpPost("login")]
       public async Task<ActionResult<dynamic>> Authenticate(
           [FromServices] DataContext context,
           [FromBody] User model
         ){
           if(!ModelState.IsValid){
               return BadRequest(ModelState);
           }

            try{

                var user = await context.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Username == model.Username && x.Password == model.Password);

                if(user == null)
                    return NotFound(new { message="Usuário ou senha inválidos."});

                var token = TokenService.GenerateToken(user);

                user.Password="";
                return Ok(new { user,token });     

            }catch(Exception){
                return BadRequest(new { message = "Não foi possível criar o usuário."});
            }
        } 

        [HttpGet("anonimo")]
        public string Anonimo () => "Anonimo";

        [HttpGet("autenticado")]
        [Authorize]
        public string Autenticado () => "Autenticado";

        [HttpGet("funcionario")]
        [Authorize(Roles="employee")]
        public string Funcionario () => "Funcionario";

        [HttpGet("gerente")]
        [Authorize(Roles="manager")]
        public string Gerente () => "Gerente";


    }
}