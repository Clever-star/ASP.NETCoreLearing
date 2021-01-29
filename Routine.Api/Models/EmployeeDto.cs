using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.Models
{
    public class EmployeeDto
    {
        public Guid Id { get; set; }//主键
        public Guid CompanyId { get; set; }//外键
        public string EmployeeNo { get; set; }
        public string Name { get; set; }
        public string GenderDisplay { get; set; }//使用性别类
        public int Age { get; set; }
    }
}
