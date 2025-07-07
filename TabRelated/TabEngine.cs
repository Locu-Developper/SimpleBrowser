using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoBrowser
{

    public partial class TabEngine
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Tag { get; set; }

        public TabEngine(string name, string url)
        {
            Name = name;
            Url = url;
            Tag= name;
        }
    }
    
    
}
