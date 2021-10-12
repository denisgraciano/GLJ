using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVC.Controls.Grid;
using GLJ.Business.Entity;

namespace GLJ.Models.Pedido
{
    public class GridPedidoModel
    {
        #region Detalhe Pedido
        public static GridColumnModelList<BEPedidoDetalhe> BEPedidoDetalheColumns
        {
            get
            {
                return CreatePedidoDetalheColumns();
            }
        }
        private static GridColumnModelList<BEPedidoDetalhe> CreatePedidoDetalheColumns()
        {
            GridColumnModelList<BEPedidoDetalhe> cn = new GridColumnModelList<BEPedidoDetalhe>();
            cn.Add(p => p.Codigo).SetHidden(true);
            cn.Add(p => p.CodigoProduto).SetAsPrimaryKey().SetHidden(true);
            cn.Add(p => p.Produto.Descricao).SetEditable(false);
            cn.Add(p => p.QuantidadeProduto).SetEditable(true).SetCustomAttributes("editrules:{number:true},editoptions: {size:10, maxlength: 2}");
            cn.Add(p => p.ValorTotalProduto).SetEditable(false);
            return cn;
        }

        #endregion

        #region Pagamento

        public static GridColumnModelList<BEPagamentoPedido> BEPagamentoPedidoColumns
        {
            get
            {
                return CreatePagamentoPedidoColumns();
            }
        }
        private static GridColumnModelList<BEPagamentoPedido> CreatePagamentoPedidoColumns()
        {
            GridColumnModelList<BEPagamentoPedido> cn = new GridColumnModelList<BEPagamentoPedido>();
            cn.Add(p => p.Codigo).SetAsPrimaryKey().SetHidden(true);
            cn.Add(p => p.TipoPagamento).SetWidth("200").SetFormatter("enumTipoPagamentoToString").SetColumnRenderer(new ComboColumnRenderer(string.Format(@"" + Utils.Root + "Pedido/CarregarTipoPagamento")));
            cn.Add(p => p.ValorTotalPagamento).SetWidth("200").SetAlign("center");
            return cn;
        }

        #endregion

        #region Consulta Pedido
        public static GridColumnModelList<BEPedido> BEPedidoColumns
        {
            get
            {
                return CreatePedidoColumns();
            }
        }
        private static GridColumnModelList<BEPedido> CreatePedidoColumns()
        {
            GridColumnModelList<BEPedido> cn = new GridColumnModelList<BEPedido>();
            cn.Add(p => p.Codigo).SetAsPrimaryKey().SetHidden(true);
            cn.Add(p => p.NumeroPedido);
            cn.Add(p => p.Cliente.Nome);
            cn.Add(p => p.Cliente.CPF);
            cn.Add(p => p.DataCompra).SetFormatter("dateFormatGrid,formatoptions:{srcformat:'d/m/Y'}"); ;
            cn.Add(p => p.DataEntrega).SetFormatter("dateFormatGrid,formatoptions:{srcformat:'d/m/Y'}"); ;
            
            return cn;
        }

        #endregion

        #region Consulta Pedido
        public static GridColumnModelList<BEPedidoObservacao> BEPedidoObservacaoColumns
        {
            get
            {
                return CreatePedidoObservacaoColumns();
            }
        }
        private static GridColumnModelList<BEPedidoObservacao> CreatePedidoObservacaoColumns()
        {
            GridColumnModelList<BEPedidoObservacao> cn = new GridColumnModelList<BEPedidoObservacao>();
            cn.Add(p => p.Codigo).SetAsPrimaryKey().SetHidden(true);
            cn.Add(p => p.Pedido.NumeroPedido);
            cn.Add(p => p.Pedido.Cliente.Nome);
            cn.Add(p => p.Pedido.DataEntrega).SetFormatter("dateFormatGrid,formatoptions:{srcformat:'d/m/Y'}");
            cn.Add(p => p.Descricao);
            return cn;
        }

        #endregion



    }
}
