using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.DtoParameters
{
    public class CompanyDtoParameters   //在CompanyController中修改GetCompanies中改变参数
    {
        //将Dto中的两个字段加上
        //public Guid Id { get; set; }   //去掉一个查询条件（不符合实际情况）
        public string CompanyName { get; set; }

        //添加搜索字节
        public string SearchTerm { get; set; }
    }
    //将查询参数封装成类的好处就是，Controller里面的方法就无需更改了。（接口Repository就不需要更改了，仅需修改实现类中的方法内容）
}
