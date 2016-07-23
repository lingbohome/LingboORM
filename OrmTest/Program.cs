using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrmModel;

namespace OrmTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //ArticlesDal ar = new ArticlesDal();
            //var vv = ar.GetPage("DeptID", 2, 3, "DeptID DESC");
           // Articles aa = ar.GetModel("ArticID=2");
           // Console.WriteLine(aa.ArticTitle + "--" + aa.MainContent + "--" + aa.UserInfo.UserName + "--" + aa.UserInfo.Telephone);
           // Articles jj = new Articles();
           // jj.ArticTitle = "测试添加";
           //// jj.ArticType = "test";
           // jj.MainContent = "我是何为何为何为";
           // jj.Article = "撒娇坎大哈省开发的回复的撒来开发技术开发";
           // jj.AuthorNameint = "凌波";
           // ar.Edit(jj,"ArticID=2");
           // ar.Delete("ArticID=2");
            userdal bb = new userdal();
            var vv = bb.GetPage("UserID", 1, 5, "deptid=4","userid desc");
           //if (vv.Department != null)
           //{
           //    Console.WriteLine(vv.DeptID + vv.UserName + vv.UserID + vv.Department.DeptName);
           //}
           //else
           //{
           //    Console.WriteLine(vv.DeptID + vv.UserName + vv.UserID);
           //}
            //tbUser tt = new tbUser();
            //tt.varName = "xixioo";
            //tt.varPassword = "vae";
            ////tt.age=130;
            ////tt.ddt = DateTime.Now;
            ////tt.varDiscribe = "fdgdgh";
            //userdal u = new userdal();
            //if (u.Edit(tt, "intID=20"))
            //{
            //    Console.WriteLine("成功");
            //}
            ////tbUser li = u.GetModel("intID=20"); 
            ////Console.WriteLine(li.intID + "--" + li.varName+"--"+li.age+"--"+li.ddt);
            //List<tbUser> vv = u.GetList();
            foreach (var item in vv)
            {
                Console.WriteLine(item.UserName+"--"+item.Department.DeptName);
           
            }
           Console.ReadKey();
        }
    }
}
