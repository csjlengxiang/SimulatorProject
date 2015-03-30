using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class JSDAL
    {
        public List<JS> Select(string sql)
        {
            List<JS> jss = new List<JS>();
            DataSet ds = DbHelper.Query(sql);
            foreach (DataRow dr in ds.Tables[0].Rows)
                jss.Add(LoadEntity(dr));
            return jss;
        }
        private JS LoadEntity(DataRow dr)
        {
            JS js = new JS();

            js.CH = dr["CH"].ToString();
            //js.CSSJ = dr["CSSJ"].ToString();
            js.HPH = dr["HPH"].ToString();
            js.JIARYYHM = dr["JIARYYHM"].ToString();
            js.JIERYYHM = dr["JIERYYHM"].ToString();
            js.JSSJ = dr["JSSJ"].ToString();
            //js.LSGJ = dr["LSGJ"].ToString();
            js.QSCZID = dr["QSCZID"].ToString();
            js.SH = dr["SH"].ToString();
            js.SJH = dr["SJH"].ToString();
            js.ZDCZID = dr["ZDCZID"].ToString();
            js.ZTBJ = dr["ZTBJ"].ToString();

            //js.CZID = dr["CZID"].ToString();
            //js.HYZRID = dr["HYZRID"].ToString();

            js.HQHYYID = dr["HQRYID"].ToString();

            js.SBBH = dr["SBBH"].ToString();

            js.ZXJD = dr["ZXJD"].ToString();
            js.ZXWD = dr["ZXWD"].ToString();
            js.ZXDD = dr["ZXDD"].ToString();
            js.ZXSJ = dr["ZXSJ"].ToString();
            //js.PM = dr["PM"].ToString();
            return js;
        }
        public bool Update(string sql)
        {
            int ret = 0;
            try
            {
                ret += DbHelper.ExecuteSql(sql);
            }
            catch
            {
                ret = 0;
            }
            return ret == 1;
        }
    }
}
