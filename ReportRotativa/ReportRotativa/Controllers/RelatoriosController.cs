using PagedList;
using Rotativa;
using Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc;
using MoneyReport.Models;
using System.Configuration;

using BoletoNet;

namespace MoneyReport.Controllers
{
    public class RelatoriosController : Controller
    {

        private Database.ComandosSqlRel comandoDb = new Database.ComandosSqlRel();
        private Database.ComandosSql comandoDbCed = new Database.ComandosSql();
        string pastaIIS = ConfigurationManager.AppSettings["PASTA_PADRAO"];

        decimal g_totalCobSimples = 0;
        int g_qtdCodSimples = 0;
        decimal g_totalFactoring = 0;
        int g_qtdFactoring = 0;
        decimal g_totalFidc = 0;
        int g_qtdFidc = 0;
        decimal g_totalCodSimplesSec = 0;
        int g_qtdCobSimplesSec = 0;
        decimal g_totalSec = 0;
        int g_qtdSec = 0;
        decimal g_totalSecRec = 0;
        int g_qtdSecRec = 0;
        decimal g_totalAquisicao = 0;
        int g_qtdAquisicao = 0;
        decimal g_totalConfissao = 0;
        int g_qtdConfissao = 0;
        decimal g_totalRecompra = 0;
        int g_qtdRecompra = 0;
        decimal g_totalFidcRec = 0;
        int g_qtdFidcRec = 0;
        decimal g_totalFidcCobSim = 0;
        int g_qtdFidcCobSim = 0;
        decimal g_totalConfSec = 0;
        int g_qtdConfSec = 0;
        decimal g_totalAquiSec = 0;
        int g_qtdAquiSec = 0;

        decimal canhotoNE = 0;
        decimal canhotoR = 0;
        decimal canhotoE = 0;
        decimal semNF = 0;
        decimal comNF = 0;
        decimal simNF = 0;
        decimal comOrdem = 0;
        decimal semOrdem = 0;
        int QTD_canhotoNE = 0;
        int QTD_canhotoR = 0;
        int QTD_canhotoE = 0;
        int QTD_semNF = 0;
        int QTD_comNF = 0;
        int QTD_simNF = 0;
        int QTD_comOrdem = 0;
        int QTD_semOrdem = 0;

        string codCedente, datIni, datFim, tipoDoc, whereCanhoto;
        bool tipDateLiq, stsBaixados, abertoRealizado, operacao_Vencimento;

        public ActionResult ExecAberto(int? pagina, Boolean? pdf)
        {
            string urlAnterior = "";
            //como deve vir na url para pegar os dadops ?cedente=1123&dt1=01/06/2020&dt2=26/09/2020&tDoc=D
            int vericaSelecao = 0;
            if ((pdf == true) || (pagina != null))
            {
                bool existe = CookiesModels.Exists("Cedente");

                if (existe == true)
                {
                    codCedente = CookiesModels.Get("Cedente");
                    datIni = CookiesModels.Get("Datini");
                    datFim = CookiesModels.Get("Datfim");
                    tipoDoc = CookiesModels.Get("Tpodoc");

                }
                else
                {
                    urlAnterior = System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
                    for (int count = 1; count <= 4; count++)
                    {
                        int Ini = urlAnterior.IndexOf('=');
                        int Fim = urlAnterior.IndexOf('&');
                        if (count == 1)
                        {
                            codCedente = urlAnterior.Substring((Ini + 1), ((Fim - 1) - Ini));
                            CookiesModels.Set("Cedente", codCedente);
                        }
                        if (count == 2)
                        {
                            datIni = urlAnterior.Substring((Ini + 1), ((Fim - 1) - Ini));
                            CookiesModels.Set("Datini", datIni);
                        }
                        if (count == 3)
                        {
                            datFim = urlAnterior.Substring((Ini + 1), ((Fim - 1) - Ini));
                            CookiesModels.Set("Datfim", datFim);
                        }
                        if (count == 4)
                        {
                            tipoDoc = urlAnterior.Substring((Ini + 1), 1);
                            CookiesModels.Set("Tpodoc", tipoDoc);
                        } 
                        int tamanho = urlAnterior.Length;
                        urlAnterior = urlAnterior.Substring((Fim + 1), ((tamanho - Fim) - 1));
                    }
                }

            }
            else
            {
                DetetaCookies();
                if (!string.IsNullOrEmpty(Request["cedente"])) codCedente = Convert.ToString(Request["cedente"]);
                if (!string.IsNullOrEmpty(Request["dt1"])) datIni = Convert.ToString(Request["dt1"]);
                if (!string.IsNullOrEmpty(Request["dt2"])) datFim = Convert.ToString(Request["dt2"]);
                if (!string.IsNullOrEmpty(Request["tDoc"])) tipoDoc = Convert.ToString(Request["tDoc"]);
            }
            if (tipoDoc == "-") tipoDoc = "";

            if (codCedente != null)
            {
                vericaSelecao = comandoDb.ContadorReleatorioAberto(datIni, datFim, codCedente, tipoDoc);
                if (vericaSelecao > 0)
                {
                    var lista = new List<CamposRelModels>();
                    ViewBag.HideTextBox = true;
                    foreach (DataRow row in comandoDb.ListaReleatorioAberto(datIni, datFim, codCedente, tipoDoc).Rows)
                    {
                        var aberto = new CamposRelModels();

                        aberto.Nom_sacado = (row["NOM_RAZAO"]).ToString();
                        aberto.Documento = (row["NUM_DOCUMENTO"]).ToString();
                        aberto.Vencto = (row["DAT_VENCIMENTO"]).ToString();
                        aberto.DA = Convert.ToInt32(row["DA"]);
                        aberto.Cod_banco = Convert.ToInt32(row["COD_BANCO_PORT"]);
                        aberto.Sts_cob = (row["COBRANCA"]).ToString();
                        aberto.Sts_doc = (row["DES_DOCUMENTO"]).ToString();
                        aberto.Vlr_tit = string.Format("{0:N}", Convert.ToDecimal(row["VLR_PREVISTO"]));
                        aberto.Operacao = (row["OPERACAO"]).ToString();
                        aberto.Dt_oper = (row["DAT_LANCAMENTO"]).ToString();
                        aberto.Fundo_id = Convert.ToInt32(row["FUNDO_ID"]);
                        aberto.Tipo_originador = (row["TIPO_ORIGINADOR"]).ToString();
                        aberto.Sts_oper = (row["STS_OPERACAO"]).ToString();

                        Totais(aberto.Fundo_id, aberto.Tipo_originador, aberto.Sts_oper, Convert.ToDecimal(row["VLR_PREVISTO"]));

                        lista.Add(aberto);
                    }
                    ViewBag.Datini = datIni;
                    ViewBag.Datfim = datFim;
                    ViewBag.DatNow = "Processado: " + DateTime.Now;
                    //executar a aplicação no IIS
                    ViewBag.CaminhoPDF = "/" + pastaIIS + "/Relatorios/ExecAberto?pdf=true";
                    //executar a aplicação no Visual Studio
                    //ViewBag.CaminhoPDF = "/Relatorios/ExecAberto?pdf=true";

                    if (pdf != true)
                    {
                        int numeroRegistros = 15;
                        int numeroPagina = (pagina ?? 1);
                        return View(lista.ToPagedList(numeroPagina, numeroRegistros));
                    }
                    else
                    {
                        int pagNumero = 1;
                        DetetaCookies();
                        var relatorioPDF = new ViewAsPdf
                        {
                            ViewName = "ExecAberto",
                            IsGrayScale = false,
                            FileName = "ExecAberto.pdf",
                            Model = lista.ToPagedList(pagNumero, lista.Count)
                        };
                        return relatorioPDF;
                    }
                }
                else
                {
                    DetetaCookies();
                    TempData["AlertaSelecao"] = "Nehum movimento encontrado em aberto para esse período!";
                    return RedirectToAction("/ExecAberto");
                }
            }
            return View();
        }

