using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVC.Controls.Grid;
using GLJ.Acesso.Business.Entity;

namespace GLJ.Models.Acesso
{
    public class GridGrupoPermissaoModel
    {

        #region Consulta Grupo Permissao
        public static GridColumnModelList<BEGrupoPermissao> BEGrupoPermissaoColumns
        {
            get
            {
                return CreateConsultaColumns();
            }
        }
        private static GridColumnModelList<BEGrupoPermissao> CreateConsultaColumns()
        {
            GridColumnModelList<BEGrupoPermissao> cn = new GridColumnModelList<BEGrupoPermissao>();
            cn.Add(p => p.Codigo).SetAsPrimaryKey();
            cn.Add(p => p.Descricao);
            cn.Add(p => p.Ativo).SetFormatter("'checkbox'"); 
            cn.Add(p => p.TrackingState).SetHidden(true);
            return cn;
        }
        #endregion

        #region Consulta Grupo View
        public static GridColumnModelList<BEGrupoPermissaoViewControle> BEGrupoViewColumns
        {
            get
            {
                return CreateGrupoViewColumns();
            }
        }
        private static GridColumnModelList<BEGrupoPermissaoViewControle> CreateGrupoViewColumns()
        {
            GridColumnModelList<BEGrupoPermissaoViewControle> cn = new GridColumnModelList<BEGrupoPermissaoViewControle>();
            cn.Add(p => p.Codigo).SetAsPrimaryKey().SetHidden(true);
            cn.Add(p => p.GrupoPermisao.Descricao);
            cn.Add(p => p.ViewControle.Controle);
            cn.Add(p => p.ViewControle.View);
            cn.Add(p => p.TrackingState).SetHidden(true);
            return cn;
        }
        #endregion
    }
}