using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Infrastructure.Services.Certificates
{
    public class CertificateDocument : IDocument
    {
        private readonly CertificateDocumentModel model;

        public CertificateDocument(CertificateDocumentModel model)
        {
            this.model = model;
        }

        public DocumentMetadata GetMetadata()
            => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);

                page.Margin(30);

                page.PageColor(Colors.White);

                page.DefaultTextStyle(x => x.FontSize(18));

                page.Content()
       .Border(2)
       .BorderColor(Colors.Grey.Medium)
       .Padding(30)
       .Column(column =>
       {
           column.Spacing(20);

           column.Item()
               .AlignCenter()
               .Text("CERTIFICATE OF COMPLETION")
               .Bold()
               .FontSize(28);

           column.Item()
               .AlignCenter()
               .Text("This certificate is proudly presented to");

           column.Item()
               .AlignCenter()
               .Text(model.StudentName)
               .Bold()
               .FontSize(26)
               .FontColor(Colors.Blue.Darken2);

           column.Item()
               .AlignCenter()
               .Text("For successfully completing the course");

           column.Item()
               .AlignCenter()
               .Text(model.CourseTitle)
               .Bold()
               .FontSize(22);

           column.Item().PaddingTop(20);

           column.Item()
               .Row(row =>
               {
                   row.RelativeItem()
                       .Column(col =>
                       {
                           col.Item().Text("Instructor").Bold();
                           col.Item().Text(model.InstructorName);
                       });

                   row.RelativeItem()
                       .Column(col =>
                       {
                           col.Item().Text("Issued At").Bold();
                           col.Item().Text(model.IssuedAt.ToString("dd MMM yyyy"));
                       });
               });

           column.Item().PaddingTop(20);

           column.Item()
               .Text($"Certificate Number : {model.CertificateNumber}")
               .FontSize(14);

           column.Item()
               .Text($"Verification Code : {model.VerificationCode}")
               .FontSize(14);
       });
            });
        }
    }
}
