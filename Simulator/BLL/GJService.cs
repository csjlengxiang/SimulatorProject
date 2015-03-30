using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    class GJService
    {
        //private GJ gj = new GJ();
        private PositionService positionService = new PositionService();
        private GJDAL gjDal = new GJDAL();

        //public GJ GetGJ()
        //{
        //    return gj;
        //}
        /// <summary>
        /// 加载轨迹点
        /// </summary>
        /// <param name="str"></param>
        /// <returns>成功返回1，失败0</returns>
        public bool LoadGJ(string str, ref GJ gj)
        {
            try
            {
                string[] strs = str.Split(' ');
                gj = new GJ();
                
                //gj.ID = strs[0];
                //gj.SH = strs[1];
                //gj.SJH = strs[2];
                //gj.DY = strs[3];
                //gj.SD = strs[4];
                //gj.DWSJ = strs[5] + " " + strs[6];
                
                //此处自增长
                 
                gj.ID = Guid.NewGuid().ToString();
                
                gj.SBBH = strs[0];
                gj.DWZT = strs[1];
                gj.DWSJ = strs[2] + " " + strs[3];
                gj.JD = strs[4];
                gj.WD = strs[5];
                
                gj.DWDD = positionService.GetNear(Convert.ToDouble(gj.JD), Convert.ToDouble(gj.WD));
            }
            catch
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 插入已load的gj点,若空则是已load
        /// </summary>
        /// <returns></returns>
        public bool Insert(GJ gj)
        {
            //if(_gj == null)
            //    return gjDal.Insert(gj);
            //else 
            return gjDal.Insert(gj);
        }
        public string GetGJStr(string sbbh, string jssj)
        {
            string sql = string.Format("select t.dwsj,t.jd,t.wd from FDS_GJB t where t.sbbh='{0}' and t.dwsj >= to_date('{1}','yyyy/mm/dd hh24:mi:ss') order by t.dwsj", sbbh, jssj);
            List<GJ> gjs = gjDal.Select(sql);
            int num = gjs.Count;
            string ret = "";
            for (int i = 0; i < num; i++)
            {
                GJ gj = gjs[i];
                ret += gj.DWSJ + ',' + gj.JD + ',' + gj.WD;
                if (i != num - 1) ret += ';';
            }
            return ret;
        }

    }
}
