using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GLJ.Models.CartaoCredito;
using GLJ.Models.BPFacade;
using GLJ.Filter.Entity;
using MVC.Controls;
using GLJ.Business.Entity;
using MVC.Controls.Grid;

namespace GLJ.Controllers
{
    public class CartaoCreditoController : BaseControllerGeneric<PDCartaoCredito>
    {

        #region Métodos Chamadas

        [AuthorizeOnion]
        public ActionResult Index()
        {
            this.LimparMensagens();
            return View();
        }
        
        [AuthorizeOnion]
        public ActionResult CadastroCartaoCredito(int? id)
        {
            if (id.HasValue && id.GetValueOrDefault() > 0)
            {
                this.ProcessData.CartaoCredito = BPFCartaoCredito.Instance.ObterTodos(new FECartaoCredito() { Codigo = id.GetValueOrDefault(), CodigoLoja = UsuarioLogin.CodigoLoja }).ResultList.FirstOrDefault();
            }

            return View(this.ProcessData);
        }

        [AuthorizeOnion]
        public ActionResult ConsultarCartaoCredito()
        {
            this.LimparMensagens();
            return View(this.ProcessData);
        }
        #endregion

        #region Metodos Especificos


        [HttpPost]
        public ActionResult Salvar(PDCartaoCredito model)
        {
            try
            {
                model.CartaoCredito.CodigoLoja = UsuarioLogin.CodigoLoja;

                this.ProcessData.CartaoCredito = new BPFCartaoCredito().Salvar(model.CartaoCredito);
                base.AdicionarMensagemSucesso("Cartão de Crédito Salvo Com sucesso!");
            }
            catch (Exception ex)
            {
                base.AdicionarMensagemErro(ex, ex.Message);
            }

            return RedirectToAction("CadastroCartaoCredito", new { id = this.ProcessData.CartaoCredito.Codigo });


        }

        #region Busca Tipo Produto
        public JsonResult BuscarCartaoCredito(FECartaoCredito feCartaoCredito)
        {
            feCartaoCredito.CodigoLoja = UsuarioLogin.CodigoLoja;

            Session["FECartaoCredito"] = feCartaoCredito;
            return Json(null);
        }

        [HttpGet]
        public JsonResult Buscar(SearchModel searchModel)
        {

            if (Session["FECartaoCredito"] == null) Session["FECartaoCredito"] = new FECartaoCredito() { CodigoLoja = UsuarioLogin.CodigoLoja };

            this.ProcessData.ListCartaoCredito = BPFCartaoCredito.Instance.ObterTodos(Session["FECartaoCredito"] as FECartaoCredito).ResultList;
            Session["listItem"] = new List<BECartaoCredito>();


            ((List<BECartaoCredito>)Session["listItem"]).AddRange(this.ProcessData.ListCartaoCredito);

            IQueryable<BECartaoCredito> model = ((List<BECartaoCredito>)Session["listItem"]).AsQueryable();

            GridData gridData = model.ToGridData(searchModel, GridCartaoCreditoModel.BECartaoCreditoColumns);
            return Json(gridData, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #endregion



    }
}
