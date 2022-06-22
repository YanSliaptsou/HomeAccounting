using HomeAccounting.Infrastructure.Services.Abstract;
using HomeAccounting.WebApi.Controllers.BaseController;
using HomeAccounting.WebApi.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HomeAccounting.WebApi.Controllers
{
    [Route("api/exchange-rates")]
    public class EchangeRatesController : BaseApiController
    {
        private readonly IExchangeRatesService exchangeRatesService;
        private const string ERROR_MESSAGE = "Such exchange rate does not exist";

        public EchangeRatesController(IExchangeRatesService exchangeRatesService)
        {
            this.exchangeRatesService = exchangeRatesService;
        }

        [HttpGet]
        public async Task<ActionResult<ExchangeRate>> GetExchangeRate([FromQuery] string currencyFrom, [FromQuery] string currencyTo)
        {
            decimal value = await exchangeRatesService.GetExchangeRate(currencyFrom.ToUpper(), currencyTo.ToUpper());

            if (value == 0)
            {
                return BadRequest(new Response<ExchangeRate>
                {
                    Data = null,
                    ErrorCode = HttpStatusCode.BadRequest.ToString(),
                    ErrorMessage = ERROR_MESSAGE,
                    IsSuccessful = false
                });
            }
            else
            {
                return Ok(
                    new Response<ExchangeRate>
                    {
                        Data = new ExchangeRate
                        {
                            CurrencyFrom = currencyFrom.ToUpper(),
                            CurrencyTo = currencyTo.ToUpper(),
                            ExchangeRateValue = Math.Round(value, 2)
                        },
                        ErrorCode = null,
                        ErrorMessage = null,
                        IsSuccessful = false
                    });
            }
        }
    }
}
