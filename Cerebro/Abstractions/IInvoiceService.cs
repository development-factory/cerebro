using Cerebro.Data;

namespace Cerebro.Abstractions;

public interface IInvoiceService
{
    void Create(Invoice invoice);
    void Delete(int id);
    IEnumerable<Invoice> GetAll();
    Invoice GetById(int id);
    void Update(Invoice invoice);
}
