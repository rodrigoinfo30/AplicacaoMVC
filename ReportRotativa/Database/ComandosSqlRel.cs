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
    public class ComandosSqlRel
    {
        //Comando para a rel. aberto menu inicio
        public DataTable ListaReleatorioAberto(string dt1, string dt2, string codcedente, string tipdoc)
        {
            string where = "";
            if (tipdoc != "")
            {
                where = " AND E.STS_DOC = '" + tipdoc + "'";
            }
            string queryString = @"SELECT  
                                    C.NOM_RAZAO,A.NUM_DOCUMENTO,CONVERT(CHAR(10),A.DAT_VENCIMENTO,103) AS 'DAT_VENCIMENTO',
                                    CASE WHEN DATEDIFF(DAY,DAT_VENCIMENTO,GETDATE()) <= 0 THEN 
                                    0 
                                    ELSE DATEDIFF(DAY,DAT_VENCIMENTO,GETDATE()) END as 'DA',
                                    A.COD_BANCO_PORT,
                                    F.DESCRICAO	AS 'COBRANCA',      
                                    E.DES_DOCUMENTO,
                                    A.VLR_PREVISTO,
	                                G.DESCRICAO AS 'OPERACAO',
                                    CONVERT(CHAR(10),A.DAT_LANCAMENTO,103) AS 'DAT_LANCAMENTO',
                                    A.FUNDO_ID,D.TIPO_ORIGINADOR,A.STS_OPERACAO
                                FROM 
                                    FATITUL0 A INNER JOIN AACEDEN0 B ON(A.COD_FORN = B.COD_CEDENTE)
                                    INNER JOIN AASACAD0 C ON (A.COD_CLIE = C.COD_SACADO)
                                    INNER JOIN FUNDOS D ON (A.FUNDO_ID=D.FUNDO_ID)
                                    INNER JOIN AATPDOC0 E ON(A.STS_DOC = E.COD_DOCUMENTO)
                                    inner join STSCOBR0 f on(a.sts_cobranca = f.cod_status)
                                    INNER JOIN STSOPER0 G ON(A.FUNDO_ID = G.FUNDO_ID AND D.TIPO_ORIGINADOR = G.TIPO_ORIGINADOR AND A.STS_OPERACAO =  G.STS_OPERACAO)
                                WHERE 
                                    DAT_PAGAMENTO = '01/01/1753' AND COD_FORN = " + codcedente + " AND VLR_PAGO = 0 AND STS_PAGO = 'A' AND STS_SALDO = '' and DAT_VENCIMENTO between '" + dt1 + "' and '" + dt2 + "'" + where + " ORDER BY A.DAT_VENCIMENTO";

            return Dados.ExecutarDataTable(queryString);
        }

        public int ContadorReleatorioAberto(string dt1, string dt2, string codcedente, string tipdoc)
        {
            string where = "";
            if (tipdoc != "")
            {
                where = " AND E.STS_DOC = '" + tipdoc + "'";
            }
            string queryString = @"SELECT  
                                    COUNT(*)
                                FROM 
                                    FATITUL0 A INNER JOIN AACEDEN0 B ON(A.COD_FORN = B.COD_CEDENTE)
                                    INNER JOIN AASACAD0 C ON (A.COD_CLIE = C.COD_SACADO)
                                    INNER JOIN FUNDOS D ON (A.FUNDO_ID=D.FUNDO_ID)
                                    INNER JOIN AATPDOC0 E ON(A.STS_DOC = E.COD_DOCUMENTO)
                                    inner join STSCOBR0 f on(a.sts_cobranca = f.cod_status)
                                    INNER JOIN STSOPER0 G ON(A.FUNDO_ID = G.FUNDO_ID AND D.TIPO_ORIGINADOR = G.TIPO_ORIGINADOR AND A.STS_OPERACAO =  G.STS_OPERACAO)
                                WHERE 
                                    DAT_PAGAMENTO = '01/01/1753' AND COD_FORN = " + codcedente + " AND VLR_PAGO = 0 AND STS_PAGO = 'A' AND STS_SALDO = '' and DAT_VENCIMENTO between '" + dt1 + "' and '" + dt2 + "'" + where;

            SqlCommand cmd = Dados.CriarCommand(queryString);
            int retorno = Convert.ToInt32(Dados.ExecutarScalar(cmd));

            return retorno;
        }
        //Comando para a rel. aberto menu fim

        public DataTable ListaReleatorioLiquidado(string dt1, string dt2, string codcedente, string tipdoc, bool tdata, bool tbaixado)
        {
            string where = "";
            string whereData = "";
            string whereDoc = "";
            if (tbaixado == true)
            {
                where = "'','BX'";
            }
            else where = "''";

            if (tdata == true)
            {
                whereData = " AND DAT_PAGAMENTO >= '" + dt1 + "' AND DAT_PAGAMENTO <= '" + dt2 + "'";
            }
            else whereData = " AND DAT_PROC_LIQ >= '" + dt1 + "' AND DAT_PROC_LIQ <= '" + dt2 + "'";

            if (tipdoc != "")
            {
                whereDoc = " AND E.STS_DOC = '" + tipdoc + "'";
            }
            else whereDoc = "";
            string queryString2 = whereData;
            string queryString = @"SELECT  
                                        C.NOM_RAZAO,A.NUM_DOCUMENTO,CONVERT(CHAR(10),A.DAT_VENCIMENTO,103) AS 'DAT_VENCIMENTO',
                                        A.COD_BANCO_PORT,
                                        F.DESCRICAO	AS 'COBRANCA',      
                                        E.DES_DOCUMENTO,
	                                    G.DESCRICAO AS 'OPERACAO',
                                        CONVERT(CHAR(10),A.DAT_LANCAMENTO,103) AS 'DAT_LANCAMENTO',
                                        CONVERT(CHAR(10),A.DAT_PAGAMENTO,103) AS 'DAT_PAGAMENTO',
                                        A.VLR_PREVISTO,
                                        A.VLR_PAGO,
                                        A.VLR_ABATIMENTO,
                                        CASE WHEN (VLR_PAGO - VLR_PREVISTO) > 0 THEN (VLR_PAGO - VLR_PREVISTO) ELSE 0 END AS 'JUROS',
                                        A.FUNDO_ID,D.TIPO_ORIGINADOR,A.STS_OPERACAO
                                    FROM 
                                        FATITUL0 A INNER JOIN AACEDEN0 B ON(A.COD_FORN = B.COD_CEDENTE)
                                        INNER JOIN AASACAD0 C ON (A.COD_CLIE = C.COD_SACADO)
                                        INNER JOIN FUNDOS D ON (A.FUNDO_ID=D.FUNDO_ID)
                                        INNER JOIN AATPDOC0 E ON(A.STS_DOC = E.COD_DOCUMENTO)
                                        INNER JOIN STSCOBR0 F ON(A.STS_COBRANCA = F.COD_STATUS)
                                        INNER JOIN STSOPER0 G ON(A.FUNDO_ID = G.FUNDO_ID AND D.TIPO_ORIGINADOR = G.TIPO_ORIGINADOR AND A.STS_OPERACAO =  G.STS_OPERACAO)
                                    WHERE 
                                        COD_FORN = " + codcedente + " AND VLR_PAGO <> 0 AND STS_PAGO = 'T' AND STS_SALDO IN(" + where + ")" + whereData + whereDoc + " ORDER BY A.DAT_PAGAMENTO";

            return Dados.ExecutarDataTable(queryString);
        }

        public int ContadorReleatorioLiquidado(string dt1, string dt2, string codcedente, string tipdoc, bool tdata, bool tbaixado)
        {
            string where = "";
            string whereData = "";
            string whereDoc = "";
            if (tbaixado == true)
            {
                where = "'','BX'";
            }
            else where = "''";

            if (tdata == true)
            {
                whereData = " AND DAT_PAGAMENTO >= '" + dt1 + "' AND DAT_PAGAMENTO <= '" + dt2 + "'";
            }
            else whereData = " AND DAT_PROC_LIQ >= '" + dt1 + "' AND DAT_PROC_LIQ <= '" + dt2 + "'";

            if (tipdoc != "")
            {
                whereDoc = " AND E.STS_DOC = '" + tipdoc + "'";
            }
            else whereDoc = "";
            string queryString2 = whereData;
            string queryString = @"SELECT  
                                        COUNT(*)
                                    FROM 
                                        FATITUL0 A INNER JOIN AACEDEN0 B ON(A.COD_FORN = B.COD_CEDENTE)
                                        INNER JOIN AASACAD0 C ON (A.COD_CLIE = C.COD_SACADO)
                                        INNER JOIN FUNDOS D ON (A.FUNDO_ID=D.FUNDO_ID)
                                        INNER JOIN AATPDOC0 E ON(A.STS_DOC = E.COD_DOCUMENTO)
                                        INNER JOIN STSCOBR0 F ON(A.STS_COBRANCA = F.COD_STATUS)
                                        INNER JOIN STSOPER0 G ON(A.FUNDO_ID = G.FUNDO_ID AND D.TIPO_ORIGINADOR = G.TIPO_ORIGINADOR AND A.STS_OPERACAO =  G.STS_OPERACAO)
                                    WHERE 
                                        COD_FORN = " + codcedente + " AND VLR_PAGO <> 0 AND STS_PAGO = 'T' AND STS_SALDO IN(" + where + ")" + whereData + whereDoc;

            SqlCommand cmd = Dados.CriarCommand(queryString);
            int retorno = Convert.ToInt32(Dados.ExecutarScalar(cmd));

            return retorno;
        }
        //Comando para a rel. aberto menu fim

        public DataTable ListaReleatorioExtrato(string dt1, string dt2, string codcedente, bool abert_reali)
        {
            string wheredata = "";
            string queryString = "";
            string selectAbertoRealizado = "";
            string orderAbertoRealizado = "";
            if (abert_reali == true)
            {
                selectAbertoRealizado = "CASE WHEN STS_DEB_CRED = 'D' THEN ";
                selectAbertoRealizado += "(DBO.CALC_VALOR_PRESENTE (A.COD_FORN, B.VLR_PARTIDA, A.DAT_VENCIMENTO, CONVERT(CHAR(10),GETDATE(),103), E.COD_TAX_JUROS, B.COD_MOV, A.FUNDO_ID, A.STS_FUNDO, A.NUM_ANO, A.NUM_LANCAMENTO)) ";
                selectAbertoRealizado += "ELSE ";
                selectAbertoRealizado += "0 ";
                selectAbertoRealizado += "END AS 'DEB', ";
                selectAbertoRealizado += "CASE WHEN STS_DEB_CRED = 'C' THEN ";
                selectAbertoRealizado += "(DBO.CALC_VALOR_PRESENTE (A.COD_FORN, B.VLR_PARTIDA, A.DAT_VENCIMENTO, CONVERT(CHAR(10),GETDATE(),103), E.COD_TAX_REMUN, B.COD_MOV, A.FUNDO_ID, A.STS_FUNDO, A.NUM_ANO, A.NUM_LANCAMENTO)) ";
                selectAbertoRealizado += "ELSE ";
                selectAbertoRealizado += "0 ";
                selectAbertoRealizado += "END AS 'CRED', ";
                selectAbertoRealizado += "VLR_PREVISTO,CONVERT(CHAR(10),A.DAT_VENCIMENTO,103) AS DAT_VENCIMENTO,VLR_PARTIDA,CONVERT(CHAR(10),B.DAT_PARTIDA, 103) AS DAT_PARTIDA ";
                orderAbertoRealizado = "ORDER BY B.COD_MOV, B.DAT_PARTIDA ";
            }
            else
            {
                selectAbertoRealizado = "C.DES_MOV,A.NUM_DOCUMENTO,D.NOM_RAZAO, ";
                selectAbertoRealizado += "CASE WHEN B.NUM_OPERACAO = -1 THEN 'AMORTIZAÇÃO' ";
                selectAbertoRealizado += "ELSE ";
                selectAbertoRealizado += "CASE WHEN B.NUM_OPERACAO = -2 THEN 'ACERTO DIRETO' ";
                selectAbertoRealizado += "ELSE ";
                selectAbertoRealizado += "CASE WHEN B.NUM_OPERACAO > 0 THEN 'ACERTO OPERAÇÃO' ";
                selectAbertoRealizado += "END ";
                selectAbertoRealizado += "END ";
                selectAbertoRealizado += "END AS 'TIPOACERTO', ";
                selectAbertoRealizado += "CASE WHEN C.STS_DEB_CRED = 'C' THEN VLR_PAGTO ELSE 0 END AS 'CRED', ";
                selectAbertoRealizado += "CASE WHEN C.STS_DEB_CRED = 'D' THEN VLR_PAGTO ELSE 0 END AS 'DEB', ";
                selectAbertoRealizado += "CONVERT(CHAR(10),B.DAT_PARTIDA, 103) AS DAT_PARTIDA, CONVERT(CHAR(10),B.DAT_PAGTO, 103) AS DAT_PAGTO, B.VLR_PARTIDA, B.VLR_PAGTO, CONVERT(CHAR(10),A.DAT_VENCIMENTO,103) AS DAT_VENCIMENTO ";
                orderAbertoRealizado = "ORDER BY B.DAT_PAGTO, B.COD_MOV, B.VLR_PAGTO ";
            }

            if (abert_reali == true)
            {
                wheredata = " AND VLR_PAGTO = 0 AND B.NUM_OPERACAO = 0 AND B.NUM_PROP = 0 AND DAT_PAGTO = '01/01/1753' ";
            }
            else wheredata = " AND VLR_PAGTO <> 0 AND B.NUM_OPERACAO <> 0 AND DAT_PAGTO >= '" + dt1 + "' and DAT_PAGTO <= '" + dt2 + "' ";

            queryString = @"SELECT C.DES_MOV, A.NUM_DOCUMENTO, D.NOM_RAZAO, " + selectAbertoRealizado;
            queryString += "FROM ";
            queryString += "FATITUL0 A INNER JOIN GAMOVCT0 B ON(A.NUM_ANO = B.NUM_ANO AND A.NUM_LANCAMENTO = B.NUM_LANCAMENTO AND A.COD_FORN = B.COD_CEDENTE) ";
            queryString += "INNER JOIN GATMOVC0 C ON(B.COD_MOV = C.COD_MOV) ";
            queryString += "INNER JOIN AASACAD0 D ON(A.COD_CLIE = D.COD_SACADO) ";
            queryString += "INNER JOIN AACEDEN0 E ON(A.COD_FORN = E.COD_CEDENTE) ";
            queryString += "WHERE ";
            queryString += "COD_FORN = " + codcedente + wheredata;
            queryString += orderAbertoRealizado;

            return Dados.ExecutarDataTable(queryString);
        }

        public int ContadorReleatorioExtrato(string dt1, string dt2, string codcedente, bool abert_reali)
        {
            string wheredata = "";
            string queryString = "";
            if (abert_reali == true)
            {
                wheredata = " AND VLR_PAGTO = 0 AND B.NUM_OPERACAO = 0 AND B.NUM_PROP = 0 AND DAT_PAGTO = '01/01/1753' ";
            }
            else wheredata = " AND VLR_PAGTO <> 0 AND B.NUM_OPERACAO <> 0 AND DAT_PAGTO >= '" + dt1 + "' and DAT_PAGTO <= '" + dt2 + "' ";

            queryString = @"SELECT COUNT(*) ";
            queryString += "FROM ";
            queryString += "FATITUL0 A INNER JOIN GAMOVCT0 B ON(A.NUM_ANO = B.NUM_ANO AND A.NUM_LANCAMENTO = B.NUM_LANCAMENTO AND A.COD_FORN = B.COD_CEDENTE) ";
            queryString += "INNER JOIN GATMOVC0 C ON(B.COD_MOV = C.COD_MOV) ";
            queryString += "INNER JOIN AASACAD0 D ON(A.COD_CLIE = D.COD_SACADO) ";
            queryString += "INNER JOIN AACEDEN0 E ON(A.COD_FORN = E.COD_CEDENTE) ";
            queryString += "WHERE ";
            queryString += "COD_FORN = " + codcedente + wheredata;

            SqlCommand cmd = Dados.CriarCommand(queryString);
            int retorno = Convert.ToInt32(Dados.ExecutarScalar(cmd));

            return retorno;
        }
        //Comando para a rel. aberto menu fim

        public DataTable ListaReleatorioOperacao(string dt1, string dt2, string codcedente)
        {
            string queryString = @"SELECT 
	                                    CONVERT(CHAR(10),DAT_PROP,103) AS DAT_PROP ,NUM_PROP,NUM_OPERACAO,QTD_TITULOS,VLR_FACE,VLR_LIQUIDO  
                                    FROM 
	                                    AAPROPO0 A INNER JOIN AACEDEN0 B ON(A.COD_CEDENTE = B.COD_CEDENTE)
                                    WHERE 
	                                    A.COD_CEDENTE = " + codcedente + " AND STS_EFETIVADO = 'S' AND DAT_PROP BETWEEN '" + dt1 + "' AND '" + dt2 + "' ORDER BY A.DAT_PROP ASC, A.NUM_OPERACAO ASC";

            return Dados.ExecutarDataTable(queryString);
        }

        public int ContadorReleatorioOperacao(string dt1, string dt2, string codcedente)
        {
            string queryString = @"SELECT 
	                                    COUNT(*)
                                    FROM 
	                                    AAPROPO0 A INNER JOIN AACEDEN0 B ON(A.COD_CEDENTE = B.COD_CEDENTE)
                                    WHERE 
	                                    A.COD_CEDENTE = " + codcedente + " AND STS_EFETIVADO = 'S' AND DAT_PROP BETWEEN '" + dt1 + "' AND '" + dt2 + "'";

            SqlCommand cmd = Dados.CriarCommand(queryString);
            int retorno = Convert.ToInt32(Dados.ExecutarScalar(cmd));

            return retorno;
        }
        //Comando para a rel. aberto menu fim

        public DataTable ListaReleatorioCanhoto(string cedente, string dt1, string dt2, bool oper_vencto, string swhere)
        {

            string whereData = "";
            if (oper_vencto == true)
            {
                whereData = " AND DAT_LANCAMENTO >= '" + dt1 + "' AND DAT_LANCAMENTO <= '" + dt2 + "'";
            }
            else whereData = " AND DAT_VENCIMENTO >= '" + dt1 + "' AND DAT_VENCIMENTO <= '" + dt2 + "'";

            string queryString = @"SELECT 
                                    S.NOM_RAZAO, F.NUM_DOCUMENTO, F.VLR_PREVISTO, CONVERT(CHAR(10),F.DAT_VENCIMENTO,103) as DAT_VENCIMENTO, CONVERT(CHAR(10),F.DAT_EMISSAO,103) as DAT_EMISSAO 
	                                ,CASE F.STS_ANAL_CANHOT WHEN 'S' THEN 'Confirmado' ELSE 
										CASE F.STS_ANAL_CANHOT WHEN 'N' THEN 'Não Confirmado' ELSE
											CASE F.STS_ANAL_CANHOT WHEN 'M' THEN 'Em Romaneio' ELSE '' 
											END 
										end
									end AS CANHOTO
	                                ,CASE F.STS_ANAL_NF WHEN 'S' THEN 'Com Nota' ELSE 
										CASE F.STS_ANAL_NF WHEN 'N' THEN 'Sem Nota' ELSE
											CASE F.STS_ANAL_NF WHEN 'F' THEN 'Nota Simples' ELSE '' 
											end
										end
									END AS NOTA
	                                ,CASE F.STS_ANAL_INICIA WHEN 'S' THEN 'Com Ordem' ELSE 
										CASE F.STS_ANAL_INICIA WHEN 'N' THEN 'Sem Ordem' ELSE '' 
										end
									END AS ORDEM
                                    ,F.STS_ANAL_CANHOT,F.STS_ANAL_NF,F.STS_ANAL_INICIA
	                            FROM 
                                    FATITUL0 F INNER JOIN AACEDEN0 C ON (F.COD_FORN = C.COD_CEDENTE) INNER JOIN AASACAD0 S ON (F.COD_CLIE = S.COD_SACADO) INNER JOIN AATPDOC0 A ON (F.STS_DOC = A.COD_DOCUMENTO), AAPARAM0 P
	                            WHERE P.RECNUM = 1 " + swhere + " AND C.STS_ANAL_CANHOT = 'S' AND F.STS_PAGO = 'A' AND F.STS_SALDO = '' AND COD_FORN = " + cedente + whereData;

            //DAT_PAGAMENTO = '01/01/1753' AND COD_FORN = " + codcedente + " AND VLR_PAGO = 0 AND STS_PAGO = 'A' AND STS_SALDO = '' and DAT_VENCIMENTO between '" + dt1 + "' and '" + dt1 + "' ORDER BY A.DAT_VENCIMENTO";

            return Dados.ExecutarDataTable(queryString);
        }

        public int ContadorReleatorioCanhoto(string cedente, string dt1, string dt2, bool oper_vencto, string swhere)
        {
            string whereData = "";
            if (oper_vencto == true)
            {
                whereData = " AND DAT_LANCAMENTO >= '" + dt1 + "' AND DAT_LANCAMENTO <= '" + dt2 + "'";
            }
            else whereData = " AND DAT_VENCIMENTO >= '" + dt1 + "' AND DAT_VENCIMENTO <= '" + dt2 + "'";

            string queryString = @"SELECT 
                                    COUNT(*)
	                            FROM 
                                    FATITUL0 F INNER JOIN AACEDEN0 C ON (F.COD_FORN = C.COD_CEDENTE) INNER JOIN AASACAD0 S ON (F.COD_CLIE = S.COD_SACADO) INNER JOIN AATPDOC0 A ON (F.STS_DOC = A.COD_DOCUMENTO), AAPARAM0 P
	                            WHERE P.RECNUM = 1 AND C.STS_ANAL_CANHOT = 'S' AND F.STS_PAGO = 'A' AND F.STS_SALDO = '' AND COD_FORN = " + cedente + whereData;

            SqlCommand cmd = Dados.CriarCommand(queryString);
            int retorno = Convert.ToInt32(Dados.ExecutarScalar(cmd));

            return retorno;
        }
        //Comando para a rel. aberto menu fim

    }
}
