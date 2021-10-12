using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using GLJ.Business.Entity;

namespace GLJ.Models.Agenda
{
    public class AgendaModelo
    {
        #region Campos
        public int id { get; set; }
        public bool allDay { get; set; }
        public bool editable { get; set; }
        public string className { get; set; }
        public string color { get; set; }
        public string fontColor { get; set; }
        public string nameImageTitle { get; set; }
        public string placa { get; set; }
        public string motorista { get; set; }

        private StatusAgendaEntrega _StatusAgenda;
        public StatusAgendaEntrega StatusAgenda {
            get { return _StatusAgenda;}
            set { 
                    _StatusAgenda = value;
                    switch (value)
                    {
                        case StatusAgendaEntrega.ENTREGUE:
                            color = "#021af1";
                            fontColor = "#000000";
                            break;
                        case StatusAgendaEntrega.OBSERVACAO:
                            color = "#f1eb46";
                            fontColor = "#000000";
                            break;
                        case StatusAgendaEntrega.APROVADO:
                            color = "#f1a6c9";
                            fontColor = "#000000";
                            break;
                        case StatusAgendaEntrega.ENTREGAPENDENTE:
                            color = "#f1a40a";
                            fontColor = "#000000";
                            break;
                        case StatusAgendaEntrega.ASSISTENCIATECNICA:
                            color = "#f10505";
                            fontColor = "#000000";
                            break;
                        case StatusAgendaEntrega.CONFIRMADOCLIENTE:
                            color = "#05f105";
                            fontColor = "#000000";
                            break;
                        default:
                            break;
                    }
                } 
        }
       
        [Display(Name = "Titulo")]
        [Required(ErrorMessage = "Título é obrigatório")]
        public string title { get; set; }

        [Display(Name = "Data Inicial")]
        public string start
        {
            get;
            set;
        }

        [Display(Name = "Data Final")]
        public string end
        {
            get;
            set;
        }

        public BEPedido Pedido { get; set; }
        #endregion

        #region Construtor
        public AgendaModelo()
        {
            this.editable = true;
            this.allDay = false;
            Pedido = new BEPedido();
        }
        #endregion
    }
}