using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Routine.Api.Data;
using Routine.Api.Entities;

namespace Routine.Api.Services
{
    public class CompanyRepository: ICompanyRepository
    {
        private readonly RoutineDbContext _context;

        public CompanyRepository(RoutineDbContext context) //构造函数：注入Dbcontext
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Company>> GetCompaniesAsync()
        {
            return await _context.Companies.ToListAsync();
        }

        public async Task<Company> GetCompanyAsync(Guid companyId)
        {
            if (companyId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(companyId));
            }

            return await _context.Companies
                .FirstOrDefaultAsync(predicate:x => x.Id == companyId);
        }

        public async Task<IEnumerable<Company>>
            GetCompaniesAsync(IEnumerable<Guid> companyIds)
        {
            if (companyIds == null)
            {
                throw new ArgumentNullException(nameof(companyIds));
            }

            return await _context.Companies
                .Where(x => companyIds.Contains(x.Id))
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public void AddCompany(Company company)
        {
            if (company == null)
            {
                throw new ArgumentNullException(nameof(company));
            }

            company.Id = Guid.NewGuid();

            foreach (var employee in company.Employees)
            {
                employee.Id = Guid.NewGuid();
            }

            _context.Companies.Add(company);
        }

        public void UpdateCompany(Company company)
        {
            // _context.Entry(company).State = EntityState.Modified;
        }

        public void DeleteCompany(Company company)
        {
            if (company == null)
            {
                throw new ArgumentNullException(nameof(company));
            }

            _context.Companies.Remove(company);
        }

        public async Task<bool> CompanyExistsAsync(Guid companyId)
        {
            if (companyId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(companyId));
            }

            return await _context.Companies.AnyAsync(predicate:x => x.Id == companyId);
        }

        //public async Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId)
        public async Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId,
            [FromQuery] string genderDisplay,
            [FromQuery] string q)//添加参数，并指定来自查询字符串   [FromQuery(Name = "gender")] string genderDisplay  当两者不同，将传入的参数名与Dto中的参数名进行绑定
        //对这里对方法进行修改后，需要修改接口
        {
            if (companyId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(companyId));
            }

            //return await _context.Employees
            //.Where(x => x.CompanyId == companyId)
            //.OrderBy(x => x.EmployeeNo)
            //.ToListAsync();

            //判断传入参数是否为空，为空时：
            if (string.IsNullOrEmpty(genderDisplay) && string.IsNullOrWhiteSpace(q))
            {
                return await _context.Employees
                    .Where(x => x.CompanyId == companyId )
                    .OrderBy(x => x.EmployeeNo)
                    .ToListAsync();
            }

            var items = _context.Employees.Where(x => x.CompanyId == companyId);

            if (!string.IsNullOrEmpty(genderDisplay))
            { 
                var genderStr = genderDisplay.Trim();
                //目前为止传入的是字符串，而Entity中是枚举格式
                var gender = Enum.Parse<Gender>(genderStr);//转为枚举格式

                items = items.Where(x => x.Gender == gender);
            }

            if (!string.IsNullOrEmpty(q))
            {
                q = q.Trim();

                items = items.Where(x => x.EmployeeNo == q);
            }

            return await items
            .OrderBy(x => x.EmployeeNo)
            .ToListAsync();
        }

        public async Task<Employee> GetEmployeeAsync(Guid companyId, Guid employeeId)
        {
            if (companyId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(companyId));
            }

            if (employeeId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(employeeId));
            }

            return await _context.Employees
                .Where(x => x.CompanyId == companyId && x.Id == employeeId)
                .FirstOrDefaultAsync();
        }

        public void AddEmployee(Guid companyId, Employee employee)
        {
            if (companyId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(companyId));
            }

            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            employee.CompanyId = companyId;
            _context.Employees.Add(employee);
        }

        public void UpdateEmployee(Employee employee)
        {
            // _context.Entry(employee).State = EntityState.Modified;
        }

        public void DeleteEmployee(Employee employee)
        {
            _context.Employees.Remove(employee);
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}
