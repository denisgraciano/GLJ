using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVC.Controls.Grid;
using GLJ.Acesso.Business.Entity;

namespace GLJ.Models.Acesso
{
    public class GridUsuarioModel
    {
        #region Usuario Grupo Permissao
        public static GridColumnModelList<BEUsuarioGrupoPermissao> BEUsuarioGrupoColumns
        {
            get
            {
                return CreateUsuarioGrupoColumns();
            }
        }
        private static GridColumnModelList<BEUsuarioGrupoPermissao> CreateUsuarioGrupoColumns()
        {
            GridColumnModelList<BEUsuarioGrupoPermissao> cn = new GridColumnModelList<BEUsuarioGrupoPermissao>();
            cn.Add(p => p.Codigo).SetAsPrimaryKey().SetHidden(true);
            cn.Add(p => p.Usuario.Login);
            cn.Add(p => p.GrupoPermisao.Descricao);
            return cn;
        }
        #endregion

        #region Consulta Usuario
        public static GridColumnModelList<BEUsuario> BEUsuarioColumns
        {
            get
            {
                return CreateUsuarioColumns();
            }
        }
        private static GridColumnModelList<BEUsuario> CreateUsuarioColumns()
        {
            GridColumnModelList<BEUsuario> cn = new GridColumnModelList<BEUsuario>();
            cn.Add(p => p.Codigo).SetAsPrimaryKey().SetHidden(true);
            cn.Add(p => p.NomeUsuario);
            cn.Add(p => p.Login);
            cn.Add(p => p.Ativo).SetFormatter("'checkbox'");
            return cn;
        }
        #endregion
    }
}