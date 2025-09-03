using E_CommerceSystem.Models;

namespace E_CommerceSystem.Services
{
    public interface IInvoiceService
    {
        byte[] GenerateInvoicePdf(InvoiceDTO invoice);
        InvoiceDTO GetInvoiceData(int orderId);
    }
}