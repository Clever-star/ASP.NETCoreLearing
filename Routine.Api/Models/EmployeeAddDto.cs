using Routine.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.Models
{
    public class EmployeeAddDto
    {
        //在创建资源的Dto中不需要Id属性，该服务是由服务端实现的，而不是由客户端输入的
        //public Guid CompanyId { get; set; }//外键，因为在方法参数中已经有了companyid这个参数，它们的值如果不同还需要进行判断操作，这种操作是冗余的
        public string EmployeeNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }//使用性别类
        public DateTime DateOfBirth { get; set; }
    }
}
