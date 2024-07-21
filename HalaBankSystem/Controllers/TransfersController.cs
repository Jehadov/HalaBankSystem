using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HalaBankSystem.Models;

namespace HalaBankSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransfersController : ControllerBase
    {
        private readonly BankSystemContext _context;

        public TransfersController(BankSystemContext context)
        {
            _context = context;
        }

        // GET: api/Transfers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transfer>>> GetTransfers()
        {
          if (_context.Transfers == null)
          {
              return NotFound();
          }
            return await _context.Transfers.ToListAsync();
        }

        // GET: api/Transfers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Transfer>> GetTransfer(int id)
        {
          if (_context.Transfers == null)
          {
              return NotFound();
          }
            var transfer = await _context.Transfers.FindAsync(id);

            if (transfer == null)
            {
                return NotFound();
            }

            return transfer;
        }

        // PUT: api/Transfers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransfer(int id, Transfer transfer)
        {
            if (id != transfer.TransferId)
            {
                return BadRequest();
            }

            _context.Entry(transfer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransferExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Transfers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Transfer>> PostTransfer(Transfer transfer)
        {
          if (_context.Transfers == null)
          {
              return Problem("Entity set 'BankSystemContext.Transfers'  is null.");
          }
            _context.Transfers.Add(transfer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTransfer", new { id = transfer.TransferId }, transfer);
        }

        // DELETE: api/Transfers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransfer(int id)
        {
            if (_context.Transfers == null)
            {
                return NotFound();
            }
            var transfer = await _context.Transfers.FindAsync(id);
            if (transfer == null)
            {
                return NotFound();
            }

            _context.Transfers.Remove(transfer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        [Route("transfering")]
        public async Task<ActionResult<BankAccount>> transfering(string Email)
        {
            // Find the bank account with the matching email and password
            var matchedAccount = await _context.BankAccounts
                .FirstOrDefaultAsync(a => a.Email == Email);

            if (matchedAccount == null)
            {
                // Return 401 Unauthorized if the account is not found or the credentials are incorrect
                return Unauthorized("Invalid email or password.");
            }

            //matchedAccount.Token = CreateJWT(matchedAccount);
            //// If account is found, return the account details
            //return Ok(new
            //{ 
            //    Token = matchedAccount.Token, 
            //    Message = "Login Successfully"
            //});

            return Ok(matchedAccount);
        }

        private bool TransferExists(int id)
        {
            return (_context.Transfers?.Any(e => e.TransferId == id)).GetValueOrDefault();
        }
    }
}
