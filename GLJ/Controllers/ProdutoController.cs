using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GLJ.Models.Produto;
using GLJ.Models.BPFacade;
using GLJ.Filter.Entity;
using Onion.Util;
using GLJ.Business.Entity;
using MVC.Controls;
using MVC.Controls.Grid;

namespace GLJ.Controllers
{
    public class ProdutoController : BaseControllerGeneric<PDProduto>
    {
        #region Métodos Chamadas
        [AuthorizeOnion]
        public ActionResult Index()
        {
            this.LimparMensagens();
            return View();
        }

        [AuthorizeOnion]
        public ActionResult CadastroProduto(int? id)
        {

            this.CarregaCombos();

            if (id.HasValue && id.GetValueOrDefault() > 0)
            {

                this.ProcessData.Produto = BPFProduto.Instance.ObterTodos(new FEProduto() { Codigo = id.GetValueOrDefault(), CodigoLoja = UsuarioLogin.CodigoLoja }).ResultList.FirstOrDefault();
            }

            return View(this.ProcessData);
        }

        [AuthorizeOnion]
        public ActionResult ConsultarProduto()
        {
            this.LimparMensagens();
            this.CarregaCombos();
            
            return View(this.ProcessData);
        }

        #endregion

        #region Metodos Especificos
        [HttpPost]
        public ActionResult Salvar(PDProduto model)
        {
            try
            {
                model.Produto.CodigoLoja = UsuarioLogin.CodigoLoja;

                this.ProcessData.Produto = new BPFProduto().Salvar(model.Produto);
                base.AdicionarMensagemSucesso("Produto Salvo Com sucesso!");
            }
            catch (Exception ex)
            {
                base.AdicionarMensagemErro(ex, ex.Message);
            }


            return RedirectToAction("ConsultarProduto");

        }

        [HttpGet]
        public JsonResult AutoCompleteProduto(string term)
        {
            FEProduto feProduto = new FEProduto();
            feProduto.CodigoLoja = UsuarioLogin.CodigoLoja;

            feProduto.Descricao = term;
            var produtos = from p in BPFProduto.Instance.ObterTodos(feProduto).ResultList
                           select new
                           {
                               label = p.Descricao,
                               value = p.Descricao,
                               Codigo = p.Codigo,
                           };

            return Json(produtos,JsonRequestBehavior.AllowGet);
        }

        #region Busca Produto

        [HttpPost]
        public JsonResult BuscarProdutoPopUp(string Descricao, string CodigoFornecedor)
        {
            FEProduto feProduto = new FEProduto { CodigoLoja = UsuarioLogin.CodigoLoja, Descricao = Descricao, CodigoFornecedorProduto = CodigoFornecedor};
            Session["FEProduto"] = feProduto;
            return Json(null);
        }



        public JsonResult BuscarProduto(FEProduto feProduto)
        {
            feProduto.CodigoLoja = UsuarioLogin.CodigoLoja;

            Session["FEProduto"] = feProduto;
            return Json(null);
        }


        [HttpGet]
        public JsonResult Buscar(SearchModel searchModel)
        {

            if (Session["FEProduto"] == null) Session["FEProduto"] = new FEProduto() { CodigoLoja = UsuarioLogin.CodigoLoja };

            this.ProcessData.ListProduto = BPFProduto.Instance.ObterTodos(Session["FEProduto"] as FEProduto).ResultList;
            Session["listItem"] = new List<BEProduto>();


            ((List<BEProduto>)Session["listItem"]).AddRange(this.ProcessData.ListProduto);

            IQueryable<BEProduto> model = ((List<BEProduto>)Session["listItem"]).AsQueryable();

            GridData gridData = model.ToGridData(searchModel, GridProdutoModel.BEProdutoColumns);
            return Json(gridData, JsonRequestBehavior.AllowGet);

        }
        #endregion

        protected void CarregaCombos()
        {

            var fornecedor = from f in BPFFornecedor.Instance.ObterTodos(new FEFornecedor() { CodigoLoja = UsuarioLogin.CodigoLoja }).ResultList
                             select new { Codigo = f.Codigo, f.Nome };

            TempData["Fornecedor"] = new SelectList(fornecedor, "Codigo", "Nome");


            var tipoProduto = from f in BPFTipoProduto.Instance.ObterTodos(new FETipoProduto() { CodigoLoja = UsuarioLogin.CodigoLoja }).ResultList
                              select new { Codigo = f.Codigo, f.Descricao };

            TempData["TipoProduto"] = new SelectList(tipoProduto, "Codigo", "Descricao");


            var listUnidade = EnumHelper.GetDescriptions<UnidadeMedida>();

            TempData["Unidade"] = new SelectList(listUnidade, "Value", "Name");

        }

        #endregion

    }
}
