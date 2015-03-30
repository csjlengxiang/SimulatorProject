using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    class JSLSService
    {
        private JSLSDAL jslsDal = new JSLSDAL();

        /// <summary>
        /// 根据加锁表，和轨迹表生成的轨迹字段，合成加锁历史表
        /// </summary>
        /// <param name="js"></param>
        /// <param name="gjStr"></param>
        /// <returns></returns>
        public void Insert(JS js, string gjStr)
        {
            /*
                     public string SBBB { get; set; }
        public string CZID { get; set; }
        public string HYZRID { get; set; }
        public string HQHYYID { get; set; }
        public string PM { get; set; }
             */
            //string sql = string.Format("insert into FDSGLXT_JSJLLSB t(JLH,QSCZID,ZDCZID,JIARYYHM,JIERYYHM,SH,SJH,CH,JSSJ,CSSJ,HPH,LSGJ,SBBH,CZID,HYZRID,HQHYYID,PM) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}',to_date('{8}','yyyy/mm/dd hh24:mi:ss'),to_date('{9}','yyyy/mm/dd hh24:mi:ss'),'{10}','{11}','{12}','{13}','{14}','{15}','{16}')",
            //    Guid.NewGuid().ToString(),
            //    js.QSCZID,
            //    js.ZDCZID,
            //    js.JIARYYHM,
            //    js.JIERYYHM,
            //    js.SH,
            //    js.SJH,
            //    js.CH,
            //    js.JSSJ,
            //    js.ZXSJ, //拆锁时间为最新的时间...
            //    js.HPH,
            //    gjStr,  //历史轨迹...
            //    js.SBBH,
            //    "",
            //    "",
            //    js.HQHYYID,
            //    ""
            //    );

            try
            {
                string sql = string.Format("insert into FDSGLXT_JSJLLSB t(JLH,QSCZID,ZDCZID,JIARYYHM,JIERYYHM,SH,SJH,CH,JSSJ,CSSJ,HPH,LSGJ,SBBH,HQRYID) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}',to_date('{8}','yyyy/mm/dd hh24:mi:ss'),to_date('{9}','yyyy/mm/dd hh24:mi:ss'),'{10}','{11}','{12}','{13}')",
                    Guid.NewGuid().ToString(),
                    js.QSCZID,
                    js.ZDCZID,
                    js.JIARYYHM,
                    js.JIERYYHM,
                    js.SH,
                    js.SJH,
                    js.CH,
                    js.JSSJ,
                    js.ZXSJ, //拆锁时间为最新的时间...
                    js.HPH,
                    gjStr,  //历史轨迹...
                    js.SBBH,
                    js.HQHYYID
                    );
                jslsDal.Insert(sql);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