        public ActionResult ExecOper(int? pagina, Boolean? pdf)
        {
            string urlAnterior = "";
            //como deve vir na url para pegar os dadops ?cedente=1123&dt1=01/01/2020&dt2=26/09/2020
            int vericaSelecao = 0;
            if ((pdf == true) || (pagina != null))
            {
                bool existe = CookiesModels.Exists("Cedente");

                if (existe == true)
                {
                    codCedente = CookiesModels.Get("Cedente");
                    datIni = CookiesModels.Get("Datini");
                    datFim = CookiesModels.Get("Datfim");
                }
                else
                {
                    urlAnterior = System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
                    for (int count = 1; count <= 3; count++)
                    {
                        int Ini = urlAnterior.IndexOf('=');
                        int Fim = urlAnterior.IndexOf('&');
                        if (count == 1)
                        {
                            codCedente = urlAnterior.Substring((Ini + 1), ((Fim - 1) - Ini));
                            CookiesModels.Set("Cedente", codCedente);
                        }
                        if (count == 2)
                        {
                            datIni = urlAnterior.Substring((Ini + 1), ((Fim - 1) - Ini));
                            CookiesModels.Set("Datini", datIni);
                        }
                        if (count == 3)
                        {
                            datFim = urlAnterior.Substring((Ini + 1), (urlAnterior.Length - (Ini + 1)));
                            CookiesModels.Set("Datfim", datFim);
                        }
                        int tamanho = urlAnterior.Length;
                        urlAnterior = urlAnterior.Substring((Fim + 1), ((tamanho - Fim) - 1));
                    }
                }

            }
            else
            {
                DetetaCookies();
                if (!string.IsNullOrEmpty(Request["cedente"])) codCedente = Convert.ToString(Request["cedente"]);
                if (!string.IsNullOrEmpty(Request["dt1"])) datIni = Convert.ToString(Request["dt1"]);
                if (!string.IsNullOrEmpty(Request["dt2"])) datFim = Convert.ToString(Request["dt2"]);
            }

            if (codCedente != null)
            {
                vericaSelecao = comandoDb.ContadorReleatorioOperacao(datIni, datFim, codCedente);
                if (vericaSelecao > 0)
                {
                    ViewBag.HideTextBox = true;
                    var lista = new List<CamposRelModels>();

                    decimal VlrTotal = 0;

                    foreach (DataRow row in comandoDb.ListaReleatorioOperacao(datIni, datFim, codCedente).Rows)
                    {
                        var oper = new CamposRelModels();

                        oper.DAT_PROP = (row["DAT_PROP"]).ToString();
                        oper.NUM_PROP = Convert.ToInt32(row["NUM_PROP"]);
                        oper.NUM_OPERACAO = Convert.ToInt32(row["NUM_OPERACAO"]);
                        oper.QTD_TITULOS = Convert.ToInt32(row["QTD_TITULOS"]);
                        oper.VLR_FACE = string.Format("{0:N}", Convert.ToDecimal(row["VLR_FACE"]));
                        oper.VLR_LIQUIDO = string.Format("{0:N}", Convert.ToDecimal(row["VLR_LIQUIDO"]));

                        VlrTotal = VlrTotal + Convert.ToDecimal(row["VLR_FACE"]);

                        lista.Add(oper);
                    }
                    if (Convert.ToDecimal(VlrTotal) > 0) ViewBag.ValorTotal = "Total Geral: " + string.Format("{0:N}", Convert.ToDecimal(VlrTotal));
                    ViewBag.Datini = datIni;
                    ViewBag.Datfim = datFim;
                    ViewBag.DatNow = "Processado: " + DateTime.Now;
                    //executar a aplicação no IIS
                    ViewBag.CaminhoPDF = "/" + pastaIIS + "/Relatorios/ExecOper?pdf=true";
                    //executar a aplicação no Visual Studio
                    //ViewBag.CaminhoPDF = "/Relatorios/ExecOper?pdf=true";

                    if (pdf != true)
                    {
                        int numeroRegistros = 15;
                        int numeroPagina = (pagina ?? 1);
                        return View(lista.ToPagedList(numeroPagina, numeroRegistros));
                    }
                    else
                    {
                        int pagNumero = 1;
                        DetetaCookies();
                        var relatorioPDF = new ViewAsPdf
                        {
                            ViewName = "ExecOper",
                            IsGrayScale = false,
                            FileName = "ExecOper.pdf",
                            Model = lista.ToPagedList(pagNumero, lista.Count)
                        };
                        return relatorioPDF;
                    }
                }
                else
                {
                    DetetaCookies();
                    TempData["AlertaSelecao"] = "Nehuma operação encontrada para esse período!";
                    return RedirectToAction("/ExecOper");
                }
            }
            return View();
        }

