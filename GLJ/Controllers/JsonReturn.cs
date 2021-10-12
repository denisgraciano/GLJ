using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Mvc;
using Onion.Business.Validation;

namespace GLJ.Controllers
{
    [Serializable]
    public class JsonReturn : JavaScriptSerializer
    {
        public Boolean Success { get; set; }

        public String Mensage { get; set; }

        public Object Data { get; set; }

        public JsonReturn()
        {
            this.Success = true;
            this.Mensage = string.Empty;
        }

        public JsonReturn(Boolean success)
        {
            this.Success = success;
            this.Mensage = string.Empty;
        }

        public JsonReturn(Boolean success, String mensagem)
        {
            this.Success = success;
            this.Mensage = mensagem;
        }

        public JsonReturn(Boolean success, String mensagem, Object data)
        {
            this.Success = success;
            this.Mensage = mensagem;
            this.Data = data;
        }

        public JsonReturn(ModelStateDictionary modelState)
        {
            this.Success = false;
            foreach (var item in modelState.Values)
            {
                foreach (var erro in item.Errors)
                {
                    this.Mensage += string.Format("- {0}<br/>", erro.ErrorMessage);
                }
            }
        }

        public JsonReturn(List<BusinessValidationResult> ValidationResults)
        {
            this.Success = false;
            foreach (BusinessValidationResult item in ValidationResults)
            {
                this.Mensage += string.Format(" - {0}<br />", item.ErrorMessage);
            }
        }
    }
}