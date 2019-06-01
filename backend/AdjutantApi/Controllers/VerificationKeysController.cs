using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AdjutantApi.Data;
using AdjutantApi.Data.Models;
using AdjutantApi.ViewModels;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Morcatko.AspNetCore.JsonMergePatch;

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
        public async Task<ActionResult<List<VerificationKey>>> Index() // Async as it can maybe take a long time
        {
            return await _context.VerificationKeys.ToListAsync();
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            var result = _context.VerificationKeys.Find(id);
            return result is null ? (IActionResult)NotFound(id) : Ok(result);
        }

        [HttpPost]
        [HttpPost("Create")] // Old
        public IActionResult Create([FromBody] VerificationKey key)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = _context.VerificationKeys.Add(key);

            _context.SaveChanges();

            //return Created("[controller]/Get", result.Entity);
            return CreatedAtAction(nameof(Get), new { result.Entity.Id }, result.Entity);
        }

        [HttpPut("{id:int}")]
        public IActionResult Put(int id, [FromBody] VerificationKey key)
        {
            key.Id = id;
            _context.Entry(key).State = EntityState.Modified;
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("PurgeAll")]
        public IActionResult PurgeAllKeys()
        {
            _context.VerificationKeys.RemoveRange(_context.VerificationKeys);

            _context.SaveChanges();

            return Ok();
        }

        public class VerificationKeyPatch
        {
            public string DisplayName { get; set; }
            public KeyConsumptionState ConsumptionState { get; set; }
        }

        // Json merge patch
        [HttpPatch("{id:int}")]
        public ActionResult<VerificationKey> Patch(int id, [FromBody] JsonMergePatchDocument<VerificationKeyPatch> patch)
        {
            if (patch is null) return BadRequest(ModelState);

            var key = _context.VerificationKeys.Find(id);
            if (key is null) return NotFound();

            patch.ApplyTo(key);
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _context.SaveChanges();

            return key;
        }

        [HttpPatch("SetDisplayName")]
        public IActionResult SetDisplayName([FromBody] DisplayNameUpdate nameUpdate)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var key = _context.VerificationKeys.FirstOrDefault(x => x.KeyValue == nameUpdate.KeyValue);

            if (key is null)
            {
                return NotFound();
            }

            key.DisplayName = nameUpdate.NewDisplayName;

            // _context.Update(key);

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

            // _context.Update(key);

            _context.SaveChanges();

            return Ok(key);
        }
    }
}