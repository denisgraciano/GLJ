using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Onion.Business;
using GLJ.Business.Process;
using GLJ.Filter.Entity;
using GLJ.Business.Entity;

namespace GLJ.Models.BPFacade
{
    public class BPFCliente : Singleton<BPCliente>
    {
        internal BECliente Salvar(BECliente beCliente)
        {
            if (beCliente.Codigo > 0)
            {

                BECliente beClienteOld = Instance.ObterTodos(new FECliente() { Codigo = beCliente.Codigo, CodigoLoja = beCliente.CodigoLoja }).ResultList.FirstOrDefault();
                beCliente = (BECliente)beClienteOld.AlterProperties(beCliente);
            }
            return Instance.Salvar(beCliente);
        }

        internal BEDadosCliente SalvarDados(BEDadosCliente beDadosCliente)
        {
            BPDadosCliente bp = new BPDadosCliente();

            if (beDadosCliente.Codigo > 0)
            {
                BEDadosCliente beDadoClienteOld = bp.ObterTodos(new FEDadosCliente() { Codigo = beDadosCliente.Codigo,CodigoCliente = beDadosCliente.CodigoCliente}).ResultList.FirstOrDefault();
                beDadosCliente = (BEDadosCliente)beDadoClienteOld.AlterProperties(beDadosCliente);
            }
            return bp.Salvar(beDadosCliente);
        }

        internal BETelefoneCliente SalvarTelefone(BETelefoneCliente beTelefoneCliente)
        {
            BPTelefoneCliente bp = new BPTelefoneCliente();

            if (beTelefoneCliente.Codigo > 0)
            {
                BETelefoneCliente beTelefoneClienteOld = bp.ObterTodos(new FETelefoneCliente() { Codigo = beTelefoneCliente.Codigo,CodigoCliente = beTelefoneCliente.CodigoCliente }).ResultList.FirstOrDefault();
                beTelefoneCliente = (BETelefoneCliente)beTelefoneClienteOld.AlterProperties(beTelefoneCliente);
            }
            return bp.Salvar(beTelefoneCliente);
        }

        internal List<BEDadosCliente> ObterDadosCliente(FEDadosCliente feDadosCliente)
        {
            BPDadosCliente bp = new BPDadosCliente();
            return bp.ObterTodos(feDadosCliente).ResultList;
        }

        internal List<BETelefoneCliente> ObterTelefonesCliente(FETelefoneCliente feTelefoneCliente)
        {
            BPTelefoneCliente bp = new BPTelefoneCliente();
            return bp.ObterTodos(feTelefoneCliente).ResultList;
        }
    }
}