using HomeAccounting.Domain.Models.Entities;
using HomeAccounting.Domain.Repositories.Interfaces;
using HomeAccounting.Infrastructure.Services.Interfaces;
using HomeAccounting.WebApi.Controllers.BaseController;
using HomeAccounting.WebApi.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeAccounting.WebApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/limits")]
    public class LimitsController : BaseApiController
    {
        private readonly ILimitsRepository _limitsRepository;
        private readonly ILimitsService _limitsService;

        public LimitsController(ILimitsRepository limitsRepository, ILimitsService limitsService)
        {
            _limitsRepository = limitsRepository;
            _limitsService = limitsService;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OutcomeLimit>>> GetAllLimits()
        {
            return Ok(await _limitsRepository.GetAllLimits());
        }

        [Route("{accountId}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OutcomeLimitResponseDto>>> GetLimitsByAccount(int accountId)
        {
            List<OutcomeLimitResponseDto> limits = new List<OutcomeLimitResponseDto>();
            foreach(var lim in await _limitsRepository.GetLimitsByAccount(accountId))
            {
                limits.Add(new OutcomeLimitResponseDto {
                    AccountId = lim.AccountId,
                    Id = lim.Id,
                    Limit = lim.Limit,
                    LimitFrom = lim.LimitFrom,
                    LimitTo = lim.LimitTo,
                    Percentage = await _limitsService.CalculatePercentage(lim.Id),
                    TotalSpend = await _limitsService.CalculateTotalSpend(lim.AccountId, (DateTime)lim.LimitFrom, (DateTime)lim.LimitTo)
                });
            }
            return Ok(limits);
        }

        [Route("concrete/{limitId}")]
        [HttpGet]
        public async Task<ActionResult<OutcomeLimit>> GetConcreteLimit(int limitId)
        {
            return Ok(await _limitsRepository.GetConcreteLimit(limitId));
        }

        [HttpPost]
        public async Task<ActionResult> CreateLimit([FromBody] OutcomeLimit outcomeLimit)
        {
            if (outcomeLimit.LimitTo < DateTime.Now)
            {
                return BadRequest("The date of LimitTo is expired");
            }

            await _limitsRepository.CreateLimit(outcomeLimit);

            return Ok(outcomeLimit);
        }

        [Route("{limitToEditId}")]
        [HttpPut]
        public async Task<ActionResult> EditLimit([FromBody] OutcomeLimit newLimit, int limitToEditId)
        {
            if (newLimit.LimitTo < DateTime.Now)
            {
                return BadRequest("The date of LimitTo is expired");
            }

            await _limitsRepository.EditLimit(newLimit, limitToEditId);
            return Ok();
        }

        [Route("{limitId}")]
        [HttpDelete]
        public async Task<ActionResult> DeleteLimit(int limitId)
        {
            await _limitsRepository.DeleteLimit(limitId);
            return Ok();
        }
    }
}
