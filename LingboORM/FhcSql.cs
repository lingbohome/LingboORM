using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LingboORM
{
   public class FhcSql
    {
       /// <summary>
       /// 创建查询list集合
       /// </summary>
       /// <param name="strTableName"></param>
       /// <param name="FieldMapping"></param>
       /// <param name="strWhere"></param>
       /// <returns></returns>
       internal static string CreateSelectList(string strTableName, List<string> FieldMapping, string strWhere)
       {
           //把字段集合转换成为字符串
           string strSelectText = string.Join(",", FieldMapping);
           string strSql = "SELECT " + strSelectText + " FROM " + strTableName + " WHERE " + strWhere;
           return strSql;
       }
       /// <summary>
       /// 创建查询分页list集合
       /// </summary>
       /// <param name="strTableName"></param>
       /// <param name="FieldMapping"></param>
       /// <param name="strWhere"></param>
       /// <returns></returns>
       internal static string CreateSelectPageList(string strTableName, List<string> FieldMapping,int start,int end, string strWhere,string romNumstr,string orderById)
       {
           //把字段集合转换成为字符串
           string strSelectText = string.Join(",", FieldMapping);
           string strSql = "SELECT " + strSelectText + " FROM (SELECT " + strSelectText + ",row_number() over (order by " + romNumstr + ") as num FROM " + strTableName + " where " + strWhere + ") t WHERE t.num between " + start + " and " + end + orderById;
           return strSql;
       }
       /// <summary>
       /// 执行创建的查询集合sql语句
       /// </summary>
       /// <param name="sql"></param>
       /// <param name="strConn"></param>
       /// <param name="obj"></param>
       /// <param name="FieldMapping"></param>
       /// <returns></returns>
       internal static List<object> ExecSelectList(string sql, string strConn, object obj, List<string> FieldMapping)
       {
           DataSet ds = SqlHelper.ExecuteDataset(strConn, CommandType.Text, sql);
           return ChangeDateSetToList(obj, ds,strConn);
       }
       /// <summary>
       /// 将查询到了数据集合转换成list集合
       /// </summary>
       /// <param name="obj"></param>
       /// <param name="li"></param>
       /// <param name="ds"></param>
       /// <returns></returns>
       internal static List<object> ChangeDateSetToList(object obj, DataSet ds,string strConn)
       {
           //获取传入的类型
           Type type = obj.GetType();
           List<object> tempList = new List<object>();
           //获取字段（实体属性不包含外键对象）
           List<string> li = FhcMapping.GetAllFieldMapping(obj,false);
           if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
           {
               foreach (DataRow item in  ds.Tables[0].Rows)
               {
                    //创建实体类型实例（即表所对应的实体类）
                    object typeTempObject = type.InvokeMember(null, BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.CreateInstance, null, null, null);
                    foreach (string key in li)
                    {
                        Type ty = type.GetProperty(key).PropertyType;
                        //判断是否为自定义类型
                        if ((!ty.IsClass) || (ty==typeof(string)))
                        {
                            //根据数据类型取出值
                            object[] arrObject = GetProType(ty, item[key].ToString());
                            //属性赋值
                            type.InvokeMember(key, BindingFlags.SetProperty, null, typeTempObject, arrObject);
                        }
                        else
                        {
                            object probj= FhcMapping.CreateObj(ty);
                            //获取外键标识
                            string mapStr = type.InvokeMember("GetMapping", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null, obj, new object[] { }).ToString();
                            string refStr = mapStr.Split(',')[2];
                            //获取主键键标识
                            string mapPr = ty.InvokeMember("GetMapping", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null, obj, new object[] { }).ToString();
                            string prim = mapPr.Split(',')[0];
                            string val = GetFieldValue(probj, prim, item[refStr].ToString());
                            probj = FhcBaseOrm.GetList(probj, strConn, prim + "=" + val).FirstOrDefault();
                            //属性赋值
                            type.InvokeMember(key, BindingFlags.SetProperty, null, typeTempObject, new object[] { probj });
                        }
                       
                       // type.GetProperty(key).SetValue(typeTempObject,TypeConvertor.ConvertType(item[key].ToString(), type.GetProperty(key).PropertyType),null);
                    }
                    //将实体添加到 List 
                    tempList.Add(typeTempObject);
                    //回收对象
                    typeTempObject = null;
               }
           }
           return tempList;
       }
       /// <summary>
       /// 创建insert语句
       /// </summary>
       /// <param name="strTableName"></param>
       /// <param name="FieldMapping"></param>
       /// <param name="FieldValues"></param>
       /// <returns></returns>
       internal static string CreateInsert(string strTableName, List<string> FieldMapping, List<string> FieldValues,object obj)
       {

           //以逗号分隔转换成sql语句
           string strFieldText = string.Join(",", FieldMapping);
           string[] newValues = new string[FieldValues.Count];
           for (int i = 0; i < FieldValues.Count; i++)
           {
               newValues[i] = GetFieldValue(obj, FieldMapping[i], FieldValues[i]);
           }
           string strValueText = string.Join(",", newValues);
           string strSql = "INSERT INTO " + strTableName + " (" + strFieldText + ") VALUES (" + strValueText + ")";
           return strSql;
       }
       /// <summary>
       /// 执行insert语句
       /// </summary>
       /// <param name="sql"></param>
       /// <param name="strConn"></param>
       /// <returns></returns>
       internal static bool ExecInsert(string sql,string strConn)
       {
           if (SqlHelper.ExecuteNonQuery(strConn, CommandType.Text, sql) > 0)
           {
               return true;
           }
           else
           {
               return false;
           }
          
       }
       /// <summary>
       /// 创建Update语句
       /// </summary>
       /// <param name="strTableName"></param>
       /// <param name="FieldMapping"></param>
       /// <param name="FieldValues"></param>
       /// <param name="obj"></param>
       /// <param name="strWhere"></param>
       /// <returns></returns>
       internal static string CreateUpdate(string strTableName, List<string> FieldMapping, List<string> FieldValues,object obj,string strWhere)
       {
            //声明字段列表
            string[] listUpdateList = new string[FieldMapping.Count];
            //为列表添加元素
           for (int i = 0; i < FieldMapping.Count; i++)
			{
			  //获取字段值
                listUpdateList[i]=(FieldMapping[i] + "=" + GetFieldValue(obj, FieldMapping[i], FieldValues[i]));
			}
           //集合转换成sql语句字符串
            string strUpdateText = string.Join(",", listUpdateList);
            string strSql = "UPDATE " + strTableName + " SET " + strUpdateText + " WHERE " + strWhere;

            return strSql;
       }
       /// <summary>
       /// 执行Update
       /// </summary>
       /// <param name="sql"></param>
       /// <param name="strConn"></param>
       /// <returns></returns>
       internal static bool ExecUpdate(string sql, string strConn)
       {
           if (SqlHelper.ExecuteNonQuery(strConn, CommandType.Text, sql) > 0)
           {
               return true;
           }
           else
           {
               return false;
           }
       }
       /// <summary>
       /// 创建delete语句
       /// </summary>
       /// <param name="strTableName"></param>
       /// <param name="strWhere"></param>
       /// <returns></returns>
       internal static string CreateDelete(string strTableName, string strWhere)
       {
           string strSql = "DELETE FROM " + strTableName + " WHERE " +strWhere;
           return strSql;
       }
       /// <summary>
       /// 执行delete语句
       /// </summary>
       /// <param name="sql"></param>
       /// <param name="strConn"></param>
       /// <returns></returns>
       internal static bool ExecDelete(string sql, string strConn)
       {
           if (SqlHelper.ExecuteNonQuery(strConn, CommandType.Text, sql) > 0)
           {
               return true;
           }
           else
           {
               return false;
           }
       }
       /// <summary>
       /// 根据字段或属性的类型取值
       /// </summary>
       /// <param name="type"></param>
       /// <param name="obj"></param>
       /// <returns></returns>
       internal static object[] GetProType(Type type, object obj)
       {
           //如果属性不存在，返回 null
           if (obj == null)
               return null;

           if (type == typeof(int))
           {
               return new object[] { Convert.ToInt32(CheckStr(obj,type)) };
           }
           if (type == typeof(bool))
           {
               return new object[] { Convert.ToBoolean(CheckStr(obj, type)) };
           }
          
           if (type == typeof(double))
           {
               return new object[] { Convert.ToDouble(CheckStr(obj,type)) };
           }
           if (type == typeof(double?))
           {
               return new object[] { (double?)(Convert.ToDouble(CheckStr(obj, type))) };
           }
           if (type == typeof(int?))
           {
               return new object[] { (int?)(Convert.ToInt32(CheckStr(obj,type))) };
           }
           if (type == typeof(bool?))
           {
               return new object[] { (bool?)(Convert.ToBoolean(CheckStr(obj, type))) };
           }
           if (type == typeof(DateTime?))
           {
               return new object[] { (DateTime?)(Convert.ToDateTime(CheckStr(obj,type))) };
           }
           if (type == typeof(decimal))
           {
               return new object[] { Convert.ToDecimal(CheckStr(obj, type)) };
           }
           else if (type == typeof(decimal?))
           {
               return new object[] { (decimal?)(Convert.ToDecimal(CheckStr(obj, type))) };
           }
           else if (type == typeof(string))
           {
               return new string[] { obj.ToString() };
           }
           else if (type == typeof(byte))
           {
               return new object[] { Convert.ToByte(CheckStr(obj, type)) };
           }
           else if (type == typeof(byte?))
           {
               return new object[] { (byte?)(Convert.ToByte(CheckStr(obj, type))) };
           }
           else if (type == typeof(DateTime))
           {
               return new object[] { Convert.ToDateTime(CheckStr(obj, type)) };
           }
           else
           {
               return new string[] { obj.ToString() };
           }
       }
       /// <summary>
       /// 值类型属性依据类型返回对应值
       /// </summary>
       /// <param name="obj"></param>
       /// <param name="type"></param>
       /// <returns></returns>
       internal static object CheckStr(object obj,Type type)
       {
           if (string.IsNullOrEmpty(obj.ToString()))
           {
               if (type == typeof(int?))
               {
                   return 0;
               }
               else if (type == typeof(int))
               {
                   return 0;
               }
               else if (type == typeof(bool))
               {
                   return false;
               }
               else if (type == typeof(bool?))
               {
                   return false;
               }
               else if (type == typeof(DateTime))
               {
                   return "1970/01/01";
               }
               else if (type == typeof(DateTime?))
               {
                   return "1970/01/01";
               }
               else if (type == typeof(byte))
               {
                   return 0;
               }
               else if (type == typeof(byte?))
               {
                   return 0;
               }
               else if (type == typeof(double?))
               {
                   return 0.0;
               }
               else if (type == typeof(decimal?))
               {
                   return 0;
               }
               else
               {
                   return 0;
               }
           }
           else
           {
               return obj;
           }
       }
       /// <summary>
       ///根据变量类型 格式化sql语句
       /// </summary>
       /// <param name="obj"></param>
       /// <param name="strKey"></param>
       /// <param name="FieldValue"></param>
       /// <returns></returns>
       internal static string GetFieldValue(object obj, string strKey, string FieldValue)
       {
           //获取类中的属性的类型
           PropertyInfo properInfo = obj.GetType().GetProperty(strKey);
           Type type = properInfo.PropertyType;
           //如果属性不存在，返回 null
           if (FieldValue == null)
               return "''";
           if (FieldValue == string.Empty)
               return "''";
           //根据属性的类型决定是否加“双引号”
           if ((type == typeof(int) || type == typeof(byte) || type == typeof(int?) || type == typeof(byte?)))
           {
               return FieldValue;
           }
           else if (type == typeof(string))
           {
               return "'" + FieldValue + "'";
           }
           else if (type == typeof(DateTime))
           {
               return "'" + FieldValue + "'";
           }
           else if (type == typeof(bool))
           {
               return "'" + FieldValue + "'";
           }
           else
           {
               return "''";
           }
       }
    }
}
