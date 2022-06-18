using AutoMapper;
using HomeAccounting.Domain.Enums;
using HomeAccounting.Domain.Models;
using HomeAccounting.Infrastructure.Extensions;
using HomeAccounting.Infrastructure.Services.Interfaces;
using HomeAccounting.WebApi.Controllers.BaseController;
using HomeAccounting.WebApi.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HomeAccounting.WebApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/ledgers")]
    public class LedgersController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly ILegderService _legderService;
        private const string ERROR_MESSAGE_ON_CREATE = "Invalid model. Not all field are inputed validely.";
        private const string ERROR_MESSAGE_ON_GET = "Such ledger does not exist.";
        private const string ERROR_MESSAGE_ON_LEDGER_REPORT = "Date to is less than date from";
        public LedgersController(IMapper mapper, ILegderService legderService)
        {
            _mapper = mapper;
            _legderService = legderService;
        }

        [Route("{ledgerId}")]
        [HttpGet]
        public async Task<ActionResult<Ledger>> GetConcreteLedger(int ledgerId)
        {
            if (await _legderService.GetConcreteLedger(ledgerId) == null)
            {
                return BadRequest(new Response<Ledger>
                {
                    Data = null,
                    IsSuccessful = false,
                    ErrorCode = HttpStatusCode.BadRequest.ToString(),
                    ErrorMessage = ERROR_MESSAGE_ON_GET
                });
            }
            else
            {
                return Ok(new Response<Ledger>
                {
                    Data = await _legderService.GetConcreteLedger(ledgerId),
                    IsSuccessful = true,
                    ErrorCode = null,
                    ErrorMessage = null
                });
            }
        }

        /*[HttpGet]
        public async Task<ActionResult<IEnumerable<LedgerResponseDto>>> GetLedgers([FromQuery] LedgerType? type, 
            [FromQuery] int? accountFromId, [FromQuery] int? accountToId)
        {
            var legders = await _legderService.GetLedgers(User.GetUserId(), type, accountFromId, accountToId);
            var newLedgers = _mapper.Map<IEnumerable<LedgerResponseDto>>(legders);

            return Ok(new Response<IEnumerable<LedgerResponseDto>> {Data = newLedgers, IsSuccessful = true, ErrorCode = null, ErrorMessage = null });
        }*/

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LedgerResponseDto>>> GetLedgers([FromQuery] int accountId, [FromQuery] DateTime dateFrom,
            [FromQuery] DateTime dateTo)
        {
            var ledgers = await _legderService.GetLedgers(accountId, dateFrom, dateTo);
            var newLedgers = _mapper.Map<IEnumerable<LedgerResponseDto>>(ledgers);

            if (dateFrom >= dateTo)
            {
                return BadRequest(new Response<IEnumerable<LedgerResponseDto>> { Data = null, ErrorCode = HttpStatusCode.BadRequest.ToString(), IsSuccessful = false, ErrorMessage = ERROR_MESSAGE_ON_LEDGER_REPORT });
            }

            return Ok(new Response<IEnumerable<LedgerResponseDto>> {Data = newLedgers, IsSuccessful = true, ErrorCode = null, ErrorMessage = null });
        }

        [HttpPost]
        public async Task<ActionResult<Ledger>> CreateLedger(LegderSendDto legder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response<Ledger> { Data = null, ErrorCode = HttpStatusCode.BadRequest.ToString(), IsSuccessful = false, ErrorMessage = ERROR_MESSAGE_ON_CREATE });
            }
            else
            {
                var ledger = _mapper.Map<Ledger>(legder);
                ledger.UserId = User.GetUserId();
                await _legderService.CreateLegder(ledger);
                return Ok(new Response<Ledger> { Data = ledger, IsSuccessful = true, ErrorCode = null, ErrorMessage = null });
            }
        }

        [Route("{legderId}")]
        [HttpPut]
        public async Task<ActionResult<Ledger>> EditLedger(int legderId, LegderSendDto ledgerToEdit)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response<Ledger> { Data = null, ErrorCode = HttpStatusCode.BadRequest.ToString(), IsSuccessful = false, ErrorMessage = ERROR_MESSAGE_ON_CREATE });
            }
            else
            {
                var ledger = _mapper.Map<Ledger>(ledgerToEdit);
                await _legderService.EditLegder(legderId, ledger);
                return Ok(new Response<Ledger> { Data = ledger, IsSuccessful = true, ErrorCode = null, ErrorMessage = null });
            }
        }

        [Route("{ledgerId}")]
        [HttpDelete]
        public async Task<ActionResult> DeleteLedger(int ledgerId)
        {
            await _legderService.DeleteLegder(ledgerId);
            return Ok();
        }
    }
}
