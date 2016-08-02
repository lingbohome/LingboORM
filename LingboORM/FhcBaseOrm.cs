using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LingboORM
{
   public class FhcBaseOrm
    {
       /// <summary>
       /// 根据映射关系获取指定对象的List集合
       /// </summary>
       /// <param name="obj"></param>
       /// <param name="conStr"></param>
       /// <param name="assemblyStr"></param>
       /// <param name="strWhere"></param>
       /// <returns></returns>
       public static List<object> GetList(object obj,string conStr, string strWhere)
       {
           //获取表名称（实体类名）
           string strTableName = FhcMapping.GetTableName(obj);
           //获取字段（实体属性）
           List<string> FieldMapping = FhcMapping.GetAllFieldMapping(obj);
           //创建SQL语句
           string strSQL = FhcSql.CreateSelectList(strTableName, FieldMapping, strWhere);
           //执行SQL
           return FhcSql.ExecSelectList(strSQL, conStr, obj, FieldMapping);
       }
       /// <summary>
       /// 根据映射关系获取指定对象的List集合的分页数数据
       /// </summary>
       /// <param name="obj"></param>
       /// <param name="conStr"></param>
       /// <param name="assemblyStr"></param>
       /// <param name="strWhere"></param>
       /// <returns></returns>
       public static List<object> GetPage(object obj, string conStr,int start,int end,string strWhere, string romNumstr,string orderById)
       {
           //获取表名称（实体类名）
           string strTableName = FhcMapping.GetTableName(obj);
           //获取字段（实体属性）
           List<string> FieldMapping = FhcMapping.GetAllFieldMapping(obj);
           //创建SQL语句
           string strSQL = FhcSql.CreateSelectPageList(strTableName, FieldMapping, start, end, strWhere, romNumstr, orderById);
           //执行SQL
           return FhcSql.ExecSelectList(strSQL, conStr, obj, FieldMapping);
       }
       /// <summary>
       /// 根据映射关系添加实体
       /// </summary>
       /// <param name="obj"></param>
       /// <param name="conStr"></param>
       /// <returns></returns>
       public static bool Add(object obj, string conStr)
       {
           //获取表名称（实体类名）
           string strTableName = FhcMapping.GetTableName(obj);
           //获取字段（实体属性）
           List<string> FieldMapping = FhcMapping.GetFieldMapping(obj);
           //获取实体属性的值
           List<string> FieldValues = FhcMapping.GetValuesFieldMapping(obj);
           //创建SQL语句
           string strSQL = FhcSql.CreateInsert(strTableName, FieldMapping, FieldValues,obj);
           //执行sql语句
           return FhcSql.ExecInsert(strSQL, conStr);
       }
        /// <summary>
        /// 根据映射关系添加实体集合（事务体添加）
        /// </summary>
        /// <param name="objList"></param>
        /// <param name="conStr"></param>
        /// <returns></returns>
        public static int AddList(List<object> objList, string conStr)
        {
            //获取表名称（实体类名）
            string strTableName = FhcMapping.GetTableName(objList[0]);
           
            //存储创建的sql语句
            List<string> commandTextList = new List<string>();
            //遍历实体对象获取实体属性的值
            foreach (var item in objList)
            {
                //获取字段（实体属性）
                List<string> FieldMapping = FhcMapping.GetFieldMapping(item);
                //获取实体属性的值
                List<string> FieldValues = FhcMapping.GetValuesFieldMapping(item);
                //创建SQL语句
                string strSQL = FhcSql.CreateInsert(strTableName, FieldMapping, FieldValues,item);
                commandTextList.Add(strSQL);
            }          
            //执行sql语句集合（事务）
            return FhcSql.ExecInsert(commandTextList, conStr);
        }
       /// <summary>
       /// 根据映射关系编辑实体
       /// </summary>
       /// <param name="obj"></param>
       /// <param name="conStr"></param>
       /// <param name="strWhere"></param>
       /// <returns></returns>
        public static bool Edit(object obj, string conStr,string strWhere)
       {
           //获取表名称（实体类名）
           string strTableName = FhcMapping.GetTableName(obj);
           //获取字段（实体属性）
           List<string> FieldMapping = FhcMapping.GetFieldMapping(obj);
           //获取实体属性的值
           List<string> FieldValues = FhcMapping.GetValuesFieldMapping(obj);
           //创建SQL语句
           string strSQL = FhcSql.CreateUpdate(strTableName, FieldMapping, FieldValues, obj, strWhere);
           //执行sql语句
           return FhcSql.ExecUpdate(strSQL, conStr);
       }
        public static int EditList(List<object> objList, string conStr, string strWhere)
        {
            //获取表名称（实体类名）
            string strTableName = FhcMapping.GetTableName(objList[0]);
            //存储创建的sql语句
            List<string> commandTextList = new List<string>();
            //遍历实体对象获取实体属性的值
            foreach (var item in objList)
            {
                //获取字段（实体属性）
                List<string> FieldMapping = FhcMapping.GetFieldMapping(item);
                //获取实体属性的值
                List<string> FieldValues = FhcMapping.GetValuesFieldMapping(item);
                //创建SQL语句
                string strSQL = FhcSql.CreateUpdate(strTableName, FieldMapping, FieldValues, item, strWhere);
                commandTextList.Add(strSQL);
            }          
          
            //执行sql语句
            return FhcSql.ExecUpdate(commandTextList, conStr);
        }
       /// <summary>
       /// 根据映射关系删除实体
       /// </summary>
       /// <param name="obj"></param>
       /// <param name="conStr"></param>
       /// <param name="strWhere"></param>
       /// <returns></returns>
       public static bool Delete(object obj, string conStr, string strWhere)
       {
           //获取表名称（实体类名）
           string strTableName = FhcMapping.GetTableName(obj);
           //创建SQL语句
           string strSQL = FhcSql.CreateDelete(strTableName,  strWhere);
           //执行sql语句
           return FhcSql.ExecDelete(strSQL, conStr);
       }
    }
}
