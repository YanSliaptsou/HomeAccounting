using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeAccounting.Domain.Models;
using HomeAccounting.Domain.Repositories.Abstarct;

namespace HomeAccounting.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        IExchangeRatesRepository _exchangeRatesRepository;
        public TestController(IExchangeRatesRepository repository)
        {
            _exchangeRatesRepository = repository;
        }

        [Route("exchange-rates")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExchangeRate>>> GetExhangeRates()
        {
            var exchangeRates = await _exchangeRatesRepository.GetAllExchaneRates();

            return Ok(exchangeRates);
        }
    }
}
