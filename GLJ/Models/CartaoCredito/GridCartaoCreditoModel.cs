using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GLJ.Business.Entity;
using MVC.Controls.Grid;

namespace GLJ.Models.CartaoCredito
{
    public class GridCartaoCreditoModel
    {

        #region Busca Cartao Credito
        public static GridColumnModelList<BECartaoCredito> BECartaoCreditoColumns
        {
            get
            {
                return CreatePersonColumns();
            }
        }
        private static GridColumnModelList<BECartaoCredito> CreatePersonColumns()
        {
            GridColumnModelList<BECartaoCredito> cn = new GridColumnModelList<BECartaoCredito>();
            cn.Add(p => p.Codigo).SetAsPrimaryKey();
            cn.Add(p => p.Nome);
            cn.Add(p => p.Ativo).SetFormatter("'checkbox'"); 
            cn.Add(p => p.TrackingState).SetHidden(true);
            return cn;
        }
        #endregion

    }
}