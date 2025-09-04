
using E_CommerceSystem.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace E_CommerceSystem.Services
{
    public class InvoiceService : IInvoiceService
    {
       
        private readonly IUserService _userService;
        private readonly IOrderProductsService _orderProductsService;
        private readonly IProductService _productService;

        public InvoiceService(IUserService userService,
                            IOrderProductsService orderProductsService, IProductService productService)
        {
            
            _userService = userService;
            _orderProductsService = orderProductsService;
            _productService = productService;

            // Set QuestPDF license (Community version - free for non-commercial use)
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public InvoiceDTO GetInvoiceData(int orderId)
        {
            var order = _orderProductsService.GetOrdersByOrderId(orderId);

            var orderProducts = _orderProductsService.GetOrdersByOrderId(orderId);
            var user = _userService.GetUserById(orderProducts.First().Order.UID);

            var items = new List<InvoiceItemDTO>();
            foreach (var op in orderProducts)
            {
                var product = _productService.GetProductById(op.PID);
                items.Add(new InvoiceItemDTO
                {
                    ProductName = product.ProductName,
                    Quantity = op.Quantity,
                    UnitPrice = product.Price,
                    TotalPrice = op.Quantity * product.Price
                });
            }

            // Calculate values (in a real app, these might come from the order or settings)
            decimal subtotal = items.Sum(i => i.TotalPrice);
            decimal tax = subtotal * 0.1m; // 10% tax
            decimal shippingCost = subtotal > 50 ? 0 : 5.99m; // Free shipping over $50

            return new InvoiceDTO
            {

                //CustomerName = user.Name,
                CustomerName = user.UName,
                CustomerEmail = user.Email,
                CustomerPhone = user.Phone,
                Items = items,
                Subtotal = subtotal,
                Tax = tax,
                ShippingCost = shippingCost,
                Total = subtotal + tax + shippingCost
            };
        }

        public byte[] GenerateInvoicePdf(InvoiceDTO invoice)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header()
                        .AlignCenter()
                        .Text("INVOICE")
                        .SemiBold().FontSize(24).FontColor(Colors.Blue.Medium);

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(column =>
                        {
                            column.Spacing(20);

                            // Invoice details
                            column.Item().Row(row =>
                            {
                                row.RelativeItem().Column(col =>
                                {
                                    col.Item().Text($"Order ID: #{invoice.OrderId}");
                                    col.Item().Text($"Date: {invoice.OrderDate:yyyy-MM-dd}");
                                });

                                row.RelativeItem().Column(col =>
                                {
                                    col.Item().Text($"Customer: {invoice.CustomerName}");
                                    col.Item().Text($"Email: {invoice.CustomerEmail}");
                                    col.Item().Text($"Phone: {invoice.CustomerPhone}");
                                });
                            });

                            // Items table
                            column.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.ConstantColumn(25); // #
                                    columns.RelativeColumn(3);  // Product
                                    columns.ConstantColumn(80); // Qty
                                    columns.ConstantColumn(80); // Unit Price
                                    columns.ConstantColumn(80); // Total
                                });

                                // Table header
                                table.Header(header =>
                                {
                                    header.Cell().Text("#");
                                    header.Cell().Text("Product");
                                    header.Cell().AlignRight().Text("Qty");
                                    header.Cell().AlignRight().Text("Unit Price");
                                    header.Cell().AlignRight().Text("Total");

                                    header.Cell().ColumnSpan(5).PaddingTop(5).BorderBottom(1).BorderColor(Colors.Black);
                                });

                                // Table items
                                for (int i = 0; i < invoice.Items.Count; i++)
                                {
                                    var item = invoice.Items[i];

                                    table.Cell().Text((i + 1).ToString());
                                    table.Cell().Text(item.ProductName);
                                    table.Cell().AlignRight().Text(item.Quantity.ToString());
                                    table.Cell().AlignRight().Text(item.UnitPrice.ToString("C"));
                                    table.Cell().AlignRight().Text(item.TotalPrice.ToString("C"));
                                }
                            });

                            // Summary
                            column.Item().AlignRight().Column(col =>
                            {
                                col.Item().Text($"Subtotal: {invoice.Subtotal:C}");
                                col.Item().Text($"Tax (10%): {invoice.Tax:C}");
                                col.Item().Text($"Shipping: {invoice.ShippingCost:C}");
                                col.Item().Text($"Total: {invoice.Total:C}").Bold();
                            });
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Thank you for your business! ");
                            x.Span($"Invoice #{invoice.OrderId}").Bold();
                        });
                });
            });

            // Generate PDF as byte array
            return document.GeneratePdf();
        }

    }
}
