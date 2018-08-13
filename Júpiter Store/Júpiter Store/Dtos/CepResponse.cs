using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Júpiter_Store.Dtos
{
    public class CepResponse
    {
        public string bairro { get; set; }
        public string cidade { get; set; }
        public string logradouro { get; set; }
        public Dictionary<string, string> estado_info { get; set; }
        public string cep { get; set; }
        public Dictionary<string, string> cidade_info { get; set; }
        public string estado { get; set; }
    }
}