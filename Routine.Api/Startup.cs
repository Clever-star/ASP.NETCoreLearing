using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
                //������������������Դ��ʽ����ͬ�ķ���
                //����һ������״̬��
                //Ĭ������¸�ֵΪfalse��������ĸ�ʽ���������ʽ��һ��ʱ�����Ҳ�����406״̬��
                setup.ReturnHttpNotAcceptable = true;
                //�����������֧�ֵĸ�ʽ(��ǰ��д��)
                //��OutputFormatters�����д�Ŷ����ʽ��������һ����Ĭ�ϵģ�Ĭ�����������ֻ��һ��JSON
                //��Ҫ֧��xml�����ڸü��������xml��ʽ����
                //setup.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                //���޸�Ĭ�ϵĸ�ʽʱ����Ӧ��input�ǹ涨�����ʽ��
                //setup.OutputFormatters.Insert(0, new XmlDataContractSerializerOutputFormatter());
            }).AddXmlDataContractSerializerFormatters();
            //���ڵ�д��������ֱ��������������ʽ

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

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
