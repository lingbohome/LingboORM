using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrmModel
{
    public class tbUser
    {
        /// <summary>
        /// 标识该实体类映射的表是否有标识列
        /// </summary>
        public static string GetIdentity()
        {
            return "intID";//如果没有标识列则返回一个""字符串
        }
       // private int? _age;
        
       //public int age {
       //    get { return (int)(_age ??1415926535898); }
       //    set { _age = value; }
       //}
        public int? age { get; set; }
        public int intID { get; set; }
        public string varName { get; set; }
        public string varDiscribe { get; set; }
        public string varPassword { get; set; }
        public DateTime? ddt { get; set; }
    }
}
