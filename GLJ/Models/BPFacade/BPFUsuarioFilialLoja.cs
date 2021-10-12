using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Onion.Business;
using GLJ.Acesso.Business.Process;
using GLJ.Acesso.Filter.Entity;
using GLJ.Acesso.Business.Entity;

namespace GLJ.Models.BPFacade
{
    public class BPFUsuarioFilialLoja : Singleton<BPUsuarioFilialLoja>
    {
        internal BEUsuarioFilialLoja Salvar(BEUsuarioFilialLoja beUsuarioFilialLoja)
        {
            if (beUsuarioFilialLoja.Codigo > 0)
            {
                BEUsuarioFilialLoja beGrupoPermissaoOld = Instance.ObterTodos(new FEUsuarioFilialLoja() { Codigo = beUsuarioFilialLoja.Codigo }).ResultList.FirstOrDefault();
                beUsuarioFilialLoja = (BEUsuarioFilialLoja)beGrupoPermissaoOld.AlterProperties(beUsuarioFilialLoja);
            }
            return Instance.Salvar(beUsuarioFilialLoja);
        }
    }
}