using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GLJ.Business.Process;
using GLJ.Models.Agenda;
using GLJ.Filter.Entity;
using GLJ.Business.Entity;
using Onion.Business;

namespace GLJ.Models.BPFacade
{
    public class BPFAgendaEntrega : Singleton<BPAgendaEntrega>
    {
        public List<AgendaModelo> ObterAgendaPedidos(FEAgendaEntrega feAgendaEntrega)
        {
            List<AgendaModelo> listaAgenda = new List<AgendaModelo>();
            feAgendaEntrega.LoadType = Onion.Business.Entity.LoadType.Medium;

            List<BEAgendaEntrega> listAgenda = Instance.ObterTodos(feAgendaEntrega).ResultList;
            foreach (BEAgendaEntrega item in listAgenda)
            {
                listaAgenda.Add(new AgendaModelo()
                {
                    Pedido = item.Pedido,
                    title = item.Descricao,
                    allDay = false,
                    editable = true,
                    StatusAgenda = item.StatusAgenda,
                    id = item.Codigo,
                    start = item.DataInicio.ToString("s"),
                    end = item.DataFim.ToString("s"),
                    nameImageTitle = "delivery.png",
                    placa = item.PlacaVeiculo,
                    motorista = item.MotoristaVeiculo
                });
            }

            return listaAgenda;
        }

        internal BEAgendaEntrega AlterarDataEntrega(FEAgendaEntrega feAgendaEntrega,DateTime dataEntrega)
        {
            if (dataEntrega.DayOfWeek == DayOfWeek.Sunday)
            {
                throw new Exception("Escolha outra data");
            }


            if (!ValidaQtdeEntrega(feAgendaEntrega, dataEntrega))
            {
                throw new Exception("Já atingiu o número máximo de entrega!!!");
            }

            BEAgendaEntrega beAgendaEntregaOld = Instance.ObterTodos(feAgendaEntrega).ResultList.SingleOrDefault();

            if (beAgendaEntregaOld.StatusAgenda == StatusAgendaEntrega.ENTREGUE)
            {
                throw new Exception("Pedido já entregue, não é possível alterar a data de Entrega.");
            }
            

            BEAgendaEntrega beAgendaEntrega = beAgendaEntregaOld.Clone() as BEAgendaEntrega;
            beAgendaEntrega.DataInicio = dataEntrega;
            beAgendaEntrega.DataFim = dataEntrega;
            beAgendaEntrega = (BEAgendaEntrega)beAgendaEntregaOld.AlterProperties(beAgendaEntrega);
            return Instance.Salvar(beAgendaEntrega);
        }

        internal BEAgendaEntrega Salvar(BEAgendaEntrega beAgendaEntrega)
        {
            if(beAgendaEntrega.Codigo > 0)
            {
                BEAgendaEntrega agendaOld = Instance.ObterTodos(new FEAgendaEntrega{Codigo = beAgendaEntrega.Codigo,CodigoPedido = beAgendaEntrega.Codigo}).ResultList.FirstOrDefault();
                beAgendaEntrega = (BEAgendaEntrega)agendaOld.AlterProperties(beAgendaEntrega);
            }
            return Instance.Salvar(beAgendaEntrega);
        }

        private bool ValidaQtdeEntrega(FEAgendaEntrega feAgendaEntrega, DateTime dataEntrega)
        {
            BEConfiguracaoLoja confLoja = BPFConfiguracaoLoja.Instance.ObterTodos(new FEConfiguracaoLoja() { CodigoLoja = (int)feAgendaEntrega.CodigoLoja }).ResultList.FirstOrDefault();

            FEAgendaEntrega filtro = new FEAgendaEntrega() 
            { 
                CodigoLoja = (int)feAgendaEntrega.CodigoLoja, 
                DataInicio = Convert.ToDateTime(dataEntrega.ToString("yyyy-MM-dd")),
                DataFim = Convert.ToDateTime(dataEntrega.ToString("yyyy-MM-dd 23:59:59"))
            };

            int TotalAgenda = Instance.ObterTodos(filtro).ResultList.Count;

            if (TotalAgenda >= confLoja.QuantidadeEntrega)
            {
                return false;
            }

            return true;
        }
    }
}
