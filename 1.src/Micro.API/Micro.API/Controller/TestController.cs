using Micro.API.DBContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Micro.API.Controller
{
    /// <summary>
    /// 测试
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    
    //[Authorize] 
    public class TestController : ControllerBase
    {
        private readonly MicroDBContext _context;
        public TestController(MicroDBContext context)
        {
            _context = context;
        }
        /// <summary>
        /// GET: api 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            var users = _context.Users.ToList();
            return users.Select(x => x.Name);
        }

        // GET api/<TestController>/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<TestController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<TestController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TestController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
