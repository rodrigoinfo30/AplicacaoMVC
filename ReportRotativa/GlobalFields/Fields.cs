using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GlobalFields
{
    public class Fields
    {
        //Propsta dados do título início
        public int RECNUM_AALITIN0 { get; set; }
        public string BUSCA_CNPJ_CPF { get; set; }
        public string NOM_RAZAO { get; set; }
        public string NUM_CNPJ_CPF { get; set; }
        public decimal VLR_TOT_TITULOS { get; set; }
        public int QTD_TOT_TITULOS { get; set; }
        public string NUM_DOCUMENTO { get; set; }
        public string NUM_NF { get; set; }
        public string SERIE_NF { get; set; }
        public string INSC_ESTADUAL { get; set; }
        public decimal VLR_TITULO { get; set; }
        public string DAT_VENCTO { get; set; }
        public string DAT_EMISSAO { get; set; }
        public string TIP_DOC { get; set; }
        public string CMC7 { get; set; }
        public int BANCO { get; set; }
        public string NUM_CGC_CPF { get; set; }
        public string NUM_NOTA_FISCAL { get; set; }
        public int COD_RECUSA { get; set; }
        public string STS_RECUSA { get; set; }
        public string STS_AVISO { get; set; }
        public string STS_BCO_CART { get; set; }
        public int COD_BANCO_CHQ { get; set; }
        //Propsta dados do título fim

        //cadastro sacado início
        public int COD_SAC { get; set; }
        public string NUM_CNPJ_CPF_SAC { get; set; }
        public string NOM_RAZAO_SAC { get; set; }
        public string INSC_ESTADUAL_SAC { get; set; }
        public string DES_END_SAC { get; set; }
        public string DES_BAIRRO_SAC { get; set; }
        public string DES_CID_SAC { get; set; }
        public string EST_SAC { get; set; }
        public string CEP_SAC { get; set; }
        public string DDD_SAC { get; set; }
        public string TEL_SAC { get; set; }
        public string EMAIL_SAC { get; set; }
        public string EMAIL_COB_SAC { get; set; }
        //cadastro sacado fim

        //proposta dados do lote início
        public string STS_ANALISE { get; set; }
        public int COD_CEDENTE { get; set; }
        public int NUM_LOTE { get; set; }
        public string NOM_CEDENTE { get; set; }
        public decimal VLR_TITULOS { get; set; }
        public int QTD_TITULOS { get; set; }
        public string STS_CONCENTRA { get; set; }
        public string STS_PRAZO_MIN { get; set; }
        public string STS_PRAZO_MAX { get; set; }
        public string TIPO_OPERACAO { get; set; }

        public int NUM_DMAIS { get; set; }

        //proposta dados do lote início
    }
}
