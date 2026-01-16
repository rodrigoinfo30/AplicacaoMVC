using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoneyReport.Models
{
    public class CamposRelModels
    {
        //títulor em aberto
        public string Nom_sacado { get; set; }
        public string Documento { get; set; }
        public string Vencto { get; set; }
        public int DA { get; set; }
        public int Cod_banco { get; set; }
        public string Sts_cob { get; set; }
        public string Sts_doc { get; set; }
        public string Vlr_tit { get; set; }
        public string Operacao { get; set; }
        public string Dt_oper { get; set; }
        public int Fundo_id { get; set; }
        public string Tipo_originador { get; set; }
        public string Sts_oper { get; set; }

        //operações
        public string DAT_PROP { get; set; }
        public int NUM_PROP { get; set; }
        public int NUM_OPERACAO { get; set; }
        public int QTD_TITULOS { get; set; }
        public string VLR_FACE { get; set; }
        public string VLR_LIQUIDO { get; set; }

        //liquidados
        public string Nom_sacado_Liq { get; set; }
        public string Documento_Liq { get; set; }
        public string Vencto_Liq { get; set; }
        public int Cod_banco_Liq { get; set; }
        public string Sts_cob_Liq { get; set; }
        public string Sts_doc_Liq { get; set; }
        public string Vlr_tit_Liq { get; set; }
        public string Vlr_tit_previsto_Liq { get; set; }
        public string Vlr_tit_abat_Liq { get; set; }
        public string Vlr_juros_Liq { get; set; }
        public string Operacao_Liq { get; set; }
        public string Dt_oper_Liq { get; set; }
        public string Dt_pag_Liq { get; set; }
        public int Fundo_id_Liq { get; set; }
        public string Tipo_originador_Liq { get; set; }
        public string Sts_oper_Liq { get; set; }

        //estrato
        public string Des_mov_Ext { get; set; }
        public string Num_documento_Ext { get; set; }
        public string Sacado_Ext { get; set; }
        public string Deb_Ext { get; set; }
        public string Cred_Ext { get; set; }
        public string Tipo_Acerto_Ext { get; set; }
        public string Dat_pagto_Ext { get; set; }
        public string Vlr_pagto_Ext { get; set; }
        public string Vlr_original_Ext { get; set; }
        public string Dat_vencto_Ext { get; set; }
        public string Vlr_partida_Ext { get; set; }
        public string Dat_partida_Ext { get; set; }

        //canhoto
        public string Sacado_Canho { get; set; }
        public string Documento_Canho { get; set; }
        public string Vlr_Canho { get; set; }
        public string Dat_vencto_Canho { get; set; }
        public string Dat_emissao_Canho { get; set; }
        public string Status_canhoto_Canho { get; set; }
        public string Status_ordem_Canho { get; set; }
        public string Status_nf_Canho { get; set; }
        public string Sts_anal_canhot_Canho { get; set; }
        public string Sts_anal_nf_Canho { get; set; }
        public string Sts_anal_inicia_Canho { get; set; }

    }
}