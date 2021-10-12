using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GLJ.Business.Entity;
using MVC.Controls.Grid;

namespace GLJ.Models.FilialLoja
{
    public class GridFilialLojaModel
    {
        #region BuscaLoja
        public static GridColumnModelList<BEFilialLoja> BEFilialLojaColumns
        {
            get
            {
                return CreatePersonColumns();
            }
        }
        private static GridColumnModelList<BEFilialLoja> CreatePersonColumns()
        {
            GridColumnModelList<BEFilialLoja> cn = new GridColumnModelList<BEFilialLoja>();
            cn.Add(p => p.Codigo).SetAsPrimaryKey();
            cn.Add(p => p.Nome);
            cn.Add(p => p.SiglaPedidoFilial);
            cn.Add(p => p.Ativo).SetFormatter("'checkbox'"); ;
            cn.Add(p => p.TrackingState).SetHidden(true);
            return cn;
        }
        #endregion
    }
}