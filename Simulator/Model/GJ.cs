using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class GJ
    {
        /// <summary>
        /// 0、定位 3、加锁4、破锁
        /// </summary>
        public enum gjState
        {
            js = 3,
            dw = 0,
            ps = 4
        }
        public string ID { get; set; }
        public string DWSJ { get; set; }
        public string JD { get; set; }
        public string WD { get; set; }
        public string DWZT { get; set; }
        public string DWDD { get; set; }
        public string DY { get; set; }
        public string SBBH { get; set; }
    }
}
