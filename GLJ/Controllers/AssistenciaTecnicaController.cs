using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GLJ.Models.AssistenciaTecnica;
using MVC.Controls;
using GLJ.Filter.Entity;
using GLJ.Business.Entity;
using GLJ.Models.BPFacade;
using MVC.Controls.Grid;

namespace GLJ.Controllers
{
    public class AssistenciaTecnicaController : BaseControllerGeneric<PDAssistenciaTecnica>
    {
        #region Métodos de Chamada
        //[AuthorizeOnion]
        public ActionResult Index()
        {
            this.LimparMensagens();
            return View();
        }

        //[AuthorizeOnion]
        public ActionResult ConsultarAssistenciaTecnica()
        {
            return View(this.ProcessData);
        }

        //[AuthorizeOnion]
        public ActionResult CadastroAssistencia(int id)
        {

            FEAssistenciaTecnica filtro = new FEAssistenciaTecnica()
            {
                Codigo = id,
                CodigoLoja = UsuarioLogin.CodigoLoja,
                CodigoFilialLoja = (int)UsuarioLogin.CodigoFilialLoja
            };

            this.ProcessData.AssistenciaTecnica = BPFAssistenciaTecnica.Instance.ObterTodos(filtro).ResultList.FirstOrDefault();
            
            if (this.ProcessData.AssistenciaTecnica.Status != StatusAssistencia.Pendente)
            {
                base.AdicionarMensagemInfo("Não pode ser alterado uma Assistência Técnica já Enviada para o Forncedor.");
                return RedirectToAction("ConsultarAssistenciaTecnica");
            }


            Session["Assistencia"] = this.ProcessData;

            return View(this.ProcessData);
        }


        #endregion

        #region Métodos Especificos
        [HttpGet]
        public JsonResult Buscar(SearchModel searchModel)
        {

            if (Session["FEAssistenciaTecnica"] == null)
            {
                Session["FEAssistenciaTecnica"] = new FEAssistenciaTecnica()
                {
                    CodigoLoja = UsuarioLogin.CodigoLoja,
                    CodigoFilialLoja = (int)UsuarioLogin.CodigoFilialLoja
                };
            }
            
            this.ProcessData.ListAssistenciaTecnica = BPFAssistenciaTecnica.Instance.ObterTodos(Session["FEAssistenciaTecnica"] as FEAssistenciaTecnica).ResultList;
            Session["listItem"] = new List<BEAssistenciaTecnica>();


            ((List<BEAssistenciaTecnica>)Session["listItem"]).AddRange(this.ProcessData.ListAssistenciaTecnica);

            IQueryable<BEAssistenciaTecnica> model = ((List<BEAssistenciaTecnica>)Session["listItem"]).AsQueryable();

            GridData gridData = model.ToGridData(searchModel, GridAssistenciaTecnicaModel.BEAssistenciaTecnica);
            return Json(gridData, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult BuscarProduto(SearchModel searchModel)
        {
            IQueryable<BEAssistenciaTecnicaDetalhe> model = ((PDAssistenciaTecnica)Session["Assistencia"]).AssistenciaTecnicaDetalhe.AsQueryable();
            GridData gridData = model.ToGridData(searchModel, GridAssistenciaTecnicaModel.BEAssistenciaTecnicaDetalheColumns);
            return Json(gridData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CarregarStatus()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            var listMotivo = Onion.Util.EnumHelper.GetDescriptions<StatusAssistencia>();

            foreach (var m in listMotivo)
                sb.Append(m.Value.ToString() + ":" + m.Name + ";");

            return Json(sb.ToString(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ObterEnumDescricaoItem(String enumValue)
        {
            return Json(new { Descricao = PDAssistenciaTecnica.GetStatusDescricaoItem(enumValue) });
        }

        [HttpPost]
        public JsonResult AdicionarProduto(Int32 Codigo, string Descricao, string Motivo)
        {

            this.ProcessData = (PDAssistenciaTecnica)Session["Assistencia"];

            
            if (String.IsNullOrEmpty(Descricao))
            {
                base.AdicionarMensagemErro("Favor preencher uma descrição do Produto");
                return Json(new JsonReturn(false, "Favor preencher uma descrição do Produto"));
            }
            
            

            if (this.ProcessData.AssistenciaTecnicaDetalhe.FirstOrDefault(prop => prop.CodigoProduto == Codigo) == null)
            {
                BEProduto produto = BPFProduto.Instance.ObterTodos(new FEProduto() { Codigo = Codigo, CodigoLoja = UsuarioLogin.CodigoLoja, LoadType = Onion.Business.Entity.LoadType.Medium }).ResultList.SingleOrDefault();
                this.ProcessData.AssistenciaTecnicaDetalhe.Add(new BEAssistenciaTecnicaDetalhe()
                {
                    Produto = produto,
                    CodigoAssistenciaTecnica = this.ProcessData.AssistenciaTecnica.Codigo,
                    CodigoProduto  = produto.Codigo,
                    Descricao = Descricao,
                    Motivo = Motivo
                });
                return Json(new JsonReturn(true));
            }
            else
            {
                return Json(new JsonReturn(false, "Já existe este item na lista"));
            }
        }

        [HttpPost]
        public ActionResult DeletarProdutoAssistencia(BEAssistenciaTecnicaDetalhe AssistenciaTecnicaDetalhe)
        {
            this.ProcessData = (PDAssistenciaTecnica)Session["Assistencia"];

            this.ProcessData.AssistenciaTecnicaDetalhe.RemoveAll(p => p.CodigoProduto == AssistenciaTecnicaDetalhe.CodigoProduto);
                  
            return Json(GridResponse.CreateSuccess(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Salvar(PDAssistenciaTecnica model)
        {
            try
            {
                this.ProcessData = (PDAssistenciaTecnica)Session["Assistencia"];

                this.ProcessData.AssistenciaTecnica = new BPFAssistenciaTecnica().Salvar(this.ProcessData.AssistenciaTecnica, this.ProcessData.AssistenciaTecnicaDetalhe);
                base.AdicionarMensagemSucesso("Assistencia Técnica Salva e Enviada Com sucesso!");
            }
            catch (Exception ex)
            {
                base.AdicionarMensagemErro(ex, ex.Message);
            }

            return RedirectToAction("ConsultarAssistenciaTecnica");


        }

        public JsonResult AtualizarStatusAssistenciaItem(BEAssistenciaTecnica beAssistencia)
        {
            beAssistencia.CodigoLoja = UsuarioLogin.CodigoLoja;
            beAssistencia.CodigoFilialLoja = (int)UsuarioLogin.CodigoFilialLoja;
            beAssistencia = new BPFAssistenciaTecnica().AlterarStatus(beAssistencia);
            return Json(beAssistencia.Codigo);
        }


        #endregion
    }
}
