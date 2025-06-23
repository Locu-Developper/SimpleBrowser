using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoBrowser
{
    class TabNode
    {
        public TabNode(string url = "https://yahoo.co.jp")
        {
            Url = url;
        }

        private string url;
        public string Url
        {
            get { return url; }
            set { url = value; }
        }
    }
}
