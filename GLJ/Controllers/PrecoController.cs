using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GLJ.Models.Preco;
using GLJ.Models.BPFacade;
using GLJ.Filter.Entity;
using MVC.Controls;
using MVC.Controls.Grid;
using GLJ.Business.Entity;

namespace GLJ.Controllers
{
    public class PrecoController : BaseControllerGeneric<PDPreco>
    {
        #region Métodos de Chamada

        [AuthorizeOnion]
        public ActionResult Index()
        {
            this.LimparMensagens();
            return View();
        }

        [AuthorizeOnion]
        public ActionResult TabelaPreco()
        {
            this.CarregaCombos();
            return View(this.ProcessData);
        }

        [AuthorizeOnion]
        public ActionResult CadastroPreco(int id)
        {
            if (id == 0)
                return RedirectToAction("TabelaPreco");

            this.LimparMensagens();

            this.ProcessData.Preco = BPFPreco.Instance.ObterTodos(new FEPreco() { Codigo = id, CodigoLoja = UsuarioLogin.CodigoLoja }).ResultList.FirstOrDefault();

            return View(this.ProcessData);
        }

        #endregion

        #region Métodos Especificos

        [HttpPost]
        public ActionResult Salvar(PDPreco model)
        {
            try
            {

                model.Preco.CodigoLoja = UsuarioLogin.CodigoLoja;

                this.ProcessData.Preco = new BPFPreco().Salvar(model.Preco);
                base.AdicionarMensagemSucesso("Preço Alterado Com sucesso!");
            }
            catch (Exception ex)
            {
                base.AdicionarMensagemErro(ex, ex.Message);
            }


            return RedirectToAction("TabelaPreco");

        }


        #region Busca Preco
        
        public JsonResult BuscarPreco(FEPreco fePreco)
        {
            fePreco.CodigoLoja = UsuarioLogin.CodigoLoja;

            Session["FEPreco"] = fePreco;
            return Json(null);
        }

        [HttpGet]
        public JsonResult Buscar(SearchModel searchModel)
        {
            if (Session["FEPreco"] == null)
            {
                Session["FEPreco"] = new FEPreco() { CodigoLoja = UsuarioLogin.CodigoLoja };
            }

            this.ProcessData.ListPreco = BPFPreco.Instance.ObterTodos(Session["FEPreco"] as FEPreco).ResultList;
            Session["listItem"] = new List<BEPreco>();


            ((List<BEPreco>)Session["listItem"]).AddRange(this.ProcessData.ListPreco);

            IQueryable<BEPreco> model = ((List<BEPreco>)Session["listItem"]).AsQueryable();

            GridData gridData = model.ToGridData(searchModel, GridPrecoModel.BEPrecoColumns);
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
