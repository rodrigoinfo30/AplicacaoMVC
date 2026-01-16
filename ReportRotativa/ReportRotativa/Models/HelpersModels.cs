using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace MoneyReport.Models
{
    public class HelpersModels
    {
        public static string QueryStringToParams(int pagina, System.Web.HttpRequestBase request, int table)
        {
            string queryRetorno = "";
            if (table == 1)
            {
                queryRetorno = "pagina1";
            }
            if (table == 2)
            {
                queryRetorno = "pagina";
            }

            string query = request.Params.ToString();
            Regex regex = new Regex(queryRetorno + ".*?&");
            query = regex.Replace(query, "");

            regex = new Regex("ALL_HTTP.*");
            query = regex.Replace(query, "");

            string teste = "?" + queryRetorno + "=" + pagina + "&" + query;

            return "?" + queryRetorno + "=" + pagina + "&" + query;

        }
    }
}