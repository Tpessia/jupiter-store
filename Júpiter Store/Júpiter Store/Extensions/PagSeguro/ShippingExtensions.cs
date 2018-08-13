using System.Collections.Generic;
using Uol.PagSeguro.Domain;

namespace Júpiter_Store.Extensions.PagSeguro
{
    public static class ShippingExtensions
    {
        private static readonly Dictionary<int, string> Codes = new Dictionary<int, string>
        {
            { 1, "Pac" },
            { 2, "Sedex" },
            { 3, "Não especificado" }
        };

        public static string GetShippingTypeName(this Shipping shipping)
        {
            return shipping.ShippingType != null ? Codes[shipping.ShippingType.Value] : null;
        }
    }
}