        public ActionResult ExecLiqui(int? pagina, Boolean? pdf)
        {
            string urlAnterior = "";
            //como deve vir na url para pegar os dadops ?cedente=1123&dt1=01/05/2020&dt2=05/05/2020&tDoc=D&tipLiq=true&baixado=false
            int vericaSelecao = 0;
            if ((pdf == true) || (pagina != null))
            {
                bool existe = CookiesModels.Exists("Cedente");

                if (existe == true)
                {
                    codCedente = CookiesModels.Get("Cedente");
                    datIni = CookiesModels.Get("Datini");
                    datFim = CookiesModels.Get("Datfim");
                    tipoDoc = CookiesModels.Get("Tpodoc");
                    tipDateLiq = Convert.ToBoolean(CookiesModels.Get("tipdateliq"));
                    stsBaixados = Convert.ToBoolean(CookiesModels.Get("Stsbaixados"));

                }
                else
                {
                    urlAnterior = System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
                    for (int count = 1; count <= 6; count++)
                    {
                        int Ini = urlAnterior.IndexOf('=');
                        int Fim = urlAnterior.IndexOf('&');
                        if (count == 1)
                        {
                            codCedente = urlAnterior.Substring((Ini + 1), ((Fim - 1) - Ini));
                            CookiesModels.Set("Cedente", codCedente);
                        }
                        if (count == 2)
                        {
                            datIni = urlAnterior.Substring((Ini + 1), ((Fim - 1) - Ini));
                            CookiesModels.Set("Datini", datIni);
                        }
                        if (count == 3)
                        {
                            datFim = urlAnterior.Substring((Ini + 1), ((Fim - 1) - Ini));
                            CookiesModels.Set("Datfim", datFim);
                        }
                        if (count == 4)
                        {
                            tipoDoc = urlAnterior.Substring((Ini + 1), 1);
                            CookiesModels.Set("Tpodoc", tipoDoc);
                        }
                        if (count == 5)
                        {
                            tipDateLiq = Convert.ToBoolean(urlAnterior.Substring((Ini + 1), ((Fim - 1) - Ini)));
                            CookiesModels.Set("tipdateliq",Convert.ToString(tipDateLiq));
                        }
                        if (count == 6)
                        {
                            stsBaixados = Convert.ToBoolean(urlAnterior.Substring((Ini + 1), (urlAnterior.Length - (Ini + 1))));
                            CookiesModels.Set("Stsbaixados", Convert.ToString(stsBaixados));
                        } 
                        int tamanho = urlAnterior.Length;
                        urlAnterior = urlAnterior.Substring((Fim + 1), ((tamanho - Fim) - 1));
                    }
                }

            }
            else
            {
                DetetaCookies();
                if (!string.IsNullOrEmpty(Request["cedente"])) codCedente = Convert.ToString(Request["cedente"]);
                if (!string.IsNullOrEmpty(Request["dt1"])) datIni = Convert.ToString(Request["dt1"]);
                if (!string.IsNullOrEmpty(Request["dt2"])) datFim = Convert.ToString(Request["dt2"]);
                if (!string.IsNullOrEmpty(Request["tDoc"])) tipoDoc = Convert.ToString(Request["tDoc"]);
                if (!string.IsNullOrEmpty(Request["tipLiq"])) tipDateLiq = Convert.ToBoolean(Request["tipLiq"]);
                if (!string.IsNullOrEmpty(Request["baixado"])) stsBaixados = Convert.ToBoolean(Request["baixado"]);
            }
            if (tipoDoc == "-") tipoDoc = "";
            if (codCedente != null)
            {
                vericaSelecao = comandoDb.ContadorReleatorioLiquidado(datIni, datFim, codCedente, tipoDoc, tipDateLiq, stsBaixados);
                if (vericaSelecao > 0)
                {
                    ViewBag.HideTextBox = true;
                    var lista = new List<CamposRelModels>();

                    foreach (DataRow row in comandoDb.ListaReleatorioLiquidado(datIni, datFim, codCedente, tipoDoc, tipDateLiq, stsBaixados).Rows)
                    {
                        var liqui = new CamposRelModels();

                        liqui.Nom_sacado_Liq = (row["NOM_RAZAO"]).ToString();
                        liqui.Documento_Liq = (row["NUM_DOCUMENTO"]).ToString();
                        liqui.Vencto_Liq = (row["DAT_VENCIMENTO"]).ToString();
                        liqui.Cod_banco_Liq = Convert.ToInt32(row["COD_BANCO_PORT"]);
                        liqui.Sts_cob_Liq = (row["COBRANCA"]).ToString();
                        liqui.Sts_doc_Liq = (row["DES_DOCUMENTO"]).ToString();
                        liqui.Operacao_Liq = (row["OPERACAO"]).ToString();
                        liqui.Dt_oper_Liq = (row["DAT_LANCAMENTO"]).ToString();
                        liqui.Dt_pag_Liq = (row["DAT_PAGAMENTO"]).ToString();
                        liqui.Vlr_tit_previsto_Liq = string.Format("{0:N}", Convert.ToDecimal(row["VLR_PREVISTO"]));
                        liqui.Vlr_tit_Liq = string.Format("{0:N}", Convert.ToDecimal(row["VLR_PAGO"]));
                        liqui.Vlr_tit_abat_Liq = string.Format("{0:N}", Convert.ToDecimal(row["VLR_ABATIMENTO"]));
                        liqui.Vlr_juros_Liq = string.Format("{0:N}", Convert.ToDecimal(row["JUROS"]));
                        liqui.Fundo_id_Liq = Convert.ToInt32(row["FUNDO_ID"]);
                        liqui.Tipo_originador_Liq = (row["TIPO_ORIGINADOR"]).ToString();
                        liqui.Sts_oper_Liq = (row["STS_OPERACAO"]).ToString();

                        Totais(liqui.Fundo_id_Liq, liqui.Tipo_originador_Liq, liqui.Sts_oper_Liq, Convert.ToDecimal(row["VLR_PAGO"]));

                        lista.Add(liqui);
                    }
                    ViewBag.Datini = datIni;
                    ViewBag.Datfim = datFim;
                    ViewBag.DatNow = "Processado: " + DateTime.Now;
                    //executar a aplicação no IIS                    
                    ViewBag.CaminhoPDF = "/" + pastaIIS + "/Relatorios/ExecLiqui?pdf=true";
                    //executar a aplicação no Visual Studio
                    //ViewBag.CaminhoPDF = "/Relatorios/ExecLiqui?pdf=true";

                    if (pdf != true)
                    {
                        int numeroRegistros = 15;
                        int numeroPagina = (pagina ?? 1);
                        return View(lista.ToPagedList(numeroPagina, numeroRegistros));
                    }
                    else
                    {
                        int pagNumero = 1;
                        DetetaCookies();
                        var relatorioPDF = new ViewAsPdf
                        {
                            ViewName = "ExecLiqui",
                            IsGrayScale = false,
                            FileName = "ExecLiqui.pdf",
                            Model = lista.ToPagedList(pagNumero, lista.Count)
                        };
                        return relatorioPDF;
                    }
                }
                else
                {
                    DetetaCookies();
                    TempData["AlertaSelecao"] = "Nehum título liquidado para esse período!";
                    return RedirectToAction("/ExecLiqui");
                }
            }
            return View();
        }

