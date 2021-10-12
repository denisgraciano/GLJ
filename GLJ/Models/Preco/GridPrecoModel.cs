using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVC.Controls.Grid;
using GLJ.Business.Entity;

namespace GLJ.Models.Preco
{
    public class GridPrecoModel
    {
        #region Busca Preco
        public static GridColumnModelList<BEPreco> BEPrecoColumns
        {
            get
            {
                return CreatePersonColumns();
            }
        }
        private static GridColumnModelList<BEPreco> CreatePersonColumns()
        {
            GridColumnModelList<BEPreco> cn = new GridColumnModelList<BEPreco>();
            cn.Add(p => p.Codigo).SetAsPrimaryKey().SetHidden(true);
            cn.Add(p => p.Produto.Codigo);
            cn.Add(p => p.Produto.Descricao);
            cn.Add(p => p.PrecoVenda);
            cn.Add(p => p.Margem);
            cn.Add(p => p.TrackingState).SetHidden(true);
            return cn;
        }
        #endregion
    }
}