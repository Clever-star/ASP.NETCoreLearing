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
        //使用CompanyRepository来操作数据库，现在构造函数对其进行注入，使用接口
        public CompaniesController(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository ?? 
                throw new ArgumentException(nameof(companyRepository));
            //在为none时抛出异常
        }
        //标注，表示使用GET方法
        [HttpGet]
        //获取Company资源，使用async异步方法，结果类型是IActionResult(?)
        //IActionResult接口定义了一些可以代表IAction返回结果的合约(?)，通常返回的是JSON格式，需要序列化
        public async Task<IActionResult> GetCompanies()
            //HTTP动词不是在方法命中体现的，但是有一些约定，如果以Get开头，没有其他标注则为httpget
        {
            var companies = await _companyRepository.GetCompaniesAsync();

            //一种不常用的方法：进行串行化，返回JSON格式的结果；但是RESTfulApi的返回结果不一定是JSON格式
            //return new JsonResult(companies);

            //Controller内置的返回方法
            //404
            //return NotFound();
            //200

            //在添加Dto后，返回类型就要从Entity变成Dto了
            var companyDtos = new List<CompanyDto>();
            foreach (var company in companies)
            {
                companyDtos.Add(new CompanyDto { 
                    Id = company.Id,
                    Name = company.Name
                });
            }
            return Ok(companyDtos);
        }

        //标明属性路由
        [HttpGet(template:"{companyId}")] //默认的Controller级别的URI是api/Companies 也就是Controller上面的Route加上自己的 api/Companies+/{companyId}  当前URI：api/Companies/{companyId}
        //返回一个具体Company，需要传入一个ID
        public async Task<IActionResult> GetCompanies(Guid companyId)
        {
            ////第一种写法：（在需要返回结果时不提倡，仅需要返回是否存在的判断时可以使用）
            ////存在的问题：在对存在结果处理的极短时间内如果对资源进行了删除等其它操作，返回结果与实际情况不匹配，会导致返回错误
            ////判断是否存在
            //var exist = await _companyRepository.CompanyExistsAsync(companyId);
            ////使用companyRepository中的是否存在判断方法
            //if (!exist)
            //{
            //    //如果不存在，相当于资源找不到，反回404
            //    return NotFound();
            //}
            ////如果存在，则在数据库中查询并返回
            var company = await _companyRepository.GetCompanyAsync(companyId);
            //第二种写法
            if (company == null)
            {
                return NotFound();
            }
            return Ok(company);
        }
    }
}
