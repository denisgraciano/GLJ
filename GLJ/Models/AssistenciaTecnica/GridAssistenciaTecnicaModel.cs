using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GLJ.Business.Entity;
using MVC.Controls.Grid;

namespace GLJ.Models.AssistenciaTecnica
{
    public class GridAssistenciaTecnicaModel
    {

        #region Consulta Assistencia Técnica
        public static GridColumnModelList<BEAssistenciaTecnica> BEAssistenciaTecnica
        {
            get
            {
                return CreatePersonColumns();
            }
        }
        private static GridColumnModelList<BEAssistenciaTecnica> CreatePersonColumns()
        {
            GridColumnModelList<BEAssistenciaTecnica> cn = new GridColumnModelList<BEAssistenciaTecnica>();
            cn.Add(p => p.Codigo).SetAsPrimaryKey().SetEditable(false);
            cn.Add(p => p.Pedido.NumeroPedido).SetEditable(false);
            cn.Add(p => p.Pedido.Cliente.Nome).SetEditable(false);
            cn.Add(p => p.Pedido.DataEntrega).SetFormatter("dateFormatGrid,formatoptions:{srcformat:'d/m/Y'}").SetEditable(false);
            cn.Add(p => p.Status).SetFormatter("enumToString").SetColumnRenderer(new ComboColumnRenderer(string.Format(@"" + Utils.Root + "AssistenciaTecnica/CarregarStatus"))); ;
            cn.Items.Add(new GridColumnModel("action", "Ações", "50", "center", "", true).SetEditable(false));
            cn.Add(p => p.TrackingState).SetHidden(true);
            return cn;
        }
        #endregion

        #region Detalhe Pedido
        public static GridColumnModelList<BEAssistenciaTecnicaDetalhe> BEAssistenciaTecnicaDetalheColumns
        {
            get
            {
                return CreateAssistenciaTecnicaDetalheColumns();
            }
        }
        private static GridColumnModelList<BEAssistenciaTecnicaDetalhe> CreateAssistenciaTecnicaDetalheColumns()
        {
            GridColumnModelList<BEAssistenciaTecnicaDetalhe> cn = new GridColumnModelList<BEAssistenciaTecnicaDetalhe>();
            cn.Add(p => p.Codigo).SetHidden(true);
            cn.Add(p => p.CodigoProduto).SetAsPrimaryKey().SetHidden(true);
            cn.Add(p => p.Produto.Descricao).SetEditable(false);
            cn.Add(p => p.Produto.Fornecedor.Nome).SetEditable(false);
            cn.Add(p => p.Motivo).SetEditable(false);
            cn.Add(p => p.Descricao).SetEditable(false);
            return cn;
        }

        #endregion

    }
}