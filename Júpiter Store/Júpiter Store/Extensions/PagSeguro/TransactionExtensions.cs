using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Uol.PagSeguro.Domain;
using Uol.PagSeguro.Enums;

namespace Júpiter_Store.Extensions.PagSeguro
{
    public static class TransactionExtensions
    {
        public static string GetTransactionStatusText(this Transaction transaction)
        {
            switch (transaction.TransactionStatus)
            {
                case TransactionStatus.Available:
                    return "Disponível";
                case TransactionStatus.Cancelled:
                    return "Cancelada";
                case TransactionStatus.InAnalysis:
                    return "Em análise";
                case TransactionStatus.InDispute:
                    return "Em disputa";
                case TransactionStatus.Initiated:
                    return "Iniciada";
                case TransactionStatus.Paid:
                    return "Paga";
                case TransactionStatus.Refunded:
                    return "Reembolsada";
                case TransactionStatus.WaitingPayment:
                    return "Aguardando pagamento";
                default:
                    return "Inválido";
            }
        }
    }
}