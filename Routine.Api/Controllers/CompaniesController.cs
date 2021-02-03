using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Routine.Api.DtoParameters;
using Routine.Api.Entities;
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
        [HttpHead] //使这个方法即支持HEAD又支持GET，当用GET请求时返回状态码和body；当使用HEAD请求时仅不返回body，其他返回与GET相同
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompanies(
            [FromQuery] CompanyDtoParameters parameters)
        {
            //throw new Exception("An Exception");
            var companies = await _companyRepository.GetCompaniesAsync(parameters);//将参数传递给Repository里的相应方法
            var companydtos = new List<CompanyDto>();
            var companyDtos = _mappper.Map<IEnumerable<CompanyDto>>(companies);
            return Ok(companyDtos);
        }

        //标明属性路由
        [HttpGet("{companyId}",Name = nameof(GetCompany))] //添加routename
        //默认的Controller级别的URI是api/Companies 也就是Controller上面的Route加上自己的 api/Companies+/{companyId}  当前URI：api/Companies/{companyId}
        //返回一个具体Company，需要传入一个ID
        public async Task<ActionResult<CompanyDto>> GetCompany(Guid companyId)
        {
            var company = await _companyRepository.GetCompanyAsync(companyId);
            if (company == null)
            {
                return NotFound();
            }
            //return Ok(company);
            return Ok(_mappper.Map<CompanyDto>(company)); 
        }

        [HttpPost]
        //创建Post方法用于创建公司
        //Post方法的参数应该是在请求的body里面（应该反串行化，成为C#的一个类：即默认[FromBody]）
        //Dto这个类是用来输出的，其中的Id属性是不用输入的，由后台生成（在此程序中，在其他RESTful风格的程序中也可由客户输入）
        //Entity实体中的Introduction属性也没有在Dto中体现，所以要添加一个类，专门用于添加公司 CompanyAddDto
        public async Task<ActionResult<CompanyDto>> CreateCompany(CompanyAddDto company) {
            //在较老的版本中，需要添加判断，因为在没有[ApiController]这个Attribute的时候，需要检查body中的参数是否转换成了CompanyAddDto这个类，如果参数为空或者不正确时，那么参数为null
            ////在使用了[ApiController]这个Attribute之后，出现这个情况是，框架会自动返回400BadRequest(来此客户的错误)
            //if (company == null)
            //{
            //    return BadRequest();
            //}

            //首先，要把CompanyAddDto这个类转化成Entity，使用AutoMapper
            var entity = _mappper.Map<Company>(company);//仅写此行代码还不能生效，需要设置map映射关系
            _companyRepository.AddCompany(entity);//map之后调用repository中的添加公司方法，但此时entirty并没有被添加到数据库中，AddCompany方法（可查看源码）只是把参数对象添加到了dbcontext中
            //在AddCompany方法中，给Id赋了guid类型的值
            
            //想要将其操作入数据库中，就需要调用dbcontext的SaveChangesAsync方法，该方法已封装，可在此直接调用
            await _companyRepository.SaveAsync();//其中具体的实现细节对Controller来说是未知的，抛出异常时，框架也会自动返回500
            
            //之后需要将插入资源后的Entity返回，这个映射之前已经设置过了
            var returnDto = _mappper.Map<CompanyDto>(entity);
            
            //Post成功执行后应该返回的状态码为201
            //可以使用CreatedAtRoute这个HelpMethod，他允许我们返回一个带地址Header的响应,地址Header中含有uri，通过uri可以找到新创建的资源
            return CreatedAtRoute(nameof(GetCompany),new { companyId = returnDto.Id}, returnDto);
        }
    }
}
