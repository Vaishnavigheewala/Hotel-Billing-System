using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using HotelBillingMVC.Models;
using iText.Kernel.Colors;
using iText.Layout.Borders;
using iText.Layout.Properties;

namespace HotelBillingMVC.Helpers
{
    public static class PdfGenerator
    {
        public static byte[] GenerateBill(OrderMaster order)
        {
            //if (order.OrderStatus != "Complete")
            //{
            //    // Return an empty byte array or throw an exception if the status is not "Complete"
            //    throw new InvalidOperationException("Cannot generate bill. The order status is not complete.");
            //    // Alternatively, you can return null or a custom message
            //    // return null;
                
            //}
            using (var memoryStream = new MemoryStream())
            {
                var writer = new PdfWriter(memoryStream);
                var pdf = new PdfDocument(writer);
                var document = new Document(pdf);

                // Styling for header
                var headerStyle = new Style()
                    .SetFontSize(20)
                    .SetBold()
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontColor(ColorConstants.BLACK);

                var headStyle = new Style()
                    .SetFontSize(18)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontColor(ColorConstants.BLACK);

                // Styling for sub-header
                var subHeaderStyle = new Style()
                    .SetFontSize(14)
                    .SetBold()
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetFontColor(ColorConstants.BLACK);

                // Styling for normal text
                var normalTextStyle = new Style()
                    .SetFontSize(12)
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetBold()
                    .SetFontColor(ColorConstants.BLACK);

                // Styling for table header
                var tableHeaderStyle = new Style()
                    .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                    .SetBold()
                    .SetTextAlignment(TextAlignment.CENTER);

                // Styling for table cell
                var tableCellStyle = new Style()
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetBorder(new SolidBorder(ColorConstants.BLACK, 1));

                document.Add(new Paragraph("Fenvial Hotel").AddStyle(headerStyle));
                document.Add(new Paragraph("Bill Details").AddStyle(headerStyle));
                document.Add(new Paragraph("1 , Vidhya Vihar Society , Rupali Complex , Surat - 392011").AddStyle(headStyle));
                document.Add(new Paragraph("PHONE NO. 9862978421").AddStyle(headStyle));
                document.Add(new Paragraph($"Bill No.: {order.Id}").AddStyle(subHeaderStyle));
                document.Add(new Paragraph($"Customer Name: {order.CustName}").AddStyle(subHeaderStyle));
                document.Add(new Paragraph($"Dining No.: {order.TableId}").AddStyle(subHeaderStyle));
                document.Add(new Paragraph($"Order Date: {order.OrderDate}").AddStyle(subHeaderStyle));
                document.Add(new Paragraph($"Payment Method: {order.PaymentType}").AddStyle(subHeaderStyle));


                //var table = new Table(4);
                //table.AddCell("Item Name");
                //table.AddCell("Quantity");
                //table.AddCell("Unit Price");
                //table.AddCell("Total Price");

                var table = new Table(UnitValue.CreatePercentArray(new float[] { 4, 2, 2, 2 })).UseAllAvailableWidth();
                table.AddHeaderCell(new Cell().Add(new Paragraph("Item Name").AddStyle(tableHeaderStyle)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Quantity").AddStyle(tableHeaderStyle)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Unit Price").AddStyle(tableHeaderStyle)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Total Price").AddStyle(tableHeaderStyle)));

                //foreach (var item in order.OrderDetails)
                //{
                //    table.AddCell(item.Menu.ItemName);
                //    table.AddCell(item.Quantity.ToString());
                //    table.AddCell(item.UnitPrice.ToString("C"));
                //    table.AddCell((item.Quantity * item.UnitPrice).ToString("C"));
                //}

                foreach (var item in order.OrderDetails)
                {
                    table.AddCell(new Cell().Add(new Paragraph(item.Menu.ItemName).AddStyle(tableCellStyle)));
                    table.AddCell(new Cell().Add(new Paragraph(item.Quantity.ToString()).AddStyle(tableCellStyle)));
                    table.AddCell(new Cell().Add(new Paragraph(item.UnitPrice.ToString("C")).AddStyle(tableCellStyle)));
                    table.AddCell(new Cell().Add(new Paragraph((item.Quantity * item.UnitPrice).ToString("C")).AddStyle(tableCellStyle)));
                }

                document.Add(table);
                document.Add(new Paragraph($"Grand Total : {order.TotalAmount.ToString("C")}").AddStyle(normalTextStyle));

                document.Close();
                return memoryStream.ToArray();
            }
        }
    }
}
