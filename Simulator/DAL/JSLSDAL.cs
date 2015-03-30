using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class JSLSDAL
    {
        public bool Insert(string sql)
        {
            int ret = 0;

            try
            {
                ret = DbHelper.ExecuteSql(sql);
            }
            catch
            {
                ret = 0;
            }
            return ret == 1;
        }
    }
}
