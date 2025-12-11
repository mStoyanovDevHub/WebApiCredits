using Microsoft.AspNetCore.Mvc;
using WebApiCredits.DAL;
using WebApiCredits.Models;

namespace WebApiCredits.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CreditController(ICreditRepository repository) : ControllerBase
    {
        private readonly ICreditRepository _repository = repository;

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Credit>>> GetAllCredits()
        {
            IEnumerable<Credit> credits;

            try
            {
                credits = await _repository.GetAllCreditsWithInvoicesAsync();
            }
            catch (Exception)
            {
                throw;
            }

            return Ok(credits);
        }

        [HttpGet("summary")]
        public async Task<ActionResult<CreditSummary>> GetCreditSummary()
        {
            CreditSummary summary;

            try
            {
                summary = await _repository.GetCreditSummaryAsync();
            }
            catch (Exception)
            {
                throw;
            }

            return Ok(summary);
        }
    }
}