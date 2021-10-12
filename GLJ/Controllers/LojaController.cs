using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GLJ.Models.Loja;
using GLJ.Models.BPFacade;
using GLJ.Filter.Entity;
using GLJ.Business.Entity;
using MVC.Controls;
using MVC.Controls.Grid;

namespace GLJ.Controllers
{
    public class LojaController : BaseControllerGeneric<PDCadastroLoja>
    {
        #region Métodos Chamada
        [AuthorizeOnion]
        public ActionResult Index()
        {
            this.LimparMensagens();
            return View();
        }

        [AuthorizeOnion]
        public ActionResult CadastroLoja(int? id)
        {
            if (id.HasValue && id.GetValueOrDefault() > 0)
            {
                this.ProcessData.Loja = BPFLoja.Instance.ObterTodos(new FELoja() { Codigo = id.GetValueOrDefault() }).ResultList.FirstOrDefault();
            }

            CarregaUFCidade();
            return View(this.ProcessData);
        }

        [AuthorizeOnion]
        public ActionResult ConsultarLoja()
        {
            CarregaUFCidade();
            return View(this.ProcessData);
        }

        public ActionResult CadastroContatos(int? codigoLoja)
        {
            this.ProcessData.Contato.Loja = BPFLoja.Instance.ObterTodos(new FELoja() { Codigo = codigoLoja }).ResultList.FirstOrDefault();

            return View(this.ProcessData);
        }
        #endregion
        
        #region "Metodos Especificos"

        #region Loja
        [HttpPost]
        public ActionResult Salvar(PDCadastroLoja model)
        {
            try
            {
                this.ProcessData.Loja = new BPFLoja().Salvar(model.Loja);
                base.AdicionarMensagemSucesso("Loja Salva Com sucesso!");
            }
            catch (Exception ex)
            {
                base.AdicionarMensagemErro(ex, ex.Message);
            }


            return RedirectToAction("CadastroLoja", new { id = this.ProcessData.Loja.Codigo });

        }
        #endregion

        #region Contato

        [HttpGet]
        public JsonResult ObterContatos(int codigoLoja, SearchModel searchModel)
        {
            IQueryable<BEContatoLoja> model = BPFContatoLoja.Instance.ObterTodos(new FEContatoLoja() { CodigoLoja = codigoLoja }).ResultList.AsQueryable();
            GridData gridData = model.ToGridData(searchModel, GridLojaModel.BEContatoLojaColumns);

            return Json(gridData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SalvarContato(BEContatoLoja contato)
        {
            this.ProcessData.Contato = new BPFContatoLoja().SalvarItemProduto(contato);

            base.AdicionarMensagemSucesso("Contato Adicionado com sucesso");
            if (Request.IsAjaxRequest())
                return Json(null);

            return RedirectToAction("CadastroContatos", new { codigoLoja = this.ProcessData.Contato.CodigoLoja });
        }
        #endregion

        #region Busca Loja
        public JsonResult BuscarLoja(FELoja feLoja)
        {
            Session["FELoja"] = feLoja;
            return Json(null);
        }

        [HttpGet]
        public JsonResult Buscar(SearchModel searchModel)
        {
            if (Session["FELoja"] == null) Session["FELoja"] = new FELoja();

            this.ProcessData.listLoja = BPFLoja.Instance.ObterTodos(Session["FELoja"] as FELoja).ResultList;
            Session["listItem"] = new List<BELoja>();


            ((List<BELoja>)Session["listItem"]).AddRange(this.ProcessData.listLoja);

            IQueryable<BELoja> model = ((List<BELoja>)Session["listItem"]).AsQueryable();

            GridData gridData = model.ToGridData(searchModel, GridLojaModel.BELojaColumns);
            return Json(gridData, JsonRequestBehavior.AllowGet);

        }
        #endregion

        private void CarregaUFCidade()
        {
            if (!string.IsNullOrEmpty(this.ProcessData.Loja.UF))
            {
                base.CarregaListaUF(this.ProcessData.Loja.UF);

                base.CarregarCidade(this.ProcessData.Loja.UF, this.ProcessData.Loja.CodigoCidade);
            }
            else
            {
                base.CarregaListaUF();
            }
        }
        #endregion

    }
}
