using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Routine.Api.Models;
using Routine.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.Controllers
{
    [ApiController]
    [Route("api/companies/{companyId}/employees")]
    public class EmployeesController:ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICompanyRepository _companyRepository;

        public EmployeesController(IMapper mapper,ICompanyRepository companyRepository) {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
        }

        [HttpGet]
        //获取一个公司下的所有员工
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployeesForCompany(Guid companyId)
        {
            //先进行判断是否存在
            if (!await _companyRepository.CompanyExistsAsync(companyId))
            { 
                return NotFound();
            }

            var employees = await _companyRepository.GetEmployeesAsync(companyId);
            //通过AutoMapper进行映射转换
            var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

            return Ok(employeeDtos);
        }

        [HttpGet("{employeeId}")]
        //获取一个公司下的其中一个员工
        public async Task<ActionResult<EmployeeDto>> GetEmployeeForCompany(Guid companyId,Guid employeeId)
        {
            //先进行判断是否存在
            if (!await _companyRepository.CompanyExistsAsync(companyId))
            {
                return NotFound();
            }

            //用另一种方式判断employee是否存在
            var employee = await _companyRepository.GetEmployeeAsync(companyId, employeeId);
            if (employee == null) {
                return NotFound();
            }

            var employeeDto = _mapper.Map<EmployeeDto>(employee);

            return Ok(employeeDto);
        }
    }
}
