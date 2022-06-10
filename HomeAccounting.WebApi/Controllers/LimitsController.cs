using HomeAccounting.Domain.Models.Entities;
using HomeAccounting.Domain.Repositories.Interfaces;
using HomeAccounting.WebApi.Controllers.BaseController;
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

        public LimitsController(ILimitsRepository limitsRepository)
        {
            _limitsRepository = limitsRepository;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OutcomeLimit>>> GetAllLimits()
        {
            return Ok(await _limitsRepository.GetAllLimits());
        }

        [Route("{accountId}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OutcomeLimit>>> GetLimitsByAccount(int accountId)
        {
            return Ok(await _limitsRepository.GetLimitsByAccount(accountId));
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
