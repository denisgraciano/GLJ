using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GLJ.Models.Configuracao;
using GLJ.Filter.Entity;
using GLJ.Business.Entity;
using GLJ.Models.BPFacade;
using MVC.Controls.Grid;
using MVC.Controls;

namespace GLJ.Controllers
{
    public class ConfiguracaoController : BaseControllerGeneric<PDConfiguracao>
    {
        #region Métodos de Chamada
        [AuthorizeOnion]
        public ActionResult Index()
        {
            this.LimparMensagens();
            return View();
        }

        [AuthorizeOnion]
        public ActionResult ConsultarConfiguracao()
        {
            base.LimparMensagens();
            return View(this.ProcessData);
        }

        public ActionResult CadastroConfiguracao(int? id)
        {
            if (id.HasValue && id.GetValueOrDefault() > 0)
            {

                this.ProcessData.Configuracao = BPFConfiguracaoLoja.Instance.ObterTodos(new FEConfiguracaoLoja() { Codigo = id.GetValueOrDefault(), CodigoLoja = UsuarioLogin.CodigoLoja }).ResultList.FirstOrDefault();
            }
            else
            {
                base.LimparMensagens();
            }

            return View(this.ProcessData);
        }

        #endregion

        #region Métodos Especificos

        [HttpGet]
        public JsonResult Buscar(SearchModel searchModel)
        {
            if (Session["FEConfiguracaoLoja"] == null) Session["FEConfiguracaoLoja"] = new FEConfiguracaoLoja() { CodigoLoja = UsuarioLogin.CodigoLoja };

            this.ProcessData.ListConfiguracao = BPFConfiguracaoLoja.Instance.ObterTodos(Session["FEConfiguracaoLoja"] as FEConfiguracaoLoja).ResultList;
            Session["listItem"] = new List<BEConfiguracaoLoja>();


            ((List<BEConfiguracaoLoja>)Session["listItem"]).AddRange(this.ProcessData.ListConfiguracao);

            IQueryable<BEConfiguracaoLoja> model = ((List<BEConfiguracaoLoja>)Session["listItem"]).AsQueryable();

            GridData gridData = model.ToGridData(searchModel, GridConfiguracaoModel.BEConfiguracaoColumns);
            return Json(gridData, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult Salvar(PDConfiguracao model)
        {
            try
            {
                model.Configuracao.CodigoLoja = UsuarioLogin.CodigoLoja;

                this.ProcessData.Configuracao = new BPFConfiguracaoLoja().Salvar(model.Configuracao);
                base.AdicionarMensagemSucesso("Configuração Salva Com sucesso!");
            }
            catch (Exception ex)
            {
                base.AdicionarMensagemErro(ex, ex.Message);
            }


            return RedirectToAction("ConsultarConfiguracao");

        }
        #endregion
    }
}
