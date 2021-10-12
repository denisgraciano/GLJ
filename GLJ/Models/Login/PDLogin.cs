using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GLJ.Acesso.Business.Entity;

namespace GLJ.Models.Login
{
    public class PDLogin : ProcessData
    {
        public BEUsuario Login { get; set; }

        public PDLogin()
        {
            this.Login = new BEUsuario();
        }
    }
}