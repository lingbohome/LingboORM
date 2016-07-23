using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrmModel
{
  public  class Articles
    {
        private int articID;
        private string articType;
        private string articTitle;
        private string mainContent;
        private string article;
        private string authorNameint;

        public static string GetMapping()
        {
            return "ArticID,true,AuthorNameint";//如果没有标识列则返回一个""字符串
        }

        public int ArticID
        {
            get { return this.articID; }
            set { this.articID = value; }
        }
        public string ArticType
        {
            get { return this.articType; }
            set { this.articType = value; }
        }
        public string ArticTitle
        {
            get { return this.articTitle; }
            set { this.articTitle = value; }
        }
        public string MainContent
        {
            get { return this.mainContent; }
            set { this.mainContent = value; }
        }
        public string Article
        {
            get { return this.article; }
            set { this.article = value; }
        }
        public string AuthorNameint
        {
            get { return this.authorNameint; }
            set { this.authorNameint = value; }
        }
        public UserInfo UserInfo { get; set; }
    }
}
