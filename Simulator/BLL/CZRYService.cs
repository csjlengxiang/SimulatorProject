using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    class CZRYService
    {
        private CZRYDAL czryDal = new CZRYDAL();
        public string GetSJHFromID(string id)
        {
            string sql = string.Format("select SJH from FDSGLXT_CZRYGLB where ID='{0}'",id);
            List<CZRY> czrys = czryDal.Select(sql);
            if(czrys.Count > 0)
                return czrys[0].SJH;
            return "";
        }
    }
}
