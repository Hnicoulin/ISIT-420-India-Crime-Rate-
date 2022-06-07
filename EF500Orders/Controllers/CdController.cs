using EF500Orders.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EF500Orders.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CdController : ControllerBase
    {
        // GET: api/<CdController>
        [HttpGet]
        public IEnumerable<CdTable> Get()
        {
            System.Diagnostics.Debug.WriteLine($"XXXin the CD ControllerXXX");
            var context = new Models.OrdersDBContext();
            return context.CdTables.ToList();
        }


        //// GET api/<CdController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<CdController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<CdController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<CdController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
