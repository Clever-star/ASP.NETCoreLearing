using AutoMapper;
using Routine.Api.Entities;
using Routine.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.Profiles
{
    public class CompanyProfile:Profile
    {
        public CompanyProfile()
        {
            //对GetCompanies()方法进行映射，只需要添加一对映射
            //泛型参数：CreateMap<原类型, 目标类型>
            //AutoMapper是基于约定的：1.源类型和目标类型的属性名一样时就会直接复制，并且会忽略空引用
            CreateMap<Company, CompanyDto>()
                .ForMember(
                dest => dest.CompanyName,
                opt => opt.MapFrom(src => src.Name));
        }
    }
}
