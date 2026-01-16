using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MoneyReport.Models
{
    public class GetViewsModels
    {
        public string Cod_cedente { get; set; }

        //Para view do aberto início
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.DateTime)]
        public string Dat_inicial_aberto { get; set; }
        public string Dat_final_aberto { get; set; }
        //Para view do aberto fim

        //Para view do liquidado início
        public string Dat_ini_liq { get; set; }
        public string Dat_fim_liq { get; set; }
        public string Sts_doc_liq { get; set; }
        public bool Pagamento { get; set; }
        public bool Processamento { get; set; }
        public bool Baixados { get; set; }
        //Para view do liquidado fim

        //Para view do operações início
        public string Dat_ini_oper { get; set; }
        public string Dat_fim_oper { get; set; }
        //Para view do operações fim

        //Para view do canhoto início
        public string Dat_ini_canhot { get; set; }
        public string Dat_fim_canhot { get; set; }
        public bool Dat_oper { get; set; }
        public bool Dat_vento { get; set; }
        public bool Canhot_confirm { get; set; }
        public bool Canhot_entregue { get; set; }
        public bool Canhot_nao_confirm { get; set; }
        public bool Tem_nota { get; set; }
        public bool Nota_simples { get; set; }
        public bool Sem_nota { get; set; }
        public bool Colete_confirm { get; set; }
        public bool Coleta_nao_confirm { get; set; }
        //Para view do canhoto fim

        //Para view do extrato início
        public string Dat_ini_extrato { get; set; }
        public string Dat_fim_estrato { get; set; }
        public bool Aberto { get; set; }
        public bool Realizado { get; set; }
        //Para view do extrato início

        //Para view do Proposta início
        public string Busca_CNPJ_CPF { get; set; }
        public string Nom_razao { get; set; }
        public string Num_CNPJ_CPF { get; set; }
        public decimal Vlr_tot_titulos { get; set; }
        public int QTD_tot_titulos { get; set; }
        public string Num_documento { get; set; }
        public string Num_nf { get; set; }
        public string Seria_NF { get; set; }
        public string Insc_estadual { get; set; }
        public decimal Vlr_titulo { get; set; }
        public string Dat_vencto { get; set; }
        public string Dat_emissao { get; set; }
        public string Tip_doc { get; set; }
        public string Cmc7 { get; set; }
        public int Banco { get; set; }
        //cadastro sacado início
        public string Num_CNPJ_CPF_Sac { get; set; }
        public string Nom_Razao_Sac { get; set; }
        public string Insc_estadual_Sac { get; set; }
        public string Des_End_Sac { get; set; }
        public string Des_Bairro_Sac { get; set; }
        public string Des_Cid_Sac { get; set; }
        public string Est_Sac { get; set; }
        public string CEP_Sac { get; set; }
        public string DDD_Sac { get; set; }
        public string Tel_Sac { get; set; }
        public string Email_Sac { get; set; }
        public string Email_Cob_Sac { get; set; }

        //cadastro sacado fim

        //Para view propcnab início
        public string Nome_arquivo_cnab { get; set; }
        //Para view propcnab fim

        //Para view do Proposta FIm

        public string cedNaWebBusca { get; set; }
        public string cedForaWebBusca { get; set; }

        //listas
        public List<SelectListItem> ListaDocumento()
        {
            List<SelectListItem> listDoc = new List<SelectListItem>();
            listDoc.Add(new SelectListItem() { Text = "Cheque", Value = "C" });
            listDoc.Add(new SelectListItem() { Text = "Duplicata", Value = "D" });
            listDoc.Add(new SelectListItem() { Text = "Nota Promissória", Value = "N" });
            return listDoc;
        }

        public List<SelectListItem> ListaEstado()
        {
            List<SelectListItem> listEstado = new List<SelectListItem>();
            listEstado.Add(new SelectListItem() { Text = "AC", Value = "AC" });
            listEstado.Add(new SelectListItem() { Text = "AL", Value = "AL" });
            listEstado.Add(new SelectListItem() { Text = "AM", Value = "AM" });
            listEstado.Add(new SelectListItem() { Text = "AP", Value = "AP" });
            listEstado.Add(new SelectListItem() { Text = "BA", Value = "BA" });
            listEstado.Add(new SelectListItem() { Text = "CE", Value = "CE" });
            listEstado.Add(new SelectListItem() { Text = "DF", Value = "DF" });
            listEstado.Add(new SelectListItem() { Text = "ES", Value = "ES" });
            listEstado.Add(new SelectListItem() { Text = "GO", Value = "GO" });
            listEstado.Add(new SelectListItem() { Text = "MA", Value = "MA" });
            listEstado.Add(new SelectListItem() { Text = "MG", Value = "MG" });
            listEstado.Add(new SelectListItem() { Text = "MS", Value = "MS" });
            listEstado.Add(new SelectListItem() { Text = "MT", Value = "MT" });
            listEstado.Add(new SelectListItem() { Text = "PA", Value = "PA" });
            listEstado.Add(new SelectListItem() { Text = "PB", Value = "PB" });
            listEstado.Add(new SelectListItem() { Text = "PE", Value = "PE" });
            listEstado.Add(new SelectListItem() { Text = "PI", Value = "PI" });
            listEstado.Add(new SelectListItem() { Text = "PR", Value = "PR" });
            listEstado.Add(new SelectListItem() { Text = "RJ", Value = "RJ" });
            listEstado.Add(new SelectListItem() { Text = "RN", Value = "RN" });
            listEstado.Add(new SelectListItem() { Text = "RO", Value = "RO" });
            listEstado.Add(new SelectListItem() { Text = "RR", Value = "RR" });
            listEstado.Add(new SelectListItem() { Text = "RS", Value = "RS" });
            listEstado.Add(new SelectListItem() { Text = "SC", Value = "SC" });
            listEstado.Add(new SelectListItem() { Text = "SE", Value = "SE" });
            listEstado.Add(new SelectListItem() { Text = "SP", Value = "SP" });
            listEstado.Add(new SelectListItem() { Text = "TO", Value = "TO" });
            return listEstado;
        }

    }
}