using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProAgil.Repository;

namespace ProAgil.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValuesController : ControllerBase
    {
        private readonly ProAgilContext _dataContext;

        public ValuesController(ProAgilContext dataContext)
        {
            _dataContext = dataContext;

        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var results = await _dataContext.Eventos.ToListAsync();
                return Ok(results);     
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro no banco de dados");
            }
           
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
                  try
            {
                var result =  await _dataContext.Eventos.FirstOrDefaultAsync(x => x.Id == id);
                return Ok(result);     
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro no banco de dados");
            }
        }

    }
}
