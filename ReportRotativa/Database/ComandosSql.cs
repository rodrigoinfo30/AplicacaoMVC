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
    public class ComandosSql
    {
        //comando tela login e recuperar senha início
        private GlobalFields.Fields fields = new GlobalFields.Fields();
        public int VerificaSenha(string usuario, int usuemp_id, string senha)
        {
            int Usuemp;

            if (usuemp_id == 10999)
            {
                Usuemp = 0;
            }
            else
            {
                Usuemp = usuemp_id;
            }

            try
            {
                string query = @"SELECT PWDCOMPARE(('" + usuario + "' + '" + senha + "' + CONVERT(VARCHAR,(" + usuemp_id + "))), SENHA_WEB, 0) FROM USUARIOS WHERE APELIDO = '" + usuario + "' AND usuemp_id = " + Usuemp + "";
                SqlCommand cmd = Dados.CriarCommand(query);
                int retorno = Convert.ToInt32(Dados.ExecutarScalar(cmd));
                return retorno;
            }
            catch (SqlException ex)
            {
                throw new Exception("ERRO: " + ex.Number + " # " + ex.Message + " # " + ex.Procedure);
            }
        }

        public DataTable ConsultaEmpresa(int usuemp_id)
        {
            if (usuemp_id == 10999)
            {
                usuemp_id = 0;
            }
            string query = @"SELECT CONTRATO_ID FROM CONTRATO WHERE CONTRATO_ID=" + usuemp_id + "";
            return Dados.ExecutarDataTable(query);
        }

        public DataTable ConsultaEmpresaUsuario(int usuemp_id, string usuario)
        {
            if (usuemp_id == 10999)
            {
                usuemp_id = 0;
            }
            string query = @"SELECT APELIDO FROM USUARIOS U INNER JOIN CONTRATO C ON (U.USUEMP_ID=C.CONTRATO_ID)WHERE CONTRATO_ID=" + usuemp_id + " AND APELIDO='" + usuario + "'";
            return Dados.ExecutarDataTable(query);
        }

        public string VerificaHabilitado(string Apelido, int Usuemp_ID)
        {
            int Usuemp;

            if (Usuemp_ID == 10999)
            {
                Usuemp = 0;
            }
            else
            {
                Usuemp = Usuemp_ID;
            }
            string query = @"SELECT HABILITADO FROM USUARIOS WHERE APELIDO = '" + Apelido + "' AND usuemp_id = " + Usuemp;
            SqlCommand cmd = Dados.CriarCommand(query);
            string retorno = Dados.ExecutarScalar(cmd).ToString();
            return retorno;
        }

        public int VerificaUltLogin(int Usuemp_ID, string Apelido)
        {
            string query = @"SELECT DATEDIFF(day,ULTLOGIN_WEB,GETDATE()) as DIAS
                             FROM USUARIOS
                             WHERE usuemp_id = " + Usuemp_ID + " AND APELIDO =  '" + Apelido + "'";
            SqlCommand cmd = Dados.CriarCommand(query);
            int retorno = Convert.ToInt32(Dados.ExecutarScalar(cmd));
            return retorno;
        }

        public DataTable PegarInfosUsuario(int Usuemp_ID, string Apelido)
        {
            string query;

            int Usuemp;

            if (Usuemp_ID == 10999)
            {
                Usuemp = 0;
            }
            else
            {
                Usuemp = Usuemp_ID;
            }

            if (Usuemp == 0)
            {
                query = @"SELECT NOM_FANTASIA FROM AACEDEN0 WHERE COD_CEDENTE=9999";
            }
            else
            {
                query = @"SELECT U.USUEMP_ID, U.NOME, U.APELIDO, (USUEMP_ID - 1000) AS COD_CEDENTE, GRUPO, A.NOM_FANTASIA FROM USUARIOS U, AACEDEN0 A WHERE A.COD_CEDENTE = (U.USUEMP_ID - 1000) AND U.USUEMP_ID = " + Usuemp_ID + " AND U.APELIDO = '" + Apelido + "'";
            }
            return Dados.ExecutarDataTable(query);
        }

        public DataTable ListaCedentesNaWeb(int porPagina, int paginaCorrente, string cedWeb)
        {
            string buscaCedente = "";
            if (cedWeb != "")
            {
                buscaCedente = " AND NOM_FANTASIA LIKE '" + cedWeb + "%'";
            }
            var offset = 1;
            if (paginaCorrente > 1)
            {
                offset = (porPagina * (paginaCorrente - 1)) + 1;
            }
            string queryString = @"DECLARE @LIMIT INT = " + porPagina + ", @OFFSET INT = " + offset + " WITH RESULTADO AS (SELECT COD_CEDENTE,NOM_FANTASIA, ROW_NUMBER() OVER (ORDER BY NOM_FANTASIA) AS LINHA FROM AACEDEN0 WHERE STS_ATIVO = 'S' AND COD_CEDENTE IN(SELECT COD_CEDENTE FROM CONTRATO) " + buscaCedente + " ) SELECT * FROM RESULTADO WHERE LINHA >= @OFFSET AND LINHA < @OFFSET + @LIMIT ORDER BY NOM_FANTASIA";



            return Dados.ExecutarDataTable(queryString);
        }

        public DataTable ListaCedentesForaWeb(int porPagina, int paginaCorrente, string cedForaWeb)
        {
            string buscaCedente = "";
            if (cedForaWeb != "")
            {
                buscaCedente = " AND NOM_FANTASIA LIKE '" + cedForaWeb + "%'";
            }
            var offset = 1;
            if (paginaCorrente > 1)
            {
                offset = (porPagina * (paginaCorrente - 1)) + 1;
            }
            string queryString = @"DECLARE @LIMIT INT = " + porPagina + ", @OFFSET INT = " + offset + " WITH RESULTADO AS (SELECT COD_CEDENTE,NOM_FANTASIA, ROW_NUMBER() OVER (ORDER BY NOM_FANTASIA) AS LINHA FROM AACEDEN0 WHERE STS_ATIVO = 'S' AND COD_CEDENTE NOT IN(SELECT COD_CEDENTE FROM CONTRATO) " + buscaCedente + " ) SELECT * FROM RESULTADO WHERE LINHA >= @OFFSET AND LINHA < @OFFSET + @LIMIT ORDER BY NOM_FANTASIA";

            return Dados.ExecutarDataTable(queryString);
        }

        public DataTable IncluirCedente(int CodCedente)
        {
            string sqlInsert = "";
            sqlInsert = "INSERT CONTRATO (CONTRATO_ID,COD_CEDENTE,COD_CED_WEB,STS_CNAB) ";
            sqlInsert += "SELECT COD_CEDENTE+1000,COD_CEDENTE,COD_CEDENTE+1000,'N' FROM AACEDEN0 WHERE COD_CEDENTE IN (" + CodCedente + ")";

            return Dados.ExecutarDataTable(sqlInsert);
        }

        public DataTable ExcluirCedente(int CodCedente)
        {
            string sqlInsert = "";
            sqlInsert = "DELETE CONTRATO ";
            sqlInsert += "WHERE COD_CEDENTE =" + CodCedente;

            return Dados.ExecutarDataTable(sqlInsert);
        }



        public void MudaParaProvisorio(int Usuemp_ID, string Apelido)
        {
            if (Usuemp_ID == 10999)
            {
                Usuemp_ID = 0;
            }

            string queryUp = @"UPDATE USUARIOS SET HABILITADO='P' WHERE APELIDO='" + Apelido + "' AND USUEMP_ID=" + Usuemp_ID + "";

            SqlCommand cmdUp = Dados.CriarCommand(queryUp);
            Dados.ExecutarNonQuery(cmdUp);
        }

        public void AtualizaUltLogin(int Usuemp_ID, string Apelido)
        {
            string queryUp = @"UPDATE USUARIOS SET ULTLOGIN_WEB = GETDATE() WHERE APELIDO ='" + Apelido + "' AND USUEMP_ID = " + Usuemp_ID + "";

            SqlCommand cmdUp = Dados.CriarCommand(queryUp);
            Dados.ExecutarNonQuery(cmdUp);
        }

        public int AtualizandoSenhaAtual(string usuario, int usuemp_id, string confsenha, string email)
        {
            int iRetorno = 0;

            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cn.ConnectionString = Dados.Conexao();
                cmd.Connection = cn;
                cmd.CommandText = "P_CRYPTO";
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter Param;

                cmd.Parameters.Add("@APELIDO", SqlDbType.VarChar, 100);
                cmd.Parameters["@APELIDO"].Value = usuario;

                cmd.Parameters.Add("@usuemp_id", SqlDbType.Int);
                cmd.Parameters["@usuemp_id"].Value = usuemp_id;

                cmd.Parameters.Add("@SENHA", SqlDbType.VarChar, 255);
                cmd.Parameters["@SENHA"].Value = confsenha;

                cmd.Parameters.Add("@EMAIL", SqlDbType.VarChar, 255);
                cmd.Parameters["@EMAIL"].Value = email;

                cmd.Parameters.Add("@ALTERAR", SqlDbType.Int);
                cmd.Parameters["@ALTERAR"].Value = 1;

                Param = new SqlParameter("@RETORNO", SqlDbType.Int, 1);
                Param.Direction = ParameterDirection.Output; // ReturnValue;
                cmd.Parameters.Add(Param);

                cn.Open();
                cmd.ExecuteNonQuery();

                iRetorno = Convert.ToInt32(cmd.Parameters["@RETORNO"].Value.ToString());

                return iRetorno;
            }
            catch (SqlException ex)
            {
                throw new Exception("ERRO: " + ex.Number + " # " + ex.Message + " # " + ex.Procedure);
            }
            finally
            {
                cn.Close();
            }
        }

        public DataTable BuscaBancos()
        {
            string query = @"SELECT (CONVERT(VARCHAR,COD_BANCO) + ' - ' +  NOM_BANCO) AS 'BANCOS', COD_BANCO FROM GABANCO0 WHERE COD_BANCO < 800 ORDER BY COD_BANCO";
            return Dados.ExecutarDataTable(query);
        }

        public DataTable ConsultaEmpresaUsuarioEmail(int codigo, string usuario, string email)
        {
            if (codigo == 10999)
            {
                codigo = 0;
            }

            string query = @"SELECT EMAIL FROM USUARIOS U INNER JOIN CONTRATO C ON (U.USUEMP_ID=C.CONTRATO_ID)WHERE CONTRATO_ID=" + codigo + " AND APELIDO='" + usuario + "' AND EMAIL='" + email + "'";
            return Dados.ExecutarDataTable(query);
        }

        //Comando para a view login fim

        //Comando para a view menu inicio
        public DataTable ListaMenu(string usuario, string usuemp_id)
        {
            if (usuemp_id == "10999")
            {
                usuemp_id = "0";
            }
            string queryString = @"SELECT 
	                                M.Menu, Descricao, M.PaiId, Icone, M.Habilitado, Acao
                                FROM 
	                                DIREITOS D INNER JOIN USUARIOS U ON (D.ID_USUEMP=U.USUEMP_ID), MENU M
                                WHERE	                                
	                                (D.APELIDO = U.APELIDO) AND 
	                                M.PAIID = D.PAIID AND 
	                                M.MENU = D.MENU AND 
	                                M.Menu > 2000 AND 
	                                U.HABILITADO <> 'P' AND D.ID_USUEMP = " + usuemp_id + " AND U.APELIDO = '" + usuario + "' order by m.PAIID";

            return Dados.ExecutarDataTable(queryString);
        }

        public DataTable ListaUsuraio(int usuemp_id)
        {
            if (usuemp_id == 10999)
            {
                usuemp_id = 0;
            }
            string queryString = @"SELECT USUEMP_ID,APELIDO,LEFT(NOM_CLIENTE,22) AS NOM_CLIENTE,(USUEMP_ID - 1000) AS COD_CEDENTE FROM USUARIOS, AAPARAM0 WHERE USUEMP_ID =" + usuemp_id;

            return Dados.ExecutarDataTable(queryString);
        }


        public DataTable ListaTitProp()
        {
            string queryString = @"SELECT 
	                                A.COD_CEDENTE,A.NUM_LOTE, C.NUM_CGC_CPF,C.NOM_RAZAO,NUM_DOCUMENTO,VLR_TITULO,DAT_VENCTO 
                                FROM 
                                    AALOTIN0 A INNER JOIN AALITIN0 B ON(A.NUM_LOTE = B.NUM_LOTE)
	                                INNER JOIN AASACAD0 C ON(B.NUM_CGC_CPF = C.NUM_CGC_CPF)
	                                INNER JOIN AACEDEN0 D ON(A.COD_CEDENTE = D.COD_CEDENTE)
                                WHERE 
	                                STS_ANALISE = 'N' AND A.NUM_LOTE = 4067
                                ORDER BY 
	                                A.RECNUM DESC";

            return Dados.ExecutarDataTable(queryString);
        }
        //Comando para a view menu fim

        //Comando para a view proposta início
        public string GeraNovoNumLote()
        {
            string queryUp = @"UPDATE AAPARAM0 SET ULT_LOTEINT = (ULT_LOTEINT + 1)";
            SqlCommand cmdUp = Dados.CriarCommand(queryUp);
            Dados.ExecutarNonQuery(cmdUp);

            string query = @"SELECT ULT_LOTEINT FROM AAPARAM0";
            SqlCommand cmd = Dados.CriarCommand(query);
            string retorno = Dados.ExecutarScalar(cmd).ToString();
            return retorno;
        }

        public DataTable BuscaSacado(string cnpj)
        {
            string query = @"SELECT COD_SACADO, NUM_CGC_CPF, NOM_RAZAO, DES_ENDERECO, DES_BAIRRO, DES_CIDADE, DES_ESTADO
                             FROM AASACAD0
                             WHERE NUM_CGC_CPF = '" + cnpj + "'";

            return Dados.ExecutarDataTable(query);
        }

        public DataTable BuscarProposta(string cedente, int Filtro)
        {
            if (cedente == "10999")
            {
                cedente = "0";
            }
            string query = "";

            if (Filtro == 0)
            {
                query = @"SELECT NUM_LOTE, CONVERT (CHAR(10), DAT_PROP, 103) AS DATA,VLR_TITULOS, QTD_TITULOS, 	
                            CASE WHEN STS_ANALISE = 'M' THEN 'ABERTO' 
	                            ELSE
		                            CASE WHEN STS_ANALISE = 'F' THEN 'EFETIVADO'
		                            ELSE
			                            CASE WHEN STS_ANALISE = 'N' THEN 'FINALIZADO/ANÁLISE'
			                            END
		                            END
	                            END AS 'STATUS'
                            FROM AALOTIN0 WHERE (COD_CEDENTE = " + cedente + ") AND (TIPO_OPERACAO <> 'P') and qtd_titulos >0 ORDER BY NUM_LOTE DESC ";
            }
            else
            {
                if (Filtro == 1)
                {
                    query = @"SELECT NUM_LOTE, CONVERT (CHAR(10), DAT_PROP, 103) AS DATA,VLR_TITULOS, QTD_TITULOS, 	
                                CASE WHEN STS_ANALISE = 'M' THEN 'ABERTO' 
	                                ELSE
		                                CASE WHEN STS_ANALISE = 'F' THEN 'EFETIVADO'
		                                ELSE
			                                CASE WHEN STS_ANALISE = 'N' THEN 'FINALIZADO/ANÁLISE'
			                                END
		                                END
	                                END AS 'STATUS'
                                FROM AALOTIN0 WHERE AALOTIN0 WHERE (COD_CEDENTE = " + cedente + ") AND (TIPO_OPERACAO <> 'P') and qtd_titulos >0 and sts_analise in ('W','M','X') ORDER BY NUM_LOTE DESC ";
                }
                else
                {
                    if (Filtro == 2)
                    {
                        query = @"SELECT NUM_LOTE, CONVERT (CHAR(10), DAT_PROP, 103) AS DATA,VLR_TITULOS, QTD_TITULOS, 	
                                    CASE WHEN STS_ANALISE = 'M' THEN 'ABERTO' 
	                                    ELSE
		                                    CASE WHEN STS_ANALISE = 'F' THEN 'EFETIVADO'
		                                    ELSE
			                                    CASE WHEN STS_ANALISE = 'N' THEN 'FINALIZADO/ANÁLISE'
			                                    END
		                                    END
	                                    END AS 'STATUS'
                                    FROM AALOTIN0 WHERE (COD_CEDENTE = " + cedente + ") AND (TIPO_OPERACAO <> 'P') and qtd_titulos >0 and sts_analise not in ('W','M','X') ORDER BY NUM_LOTE DESC ";
                    }
                }
            }
            return Dados.ExecutarDataTable(query);
        }

        public void Incluir(GlobalFields.Fields fields)
        {
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cn.ConnectionString = Dados.Conexao();
                cmd.Connection = cn;
                cmd.CommandText = "NET_SACADOS";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ACAO", "ADD");
                cmd.Parameters.AddWithValue("@RECNUM", 0);
                cmd.Parameters.AddWithValue("@COD_SACADO", fields.COD_SAC);
                cmd.Parameters.AddWithValue("@NUM_CGC_CPF", fields.NUM_CNPJ_CPF_SAC);
                cmd.Parameters.AddWithValue("@NOM_RAZAO", fields.NOM_RAZAO_SAC);
                //DADOS COBRANÇA
                cmd.Parameters.AddWithValue("@DES_END_COB", fields.DES_END_SAC);
                cmd.Parameters.AddWithValue("@DES_BAI_COB", fields.DES_BAIRRO_SAC);
                cmd.Parameters.AddWithValue("@DES_CID_COB", fields.DES_CID_SAC);
                cmd.Parameters.AddWithValue("@DES_EST_COB", fields.EST_SAC);
                cmd.Parameters.AddWithValue("@NUM_CEP_COB", fields.CEP_SAC);
                //DADOS ENDEREÇO
                cmd.Parameters.AddWithValue("@DES_ENDERECO", fields.DES_END_SAC);
                cmd.Parameters.AddWithValue("@DES_BAIRRO", fields.DES_BAIRRO_SAC);
                cmd.Parameters.AddWithValue("@DES_CIDADE", fields.DES_CID_SAC);
                cmd.Parameters.AddWithValue("@DES_ESTADO", fields.EST_SAC);
                cmd.Parameters.AddWithValue("@NUM_CEP", fields.CEP_SAC);
                cmd.Parameters.AddWithValue("@STS_SACADO_NOVO", "N");
                cmd.Parameters.AddWithValue("@STS_CHEQUE_DEV", "N");
                cmd.Parameters.AddWithValue("@STS_DUP_PROT", "N");
                cmd.Parameters.AddWithValue("@STS_NEGATIVADO", "N");
                cmd.Parameters.AddWithValue("@STS_ANAL_TITULO", "S");
                cmd.Parameters.AddWithValue("@STS_ANAL_CANHOT", "S");
                cmd.Parameters.AddWithValue("@NUM_DDD", fields.DDD_SAC);
                cmd.Parameters.AddWithValue("@NUM_TELEFONE", fields.TEL_SAC);
                cmd.Parameters.AddWithValue("@NUM_DDD2", fields.DDD_SAC);
                cmd.Parameters.AddWithValue("@NUM_TELEFONE2", fields.TEL_SAC);
                cmd.Parameters.AddWithValue("@NUM_IE", fields.INSC_ESTADUAL_SAC);


                cn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                // TRATO ERROS MAIS COMUNS PARA MOSTRAR MENSAGENS AMIGAVEIS AO USUARIO
                // 2627 PRIMARY KEY duplicado
                if (ex.Number == 2627)
                {
                    throw new Exception("Este SACADO já foi cadastrado anteriormente!");
                }
                else
                {
                    throw new Exception("Erro no número " + ex.Number + " - " + ex.Message + " - " + ex.Procedure);
                }
            }
            finally
            {
                cn.Close();
            }
        }

        public string GeraNovoNumSacado()
        {
            // RECUPERO O NOVO NUM DE LOTE
            string queryUp = @"UPDATE AAPARAM0 SET NUM_ULT_SACADO = (NUM_ULT_SACADO + 1)";
            SqlCommand cmdUp = Dados.CriarCommand(queryUp);
            Dados.ExecutarNonQuery(cmdUp);

            string query = @"SELECT NUM_ULT_SACADO FROM AAPARAM0";
            SqlCommand cmd = Dados.CriarCommand(query);
            string retorno = Dados.ExecutarScalar(cmd).ToString();
            return retorno;
        }

        public void RemoverTitulo(GlobalFields.Fields fields)
        {
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cn.ConnectionString = Dados.Conexao();
                cmd.Connection = cn;
                cmd.CommandText = "NET_PROPOSTAS";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ACAO", "DEL_TITULO");
                cmd.Parameters.AddWithValue("@RECNUM", fields.RECNUM_AALITIN0);
                cmd.Parameters.AddWithValue("@NUM_LOTE", fields.NUM_LOTE);

                cn.Open();
                cmd.ExecuteNonQuery();

            }
            catch (SqlException ex)
            {
                throw new Exception("ERRO: " + ex.Number + " # " + ex.Message + " # " + ex.Procedure);
            }
            finally
            {
                cn.Close();
            }
        }

        public DataTable Status_Lote(int Lote)
        {
            string query = @"SELECT STS_ANALISE FROM AALOTIN0 WHERE NUM_LOTE = " + Lote.ToString();

            return Dados.ExecutarDataTable(query);
        }

        public DataTable ListaCedente(string cedente)
        {
            string query = @"SELECT EXIGE_CMC7 FROM AACEDEN0 WHERE COD_CEDENTE = " + cedente;

            return Dados.ExecutarDataTable(query);
        }

        public void CriarProposta(GlobalFields.Fields fields)
        {
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {

                cn.ConnectionString = Dados.Conexao();
                cmd.Connection = cn;
                cmd.CommandText = "NET_PROPOSTAS";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ACAO", "ADD_PROPOSTA");
                cmd.Parameters.AddWithValue("@NUM_LOTE", fields.NUM_LOTE);
                cmd.Parameters.AddWithValue("@COD_CEDENTE", fields.COD_CEDENTE);
                cmd.Parameters.AddWithValue("@VLR_TITULOS", fields.VLR_TITULOS);
                cmd.Parameters.AddWithValue("@NOM_CEDENTE", fields.NOM_CEDENTE);
                cmd.Parameters.AddWithValue("@QTD_TITULOS", fields.QTD_TITULOS);
                cmd.Parameters.AddWithValue("@STS_ANALISE", fields.STS_ANALISE);
                cmd.Parameters.AddWithValue("@STS_CONCENTRA", fields.STS_CONCENTRA);
                cmd.Parameters.AddWithValue("@STS_PRAZO_MIN", fields.STS_PRAZO_MIN);
                cmd.Parameters.AddWithValue("@STS_PRAZO_MAX", fields.STS_PRAZO_MAX);
                cmd.Parameters.AddWithValue("@TIPO_OPERACAO", fields.TIPO_OPERACAO);



                cn.Open();
                cmd.ExecuteNonQuery();

            }
            catch (SqlException ex)
            {
                throw new Exception("ERRO (PropostaDAL): " + ex.Number + " # " + ex.Message + " # " + ex.Procedure);
            }
            finally
            {
                cn.Close();
            }
        }

        public DataTable AtualizarGridTitulos(int inumlote)
        {
            string query = @"SELECT L.RECNUM, L.NUM_CGC_CPF AS 'CNPJ\CPF', A.NOM_RAZAO AS 'NOME SACADO',
	                            NUM_DOCUMENTO AS 'DOCUMENTO', VLR_TITULO AS 'VLR. TÍTULO', CONVERT (CHAR(10), DAT_VENCTO, 103) AS 'VENCIMENTO'
                           FROM AALITIN0 L INNER JOIN AASACAD0 A ON (L.NUM_CGC_CPF=A.NUM_CGC_CPF)
                           WHERE NUM_LOTE = " + inumlote;
            return Dados.ExecutarDataTable(query);
        }

        public void IncluirTtitulo(GlobalFields.Fields proposta)
        {
            string sCod_documento, sSts_Dmais, sNum_Dmais_Ced, sNum_Dmais_Param, sSts_Doc;

            SqlConnection conexao = new SqlConnection();
            conexao.ConnectionString = Dados.Conexao();
            SqlCommand comando = new SqlCommand();
            comando.Connection = conexao;
            comando.CommandText = @"SELECT AACEDEN0.COD_DOCUMENTO, STS_D_MAIS, AACEDEN0.NUM_D_MAIS,AAPARAM0.NUM_D_MAIS,AATPDOC0.STS_DOC
                                        FROM AACEDEN0 INNER JOIN AATPDOC0 ON (AACEDEN0.COD_DOCUMENTO=AATPDOC0.COD_DOCUMENTO),AAPARAM0
                                        WHERE COD_CEDENTE = " + proposta.COD_CEDENTE;
            conexao.Open();
            comando.CommandType = CommandType.Text;
            SqlDataReader dr = comando.ExecuteReader();
            dr.Read();
            sCod_documento = dr.GetValue(0).ToString();
            sSts_Dmais = dr.GetValue(1).ToString();
            sNum_Dmais_Ced = dr.GetValue(2).ToString();
            sNum_Dmais_Param = dr.GetValue(3).ToString();
            sSts_Doc = dr.GetValue(4).ToString();
            dr.Close();
            conexao.Close();

            //Verificando D+
            if (sSts_Dmais == "S")
            {
                proposta.NUM_DMAIS = Convert.ToInt32(sNum_Dmais_Ced);
            }
            else
            {
                proposta.NUM_DMAIS = Convert.ToInt32(sNum_Dmais_Param);
            }

            //Verificando Tipo de documento
            if (sSts_Doc == proposta.TIP_DOC)
            {
                proposta.TIP_DOC = sCod_documento;
            }

            //Incluindo o Título
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cn.ConnectionString = Dados.Conexao();
                cmd.Connection = cn;
                cmd.CommandText = "NET_PROPOSTAS";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ACAO", "ADD_TITULO");
                cmd.Parameters.AddWithValue("@NUM_LOTE", proposta.NUM_LOTE);
                cmd.Parameters.AddWithValue("@COD_CEDENTE", proposta.COD_CEDENTE);
                cmd.Parameters.AddWithValue("@NUM_CGC_CPF", proposta.NUM_CGC_CPF);
                cmd.Parameters.AddWithValue("@NUM_DOCUMENTO", proposta.NUM_DOCUMENTO);
                cmd.Parameters.AddWithValue("@NUM_NOTA_FISCAL", proposta.NUM_NOTA_FISCAL);
                cmd.Parameters.AddWithValue("@STS_DOC", proposta.TIP_DOC);
                cmd.Parameters.AddWithValue("@DAT_VENCTO", proposta.DAT_VENCTO);
                cmd.Parameters.AddWithValue("@VLR_TITULO", proposta.VLR_TITULO);
                cmd.Parameters.AddWithValue("@NUM_DMAIS", proposta.NUM_DMAIS);
                cmd.Parameters.AddWithValue("@COD_RECUSA", proposta.COD_RECUSA);
                cmd.Parameters.AddWithValue("@STS_RECUSA", proposta.STS_RECUSA);
                cmd.Parameters.AddWithValue("@STS_AVISO", proposta.STS_AVISO);
                cmd.Parameters.AddWithValue("@STS_BCO_CART", proposta.STS_BCO_CART);
                cmd.Parameters.AddWithValue("@DAT_EMISSAO", proposta.DAT_EMISSAO);
                cmd.Parameters.AddWithValue("@COD_BANCO_CHQ", proposta.COD_BANCO_CHQ);
                cmd.Parameters.AddWithValue("@COD_CMC7", proposta.CMC7);
                cmd.Parameters.AddWithValue("@NUM_IE", proposta.INSC_ESTADUAL);

                cn.Open();
                cmd.ExecuteNonQuery();

            }
            catch (SqlException ex)
            {
                throw new Exception("ERRO: " + ex.Number + " # " + ex.Message + " # " + ex.Procedure);
            }
            finally
            {
                cn.Close();
            }
        }

        public DataTable AtualizarInfosProp(GlobalFields.Fields proposta)
        {
            string query = @"SELECT 
                              SUM(AALITIN0.VLR_TITULO) AS TOTAL,
                              COUNT(AALITIN0.NUM_LOTE) AS QTDTITULOS,
                              AALOTIN0.DAT_PROP AS DATPROP,
                              AALOTIN0.NUM_LOTE AS LOTE  
                            FROM
                              AALITIN0
                              INNER JOIN dbo.AALOTIN0 ON (AALITIN0.NUM_LOTE = dbo.AALOTIN0.NUM_LOTE)
                            WHERE
                              AALITIN0.NUM_LOTE = " + proposta.NUM_LOTE + " GROUP BY AALOTIN0.DAT_PROP, AALOTIN0.NUM_LOTE";
            return Dados.ExecutarDataTable(query);
        }

        public DataTable ListaTitulos(int inumlote)
        {
            string query = @"SELECT L.RECNUM, L.NUM_CGC_CPF AS 'CNPJ_CPF', A.NOM_RAZAO AS 'NOME_SACADO',
	                            NUM_DOCUMENTO AS 'DOCUMENTO', VLR_TITULO AS 'VLR_TÍTULO', CONVERT (CHAR(10), DAT_VENCTO, 103) AS 'VENCIMENTO'
                           FROM AALITIN0 L INNER JOIN AASACAD0 A ON (L.NUM_CGC_CPF=A.NUM_CGC_CPF)
                           WHERE NUM_LOTE = " + inumlote;
            return Dados.ExecutarDataTable(query);
        }
        //Comando para a view proposta fim

        public int TotalRegistrosNaWeb(string cedCadastrado)
        {
            string buscaCedente = "";
            if (cedCadastrado != "")
            {
                buscaCedente = " AND NOM_FANTASIA LIKE '" + cedCadastrado + "%'";
            }
            string queryString = @"SELECT 
	                                    count(*) 
                                    FROM 
	                                    AACEDEN0
                                    WHERE 
	                                    STS_ATIVO = 'S' AND COD_CEDENTE IN(SELECT COD_CEDENTE FROM CONTRATO) " + buscaCedente;

            SqlCommand cmd = Dados.CriarCommand(queryString);
            int retorno = Convert.ToInt32(Dados.ExecutarScalar(cmd));

            return retorno;
        }

        public int TotalRegistrosForaWeb(string cedNaoCadastrado)
        {
            string buscaCedente = "";
            if (cedNaoCadastrado != "")
            {
                buscaCedente = " AND NOM_FANTASIA LIKE '" + cedNaoCadastrado + "%'";
            }
            string queryString = @"SELECT 
	                                    count(*) 
                                    FROM 
	                                    AACEDEN0
                                    WHERE 
	                                    STS_ATIVO = 'S' AND COD_CEDENTE NOT IN(SELECT COD_CEDENTE FROM CONTRATO) " + buscaCedente;

            SqlCommand cmd = Dados.CriarCommand(queryString);
            int retorno = Convert.ToInt32(Dados.ExecutarScalar(cmd));

            return retorno;
        }


    }
}
