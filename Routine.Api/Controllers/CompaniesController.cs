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
    [ApiController]//应用于Controller(?)
    [Route(template: "api/companies")]//将每一个方法需要配置的部分移出来
    //[Route(template: "api/[controller]")] //另一种写法：动态变化取Controller前的名字
    //需要使用属性路由
    public class CompaniesController: ControllerBase
    //继承于Controller更适合既支持MVCWeb又支持WebApi的程序（Controller也继承于ControllerBase）
    //继承ControllerBase适用于仅需要支持WebApi的程序
    //ControllerBase提供了很多用于处理HTTP请求的属性和方法
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mappper;

        //使用CompanyRepository来操作数据库，现在构造函数对其进行注入，使用接口
        public CompaniesController(ICompanyRepository companyRepository,IMapper mappper) //在构造函数中注入，map实例实现IMapper接口  快捷键Alt+Enter
        {
            _companyRepository = companyRepository ?? 
                throw new ArgumentException(nameof(companyRepository));
            //在为none时抛出异常

            _mappper = mappper ??
                throw new ArgumentException(nameof(mappper)); ;
        }
        //标注，表示使用GET方法
        [HttpGet]
        //获取Company资源，使用async异步方法，结果类型是IActionResult(?)
        //IActionResult接口定义了一些可以代表IAction返回结果的合约(?)，通常返回的是JSON格式，需要序列化
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompanies()
        {
            var companies = await _companyRepository.GetCompaniesAsync();
            var companydtos = new List<CompanyDto>();
            var companyDtos = _mappper.Map<IEnumerable<CompanyDto>>(companies);
            return Ok(companyDtos);
        }

        //标明属性路由
        [HttpGet(template:"{companyId}")] //默认的Controller级别的URI是api/Companies 也就是Controller上面的Route加上自己的 api/Companies+/{companyId}  当前URI：api/Companies/{companyId}
        //返回一个具体Company，需要传入一个ID
        public async Task<ActionResult<CompanyDto>> GetCompanies(Guid companyId)
        {
            var company = await _companyRepository.GetCompanyAsync(companyId);
            if (company == null)
            {
                return NotFound();
            }
            //return Ok(company);
            return Ok(_mappper.Map<CompanyDto>(company)); 
        }
    }
}
