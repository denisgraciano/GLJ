using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GLJ.Models.Pedido;
using MVC.Controls;
using GLJ.Business.Entity;
using MVC.Controls.Grid;
using GLJ.Models.BPFacade;

namespace GLJ.Controllers
{
    public class PagamentoController : EntregaController
    {
        [HttpGet]
        public JsonResult BuscarPagamentos(SearchModel searchModel)
        {
            IQueryable<BEPagamentoPedido> model = ((PDPedido)Session["Pedido"]).PagamentosPedido.AsQueryable();
            GridData gridData = model.ToGridData(searchModel, GridPedidoModel.BEPagamentoPedidoColumns);
            return Json(gridData, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Salvar informações de forma de pagamento da ABA 2 (Pagamento)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SalvarPagamento(PDPedido model)
        {
            this.ProcessData = (PDPedido)Session["Pedido"];
            try
            {
                return Json(new JsonReturn(true));
            }
            catch (Exception ex)
            {
                return Json(new JsonReturn(false, ex.Message));
            }
        }

        [HttpPost]
        public JsonResult AdicionarPagamentoCartao(BEPagamentoCartao model)
        {
            this.ProcessData = (PDPedido)Session["Pedido"];
            try
            {
                if (ModelState.IsValid)
                {

                    if (model.NumeroParcelas >= 1)
                    {
                        decimal vlrParcela = model.ValorPagamento / model.NumeroParcelas;
                        model.ValorParcela = vlrParcela;
                    }
                    model.DataPagamento = DateTime.Now;

                    this.ProcessData.AdicionarPagamentoCartao(model);
                    return Json(new JsonReturn(true, "Pagamento em cartão adicionado.", new { this.ProcessData.ValorTotal, this.ProcessData.ValorRestantePagamento, this.ProcessData.TotalItens }));
                }
                else
                {
                    return Json(new JsonReturn(ModelState));
                }
            }
            catch (Exception ex)
            {
                return Json(false, ex.Message);
            }
        }

        [HttpPost]
        public JsonResult AdicionarPagamentoCheque(BEPagamentoCheque model)
        {
            this.ProcessData = (PDPedido)Session["Pedido"];
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.NumeroParcela > 1)
                    {
                        decimal vlrParcela = model.ValorPagamento / model.NumeroParcela;
                        model.ValorParcela = vlrParcela;
                    }


                    this.ProcessData.AdicionarPagamentoCheque(model);

                    return Json(new JsonReturn(true, "Pagamento em cheque adicionado.", new { this.ProcessData.ValorTotal, this.ProcessData.ValorRestantePagamento, this.ProcessData.TotalItens }));
                }
                else
                {
                    return Json(new JsonReturn(ModelState));
                }
            }
            catch (Exception ex)
            {
                return Json(false, ex.Message);
            }
        }

        [HttpPost]
        public JsonResult AdicionarPagamentoDinheiro(BEPagamentoDinheiro model)
        {
            this.ProcessData = (PDPedido)Session["Pedido"];
            try
            {
                if (ModelState.IsValid)
                {
                    this.ProcessData.AdicionarPagamentoDinheiro(model);
                    return Json(new JsonReturn(true, "Pagamento em dinheiro adicionado.", new { this.ProcessData.ValorTotal, this.ProcessData.ValorRestantePagamento, this.ProcessData.TotalItens }));
                }
                else
                {
                    return Json(new JsonReturn(ModelState));
                }
            }
            catch (Exception ex)
            {
                return Json(false, ex.Message);
            }
        }

        [HttpPost]
        public JsonResult AdicionarPagamentoDebito(BEPagamentoDebito model)
        {
            this.ProcessData = (PDPedido)Session["Pedido"];
            try
            {
                if (ModelState.IsValid)
                {
                    model.Valor = model.ValorPagamento;
                    model.DataPagamento = DateTime.Now;
                    this.ProcessData.AdicionarPagamentoDebito(model);
                    return Json(new JsonReturn(true, "Pagamento em Cartão de Débito adicionado.", new { this.ProcessData.ValorTotal, this.ProcessData.ValorRestantePagamento, this.ProcessData.TotalItens }));
                }
                else
                {
                    return Json(new JsonReturn(ModelState));
                }
            }
            catch (Exception ex)
            {
                return Json(false, ex.Message);
            }
        }

        public ActionResult CarregarTipoPagamento(string enumValue)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            var list = Onion.Util.EnumHelper.GetDescriptions<TipoPagamento>();

            if (string.IsNullOrEmpty(enumValue))
            {
                foreach (var m in list)
                    sb.Append(m.Value.ToString() + ":" + m.Name + ";");
                return Json(sb.ToString(), JsonRequestBehavior.AllowGet);
            }
            else
            {
                Int32 intEnum;
                Int32.TryParse(enumValue, out intEnum);
                return Json(new { Descricao = Onion.Util.EnumHelper.GetDescription<TipoPagamento>(intEnum) });
            }


        }

        [HttpPost]
        public ActionResult DeletarPagamento(BEPagamentoPedido bePagamentoPedido)
        {
            List<object> gridResponse = new List<object>();
            try
            {

//                IQueryable<BEPagamentoPedido> model = ((PDPedido)Session["Pedido"]).PagamentosPedido.Remove(bePagamentoPedido.Codigo);
                this.ProcessData.RemoverPagamento(bePagamentoPedido);
                gridResponse = GridResponse.CreateSuccess();


            }
            catch (Exception)
            {
                gridResponse = GridResponse.CreateFailure("Pagamento não pode ser removido.");
            }

            return Json(gridResponse, JsonRequestBehavior.AllowGet);
        }
    }
}
