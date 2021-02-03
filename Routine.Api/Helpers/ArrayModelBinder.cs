using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Routine.Api.Helpers
{
    public class ArrayModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            //ModelBindingContext中有Model的源数据，用这些原数据来完成BindModelAsync这个方法
            //由于get方法的参数是IEnumerable类型，所以需要保证ArrayModelBinder类作用域IEnumerable类型
            //进行判断它所作用的类型不是IEnumerable类型时
            if (!bindingContext.ModelMetadata.IsEnumerableType)
            {
                //返回失败
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;//
            }

            //否则就获取值（ValueProvider值提供者）
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).ToString();//获取到的就是逗号分隔开的字符串
            
            //判断字符串是否为空
            if (string.IsNullOrWhiteSpace(value))
            {
                //如果为空，则说明传入的参数为空，不需要进行其他特殊处理，此时是成功的
                //应该在action中返回BadRequest
                bindingContext.Result = ModelBindingResult.Success(null);
                return Task.CompletedTask;//
            }

            //如果value值不是空的且它所作用的类型是IEnumerable类型
            //首先需要获得IEnumerable中model的类型（在该代码中就是guid）
            var elementType = bindingContext.ModelType.GetTypeInfo().GenericTypeArguments[0];//第0个为类型
            //创建转换器
            var converter = TypeDescriptor.GetConverter(elementType);//转换器为了转换字符串为Guid类型

            //进行转换
            var values = value.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                //Split函数的参数：第一个new[] { "," }表示新建一个数组，以","为分隔
                //第二个参数：StringSplitOptions.RemoveEmptyEntries去掉为空的部分

                //针对每一个元素使用转换器转换
                .Select(x => converter.ConvertFromString(x.Trim())).ToArray();
            //此时value返回的是一个object数组

            //新建一个数组，转化为具体的类型
            var typedValues = Array.CreateInstance(elementType, values.Length);//通过这个方法设定为相同长度
            values.CopyTo(typedValues, 0);//values 复制到TypeValues 从第0个开始拷贝

            bindingContext.Model = typedValues;//设置最终的返回结果
            //此时bindingContext.Model的值就是IEnumerable<Guid>这个类型.

            //最后返回成功
            bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);

            return Task.CompletedTask;
        }
    }
}
