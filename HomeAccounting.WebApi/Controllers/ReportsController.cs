using AutoMapper;
using HomeAccounting.Domain.Db;
using HomeAccounting.Domain.Models.Entities.Reports;
using HomeAccounting.Domain.Repositories.Interfaces;
using HomeAccounting.Infrastructure.Extensions;
using HomeAccounting.Infrastructure.Services.Interfaces;
using HomeAccounting.WebApi.Controllers.BaseController;
using HomeAccounting.WebApi.DTOs;
using HomeAccounting.WebApi.DTOs.ReportDto.Income;
using HomeAccounting.WebApi.DTOs.ReportDto.Outcome;
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
        private readonly IMapper _mapper;
        private const string ERROR_MESSAGE = "Date to is less than date from.";
        public ReportsController(IRepConstructorService repConstructorService, IMapper mapper)
        {
            _repConstructorService = repConstructorService;
            _mapper = mapper;
        }

        [Route("Income")]
        [HttpGet]
        public async Task<ActionResult<IncomeReportDto>> GetIncomeReport([FromQuery] DateTime dateFrom, [FromQuery] DateTime dateTo)
        {
            if (dateFrom >= dateTo)
            {
                return BadRequest(new Response<IncomeReportDto>
                {
                    Data = null,
                    ErrorCode = HttpStatusCode.BadRequest.ToString(),
                    IsSuccessful = false,
                    ErrorMessage = ERROR_MESSAGE
                });
            }

            var report = await _repConstructorService.GetFullIncomeReport(User.GetUserId(), dateFrom, dateTo);
            var reportDto = _mapper.Map<IncomeReportDto>(report);
            
            return Ok(new Response<IncomeReportDto>
            {
                Data = reportDto,
                ErrorCode = null,
                ErrorMessage = null,
                IsSuccessful = true
            });
        }

        [Route("Outcome")]
        [HttpGet]
        public async Task<ActionResult<OutcomeReportDto>> GetOutcomeReport([FromQuery] DateTime dateFrom, [FromQuery] DateTime dateTo)
        {
            if (dateFrom >= dateTo)
            {
                return BadRequest(new Response<OutcomeReportDto>
                {
                    Data = null,
                    ErrorCode = HttpStatusCode.BadRequest.ToString(),
                    IsSuccessful = false,
                    ErrorMessage = ERROR_MESSAGE
                });
            }

            var report = await _repConstructorService.GetFullOutcomeReport(User.GetUserId(), dateFrom, dateTo);
            var reportDto = _mapper.Map<OutcomeReportDto>(report);
            return Ok(new Response<OutcomeReportDto> 
            {
                Data = reportDto,
                ErrorCode = null,
                ErrorMessage = null,
                IsSuccessful = true
            });
        }
    }
}
