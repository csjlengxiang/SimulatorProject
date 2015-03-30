using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class GJDAL
    {
        public bool Insert(GJ gj)
        {
            int ret = 0;
            try
            {
                //string sql = string.Format("insert into FDS_GJB(ID,SH,SJH,DWSJ,DWDD,JD,WD,DWZT,DY,SD) values('{0}','{1}','{2}',to_date('{3}','yyyy/mm/dd hh24:mi:ss'),'{4}','{5}','{6}','{7}','{8}','{9}')",
                //    gj.ID, gj.SH, gj.SJH, gj.DWSJ, gj.DWDD, gj.JD, gj.WD, gj.DWZT, gj.DY, gj.SD);

                string sql = string.Format("insert into FDS_GJB(ID,DWSJ,DWDD,JD,WD,DWZT,SH,SBBH) values('{0}',to_date('{1}','yyyy/mm/dd hh24:mi:ss'),'{2}','{3}','{4}','{5}','{6}','{7}')",
                    gj.ID, gj.DWSJ, gj.DWDD, gj.JD, gj.WD, gj.DWZT, gj.SH, gj.SBBH);

                ret = DbHelper.ExecuteSql(sql);
            }
            catch
            {
                ret = 0;
            }
            return ret == 1;
        }
        public List<GJ> Select(string sql)
        {
            List<GJ> gjs = new List<GJ>();
            DataSet ds = DbHelper.Query(sql);
            foreach (DataRow dr in ds.Tables[0].Rows)
                gjs.Add(LoadEntity(dr));
            return gjs;
        }
        private GJ LoadEntity(DataRow dr)
        {
            GJ gj = new GJ();

            //gj.ID = dr["ID"].ToString();
            //gj.SH = dr["SH"].ToString();
            //gj.SJH = dr["SJH"].ToString();
            gj.DWSJ = dr["DWSJ"].ToString();
            gj.JD = dr["JD"].ToString();
            gj.WD = dr["WD"].ToString();
            //gj.DWZT = dr["DWZT"].ToString();
            //gj.DWDD = dr["DWDD"].ToString();
            //gj.SD = dr["SD"].ToString();
            //gj.DY = dr["DY"].ToString();
            //gj.SBBH = dr["SBBH"].ToString();

            return gj;
        }
    }
}
