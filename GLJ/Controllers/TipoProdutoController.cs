using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GLJ.Models.TipoProduto;
using GLJ.Models.BPFacade;
using GLJ.Filter.Entity;
using MVC.Controls;
using MVC.Controls.Grid;
using GLJ.Business.Entity;

namespace GLJ.Controllers
{
    public class TipoProdutoController : BaseControllerGeneric<PDTipoProduto>
    {
        #region Métodos Chamada

        [AuthorizeOnion]
        public ActionResult Index()
        {
            this.LimparMensagens();
            return View();
        }

        [AuthorizeOnion]
        public ActionResult CadastroTipoProduto(int?  id)
        {
            if (id.HasValue && id.GetValueOrDefault() > 0)
            {
                this.ProcessData.TipoProduto = BPFTipoProduto.Instance.ObterTodos(new FETipoProduto() { Codigo = id.GetValueOrDefault(), CodigoLoja = UsuarioLogin.CodigoLoja }).ResultList.FirstOrDefault();
            }

            return View(this.ProcessData);
        }

        [AuthorizeOnion]
        public ActionResult ConsultarTipoProduto()
        {
            this.LimparMensagens();
            return View(this.ProcessData);
        }

        #endregion
        
        #region Metodos Especificos


        [HttpPost]
        public ActionResult Salvar(PDTipoProduto model)
        {
            try
            {
                model.TipoProduto.CodigoLoja = UsuarioLogin.CodigoLoja;

                this.ProcessData.TipoProduto = new BPFTipoProduto().Salvar(model.TipoProduto);
                base.AdicionarMensagemSucesso("Tipo de Produto Salvo Com sucesso!");
                
               

            }
            catch (Exception ex)
            {
                base.AdicionarMensagemErro(ex, ex.Message);
            }

            return RedirectToAction("CadastroTipoProduto", new { id = this.ProcessData.TipoProduto.Codigo });
                     

        }

        #region Busca Tipo Produto
        public JsonResult BuscarTipoProduto(FETipoProduto feTipoProduto)
        {
            feTipoProduto.CodigoLoja = UsuarioLogin.CodigoLoja;

            Session["FETipoProduto"] = feTipoProduto;
            return Json(null);
        }

        [HttpGet]
        public JsonResult Buscar(SearchModel searchModel)
        {

            if (Session["FETipoProduto"] == null) Session["FETipoProduto"] = new FETipoProduto() { CodigoLoja = UsuarioLogin.CodigoLoja };

            this.ProcessData.ListTipoProduto = BPFTipoProduto.Instance.ObterTodos(Session["FETipoProduto"] as FETipoProduto).ResultList;
            Session["listItem"] = new List<BETipoProduto>();


            ((List<BETipoProduto>)Session["listItem"]).AddRange(this.ProcessData.ListTipoProduto);

            IQueryable<BETipoProduto> model = ((List<BETipoProduto>)Session["listItem"]).AsQueryable();

            GridData gridData = model.ToGridData(searchModel, GridTipoProdutoModel.BETipoProdutoColumns);
            return Json(gridData, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #endregion
    }
}
