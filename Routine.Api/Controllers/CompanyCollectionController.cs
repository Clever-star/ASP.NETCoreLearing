using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Routine.Api.Entities;
using Routine.Api.Helpers;
using Routine.Api.Models;
using Routine.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.Controllers
{
    [ApiController]
    [Route("api/companycollections")]
    public class CompanyCollectionController:ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICompanyRepository _companyRepository;

        public CompanyCollectionController(IMapper mapper,ICompanyRepository companyRepository)
        {
            //为空时抛出异常
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
        }

        //需要接收一组key
        //写法一：1,2,3,4
        //写法二：key1=value1,key2=value2,key3=value3
        [HttpGet("{ids}",Name = nameof(GetCompanyCollection))]
        public async Task<IActionResult> GetCompanyCollection(
        [FromRoute]
        [ModelBinder(BinderType = typeof(ArrayModelBinder))]
        IEnumerable<Guid> ids)
        //如果不写[FromRoute]就默认从body中获得，但get请求不应该有请求body
        //像这种集合类的参数是无法从路由中获得的，因为路由是逗号分开的字符串。现在，仅通过隐式的绑定无法从路由中获得ID的集合
        //ASP.NET core允许我们创建自定义的model绑定，建立一个helpers文件夹
        {
            //首先判断Id参数是否为null
            if (ids == null)
            {
                //为null时，说明请求不合理，属于客户端的错误，返回BadRequest
                return BadRequest();
            }

            //如果不为空
            var entities = await _companyRepository.GetCompaniesAsync(ids);
            //只有当查询的个数和找到的个数相等时，才算查询成功
            if (ids.Count() != entities.Count())
            {
                return NotFound();
            }

            //把company的实体转换为dto类 返回前端展示。
            var dtosToReturn = _mapper.Map<IEnumerable<CompanyDto>>(entities);
            return Ok(dtosToReturn);
        }


        [HttpPost]
        //返回结果是一个集合
        public async Task<ActionResult<IEnumerable<CompanyDto>>> CreateCompanyCollection(
            IEnumerable<CompanyAddDto> companyCollection)
        {
            var companyEntities = _mapper.Map<IEnumerable<Company>>(companyCollection);
            foreach (var company in companyEntities)
            {
                _companyRepository.AddCompany(company);
            }

            await _companyRepository.SaveAsync();

            //POST成功后，需要返回成功的资源，作为返回的第三个参数
            var dtosToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);

            //第二个参数是egt方法所需的参数，在这里是ids
            var idsString = string.Join(",", dtosToReturn.Select(x => x.Id));
            //将这组资源的id串联成字符串，值之间以逗号隔开

            return CreatedAtRoute(nameof(GetCompanyCollection),
                new {ids =idsString } ,
                dtosToReturn);
        }
    }
}
