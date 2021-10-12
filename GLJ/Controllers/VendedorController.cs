using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GLJ.Models.Vendedor;
using GLJ.Models.BPFacade;
using GLJ.Acesso.Filter.Entity;
using GLJ.Filter.Entity;
using MVC.Controls;
using GLJ.Acesso.Business.Entity;
using MVC.Controls.Grid;

namespace GLJ.Controllers
{
    public class VendedorController : BaseControllerGeneric<PDVendedor>
    {

        #region Métodos Chamada
        [AuthorizeOnion]
        public ActionResult Index()
        {
            this.LimparMensagens();
            return View();
        }

        [AuthorizeOnion]
        public ActionResult CadastroVendedor(int? id)
        {
            if (id.HasValue && id.GetValueOrDefault() > 0)
            {

                this.ProcessData.Vendedor = BPFVendedor.Instance.ObterTodos(new FEVendedor() { Codigo = id.GetValueOrDefault(), CodigoLoja = UsuarioLogin.CodigoLoja }).ResultList.FirstOrDefault();
            }
            else
            {
                base.LimparMensagens();
            }

            this.CarregaCombos();
            return View(this.ProcessData);
        }

        [AuthorizeOnion]
        public ActionResult ConsultarVendedor()
        {
            this.CarregaCombos();
            base.LimparMensagens();
            return View(this.ProcessData);
        }

        #endregion

        #region Metodos Especificos
        [HttpPost]
        public ActionResult Salvar(PDVendedor model)
        {
            try
            {
                model.Vendedor.CodigoLoja = UsuarioLogin.CodigoLoja;

                this.ProcessData.Vendedor = new BPFVendedor().Salvar(model.Vendedor);
                base.AdicionarMensagemSucesso("Vendedor Salvo Com sucesso!");
            }
            catch (Exception ex)
            {
                base.AdicionarMensagemErro(ex, ex.Message);
            }


            return RedirectToAction("CadastroVendedor", new { id = this.ProcessData.Vendedor.Codigo });

        }

        #region Busca Vendedor
        public JsonResult BuscarVendedor(FEVendedor feVendedor)
        {
            feVendedor.CodigoLoja = UsuarioLogin.CodigoLoja;

            Session["FEVendedor"] = feVendedor;
            return Json(null);
        }

        [HttpGet]
        public JsonResult Buscar(SearchModel searchModel)
        {
            if (Session["FEVendedor"] == null)
            {
                Session["FEVendedor"] = new FEVendedor() { CodigoLoja = UsuarioLogin.CodigoLoja };
            }

            this.ProcessData.ListVendedor = BPFVendedor.Instance.ObterTodos(Session["FEVendedor"] as FEVendedor).ResultList;
            Session["listItem"] = new List<BEVendedor>();


            ((List<BEVendedor>)Session["listItem"]).AddRange(this.ProcessData.ListVendedor);

            IQueryable<BEVendedor> model = ((List<BEVendedor>)Session["listItem"]).AsQueryable();

            GridData gridData = model.ToGridData(searchModel, GridVendedorModel.BEVendedorColumns);
            return Json(gridData, JsonRequestBehavior.AllowGet);

        }
        #endregion

        private void CarregaCombos()
        {
            var usuarios = from f in BPFUsuario.Instance.ObterTodos(new FEUsuario() { Ativo = true, CodigoLoja = UsuarioLogin.CodigoLoja }).ResultList
                         select new { CodigoUsuario = f.Codigo, f.NomeUsuario };

            TempData["Usuarios"] = new SelectList(usuarios, "CodigoUsuario", "NomeUsuario");
        }
        #endregion
    }
}
