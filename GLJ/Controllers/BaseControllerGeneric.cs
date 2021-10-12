using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GLJ.Models;

namespace GLJ.Controllers
{
    public class BaseControllerGeneric<PD> : BaseController
        where PD : IProcessData, new()
    {
        public BaseControllerGeneric()
        {
            this.ProcessData = new PD();
            this.LimparMensagens();
        }

        public new PD ProcessData { get; set; }

    }
}