using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVC.Controls.Grid;
using GLJ.Business.Entity;

namespace GLJ.Models.TipoProduto
{
    public class GridTipoProdutoModel
    {
        #region Busca Tipo Produto
        public static GridColumnModelList<BETipoProduto> BETipoProdutoColumns
        {
            get
            {
                return CreatePersonColumns();
            }
        }
        private static GridColumnModelList<BETipoProduto> CreatePersonColumns()
        {
            GridColumnModelList<BETipoProduto> cn = new GridColumnModelList<BETipoProduto>();
            cn.Add(p => p.Codigo).SetAsPrimaryKey();
            cn.Add(p => p.Descricao);
            cn.Add(p => p.TrackingState).SetHidden(true);
            return cn;
        }
        #endregion
    }
}