        public ActionResult ExecExtr(int? pagina, Boolean? pdf)
        {
            string urlAnterior = "";
            //como deve vir na url para pegar os dados aberto ?cedente=1123&dt1=01/09/2020&dt2=26/09/2020&abertReali=true
            //como deve vir na url para pegar os dados relizado ?cedente=1123&dt1=01/05/2020&dt2=20/05/2020&abertReali=false
            int vericaSelecao = 0;
            if ((pdf == true) || (pagina != null))
            {
                bool existe = CookiesModels.Exists("Cedente");

                if (existe == true)
                {
                    codCedente = CookiesModels.Get("Cedente");
                    datIni = CookiesModels.Get("Datini");
                    datFim = CookiesModels.Get("Datfim");
                    abertoRealizado = Convert.ToBoolean(CookiesModels.Get("Abertorealizado"));

                }
                else
                {
                    urlAnterior = System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
                    for (int count = 1; count <= 4; count++)
                    {
                        int Ini = urlAnterior.IndexOf('=');
                        int Fim = urlAnterior.IndexOf('&');
                        if (count == 1)
                        {
                            codCedente = urlAnterior.Substring((Ini + 1), ((Fim - 1) - Ini));
                            CookiesModels.Set("Cedente", codCedente);
                        }
                        if (count == 2)
                        {
                            datIni = urlAnterior.Substring((Ini + 1), ((Fim - 1) - Ini));
                            CookiesModels.Set("Datini", datIni);
                        }
                        if (count == 3)
                        {
                            datFim = urlAnterior.Substring((Ini + 1), ((Fim - 1) - Ini));
                            CookiesModels.Set("Datfim", datFim);
                        }
                        if (count == 4)
                        {
                            abertoRealizado = Convert.ToBoolean(urlAnterior.Substring((Ini + 1), (urlAnterior.Length - (Ini + 1))));
                            CookiesModels.Set("Abertorealizado", Convert.ToString(abertoRealizado));
                        }
                        int tamanho = urlAnterior.Length;
                        urlAnterior = urlAnterior.Substring((Fim + 1), ((tamanho - Fim) - 1));
                    }
                }

            }
            else
            {
                DetetaCookies();
                if (!string.IsNullOrEmpty(Request["cedente"])) codCedente = Convert.ToString(Request["cedente"]);
                if (!string.IsNullOrEmpty(Request["dt1"])) datIni = Convert.ToString(Request["dt1"]);
                if (!string.IsNullOrEmpty(Request["dt2"])) datFim = Convert.ToString(Request["dt2"]);
                if (!string.IsNullOrEmpty(Request["abertReali"])) abertoRealizado = Convert.ToBoolean(Request["abertReali"]);
            }

            if (codCedente != null)
            {
                vericaSelecao = comandoDb.ContadorReleatorioExtrato(datIni, datFim, codCedente, abertoRealizado);
                if (vericaSelecao > 0)
                {
                    ViewBag.HideTextBox = true;
                    var lista = new List<CamposRelModels>();

                    decimal VlrTotalDeb = 0;
                    decimal VlrTotalCred = 0;
                    decimal VlrTotalSaldo = 0;
                    if (abertoRealizado)
                    {
                        ViewBag.Aberto = true;
                    }
                    else ViewBag.Realizado = true;
                    foreach (DataRow row in comandoDb.ListaReleatorioExtrato(datIni, datFim, codCedente, abertoRealizado).Rows)
                    {
                        var extr = new CamposRelModels();

                        extr.Des_mov_Ext = (row["DES_MOV"]).ToString();
                        extr.Num_documento_Ext = (row["NUM_DOCUMENTO"]).ToString();
                        extr.Sacado_Ext = (row["NOM_RAZAO"]).ToString();
                        if (abertoRealizado == true)
                        {
                            extr.Deb_Ext = string.Format("{0:N}", Convert.ToDecimal(row["DEB"]));
                            extr.Cred_Ext = string.Format("{0:N}", Convert.ToDecimal(row["CRED"]));
                            extr.Vlr_original_Ext = string.Format("{0:N}", Convert.ToDecimal(row["VLR_PREVISTO"]));
                        }
                        else
                        {
                            extr.Deb_Ext = string.Format("{0:N}", Convert.ToDecimal(row["DEB"]));
                            extr.Cred_Ext = string.Format("{0:N}", Convert.ToDecimal(row["CRED"]));
                            extr.Tipo_Acerto_Ext = (row["TIPOACERTO"]).ToString();
                            extr.Dat_pagto_Ext = (row["DAT_PAGTO"]).ToString();
                            extr.Vlr_pagto_Ext = string.Format("{0:N}", Convert.ToDecimal(row["VLR_PAGTO"]));
                        }
                        extr.Dat_vencto_Ext = (row["DAT_VENCIMENTO"]).ToString();
                        extr.Vlr_partida_Ext = string.Format("{0:N}", Convert.ToDecimal(row["VLR_PARTIDA"]));
                        extr.Dat_partida_Ext = (row["DAT_PARTIDA"]).ToString();

                        VlrTotalDeb = VlrTotalDeb + Convert.ToDecimal(row["DEB"]);
                        VlrTotalCred = VlrTotalCred + Convert.ToDecimal(row["CRED"]);
                        VlrTotalSaldo = VlrTotalDeb - VlrTotalCred;

                        lista.Add(extr);
                    }
                    if (VlrTotalDeb > 0) ViewBag.ValorTotalDeb = "Total Débito: " + string.Format("{0:N}", VlrTotalDeb);
                    if (VlrTotalCred > 0) ViewBag.ValorTotalCred = "Total Crédito: " + string.Format("{0:N}", VlrTotalCred);
                    if (VlrTotalSaldo > 0) ViewBag.ValorTotalSaldo = "Total Saldo: " + string.Format("{0:N}", VlrTotalSaldo);
                    if (abertoRealizado == false) TempData["Realizado"] = "S";
                    ViewBag.Datini = datIni;
                    ViewBag.Datfim = datFim;
                    ViewBag.DatNow = "Processado: " + DateTime.Now;
                    //executar a aplicação no IIS                    
                    ViewBag.CaminhoPDF = "/" + pastaIIS + "/Relatorios/ExecExtr?pdf=true";
                    //executar a aplicação no Visual Studio
                    //ViewBag.CaminhoPDF = "/Relatorios/ExecExtr?pdf=true";

                    if (pdf != true)
                    {
                        int numeroRegistros = 15;
                        int numeroPagina = (pagina ?? 1);
                        return View(lista.ToPagedList(numeroPagina, numeroRegistros));
                    }
                    else
                    {
                        int pagNumero = 1;
                        DetetaCookies();
                        var relatorioPDF = new ViewAsPdf
                        {
                            ViewName = "ExecExtr",
                            IsGrayScale = false,
                            FileName = "ExecExtr.pdf",
                            Model = lista.ToPagedList(pagNumero, lista.Count)
                        };
                        return relatorioPDF;
                    }
                }
                else
                {
                    DetetaCookies();
                    if (abertoRealizado == true)
                    {
                        TempData["AlertaSelecao"] = "Nehum movimento encontrado em aberto!";
                    }
                    else TempData["AlertaSelecao"] = "Nehum movimento encontrado como realizado para esse período!";
                    return RedirectToAction("/ExecExtr");
                }
            }
            return View();
        }

