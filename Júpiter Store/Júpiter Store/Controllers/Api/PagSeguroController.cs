using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Mvc;
using System.Xml;

namespace Júpiter_Store.Controllers.Api
{
    public class PagSeguroController : ApiController
    {
        private const string CheckoutUri = @"https://pagseguro.uol.com.br/v2/checkout";

        //[System.Web.Mvc.HttpPost]
        public IHttpActionResult Checkout()
        {
            var postData = new System.Collections.Specialized.NameValueCollection
            {
                {"email", "thiago.pessia@gmail.com"},
                {"token", "E14085135E404685AAD1CA67B57E474F"},
                {"currency", "BRL"},
                {"itemId1", "0001"},
                {"itemDescription1", "ProdutoPagSeguro1"},
                {"itemAmount1", "3.00"},
                {"itemQuantity1", "1"},
                {"itemWeight1", "200"},
                {"reference", "REF1234"},
                {"senderName", "Jose Comprador"},
                {"senderAreaCode", "11"},
                {"senderPhone", "99999999999"},
                {"senderEmail", "c41254692078685555836@sandbox.pagseguro.com.br"},
                {"shippingAddressRequired", "false"}
            };


            string xmlString;

            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";

                var result = wc.UploadValues(CheckoutUri, postData);

                xmlString = Encoding.ASCII.GetString(result);
            }


            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);

            var code = xmlDoc.GetElementsByTagName("code")[0];
            var date = xmlDoc.GetElementsByTagName("date")[0];

            var paymentUrl = $"{CheckoutUri}/payment.html?code={code.InnerText}";

            return Ok(paymentUrl);
        }
    }
}
