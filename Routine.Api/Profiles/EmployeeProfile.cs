﻿using AutoMapper;
using Routine.Api.Entities;
using Routine.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.Profiles
{
    public class EmployeeProfile:Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Employee, EmployeeDto>()
                .ForMember(
                dest => dest.Name,
                opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))//将FirstName和LastName连在一起中间加一个空格
                .ForMember(
                dest => dest.GenderDisplay,
                opt => opt.MapFrom(src => src.Gender.ToString()))
                .ForMember(
                dest => dest.Age,
                opt => opt.MapFrom(src => DateTime.Now.Year - src.DateOfBirth.Year));

            CreateMap<EmployeeAddDto,Employee>();//属性几乎一致，不需要针对特殊属性进行配置
        }
    }
}
