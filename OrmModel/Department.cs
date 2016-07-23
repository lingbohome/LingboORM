using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrmModel
{
  public  class Department
    {
      public static string GetMapping()
      {
          return "DeptID,true,no";//如果没有标识列则返回一个""字符串
      }
        public int DeptID { get; set; }//部门ID

        public string DeptName { get; set; }//部门名称

        public string ManagerID { get; set; }//部门负责人主管ID

        public string DeptInfo { get; set; }//备注信息
    }
}
