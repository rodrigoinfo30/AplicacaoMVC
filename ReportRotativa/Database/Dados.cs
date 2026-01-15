using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    class Dados
    {
        /// <summary>
        /// Metodo - ExecutarDataTabe
        /// </summary>
        public static DataTable ExecutarDataTable(string sql)
        {
            SqlDataAdapter da = new SqlDataAdapter(sql, Dados.Conexao());
            DataTable tabela = new DataTable();
            da.Fill(tabela);
            return tabela;
        }

        /// <summary>
        /// Metodo - CriarCommand
        /// </summary>
        public static SqlCommand CriarCommand(string sql)
        {
            SqlConnection cn = new SqlConnection(Dados.Conexao());
            SqlCommand cmd = new SqlCommand(sql, cn);
            return cmd;
        }

        /// <summary>
        /// Metodo - ExecutarNonQuery
        /// </summary>
        public static int ExecutarNonQuery(SqlCommand cmd)
        {
            int resultado = 0;
            try
            {
                cmd.Connection.Open();
                resultado = Convert.ToInt32(cmd.ExecuteNonQuery());
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro no servidor " + ex.Number);
            }
            finally
            {
                cmd.Connection.Close();
            }
            return resultado;
        }

        /// <summary>
        /// Metodo - ExecutarScalar
        /// </summary>
        public static object ExecutarScalar(SqlCommand cmd)
        {
            object resultado = 0;
            try
            {
                cmd.Connection.Open();
                resultado = cmd.ExecuteScalar();
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro no servidor " + ex.Number);
            }
            finally
            {
                cmd.Connection.Close();
            }
            return resultado;
        }

        /// <summary>
        /// Cofngiração da String de Conexção
        /// Para alterar dados de conexao, mudar parametros no web.config
        /// </summary>
        public static string Conexao()
        {
            return ConfigurationManager.AppSettings["sqlconnetion"];
        }

        public static SqlDataReader ExecutarDataReader(string sql)
        {
            SqlConnection cn = new SqlConnection(Dados.Conexao());
            cn.Open();
            SqlDataReader dr = new SqlCommand(sql, cn).ExecuteReader();
            return dr;
        }
    }
}
