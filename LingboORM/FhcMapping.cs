using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LingboORM
{
   public class FhcMapping
    {
       /// <summary>
        /// 获取实体所映射的表名（实体命名必须与数据库表一一对应）
       /// </summary>
       /// <param name="obj"></param>
       /// <returns></returns>
       internal static string GetTableName(object obj)
       {
           //获取传入的实体的类型
           Type type = obj.GetType();
           //传入的类名称（实体命名必须与数据库表一一对应）
           string strClassName = type.Name.ToString();
          
           return strClassName;
       }
       /// <summary>
       ///  获取实体所映射的表名的所有字段（实体类里属性必须与数据库表中字段一一对应）
       /// </summary>
       /// <param name="obj"></param>
       /// <returns></returns>
       internal static List<string> GetAllFieldMapping(object obj,bool flg=true)
       {
           //获取传入的实体的类型
           Type type = obj.GetType();
           PropertyInfo[] pr= type.GetProperties();
           List<string> li = new List<string>();
           foreach (PropertyInfo item in pr)
           {
               if (flg)
               {
                   Type ty = type.GetProperty(item.Name).PropertyType;
                   //判断是否为自定义类型
                   if ((!ty.IsClass) || (ty == typeof(string)))
                   {
                       li.Add(item.Name);
                   }
                  
               }
               else
               {
                   li.Add(item.Name);
               }
              
             
           }
           return li;
       }
       /// <summary>
       /// 获取实体所映射的表名的字段（区分主键标识列）
       /// </summary>
       /// <param name="obj"></param>
       /// <returns></returns>
       internal static List<string> GetFieldMapping(object obj)
       {
           //获取传入的实体的类型
           Type type = obj.GetType();
           PropertyInfo[] pr = type.GetProperties();
           List<string> li = new List<string>();
           //获取主键是否设置了标识列
           string mapStr = type.InvokeMember("GetMapping", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null, obj, new object[] { }).ToString();
           string idenStr = mapStr.Split(',')[0];
           bool isIden = Convert.ToBoolean(mapStr.Split(',')[1]);
           foreach (PropertyInfo item in pr)
           {
               if (!item.Name.Equals(idenStr))
               {
                   if (item.GetValue(obj, null) != null)
                   {
                       //if (!item.GetValue(obj, null).ToString().Equals("0001/1/1 0:00:00")&&!item.GetValue(obj, null).ToString().Equals("-1412671782"))
                       //{

                       li.Add(item.Name);
                       // }
                   }

               }
               else
               {
                   if (!isIden)
                   {
                       li.Add(item.Name);
                   }
               }
           }
           return li;
       }
       /// <summary>
       /// 获取设置实体的值
       /// </summary>
       /// <param name="obj"></param>
       /// <returns></returns>
       internal static List<string> GetValuesFieldMapping(object obj)
       {
           //获取传入的实体的类型
           Type type = obj.GetType();
           PropertyInfo[] pr = type.GetProperties();
           List<string> li = new List<string>();
           //获取属性的值
           //object arrObject = obj.GetType().InvokeMember(key, BindingFlags.GetProperty, null, obj, null);
           //获取主键是否设置了标识列
           string mapStr = type.InvokeMember("GetMapping", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null, obj, new object[] { }).ToString();
           string idenStr = mapStr.Split(',')[0];
           bool isIden =Convert.ToBoolean( mapStr.Split(',')[1]);
           foreach (PropertyInfo item in pr)
           {
               if (!item.Name.Equals(idenStr))
               {
                   if (item.GetValue(obj, null) != null)
                   {
                       //if (!item.GetValue(obj, null).ToString().Equals("0001/1/1 0:00:00") && !item.GetValue(obj, null).ToString().Equals("-1412671782"))
                       //{

                           li.Add(item.GetValue(obj, null).ToString());
                      // }
                   
                   }
               }
               else
               {
                   if (!isIden)
                   {
                       li.Add(item.GetValue(obj, null).ToString());
                   }
               }
           }
           return li;
       }
       /// <summary>
       /// 根据类型创建该类型对象实例
       /// </summary>
       /// <param name="type"></param>
       /// <returns></returns>
       internal static object CreateObj(Type type)
       {
           //根据 Mapping 类型创建对象
           object MappingClass = type.InvokeMember(null, BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.CreateInstance, null, null, null);

           //返回对象的实例
           return MappingClass;
       }
       /// <summary>
       /// 通过反射动态创建类的对象并返回该对象
       /// </summary>
       /// <param name="obj"></param>
       /// <param name="mappingType"></param>
       /// <param name="assemblyStr"></param>
       /// <returns></returns>
       //internal static object GetMappingObject(object obj, ref Type mappingType, string assemblyStr)
       //{
       //    //获取传入的实体的类型
       //    Type type = obj.GetType();

       //    //传入的类名称
       //    string strClassName = type.ToString();

       //    //如果获取到的是带有命名空间的类型
       //    if (type.ToString().Contains("."))
       //    {
       //        strClassName = type.ToString().Substring(type.ToString().LastIndexOf("."), type.ToString().Length - type.ToString().LastIndexOf("."));
       //    }

       //    //取出映射文件名称  ClassName + Mapping
       //    string strMappingClassName = assemblyStr + strClassName;

       //    //反射出映射类的实例
       //    object MappingObject = Assembly.Load(assemblyStr).CreateInstance(strMappingClassName);

       //    //获取 Mapping 的类型
       //    mappingType = MappingObject.GetType();

       //    //根据 Mapping 类型创建对象
       //    object MappingClass = mappingType.InvokeMember(null, BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.CreateInstance, null, null, null);

       //    //返回对象的实例
       //    return MappingClass;
       //}
    }
}
