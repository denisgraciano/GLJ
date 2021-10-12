using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GLJ.Models.Fornecedor;
using GLJ.Models.BPFacade;
using GLJ.Filter.Entity;
using MVC.Controls;
using GLJ.Business.Entity;
using MVC.Controls.Grid;

namespace GLJ.Controllers
{
    public class FornecedorController : BaseControllerGeneric<PDFornecedor>
    {
        #region Métodos Chamada
        [AuthorizeOnion]
        public ActionResult Index()
        {
            this.LimparMensagens();
            return View();
        }

        [AuthorizeOnion]
        public ActionResult CadastroFornecedor(int? id)
        {
            if (id.HasValue && id.GetValueOrDefault() > 0)
            {
                this.ProcessData.Fornecedor = BPFFornecedor.Instance.ObterTodos(new FEFornecedor() { Codigo = id.GetValueOrDefault(), CodigoLoja = UsuarioLogin.CodigoLoja }).ResultList.FirstOrDefault();
            }

            CarregaUFCidade();
            this.CarregaCombo();
            return View(this.ProcessData);
        }

        
        public ActionResult CadastroContatos(int? codigoFornecedor)
        {

            this.ProcessData.Contato.Fornecedor = BPFFornecedor.Instance.ObterTodos(new FEFornecedor() { Codigo = codigoFornecedor, CodigoLoja = UsuarioLogin.CodigoLoja }).ResultList.FirstOrDefault();

            return View(this.ProcessData);
        }

        [AuthorizeOnion]
        public ActionResult ConsultarFornecedor()
        {
            CarregaUFCidade();
            return View(this.ProcessData);
        }

        #endregion

        #region Metodos Especificos
        [HttpPost]
        public ActionResult Salvar(PDFornecedor model)
        {
            try
            {


                model.Fornecedor.CodigoLoja = UsuarioLogin.CodigoLoja;

                if (model.Fornecedor.CNPJ != "")
                {
                    model.Fornecedor.TipoCliente = TipoCliente.PessoaJuridica;
                }
                else
                {
                    model.Fornecedor.TipoCliente = TipoCliente.PessoaFisica;
                }

                this.ProcessData.Fornecedor = new BPFFornecedor().Salvar(model.Fornecedor);
                base.AdicionarMensagemSucesso("Fornecedor Salvo Com sucesso!");
            }
            catch (Exception ex)
            {
                base.AdicionarMensagemErro(ex, ex.Message);
            }


            return RedirectToAction("CadastroFornecedor", new { id = this.ProcessData.Fornecedor.Codigo });

        }


        #region Contato Fornecedor
        [HttpGet]
        public JsonResult ObterContatos(int codigoFornecedor, SearchModel searchModel)
        {
            IQueryable<BEContatoFornecedor> model = BPFContatoFornecedor.Instance.ObterTodos(new FEContatoFornecedor() { CodigoFornecedor = codigoFornecedor }).ResultList.AsQueryable();
            GridData gridData = model.ToGridData(searchModel, GridFornecedorModel.BEContatoForncedorColumns);

            return Json(gridData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SalvarContato(BEContatoFornecedor contato)
        {
            this.ProcessData.Contato = new BPFContatoFornecedor().SalvarContatoFornecedor(contato);

            base.AdicionarMensagemSucesso("Contato Adicionado com sucesso");
            if (Request.IsAjaxRequest())
                return Json(null);

            return RedirectToAction("CadastroContatos", new { codigoFornecedor = this.ProcessData.Contato.CodigoFornecedor });
        }
        #endregion

        #region Busca Fornecedor
        public JsonResult BuscarFornecedor(FEFornecedor feFornecedor)
        {

            feFornecedor.CodigoLoja = UsuarioLogin.CodigoLoja;

            Session["FEFornecedor"] = feFornecedor;
            return Json(null);
        }

        [HttpGet]
        public JsonResult Buscar(SearchModel searchModel)
        {

            if (Session["FEFornecedor"] == null) Session["FEFornecedor"] = new FEFornecedor() { CodigoLoja = UsuarioLogin.CodigoLoja };

            this.ProcessData.ListFornecedor = BPFFornecedor.Instance.ObterTodos(Session["FEFornecedor"] as FEFornecedor).ResultList;
            Session["listItem"] = new List<BEFornecedor>();


            ((List<BEFornecedor>)Session["listItem"]).AddRange(this.ProcessData.ListFornecedor);

            IQueryable<BEFornecedor> model = ((List<BEFornecedor>)Session["listItem"]).AsQueryable();

            GridData gridData = model.ToGridData(searchModel, GridFornecedorModel.BEFornecedorColumns);
            return Json(gridData, JsonRequestBehavior.AllowGet);

        }
        #endregion

        private void CarregaUFCidade()
        {
            if (!string.IsNullOrEmpty(this.ProcessData.Fornecedor.UF))
            {
                base.CarregaListaUF(this.ProcessData.Fornecedor.UF);

                base.CarregarCidade(this.ProcessData.Fornecedor.UF, this.ProcessData.Fornecedor.CodigoCidade);
            }
            else
            {
                base.CarregaListaUF();
            }
        }

        private void CarregaCombo()
        {
            var listTipoPessoa = Onion.Util.EnumHelper.GetDescriptions<TipoCliente>();

            TempData["TipoPessoa"] = new SelectList(listTipoPessoa, "Value", "Name");
        }
        #endregion

    }
}
