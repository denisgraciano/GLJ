using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using GLJ.Acesso.Business.Entity;

namespace GLJ.Models
{
    public class ProcessData : IProcessData
    {
        [Display(Name = "Estado")]
        public String UF { get; set; }
        [Display(Name = "Cidade")]
        public String CodigoCidade { get; set; }

        public BEUsuario UsuarioLogin
        {
            get;
            set;
        }

        public ProcessData()
        {

        }



    }
}