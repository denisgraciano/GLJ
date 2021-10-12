using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVC.Controls.Grid;
using GLJ.Business.Entity;

namespace GLJ.Models.Agenda
{
    public class GridPedidoDetalheModel
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
            cn.Add(p => p.QuantidadeProduto).SetEditable(false);
            return cn;
        }

        #endregion
    }
}