        public ActionResult ExecCanho(int? pagina, Boolean? pdf)
        {
            string urlAnterior = "";
            //como deve vir na url para pegar os dadops ?cedente=1123&dt1=01/04/2020&dt2=26/09/2020&operVencto=true&whereSelecao=F.Sts_Anal_inicia=S
            int vericaSelecao = 0;
            if ((pdf == true) || (pagina != null))
            {
                bool existe = CookiesModels.Exists("Cedente");

                if (existe == true)
                {
                    codCedente = CookiesModels.Get("Cedente");
                    datIni = CookiesModels.Get("Datini");
                    datFim = CookiesModels.Get("Datfim");
                    operacao_Vencimento = Convert.ToBoolean(CookiesModels.Get("Operacaovencimento"));
                    whereCanhoto = CookiesModels.Get("Wherecanhoto");

                }
                else
                {
                    urlAnterior = System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
                    for (int count = 1; count <= 5; count++)
                    {
                        int Ini = urlAnterior.IndexOf('=');
                        int Fim = urlAnterior.IndexOf('&');
                        if (count == 1)
                        {
                            codCedente = urlAnterior.Substring((Ini + 1), ((Fim - 1) - Ini));
                            CookiesModels.Set("Cedente", codCedente);
                        }
                        if (count == 2)
                        {
                            datIni = urlAnterior.Substring((Ini + 1), ((Fim - 1) - Ini));
                            CookiesModels.Set("Datini", datIni);
                        }
                        if (count == 3)
                        {
                            datFim = urlAnterior.Substring((Ini + 1), ((Fim - 1) - Ini));
                            CookiesModels.Set("Datfim", datFim);
                        }
                        if (count == 4)
                        {
                            operacao_Vencimento = Convert.ToBoolean(urlAnterior.Substring((Ini + 1), ((Fim - 1) - Ini)));
                            CookiesModels.Set("Operacaovencimento", Convert.ToString(operacao_Vencimento));
                        }
                        if (count == 5)
                        {
                            whereCanhoto = urlAnterior.Substring((Ini + 1), (urlAnterior.Length - (Ini + 1)));
                            CookiesModels.Set("Wherecanhoto", whereCanhoto);
                        }
                        int tamanho = urlAnterior.Length;
                        urlAnterior = urlAnterior.Substring((Fim + 1), ((tamanho - Fim) - 1));
                    }
                }

            }
            else
            {
                DetetaCookies();
                if (!string.IsNullOrEmpty(Request["cedente"])) codCedente = Convert.ToString(Request["cedente"]);
                if (!string.IsNullOrEmpty(Request["dt1"])) datIni = Convert.ToString(Request["dt1"]);
                if (!string.IsNullOrEmpty(Request["dt2"])) datFim = Convert.ToString(Request["dt2"]);
                if (!string.IsNullOrEmpty(Request["operVencto"])) operacao_Vencimento = Convert.ToBoolean(Request["operVencto"]);
                if (!string.IsNullOrEmpty(Request["whereSelecao"])) whereCanhoto = Convert.ToString(Request["whereSelecao"]);
            }

            if (codCedente != null)
            {
                ViewBag.HideTextBox = true;
                vericaSelecao = comandoDb.ContadorReleatorioCanhoto(codCedente, datIni, datFim, operacao_Vencimento, whereCanhoto);
                if (vericaSelecao > 0)
                {
                    var lista = new List<CamposRelModels>();

                    whereCanhoto = whereCanhoto.Replace("=S", "= 'S'");
                    whereCanhoto = whereCanhoto.Replace("=N", "= 'N'");
                    whereCanhoto = whereCanhoto.Replace("=M", "= 'M'");
                    whereCanhoto = whereCanhoto.Replace("=F", "= 'F'");

                    foreach (DataRow row in comandoDb.ListaReleatorioCanhoto(codCedente, datIni, datFim, operacao_Vencimento, whereCanhoto).Rows)
                    {
                        var canhot = new CamposRelModels();

                        canhot.Sacado_Canho = (row["NOM_RAZAO"]).ToString();
                        canhot.Documento_Canho = (row["NUM_DOCUMENTO"]).ToString();
                        canhot.Vlr_Canho = string.Format("{0:N}", Convert.ToDecimal(row["VLR_PREVISTO"]));
                        canhot.Dat_vencto_Canho = (row["DAT_VENCIMENTO"]).ToString();
                        canhot.Dat_emissao_Canho = (row["DAT_EMISSAO"]).ToString();
                        canhot.Status_canhoto_Canho = (row["CANHOTO"]).ToString();
                        canhot.Status_nf_Canho = (row["NOTA"]).ToString();
                        canhot.Status_ordem_Canho = (row["ORDEM"]).ToString();
                        canhot.Sts_anal_canhot_Canho = (row["STS_ANAL_CANHOT"]).ToString();
                        canhot.Sts_anal_nf_Canho = (row["STS_ANAL_NF"]).ToString();
                        canhot.Sts_anal_inicia_Canho = (row["STS_ANAL_INICIA"]).ToString();

                        TotaisCanhoto(canhot.Sts_anal_canhot_Canho, canhot.Sts_anal_nf_Canho, canhot.Sts_anal_inicia_Canho, Convert.ToDecimal(row["VLR_PREVISTO"]));

                        lista.Add(canhot);
                    }

                    if (operacao_Vencimento == true)
                    {
                        TempData["OperVencto"] = "S";
                    }
                    else TempData["OperVencto"] = null;
                    ViewBag.Datini = datIni;
                    ViewBag.Datfim = datFim;
                    ViewBag.DatNow = "Processado: " + DateTime.Now;
                    //executar a aplicação no IIS                    
                    ViewBag.CaminhoPDF = "/" + pastaIIS + "/Relatorios/ExecCanho?pdf=true";
                    //executar a aplicação no Visual Studio
                    //ViewBag.CaminhoPDF = "/Relatorios/ExecCanho?pdf=true";

                    if (pdf != true)
                    {
                        int numeroRegistros = 15;
                        int numeroPagina = (pagina ?? 1);
                        return View(lista.ToPagedList(numeroPagina, numeroRegistros));
                    }
                    else
                    {
                        int pagNumero = 1;
                        DetetaCookies();
                        var relatorioPDF = new ViewAsPdf
                        {
                            ViewName = "ExecCanho",
                            IsGrayScale = false,
                            FileName = "ExecCanho.pdf",
                            Model = lista.ToPagedList(pagNumero, lista.Count)
                        };
                        return relatorioPDF;
                    }
                }
                else
                {
                    DetetaCookies();
                    TempData["AlertaSelecao"] = "Nehum canhoto encontrado para esse período!";
                    return RedirectToAction("/ExecCanho");
                }
            }
            return View();
        }

