using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class CZRYDAL
    {
        public List<CZRY> Select(string sql)
        {
            List<CZRY> czrys = new List<CZRY>();
            DataSet ds = DbHelper.Query(sql);
            foreach (DataRow dr in ds.Tables[0].Rows)
                czrys.Add(LoadEntity(dr));
            return czrys;
        }
        private CZRY LoadEntity(DataRow dr)
        {
            CZRY czry = new CZRY();

            czry.SJH = dr["SJH"].ToString();

            return czry;
        }
    }
}
