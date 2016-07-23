using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrmModel
{
   public class UserInfo
    {

        public static string GetMapping()
        {
            return "UserID,false,DeptID";//如果没有标识列则返回一个""字符串
        }

        public string UserID { get; set; }
        public string UserName { get; set; }
        public Nullable<int> DeptID { get; set; }
        public string Password { get; set; }
        public string Cellphone { get; set; }
        public Nullable<byte> UserType { get; set; }
        public Department Department { get; set; }
    }
}
