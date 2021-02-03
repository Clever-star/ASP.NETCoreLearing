using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Routine.Api.Entities;
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
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployeesForCompany(Guid companyId, 
            [FromQuery] string genderDisplay,
            string q)
            //当查询参数动态变化时，频繁操作函数不方便，将这些参数在ResourceParameters置入一个类中更便于操作
        {
            //先进行判断是否存在
            if (!await _companyRepository.CompanyExistsAsync(companyId))
            { 
                return NotFound();
            }

            var employees = await _companyRepository.GetEmployeesAsync(companyId,genderDisplay,q);//传入参数
            //通过AutoMapper进行映射转换
            var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

            return Ok(employeeDtos);
        }

        [HttpGet("{employeeId}", Name = nameof(GetEmployeeForCompany))]
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

        [HttpPost]//Controller级别的路由模板已经够用了，此时就不需要再在action级别的路由添加模板了
        public async Task<ActionResult<EmployeeDto>> CreateEmployeeForCompany(Guid companyId, EmployeeAddDto employee)
        {

            //先进行判断该公司是否存在
            if (!await _companyRepository.CompanyExistsAsync(companyId))
            {
                return NotFound();
            }

            var entity = _mapper.Map<Employee>(employee);//需要配置映射
            _companyRepository.AddEmployee(companyId,entity);

            await _companyRepository.SaveAsync();

            var dtoToReturn = _mapper.Map<EmployeeDto>(entity);

            return CreatedAtRoute(nameof(GetEmployeeForCompany), new { companyId = companyId , employeeId = dtoToReturn.Id }, dtoToReturn );//传入一个对象，包含两个参数;其中对象里的第一个参数可以简写成companyId
        }
    }
}
