using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    class JSService
    {
        private JSDAL jsDal = new JSDAL();

        /// <summary>
        /// 根据锁号挑选出未销号的加锁信息，有且只有一条...
        /// </summary>
        /// <param name="sh"></param>
        /// <returns></returns>
        private JS SelectBySBBH(string sbbh)
        {
            string sql = string.Format("select * from FDSGLXT_JSJLB where sbbh='{0}'", sbbh);
            List<JS> list = jsDal.Select(sql);
            if (list.Count > 0)
                return jsDal.Select(sql)[0];
            else return null;
        }
        /// <summary>
        /// 拿最新轨迹点去更新加锁表，根据锁号来查询的
        /// </summary>
        /// <param name="gj"></param>
        /// <returns></returns>
        public bool UpdateByGJAndGetJS(GJ gj, ref string preZTBJ, ref JS js)
        {
            /*
            zxjd jd
            zxwd wd
            zxsj dwsj
            zxdy dy
            zxsd sd
            zxdd dwdd
            ztbj dwzt
            sh sh
             */

            JS js = SelectBySBBH(gj.SBBH);
            if (js == null) return false;
            
            preZTBJ = js.ZTBJ;
            
            //string sql = string.Format("update FDSGLXT_JSJLB set zxjd='{0}',zxwd='{1}',zxsj=to_date('{2}','yyyy/mm/dd hh24:mi:ss'),zxdy='{3}',zxsd='{4}',zxdd='{5}',ztbj='{6}' where sh='{7}'", gj.JD, gj.WD, gj.DWSJ, gj.DY, gj.SD, gj.DWDD, gj.DWZT, gj.SH);
            string sql = string.Format("update FDSGLXT_JSJLB set zxjd='{0}',zxwd='{1}',zxsj=to_date('{2}','yyyy/mm/dd hh24:mi:ss'),zxdd='{3}',ztbj='{4}' where sbbh='{5}'", 
                gj.JD, gj.WD, gj.DWSJ, gj.DWDD, gj.DWZT, gj.SBBH);
            
            //更新js状态
            js.ZTBJ = gj.DWZT;
            js.ZXJD = gj.JD;
            js.ZXWD = gj.WD;
            js.ZXSJ = gj.DWSJ;
            js.ZXDD = gj.DWDD;
            
            bool suc = jsDal.Update(sql);
            return suc;
        }
        /// <summary>
        /// 根据锁号销号
        /// </summary>
        /// <param name="sh"></param>
        /// <returns></returns>
        public bool XiaoHao(string sbbh)
        {
            //string sql = string.Format("update FDSGLXT_JSJLB set ztbj='{0}' where sh='{1}'", "4", sh);
            string sql = string.Format("delete FDSGLXT_JSJLB where sbbh='{0}'", sbbh);
            
            return jsDal.Update(sql);
        }
    }
}
