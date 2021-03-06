using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GLJ.Acesso.Business.Entity;
using GLJ.Business.Entity;
using GLJ.Models;
using GLJ.Models.BPFacade;
using GLJ.Filter.Entity;

namespace GLJ.Controllers
{
    public class BaseController : Controller
    {
        protected String MsgError
        {
            get
            {
                return (Session["msgError"] != null ? Session["msgError"].ToString() : string.Empty);
            }
        }

        protected String MsgSuccess
        {
            get
            {
                return (Session["msgSuccess"] != null ? Session["msgSuccess"].ToString() : string.Empty);
            }
        }

        protected String MsgInfo
        {
            get
            {
                return (Session["msgInfo"] != null ? Session["msgInfo"].ToString() : string.Empty);
            }
        }

        protected String MsgWarning
        {
            get
            {
                return (Session["msgWarning"] != null ? Session["msgWarning"].ToString() : string.Empty);
            }
        }

        protected BEUsuario UsuarioLogin
        {
            get { return (BEUsuario)Session["UsuarioLogin"]; }
            set
            {
                Session["UsuarioLogin"] = value;

                if (this.ProcessData != null)
                {
                    this.ProcessData.UsuarioLogin = Session["UsuarioLogin"] as BEUsuario;
                }
            }
        }

        public IProcessData ProcessData { get; set; }

        protected void AdicionarMensagemErro(Exception ex, String msg)
        {
            Session.Remove("msgError");
            Session.Add("msgError", string.Format("{0} : {1}", msg, ex.Message));
        }
        
        protected void AdicionarMensagemErro(String msg)
        {
            Session.Remove("msgError");
            Session.Add("msgError", string.Format("{0}", msg.Replace("\n", "<br />")));
        }

        protected void AdicionarMensagemSucesso(String msg)
        {
            Session.Remove("msgSuccess");
            Session.Add("msgSuccess", string.Format("{0}", msg));
        }

        protected void AdicionarMensagemInfo(String msg)
        {
            Session.Remove("msgInfo");
            Session.Add("msgInfo", string.Format("{0}", msg));
        }

        protected void AdicionarMensagemAtencao(String msg)
        {
            Session.Remove("msgWarning");
            Session.Add("msgWarning", string.Format("{0}", msg));
        }

        protected void LimparMensagens()
        {
            if (Session != null)
            {
                Session.Remove("msgError");
                Session.Remove("msgSuccess");
                Session.Remove("msgInfo");
                Session.Remove("msgWarning");
            }
        }

        protected virtual void CarregaListaUF(String siglaSelecionado = null)
        {
            var list = from uf in BPFEstado.Instance.ListaEstados()
                       select new
                       {
                           Codigo = uf.CodigoUF,
                           UF = uf.SiglaUF,
                           SiglaUF = uf.SiglaUF
                       };

            ViewData["Estados"] = new SelectList(list, "UF", "SiglaUF", (!string.IsNullOrEmpty(siglaSelecionado) ? siglaSelecionado : ""));

            List<BECidade> listCidade = new List<BECidade>();
            ViewData["Cidades"] = new SelectList(listCidade, "CodigoCidade", "NomeCidade");
        }

        protected virtual void CarregarCidade(String codigoEstado, int? idCidadeSelecionado = null)
        {
            List<BECidade> listCidade = BPFCidade.Instance.ObterTodos(new FECidade() { SiglaUF = codigoEstado }).ResultList;
            listCidade.OrderBy(e => e.CapitalCidade);
            ViewData["Cidades"] = new SelectList(listCidade, "CodigoCidade", "NomeCidade", (idCidadeSelecionado.HasValue ? idCidadeSelecionado.GetValueOrDefault() : 0));
        }

        [HttpGet]
        public JsonResult ObterCidade(String codigoEstado)
        {
            this.CarregarCidade(codigoEstado);
            return Json(ViewData["Cidades"], JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RemoverMensagem(string session)
        {
            if (Session != null)
            {
                Session.Remove(session);
            }
            return Json(null);
        }

        public void CarregaComboLoja()
        {


            var lojas = from f in BPFLoja.Instance.ObterTodos(new FELoja() { Codigo = UsuarioLogin.CodigoLoja }).ResultList
                           select new { CodigoLoja = f.Codigo, f.Nome };

            TempData["Lojas"] = new SelectList(lojas, "CodigoLoja", "Nome");

        }

        [HttpGet]
        public JsonResult AutoCompleteBanco(string term)
        {
            FEBanco feBanco = new FEBanco();
            feBanco.CodigoBCO = term;


            var produtos = from b in BPFBanco.Instance.ObterTodos(feBanco).ResultList
                           select new
                           {
                               label = b.CodigoBCO + " - " + b.NomeBanco,
                               value = b.CodigoBCO + " - " + b.NomeBanco,
                               Codigo = b.Codigo,
                           };

            return Json(produtos, JsonRequestBehavior.AllowGet);
        }

    }
}