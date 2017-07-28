using DocuSign.eSign.Client;
using System;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Xml;

namespace Webhook.Controllers
{
    public class WebhookController : ApiController
    {
        // Process the incoming webhook data. See the DocuSign Connect guide
        // for more information
        //
        // Strategy: examine the data to pull out the envelope_id and time_generated fields.
        // Then store the entire xml on our local file system using those fields.
        //
        // If the envelope status=="Completed" then store the files as doc1.pdf, doc2.pdf, etc
        //
        // This function could also enter the data into a dbms, add it to a queue, etc.
        // Note that the total processing time of this function must be less than
        // 100 seconds to ensure that DocuSign's request to your app doesn't time out.
        // Tip: aim for no more than a couple of seconds! Use a separate queuing service
        // if need be.
        public void Post(HttpRequestMessage request)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(request.Content.ReadAsStreamAsync().Result);

            var mgr = new XmlNamespaceManager(xmldoc.NameTable);
            mgr.AddNamespace("a", "http://www.docusign.net/API/3.0");

            XmlNode envelopeStatus = xmldoc.SelectSingleNode("//a:EnvelopeStatus", mgr);
            XmlNode envelopeId = envelopeStatus.SelectSingleNode("//a:EnvelopeID", mgr);
            XmlNode status = envelopeStatus.SelectSingleNode("./a:Status", mgr);
            if(envelopeId != null)
            {
                System.IO.File.WriteAllText(HttpContext.Current.Server.MapPath("~/Documents/" +
                    envelopeId.InnerText + "_" + status.InnerText + "_" + Guid.NewGuid() + ".xml"), xmldoc.OuterXml);
            }

            if (status.InnerText == "Completed") {
                // Loop through the DocumentPDFs element, storing each document.

                XmlNode docs = xmldoc.SelectSingleNode("//a:DocumentPDFs", mgr);
                foreach (XmlNode doc in docs.ChildNodes)
                {
                    string documentName = doc.ChildNodes[0].InnerText; // pdf.SelectSingleNode("//a:Name", mgr).InnerText;
                    string documentId = doc.ChildNodes[2].InnerText; // pdf.SelectSingleNode("//a:DocumentID", mgr).InnerText;
                    string byteStr = doc.ChildNodes[1].InnerText; // pdf.SelectSingleNode("//a:PDFBytes", mgr).InnerText;

                    System.IO.File.WriteAllText(HttpContext.Current.Server.MapPath("~/Documents/" + envelopeId.InnerText + "_" + documentId + "_" + documentName), byteStr);
                }
            }
        }
    }
}
