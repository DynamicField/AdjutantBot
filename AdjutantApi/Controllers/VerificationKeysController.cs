using System.Linq;
using AdjutantApi.Data;
using AdjutantApi.Data.Models;
using AdjutantApi.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AdjutantApi.Controllers
{
    [ApiController, Route("[controller]")]
    public class VerificationKeysController : Controller
    {
        private readonly AdjutantContext _context;
        
        public VerificationKeysController(AdjutantContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            return Ok(_context.VerificationKeys.Select(x => x));
        }

        [HttpGet("{id}")]
        public IActionResult GetVerificationKey(int id)
        {
            var result = _context.VerificationKeys.FirstOrDefault(x => x.Id == id);
            return result is null? (IActionResult)NotFound(id) : Ok(result);
        }
        
        [HttpPost("Create")]
        public IActionResult CreateNewVerificationKey([FromBody] VerificationKey key)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = _context.VerificationKeys.Add(new VerificationKey
            {
                KeyValue = key.KeyValue
            });

            _context.SaveChanges();
            
            //return Created("[controller]/GetVerificationKey", result.Entity);
            return CreatedAtAction(nameof(GetVerificationKey), new {Id = result.Entity.Id}, result.Entity);
        }

        [HttpDelete("PurgeAll")]
        public IActionResult PurgeAllKeys()
        {
            foreach (var key in _context.VerificationKeys)
            {
                _context.VerificationKeys.Remove(key);
            }

            _context.SaveChanges();

            return Ok();
        }

        [HttpPatch("SetDisplayName")]
        public IActionResult SetDisplayNameForKey([FromBody] DisplayNameUpdate nameUpdate)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var key = _context.VerificationKeys.FirstOrDefault(x => x.KeyValue == nameUpdate.KeyValue);

            if (key is null)
            {
                return NotFound();
            }

            key.DisplayName = nameUpdate.NewDisplayName;

            _context.Update(key);

            _context.SaveChanges();

            return Ok(key);
        }

        [HttpPatch("UpdateKeyConsumptionState")]
        public IActionResult UpdateKeyConsumptionState([FromBody] KeyConsumptionUpdate consumptionStateUpdate)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            var key = _context.VerificationKeys.FirstOrDefault(x => x.KeyValue == consumptionStateUpdate.KeyValue);

            if (key is null)
            {
                return NotFound();
            }

            key.ConsumptionState = consumptionStateUpdate.NewState;

            _context.Update(key);

            _context.SaveChanges();

            return Ok(key);
        }
    }
}