        private void DetetaCookies()
        {
            CookiesModels.Delete("Cedente", CookiesModels.Get("Cedente"));
            CookiesModels.Delete("Datini", CookiesModels.Get("Datini"));
            CookiesModels.Delete("Datfim", CookiesModels.Get("Datfim"));
            CookiesModels.Delete("Tpodoc", CookiesModels.Get("Tpodoc"));
            CookiesModels.Delete("tipdateliq", CookiesModels.Get("tipdateliq"));
            CookiesModels.Delete("Stsbaixados", CookiesModels.Get("Stsbaixados"));
            CookiesModels.Delete("Abertorealizado", CookiesModels.Get("Abertorealizado"));
            CookiesModels.Delete("Operacaovencimento", CookiesModels.Get("Operacaovencimento"));
            CookiesModels.Delete("Wherecanhoto", CookiesModels.Get("Wherecanhoto"));
        }

        public void Totais(int fundo_id, string originador, string operacao, decimal valor)
        {

            if ((fundo_id == 0) && (originador == "F") && (operacao == "F"))
            {
                g_totalFactoring = valor + g_totalFactoring;
                g_qtdFactoring = g_qtdFactoring + 1;
                if (g_totalFactoring > 0) ViewBag.totalFactoring = "Valor Total Factoring : " + string.Format("{0:N}", g_totalFactoring);
                if (g_qtdFactoring > 0) ViewBag.qtdFactoring = "Qtd. Total Factoring: " + Convert.ToString(g_qtdFactoring);
            }
            // total C.SIMPLES
            if ((fundo_id == 0) && (originador == "F") && (operacao == "C"))
            {
                g_totalCobSimples = valor + g_totalCobSimples;
                g_qtdCodSimples = g_qtdCodSimples + 1;
                if (g_totalCobSimples > 0) ViewBag.totalCobSimples = "Valor Total Cob. Simples: " + string.Format("{0:N}", g_totalCobSimples);
                if (g_qtdCodSimples > 0) ViewBag.qtdCodSimples = "Qtd. Total Cob. Simples: " + Convert.ToString(g_qtdCodSimples);
            }

            // total AQUISIÇÃO
            if ((fundo_id == 0) && (originador == "F") && (operacao == "A"))
            {
                g_totalAquisicao = valor + g_totalAquisicao;
                g_qtdAquisicao = g_qtdAquisicao + 1;
                if (g_totalAquisicao > 0) ViewBag.totalAquisicao = "Valor Total Aquisição: " + string.Format("{0:N}", g_totalAquisicao);
                if (g_qtdAquisicao > 0) ViewBag.qtdAquisicao = "Qtd. Total Aquisição: " + Convert.ToString(g_qtdAquisicao);
            }

            // total CONFISSÃO
            if ((fundo_id == 0) && (originador == "F") && (operacao == "P"))
            {
                g_totalConfissao = valor + g_totalConfissao;
                g_qtdConfissao = g_qtdConfissao + 1;
                if (g_totalConfissao > 0) ViewBag.totalConfissaos = "Valor Total Confissão: " + string.Format("{0:N}", g_totalConfissao);
                if (g_qtdConfissao > 0) ViewBag.qtdConfissao = "Qtd. Total Confissão: " + Convert.ToString(g_qtdConfissao);
            }

            // total Recompra
            if ((fundo_id == 0) && (originador == "F") && (operacao == "R"))
            {
                g_totalRecompra = valor + g_totalRecompra;
                g_qtdRecompra = g_qtdRecompra + 1;
                if (g_totalRecompra > 0) ViewBag.totalRecompra = "Valor Total Recompra: " + string.Format("{0:N}", g_totalRecompra);
                if (g_qtdRecompra > 0) ViewBag.qtdRecompra = "Qtd. Total Recompra: " + Convert.ToString(g_qtdRecompra);
            }

            // total FIDC
            if ((fundo_id == 1) && (originador == "I") && (operacao == "F"))
            {
                g_totalFidc = valor + g_totalFidc;
                g_qtdFidc = g_qtdFidc + 1;
                if (g_totalFidc > 0) ViewBag.totalFidc = "Valor Total FIDC: " + string.Format("{0:N}", g_totalFidc);
                if (g_qtdFidc > 0) ViewBag.qtdFidc = "Qtd. Total FIDC: " + Convert.ToString(g_qtdFidc);
            }

            // total FIDC/RECOMPRA
            if ((fundo_id == 1) && (originador == "I") && (operacao == "R"))
            {
                g_totalFidcRec = valor + g_totalFidcRec;
                g_qtdFidcRec = g_qtdFidcRec + 1;
                if (g_totalFidcRec > 0) ViewBag.totalFidcRec = "Valor Total FIDC/Rec.: " + string.Format("{0:N}", g_totalFidcRec);
                if (g_qtdFidcRec > 0) ViewBag.qtdFidcRec = "Qtd. Total FIDC/Rec.: " + Convert.ToString(g_qtdFidcRec);
            }

            // total FIDC/COB. SIMPLES
            if ((fundo_id == 1) && (originador == "I") && (operacao == "C"))
            {
                g_totalFidcCobSim = valor + g_totalFidcCobSim;
                g_qtdFidcCobSim = g_qtdFidcCobSim + 1;
                if (g_totalFidcCobSim > 0) ViewBag.totalFidcCobSim = "Valor Total FIDC/COB.SIMPLES: " + string.Format("{0:N}", g_totalFidcCobSim);
                if (g_qtdFidcCobSim > 0) ViewBag.qtdFidcCobSim = "Qtd. Total FIDC/COB.SIMPLES: " + Convert.ToString(g_qtdFidcCobSim);
            }

            // total C.SIMPLES - SEC.
            if ((fundo_id == 2) && (originador == "S") && (operacao == "C"))
            {
                g_totalCodSimplesSec = valor + g_totalCodSimplesSec;
                g_qtdCobSimplesSec = g_qtdCobSimplesSec + 1;
                if (g_totalCodSimplesSec > 0) ViewBag.totalCodSimplesSec = "Valor Total C.Simples - Sec.: " + string.Format("{0:N}", g_totalCodSimplesSec);
                if (g_qtdCobSimplesSec > 0) ViewBag.qtdCobSimplesSec = "Qtd. Total C.Simples - Sec.: " + Convert.ToString(g_qtdCobSimplesSec);
            }
            // total SECURITIZAÇÃO
            if ((fundo_id == 2) && (originador == "S") && (operacao == "F"))
            {
                g_totalSec = valor + g_totalSec;
                g_qtdSec = g_qtdSec + 1;
                if (g_totalSec > 0) ViewBag.totalSec = "Valor Total Securitização: " + string.Format("{0:N}", g_totalSec);
                if (g_qtdSec > 0) ViewBag.qtdSec = "Qtd. Total Securitização: " + Convert.ToString(g_qtdSec);
            }
            // total SEC/REC
            if ((fundo_id == 2) && (originador == "S") && (operacao == "R"))
            {
                g_totalSecRec = valor + g_totalSecRec;
                g_qtdSecRec = g_qtdSecRec + 1;
                if (g_totalSecRec > 0) ViewBag.totalSecRec = "Valor Total Sec./Rec.: " + string.Format("{0:N}", g_totalSecRec);
                if (g_qtdSecRec > 0) ViewBag.qtdSecRec = "Qtd. Total Sec./Rec.: " + Convert.ToString(g_qtdSecRec);
            }
            // total CONFISSÂO/SEC
            if ((fundo_id == 2) && (originador == "S") && (operacao == "P"))
            {
                g_totalConfSec = valor + g_totalConfSec;
                g_qtdConfSec = g_qtdConfSec + 1;
                if (g_totalConfSec > 0) ViewBag.totalConfSec = "Valor Total Conf./Sec.: " + string.Format("{0:N}", g_totalConfSec);
                if (g_qtdConfSec > 0) ViewBag.qtdConfSec = "Qtd. Total Conf./Sec.: " + Convert.ToString(g_qtdConfSec);
            }
            // total AQUISIÇÃO/SEC
            if ((fundo_id == 2) && (originador == "S") && (operacao == "A"))
            {
                g_totalAquiSec = valor + g_totalAquiSec;
                g_qtdAquiSec = g_qtdAquiSec + 1;
                if (g_totalAquiSec > 0) ViewBag.totalAquiSec = "Valor Total Aqui./Sec.: " + string.Format("{0:N}", g_totalAquiSec);
                if (g_qtdAquiSec > 0) ViewBag.qtdAquiSecc = "Qtd. Total Aqui./Sec: " + Convert.ToString(g_qtdAquiSec);
            }

        }

