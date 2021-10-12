using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GLJ.Acesso.Business.Process;
using GLJ.Acesso.Business.Entity;
using GLJ.Acesso.Filter.Entity;

namespace GLJ.Models.BPFacade
{
    public class BPFUsuario : Onion.Business.Singleton<BPUsuario>
    {
        internal BEUsuario Salvar(BEUsuario beUsuario)
        {
            if (beUsuario.Codigo > 0)
            {
                BEUsuario beUsuarioOld = Instance.ObterTodos(new FEUsuario() { Codigo = beUsuario.Codigo }).ResultList.FirstOrDefault();
                beUsuario = (BEUsuario)beUsuarioOld.AlterProperties(beUsuario);
            }
            return Instance.Salvar(beUsuario);
        }
    }
}