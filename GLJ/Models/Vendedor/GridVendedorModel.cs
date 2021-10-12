using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVC.Controls.Grid;
using GLJ.Acesso.Business.Entity;

namespace GLJ.Models.Vendedor
{
    public class GridVendedorModel
    {
        #region BuscaLoja
        public static GridColumnModelList<BEVendedor> BEVendedorColumns
        {
            get
            {
                return CreatePersonColumns();
            }
        }
        private static GridColumnModelList<BEVendedor> CreatePersonColumns()
        {
            GridColumnModelList<BEVendedor> cn = new GridColumnModelList<BEVendedor>();
            cn.Add(p => p.Codigo).SetAsPrimaryKey();
            cn.Add(p => p.Usuario.NomeUsuario);
            cn.Add(p => p.ComissaoVendedor);
            cn.Add(p => p.MargemVenda);
            cn.Add(p => p.TrackingState).SetHidden(true);
            return cn;
        }
        #endregion
    }
}