using HomeAccounting.Infrastructure.Services.Abstract;
using HomeAccounting.WebApi.Controllers.BaseController;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeAccounting.WebApi.Controllers
{
    [Route("api/exchange-rates")]
    public class EchangeRatesController : BaseApiController
    {
        private readonly IExchangeRatesService exchangeRatesService;

        public EchangeRatesController(IExchangeRatesService exchangeRatesService)
        {
            this.exchangeRatesService = exchangeRatesService;
        }

        [HttpGet]
        public async Task<ActionResult<decimal>> GetExchangeRate([FromQuery] string currencyFrom, [FromQuery] string currencyTo)
        {
            decimal value = await exchangeRatesService.GetExchangeRate(currencyFrom, currencyTo);

            if (currencyFrom.ToUpper() == currencyTo.ToUpper())
            {
                return Ok($"1 {currencyFrom.ToUpper()} = {1} {currencyTo.ToUpper()}");
            }
            else if (value == 0)
            {
                return BadRequest("Your currencies rate is not found");
            }
            else
            {
                return Ok($"1 {currencyFrom.ToUpper()} = {value} {currencyTo.ToUpper()}");
            }
        }
    }
}
