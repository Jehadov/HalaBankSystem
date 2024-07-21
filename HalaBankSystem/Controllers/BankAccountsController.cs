using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HalaBankSystem.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol;

namespace HalaBankSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankAccountsController : ControllerBase
    {
        private readonly BankSystemContext _context;

        public BankAccountsController(BankSystemContext context)
        {
            _context = context;
        }

        // GET: api/BankAccounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BankAccount>>> GetBankAccounts()
        {
          if (_context.BankAccounts == null)
          {
              return NotFound();
          }
            return await _context.BankAccounts.ToListAsync();
        }


        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<BankAccount>> LoginAcc(string Email, string Password)
        {
            // Find the bank account with the matching email and password
            var matchedAccount = await _context.BankAccounts
                .FirstOrDefaultAsync(a => a.Email == Email && a.Passwords == Password);

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


        // GET: api/BankAccounts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BankAccount>> GetBankAccount(int id)
        {
          if (_context.BankAccounts == null)
          {
              return NotFound();
          }
            var bankAccount = await _context.BankAccounts.FindAsync(id);

            if (bankAccount == null)
            {
                return NotFound();
            }

            return bankAccount;
        }

        // PUT: api/BankAccounts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBankAccount(int id, BankAccount bankAccount)
        {
            if (id != bankAccount.AccountId)
            {
                return BadRequest();
            }

            _context.Entry(bankAccount).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BankAccountExists(id))
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

        // POST: api/BankAccounts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BankAccount>> PostBankAccount(BankAccount bankAccount)
        {
          if (_context.BankAccounts == null)
          {
              return Problem("Entity set 'BankSystemContext.BankAccounts'  is null.");
          }
            _context.BankAccounts.Add(bankAccount);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBankAccount", new { id = bankAccount.AccountId }, bankAccount);
        }

        // DELETE: api/BankAccounts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBankAccount(int id)
        {
            if (_context.BankAccounts == null)
            {
                return NotFound();
            }
            var bankAccount = await _context.BankAccounts.FindAsync(id);
            if (bankAccount == null)
            {
                return NotFound();
            }

            _context.BankAccounts.Remove(bankAccount);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BankAccountExists(int id)
        {
            return (_context.BankAccounts?.Any(e => e.AccountId == id)).GetValueOrDefault();
        }


        private string CreateJWT(BankAccount bankAccount) 
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("Key....");
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role ,bankAccount.RoleName),
                new Claim(ClaimTypes.Name ,bankAccount.Fullname)
            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddMinutes(60),
                SigningCredentials = credentials
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }
    }
}
