using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GLJ.Business.Entity;
using MVC.Controls.Grid;

namespace GLJ.Models.Estoque
{
    public class GridEstoqueModel
    {
        #region Busca Estoque
        public static GridColumnModelList<BEEstoque> BEEstoqueColumns
        {
            get
            {
                return CreatePersonColumns();
            }
        }
        private static GridColumnModelList<BEEstoque> CreatePersonColumns()
        {
            GridColumnModelList<BEEstoque> cn = new GridColumnModelList<BEEstoque>();
            cn.Add(p => p.Codigo).SetAsPrimaryKey().SetHidden(true);
            cn.Add(p => p.Produto.Codigo);
            cn.Add(p => p.Produto.Descricao);
            cn.Add(p => p.TotalEstoque);
            cn.Add(p => p.Livre);
            cn.Add(p => p.Vendido);
            cn.Add(p => p.TrackingState).SetHidden(true);
            return cn;
        }
        #endregion
    }
}