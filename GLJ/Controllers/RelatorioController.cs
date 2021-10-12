using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GLJ.Models.Relatorio;

namespace GLJ.Controllers
{
    public class RelatorioController : BaseControllerGeneric<PDRelatorio>
    {
        public ActionResult Index(int id)
        {

            this.ProcessData.FEEspelhoPedido.CodigoLoja = UsuarioLogin.CodigoLoja;
            this.ProcessData.FEEspelhoPedido.CodigoFilialLoja = (int)UsuarioLogin.CodigoFilialLoja;
            this.ProcessData.FEEspelhoPedido.CodigoPedido = id;

            Session["FEEspelhoPedido"] = this.ProcessData.FEEspelhoPedido;

            this.ProcessData.FEDadosLoja.CodigoLoja = UsuarioLogin.CodigoLoja;
            this.ProcessData.FEDadosLoja.CodigoFilialLoja = (int)UsuarioLogin.CodigoFilialLoja;

            Session["FEDadosLoja"] = this.ProcessData.FEDadosLoja;

            this.ProcessData.FEEspelhoPedidoDetalhe.CodigoPedido = id;
            Session["FEEspelhoPedidoDetalhe"] = this.ProcessData.FEEspelhoPedidoDetalhe;

            this.ProcessData.FEEspelhoPedidoPagamento.CodigoLoja = UsuarioLogin.CodigoLoja;
            this.ProcessData.FEEspelhoPedidoPagamento.CodigoFilialLoja = (int)UsuarioLogin.CodigoFilialLoja;
            this.ProcessData.FEEspelhoPedidoPagamento.CodigoPedido = id;
            Session["FEEspelhoPedidoPagamento"] = this.ProcessData.FEEspelhoPedidoPagamento;


            return View();
        }

        

    }
}
