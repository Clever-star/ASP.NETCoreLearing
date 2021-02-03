using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Routine.Api.Data;
using Routine.Api.DtoParameters;
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

        public async Task<IEnumerable<Company>> GetCompaniesAsync(CompanyDtoParameters parameters)//修改接口的实现类中方法的参数
        {
            //首先，检查参数是否为null，如果为null则抛出一个异常
            if (parameters == null) {
                throw new ArgumentException(nameof(parameters));
            }
            //分别判断参数中是否有查询条件
            if (string.IsNullOrWhiteSpace(parameters.CompanyName) &&
                string.IsNullOrWhiteSpace(parameters.SearchTerm))
            {
                //判断，如果都为空的话，就直接返回
                return await _context.Companies.ToListAsync();
            }

            //建立一个查询表达式，此时还没有执行对数据库的查询
            var queryExpression = _context.Companies as IQueryable<Company>;
            //逐个判断（过滤条件和搜索条件）
            //过滤条件：如果公司名不为空
            if (!string.IsNullOrWhiteSpace(parameters.CompanyName)) {
                parameters.CompanyName = parameters.CompanyName.Trim();
                //将CompanyName作为过滤条件
                queryExpression = queryExpression.Where(x => x.Name == parameters.CompanyName);
            }

            //查询条件
            if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            {
                parameters.SearchTerm = parameters.SearchTerm.Trim();
                //将CompanyName作为过滤条件
                queryExpression = queryExpression.Where(x => x.Name.Contains(parameters.SearchTerm) ||
                x.Introduction.Contains(parameters.SearchTerm));
            }

            return await queryExpression.ToListAsync();//遇到ToList后才真正执行查数据库
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

            //foreach (var employee in company.Employees)
            //{
            //    employee.Id = Guid.NewGuid();
            //}
            //在添加公司时，因为employee为空出错，所以添加一个判断
            if (company.Employees != null)
            {
                foreach (var employee in company.Employees)
                {
                    employee.Id = Guid.NewGuid();
                }
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
