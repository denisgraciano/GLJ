using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GLJ.Models.Estoque;
using GLJ.Filter.Entity;
using GLJ.Models.BPFacade;
using MVC.Controls;
using GLJ.Business.Entity;
using MVC.Controls.Grid;

namespace GLJ.Controllers
{
    public class EstoqueController : BaseControllerGeneric<PDEstoque>
    {
        #region Métodos Chamadas

        [AuthorizeOnion]
        public ActionResult Index()
        {
            this.LimparMensagens();
            return View();
        }

        [AuthorizeOnion]
        public ActionResult ConsultarEstoque()
        {
            this.CarregaCombos();
            
            return View(this.ProcessData);
        }

        [AuthorizeOnion]
        public ActionResult CadastroEstoque(int id)
        {
            if (id == 0)
                return RedirectToAction("ConsultarEstoque");

            this.LimparMensagens();


            this.ProcessData.Estoque = BPFEstoque.Instance.ObterTodos(new FEEstoque() { Codigo = id, CodigoLoja = UsuarioLogin.CodigoLoja }).ResultList.FirstOrDefault();

            return View(this.ProcessData);
        }

        #endregion

        #region Métodos Espicífico

        [HttpPost]
        public ActionResult Salvar(PDEstoque model)
        {
            try
            {
                model.Estoque.CodigoLoja = UsuarioLogin.CodigoLoja;

                this.ProcessData.Estoque = new BPFEstoque().Salvar(model.Estoque);
                base.AdicionarMensagemSucesso("Estoque Alterado Com sucesso!");
            }
            catch (Exception ex)
            {
                base.AdicionarMensagemErro(ex, ex.Message);
            }


            return RedirectToAction("ConsultarEstoque");

        }

        #region Busca Estoque
        public JsonResult BuscarEstoque(FEEstoque feEstoque)
        {

            feEstoque.CodigoLoja = UsuarioLogin.CodigoLoja;

            Session["FEEstoque"] = feEstoque;
            return Json(null);
        }

        [HttpGet]
        public JsonResult Buscar(SearchModel searchModel)
        {

            if (Session["FEEstoque"] == null)
            {

                Session["FEEstoque"] = new FEEstoque() { CodigoLoja = UsuarioLogin.CodigoLoja };
            }

            this.ProcessData.ListEstoque = BPFEstoque.Instance.ObterTodos(Session["FEEstoque"] as FEEstoque).ResultList;
            Session["listItem"] = new List<BEEstoque>();


            ((List<BEEstoque>)Session["listItem"]).AddRange(this.ProcessData.ListEstoque);

            IQueryable<BEEstoque> model = ((List<BEEstoque>)Session["listItem"]).AsQueryable();

            GridData gridData = model.ToGridData(searchModel, GridEstoqueModel.BEEstoqueColumns);
            return Json(gridData, JsonRequestBehavior.AllowGet);

        }
        #endregion

        protected void CarregaCombos()
        {

            var produto = from f in BPFProduto.Instance.ObterTodos(new FEProduto() { CodigoLoja = UsuarioLogin.CodigoLoja }).ResultList
                          select new { Codigo = f.Codigo, f.Descricao };

            TempData["Produto"] = new SelectList(produto, "Codigo", "Descricao");
        }

        #endregion

    }
}
