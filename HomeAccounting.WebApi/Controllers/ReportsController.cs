using HomeAccounting.Domain.Db;
using HomeAccounting.Domain.Models.Entities.Reports;
using HomeAccounting.Domain.Repositories.Interfaces;
using HomeAccounting.Infrastructure.Extensions;
using HomeAccounting.Infrastructure.Services.Interfaces;
using HomeAccounting.WebApi.Controllers.BaseController;
using HomeAccounting.WebApi.DTOs.ReportDto;
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
    [Route("api/reports")]
    public class ReportsController : BaseApiController
    {

        private readonly IRepConstructorService _repConstructorService;
        public ReportsController(IRepConstructorService repConstructorService)
        {
            _repConstructorService = repConstructorService;
        }

        [Route("Income")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IncomeReport>>> GetIncomeReport([FromQuery] DateTime dateFrom, [FromQuery] DateTime dateTo)
        {
            var report = await _repConstructorService.GetFullIncomeReport(User.GetUserId(), dateFrom, dateTo);
            return Ok(report);
        }

        [Route("Outcome")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OutcomeReport>>> GetOutcomeReport([FromQuery] DateTime dateFrom, [FromQuery] DateTime dateTo)
        {
            var report = await _repConstructorService.GetFullOutcomeReport(User.GetUserId(), dateFrom, dateTo);
            return Ok(report);
        }
    }
}
