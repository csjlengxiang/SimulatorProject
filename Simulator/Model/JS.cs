using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class JS
    {
        public enum jsState
        {
            yjs = 0,
            js = 1,
            dw = 2,
            ps = 3,
            cs = 4,
            xh = 5
        }


        public string QSCZID { get; set; }
        public string ZDCZID { get; set; }
        public string JIARYYHM { get; set; }
        public string JIERYYHM { get; set; }
        public string SH { get; set; }
        public string SJH { get; set; }
        public string CH { get; set; }
        public string JSSJ { get; set; }
        public string CSSJ { get; set; }
        public string ZTBJ { get; set; }
        public string HPH { get; set; }
        public string YJSPCH { get; set; }
 
        #region 新增车长、货运主任、后勤货运员ID、品名
        public string CZID { get; set; }
        public string HYZRID { get; set; }
        public string HQHYYID { get; set; }

        public string SBBH { get; set; }

        public string ZXJD { get; set; }
        public string ZXWD { get; set; }
        public string ZXDY { get; set; }
        public string ZXDD { get; set; }
        
        public string ZXSJ { get; set; }
        public string PM { get; set; }


        #endregion 
    }
}