        public void TotaisCanhoto(string canhoto, string NF, string Ordem, decimal valor)
        {
            //Canhoto não confirmado
            if (canhoto == "N")
            {
                canhotoNE = valor + canhotoNE;
                QTD_canhotoNE = QTD_canhotoNE + 1;
                if (canhotoNE > 0) ViewBag.totalcanhotoNE = "Total de Canhotos Não Entregue: " + string.Format("{0:N}", canhotoNE) + " QTD: " + Convert.ToString(QTD_canhotoNE);
            }
            //Canhoto confirmado
            if (canhoto == "S")
            {
                canhotoE = valor + canhotoE;
                QTD_canhotoE = QTD_canhotoE + 1;
                if (canhotoE > 0) ViewBag.totalcanhotoE = "Total de Canhotos Entregue: " + string.Format("{0:N}", canhotoE) + " QTD: " + Convert.ToString(QTD_canhotoE);
            }
            //Min. Entregue
            if (canhoto == "M")
            {
                canhotoR = valor + canhotoR;
                QTD_canhotoR = QTD_canhotoR + 1;
                if (canhotoR > 0) ViewBag.totalcanhotoR = "Total de Canhotos em Romaneio: " + string.Format("{0:N}", canhotoR) + " QTD: " + Convert.ToString(QTD_canhotoR);
            }

            //Sem nota
            if (NF == "N")
            {
                semNF = valor + semNF;
                QTD_semNF = QTD_semNF + 1;
                if (semNF > 0) ViewBag.totalsemNF = "Total sem Nota Fiscal: " + string.Format("{0:N}", semNF) + " QTD: " + Convert.ToString(QTD_semNF);
            }
            //Com nota
            if (NF == "S")
            {
                comNF = valor + comNF;
                QTD_comNF = QTD_comNF + 1;
                if (comNF > 0) ViewBag.totalcomNF = "Total com Nota Fiscal: " + string.Format("{0:N}", comNF) + "QTD: " + Convert.ToString(QTD_comNF);
            }
            //Nota simples
            if (NF == "F")
            {
                simNF = valor + simNF;
                QTD_simNF = QTD_simNF + 1;
                if (simNF > 0) ViewBag.totalsimNF = "Total com Nota Fiscal Simples: " + string.Format("{0:N}", simNF) + " QTD: " + Convert.ToString(QTD_simNF);
            }

            //Com ordem
            if (Ordem == "S")
            {
                comOrdem = valor + comOrdem;
                QTD_comOrdem = QTD_comOrdem + 1;
                if (comOrdem > 0) ViewBag.totalcomOrdem = "Total com Ordem de Coleta: " + string.Format("{0:N}", comOrdem) + " QTD: " + Convert.ToString(QTD_comOrdem);
            }
            //Sem ordem
            if (Ordem == "N")
            {
                semOrdem = valor + semOrdem;
                QTD_semOrdem = QTD_semOrdem + 1;
                if (semOrdem > 0) ViewBag.totalsemOrdem = "Total sem Ordem de Coleta: " + string.Format("{0:N}", semOrdem) + " QTD: " + Convert.ToString(QTD_semOrdem);
            }

        }

    }
}