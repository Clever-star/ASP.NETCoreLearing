using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Routine.Api.Data;
using Routine.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(configure:setup =>
            {
                //处理服务器与请求的资源格式不相同的方法
                //方法一：返回状态码
                //默认情况下该值为false，当请求的格式与服务器格式不一致时，并且不返回406状态码
                setup.ReturnHttpNotAcceptable = true;
                //方法二：添加支持的格式(以前的写法)
                //在OutputFormatters集合中存放多个格式化器，第一个是默认的；默认情况下里面只有一个JSON
                //想要支持xml，就在该集合中添加xml格式化器
                //setup.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                //想修改默认的格式时（相应的input是规定输入格式）
                //setup.OutputFormatters.Insert(0, new XmlDataContractSerializerOutputFormatter());
            }).AddXmlDataContractSerializerFormatters();
            //现在的写法，可以直接添加输入输出格式

            //将这套服务注册到现在的程序及当中
            //使用AddAutoMapper这个方法，参数要有一个Assembly类的一个数组，AutoMapper将在这个程序集里面扫描寻找AutoMapper的配置文件
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            //寻找当前应用域下的Assemblies

            services.AddScoped<ICompanyRepository, CompanyRepository>();
            
            services.AddDbContext<RoutineDbContext>(optionsAction:option =>
            {
                option.UseSqlite(connectionString:"Data Source=routine.db");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //使用一个中间件(?)并对其进行配置
            else {
                app.UseExceptionHandler(appBuilder => {
                    appBuilder.Run(async context =>{
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("Unexpected Error!");
                });
                });
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
