using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.Models
{
    //该类专门用于新增创建
    //【注】尽管有时候用于创建和查询的Dto是一样的，也要分为不同的Dto，因为业务需求是动态变化的，分为两个类便于重构
    public class CompanyAddDto
    {
        //不需要传入实体中的Id属性
        public string Name { get; set; }
        public string Introduction { get; set; }
        public ICollection<EmployeeAddDto> Employees { get; set; } = new List<EmployeeAddDto>();//这样可以避免发生空引用异常
        //该属性（Employees）与Comany中的导航属性Employees相同
    }
}
