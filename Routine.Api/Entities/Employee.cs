using System;

namespace Routine.Api.Entities
{
    public class Employee
    {
        public Guid Id { get; set; }//主键
        public Guid CompanyId { get; set; }//外键
        public string EmployeeNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }//使用性别类
        public DateTime DateOfBirth { get; set; }
        public Company Company { get; set; }//?关联的导航属性指向Company
    }
}