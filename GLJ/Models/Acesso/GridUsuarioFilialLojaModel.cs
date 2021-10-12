using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GLJ.Acesso.Business.Entity;
using MVC.Controls.Grid;

namespace GLJ.Models.Acesso
{
    public class GridUsuarioFilialLojaModel
    {
        #region Consulta Usuario
        public static GridColumnModelList<BEUsuarioFilialLoja> BEUsuarioFilialLojaColumns
        {
            get
            {
                return CreateUsuarioColumns();
            }
        }
        private static GridColumnModelList<BEUsuarioFilialLoja> CreateUsuarioColumns()
        {
            GridColumnModelList<BEUsuarioFilialLoja> cn = new GridColumnModelList<BEUsuarioFilialLoja>();
            cn.Add(p => p.Codigo).SetAsPrimaryKey();
            cn.Add(p => p.CodigoFilialLoja);
            cn.Add(p => p.CodigoUsuario);
            return cn;
        }
        #endregion
    }
}