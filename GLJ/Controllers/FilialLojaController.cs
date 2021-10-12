using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GLJ.Models.FilialLoja;
using GLJ.Models.BPFacade;
using GLJ.Filter.Entity;
using MVC.Controls;
using GLJ.Business.Entity;
using MVC.Controls.Grid;

namespace GLJ.Controllers
{
    public class FilialLojaController : BaseControllerGeneric<PDFilialLoja>
    {
        #region Métodos Chamada

        [AuthorizeOnion]
        public ActionResult Index()
        {
            this.LimparMensagens();
            return View();
        }

        [AuthorizeOnion]
        public ActionResult CadastroFilialLoja(int? id)
        {

            if (id.HasValue && id.GetValueOrDefault() > 0)
            {


                this.ProcessData.FilialLoja = BPFFilialLoja.Instance.ObterTodos(new FEFilialLoja() { Codigo = id.GetValueOrDefault(), CodigoLoja = UsuarioLogin.CodigoLoja }).ResultList.FirstOrDefault();
            }

            return View(this.ProcessData);
        }

        [AuthorizeOnion]
        public ActionResult ConsultarFilialLoja()
        {
            this.LimparMensagens();
            return View(this.ProcessData);
        }


        #endregion

        #region Metodos Especificos
        [HttpPost]
        public ActionResult Salvar(PDFilialLoja model)
        {
            try
            {

                model.FilialLoja.CodigoLoja = UsuarioLogin.CodigoLoja;

                this.ProcessData.FilialLoja = new BPFFilialLoja().Salvar(model.FilialLoja);
                base.AdicionarMensagemSucesso("Filial Salva Com sucesso!");
            }
            catch (Exception ex)
            {
                base.AdicionarMensagemErro(ex, ex.Message);
            }


            return RedirectToAction("CadastroFilialLoja", new { id = this.ProcessData.FilialLoja.Codigo });

        }

        #region Busca Loja
        public JsonResult BuscarFilialLoja(FEFilialLoja feFilialLoja)
        {

            feFilialLoja.CodigoLoja = UsuarioLogin.CodigoLoja;

            Session["FEFilialLoja"] = feFilialLoja;
            return Json(null);
        }

        [HttpGet]
        public JsonResult Buscar(SearchModel searchModel)
        {

            if (Session["FEFilialLoja"] == null)
            {
                Session["FEFilialLoja"] = new FEFilialLoja() { CodigoLoja = UsuarioLogin.CodigoLoja };
            }

            this.ProcessData.ListFilialLoja = BPFFilialLoja.Instance.ObterTodos(Session["FEFilialLoja"] as FEFilialLoja).ResultList;
            Session["listItem"] = new List<BEFilialLoja>();


            ((List<BEFilialLoja>)Session["listItem"]).AddRange(this.ProcessData.ListFilialLoja);

            IQueryable<BEFilialLoja> model = ((List<BEFilialLoja>)Session["listItem"]).AsQueryable();

            GridData gridData = model.ToGridData(searchModel, GridFilialLojaModel.BEFilialLojaColumns);
            return Json(gridData, JsonRequestBehavior.AllowGet);

        }
        #endregion
        #endregion
    }
}
