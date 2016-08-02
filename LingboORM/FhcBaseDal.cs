using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LingboORM
{
   public class FhcBaseDal<T> where T:class,new()
    {
       //定义连接字符串，实体类层程序集
       string conStr;//, assemblyStr;
       //初始化连接字符串，实体类层程序集
       public FhcBaseDal()
        {

            conStr = ConfigurationManager.ConnectionStrings["LingboConn"].ToString();

           // assemblyStr = ConfigurationManager.AppSettings["Model"];
        }
       /// <summary>
       /// 根据条件查询表的数据集合并返回List集合
       /// </summary>
       /// <param name="strWhere"></param>
       /// <returns></returns>
       public List<T> GetList(string strWhere = "1=1")
       {
           List<T> list = new List<T>();

           List<object> tempList = FhcBaseOrm.GetList(new T(), conStr, strWhere);
           foreach (object tempobject in tempList)
           {
               list.Add((T)tempobject);
           }
           return list;
       }
       /// <summary>
       /// 分页获取相应集合数据
       /// </summary>
       /// <param name="romNumstr"></param>
       /// <param name="strWhere"></param>
       /// <returns></returns>
       public List<T> GetPage(string romNumstr, int pageIndex, int pageSize,string strWhere="1=1",string orderById="")
       {
           List<T> list = new List<T>();
           int start = (pageIndex - 1) * pageSize + 1;
           int end = pageIndex * pageSize;
           if (orderById != string.Empty)
           {
               orderById = " order by " + orderById;
           }
           List<object> tempList = FhcBaseOrm.GetPage(new T(), conStr, start, end, strWhere, romNumstr, orderById);
           foreach (object tempobject in tempList)
           {
               list.Add((T)tempobject);
           }
           return list;
       }
       /// <summary>
       /// 根据条件查询表的一条数据并返回该表映射的对象
       /// </summary>
       /// <param name="strWhere"></param>
       /// <returns></returns>
       public T GetModel(string strWhere = "1=1")
       {
           return  (T)FhcBaseOrm.GetList(new T(), conStr, strWhere).FirstOrDefault();
       }
       /// <summary>
       /// 添加实体
       /// </summary>
       /// <param name="t"></param>
       /// <returns></returns>
       public bool Add(T t)
       {

           if (FhcBaseOrm.Add(t, conStr))
           {
               return true;
           }
           else
           {
               return false;
           }
       }
       /// <summary>
       /// 事务批量添加实体
       /// </summary>
       /// <param name="tList"></param>
       /// <returns></returns>
        public int Add(List<T> tList)
        {
            List<object> objList = null;
            if (tList.Count > 0)
            {
                objList = new List<object>();
                foreach (var item in tList)
                {
                    objList.Add(item);
                }
                return FhcBaseOrm.AddList(objList, conStr);
            }
            return 0;  
        }
        /// <summary>
        /// 编辑实体
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Edit(T t, string strWhere="")
       {
           if (FhcBaseOrm.Edit(t, conStr, strWhere))
           {
               return true;
           }
           else
           {
               return false;
           }
       }
       /// <summary>
        /// 事务批量编辑实体
       /// </summary>
       /// <param name="tList"></param>
       /// <returns></returns>
        public int Edit(List<T> tList)
        {
            List<object> objList = null;
            if (tList.Count > 0)
            {
                objList = new List<object>();
                foreach (var item in tList)
                {
                    objList.Add(item);
                }
                return FhcBaseOrm.EditList(objList, conStr,"");
            }
            return 0;  
        }
       /// <summary>
       /// 删除实体
       /// </summary>
       /// <param name="t"></param>
       /// <param name="strWhere"></param>
       /// <returns></returns>
       public bool Delete(string strWhere)
       {
           if (FhcBaseOrm.Delete(new T(), conStr, strWhere))
           {
               return true;
           }
           else
           {
               return false;
           }
       }

    }
}
