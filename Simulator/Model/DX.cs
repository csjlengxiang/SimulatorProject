using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class DX
    {
        public static enum dxlx
        {
            js = 1,
            ps = 2
        }
        public string ID { get; set; }
        public string JSRID { get; set; }
        public string FSSJ { get; set; }
        public string FSNR { get; set; }
        public string FSLX { get; set; }
    }
}
