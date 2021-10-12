using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Onion.Business;
using GLJ.Acesso.Business.Process;
using GLJ.Acesso.Business.Entity;
using GLJ.Acesso.Filter.Entity;

namespace GLJ.Models.BPFacade
{
    public class BPFGrupoPermissao : Singleton<BPGrupoPermissao>
    {
        internal BEGrupoPermissao Salvar(BEGrupoPermissao beGrupoPermissao)
        {
            if (beGrupoPermissao.Codigo > 0)
            {
                BEGrupoPermissao beGrupoPermissaoOld = Instance.ObterTodos(new FEGrupoPermissao() { Codigo = beGrupoPermissao.Codigo }).ResultList.FirstOrDefault();
                beGrupoPermissao = (BEGrupoPermissao)beGrupoPermissaoOld.AlterProperties(beGrupoPermissao);
            }
            return Instance.Salvar(beGrupoPermissao);
        }
    }
}