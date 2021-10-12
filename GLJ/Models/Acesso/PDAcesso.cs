using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GLJ.Acesso.Business.Entity;
using System.Web.Mvc;
using GLJ.Acesso.Filter.Entity;

namespace GLJ.Models.Acesso
{
    public class PDAcesso : ProcessData
    {
        public BEGrupoPermissao GrupoPermissao { get; set; }
        public List<SelectListItem> lsitView { get; set; }
        public FEGrupoPermissao feGrupoPermissao { get; set; }
        public List<BEGrupoPermissao> listGrupoPermissao { get; set; }
        public BEUsuario Usuario { get; set; }
        public BEGrupoPermissaoViewControle GrupoView { get; set; }
        public BEUsuarioGrupoPermissao UsuarioGrupo { get; set; }
        public List<BEUsuarioGrupoPermissao> UsuariosGrupos { get; set; }
        public FEUsuario feUsuario { get; set; }
        public List<BEUsuario> listUsuario { get; set; }
        public BEUsuarioFilialLoja UsuarioFilialLoja { get; set; }
        public List<BEUsuarioFilialLoja> ListUsuarioFilialLoja { get; set; }
        public FEUsuarioFilialLoja FEUsuarioFilialLoja { get; set; }

        public PDAcesso()
        {
            this.GrupoPermissao = new BEGrupoPermissao();
            this.feGrupoPermissao = new FEGrupoPermissao();
            this.listGrupoPermissao = new List<BEGrupoPermissao>();
            this.GrupoView = new BEGrupoPermissaoViewControle();
            this.Usuario = new BEUsuario();
            this.UsuarioGrupo = new BEUsuarioGrupoPermissao();
            this.UsuariosGrupos = new List<BEUsuarioGrupoPermissao>();
            this.feUsuario = new FEUsuario();
            this.listUsuario = new List<BEUsuario>();
            this.UsuarioFilialLoja = new BEUsuarioFilialLoja();
            this.ListUsuarioFilialLoja = new List<BEUsuarioFilialLoja>();
            this.FEUsuarioFilialLoja = new FEUsuarioFilialLoja();
        }

    }
}