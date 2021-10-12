using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GLJ.Models.Agenda;
using GLJ.Models.BPFacade;
using GLJ.Filter.Entity;
using GLJ.Business.Entity;
using MVC.Controls;
using MVC.Controls.Grid;
using GLJ.Models.Pedido;

namespace GLJ.Controllers
{
    public class AgendaController : BaseControllerGeneric<PDAgenda>
    {
        public ActionResult Index()
        {
            this.CarregaCombos();
            return View();
        }

        public JsonResult BuscarAgenda(double? start, double? end)
        {
            FEAgendaEntrega feAgendaEntrega = new FEAgendaEntrega() 
            { 
                CodigoLoja = UsuarioLogin.CodigoLoja
            };
            List<AgendaModelo> listAgenda = new BPFAgendaEntrega().ObterAgendaPedidos(feAgendaEntrega);


            return Json(listAgenda.ToArray(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SalvarEvento(int codigoAgenda, int codigoPedido,string data)
        {
            try
            {


               DateTime dataEntrega = new DateTime();
               if (DateTime.TryParse(data, out dataEntrega))
               {
                   FEAgendaEntrega feAgendaEntrega = new FEAgendaEntrega { Codigo = codigoAgenda, CodigoPedido = codigoPedido, CodigoLoja = UsuarioLogin.CodigoLoja };
                   new BPFAgendaEntrega().AlterarDataEntrega(feAgendaEntrega, dataEntrega);
                   return Json(new JsonReturn(true,"Data Altera com sucesso"),JsonRequestBehavior.AllowGet);
               }
               else
               {
                   return Json(new JsonReturn(false,"Erro ao alterar a data de entrega"), JsonRequestBehavior.AllowGet);
               }
            }
            catch (Exception ex)
            {
                return Json(new JsonReturn(false,ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult BuscaEvento(int codigoAgenda, int codigoPedido, int codigoCliente)
        {
            try
            {
                FEPedidoDetalhe fePedidoDetalhe = new FEPedidoDetalhe()
                {
                    CodigoPedido = codigoPedido
                };

                Session["FEPedidoDetalhe"] = fePedidoDetalhe;

                FEPedidoObservacao fePedidoObservacao = new FEPedidoObservacao()
                {
                    CodigoPedido = codigoPedido
                };

                Session["FEPedidoObservacao"] = fePedidoObservacao;

                FEDadosCliente feDadosCliente = new FEDadosCliente()
                {
                    CodigoCliente = codigoCliente
                };

                

                var DetalheCliente = from d in BPFDadosCliente.Instance.ObterTodos(feDadosCliente).ResultList
                                     select new
                                     {
                                         Endereco = d.Endereco,
                                         Bairro = d.Bairro,
                                         CEP = d.CEP,
                                         Referencia =  d.Referencia
                                     };
                
                if (DetalheCliente.Count() > 0)
                {
                    return Json(new JsonReturn(true, "", DetalheCliente.FirstOrDefault()), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new JsonReturn(false, null), JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json(new JsonReturn(false, ex.Message), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult SalvarAlteracao(int? CodAgenda, int? StatusAgenda, int? CodigoPedido, string PlacaVeiculo, string Motorista)
        {
            try
            {
                BEAgendaEntrega entidade = BPFAgendaEntrega.Instance.ObterTodos(new FEAgendaEntrega() { Codigo = CodAgenda, CodigoPedido = CodigoPedido }).ResultList.FirstOrDefault();

                entidade.StatusAgenda = (StatusAgendaEntrega)StatusAgenda;
                entidade.PlacaVeiculo = PlacaVeiculo;
                entidade.MotoristaVeiculo = Motorista;

                BPFAgendaEntrega.Instance.Salvar(entidade);

                this.CarregaCombos();

                return Json(new JsonReturn
                {
                    Success = true,
                    Mensage = "Alteração Realizada Com Sucesso.",
                });


            }
            catch (Exception ex)
            {
                return Json(new JsonReturn(false, ex.Message));
            }
        }

        [HttpGet]
        public JsonResult Buscar(int? idPedido, SearchModel searchModel)
        {

            if (Session["FEPedidoDetalhe"] == null) Session["FEPedidoDetalhe"] = new FEPedidoDetalhe() { CodigoPedido = (int)idPedido };

            this.ProcessData.PedidoDetalhe = BPFPedidoDetalhe.Instance.ObterTodos(Session["FEPedidoDetalhe"] as FEPedidoDetalhe).ResultList;
            Session["listItem"] = new List<BEPedidoDetalhe>();


            ((List<BEPedidoDetalhe>)Session["listItem"]).AddRange(this.ProcessData.PedidoDetalhe);

            IQueryable<BEPedidoDetalhe> model = ((List<BEPedidoDetalhe>)Session["listItem"]).AsQueryable();

            GridData gridData = model.ToGridData(searchModel, GridPedidoDetalheModel.BEPedidoDetalheColumns);
            return Json(gridData, JsonRequestBehavior.AllowGet);

        }


        [HttpGet]
        public JsonResult ObterPedidoObservacao(int? idPedido, SearchModel searchModel)
        {

            if (Session["FEPedidoObservacao"] == null) Session["FEPedidoObservacao"] = new FEPedidoObservacao() { CodigoPedido = (int)idPedido };

            this.ProcessData.PedidoObservacao = BPFPedidoObservacao.Instance.ObterTodos(Session["FEPedidoObservacao"] as FEPedidoObservacao).ResultList;
            Session["listObs"] = new List<BEPedidoObservacao>();

            ((List<BEPedidoObservacao>)Session["listObs"]).AddRange(this.ProcessData.PedidoObservacao);

            IQueryable<BEPedidoObservacao> model = ((List<BEPedidoObservacao>)Session["listObs"]).AsQueryable();

            GridData gridData = model.ToGridData(searchModel, GridPedidoModel.BEPedidoObservacaoColumns);

            return Json(gridData, JsonRequestBehavior.AllowGet);
        }


        private void CarregaCombos()
        {
            var listTipo = Onion.Util.EnumHelper.GetDescriptions<StatusAgendaEntrega>();
            TempData["Status"] = new SelectList(listTipo, "Value", "Name");

        }

    }
}
