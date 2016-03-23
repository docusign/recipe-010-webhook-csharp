using DocuSign.eSign.Api;
using DocuSign.eSign.Model;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Webhook.Helpers;

namespace Webhook.Controllers
{
    public class Webhook012Controller : Controller
    {
        public ActionResult SendSignRequest()
        {
            return View();
        }

        public ActionResult SendSignatureRequest()
        {
            string ds_signer1_name = WebhookLibrary.GetFakeName();
            string ds_signer1_email = WebhookLibrary.GetFakeEmail(ds_signer1_name);
            string ds_cc1_name = WebhookLibrary.GetFakeName();
            string ds_cc1_email = WebhookLibrary.GetFakeEmail(ds_cc1_name);
            //string webhook_url = Request.Url.GetLeftPart(UriPartial.Authority) + "/api/Webhook";

            if (WebhookLibrary.AccountId == null)
            {
                return Content("[\"ok\" => false, \"html\" => \"<h3>Problem</h3><p>Couldn't login to DocuSign: \"]");
            }

            // The envelope request includes a signer-recipient and their tabs object,
            // and an eventNotification object which sets the parameters for
            // webhook notifications to us from the DocuSign platform
            //List<EnvelopeEvent> envelope_events = new List<EnvelopeEvent>();

            //EnvelopeEvent envelope_event1 = new EnvelopeEvent();
            //envelope_event1.EnvelopeEventStatusCode = "sent";
            //envelope_events.Add(envelope_event1);
            //EnvelopeEvent envelope_event2 = new EnvelopeEvent();
            //envelope_event2.EnvelopeEventStatusCode = "delivered";
            //envelope_events.Add(envelope_event2);
            //EnvelopeEvent envelope_event3 = new EnvelopeEvent();
            //envelope_event3.EnvelopeEventStatusCode = "completed";
            //envelope_events.Add(envelope_event3);
            //EnvelopeEvent envelope_event4 = new EnvelopeEvent();
            //envelope_event4.EnvelopeEventStatusCode = "declined";
            //envelope_events.Add(envelope_event4);
            //EnvelopeEvent envelope_event5 = new EnvelopeEvent();
            //envelope_event5.EnvelopeEventStatusCode = "voided";
            //envelope_events.Add(envelope_event5);

            //List<RecipientEvent> recipient_events = new List<RecipientEvent>();
            //RecipientEvent recipient_event1 = new RecipientEvent();
            //recipient_event1.RecipientEventStatusCode = "Sent";
            //recipient_events.Add(recipient_event1);
            //RecipientEvent recipient_event2 = new RecipientEvent();
            //recipient_event2.RecipientEventStatusCode = "Delivered";
            //recipient_events.Add(recipient_event2);
            //RecipientEvent recipient_event3 = new RecipientEvent();
            //recipient_event3.RecipientEventStatusCode = "Completed";
            //recipient_events.Add(recipient_event3);
            //RecipientEvent recipient_event4 = new RecipientEvent();
            //recipient_event4.RecipientEventStatusCode = "Declined";
            //recipient_events.Add(recipient_event4);
            //RecipientEvent recipient_event5 = new RecipientEvent();
            //recipient_event5.RecipientEventStatusCode = "AuthenticationFailed";
            //recipient_events.Add(recipient_event5);
            //RecipientEvent recipient_event6 = new RecipientEvent();
            //recipient_event6.RecipientEventStatusCode = "AutoResponded";
            //recipient_events.Add(recipient_event6);

            //EventNotification event_notification = new EventNotification();
            //event_notification.Url = webhook_url;
            //event_notification.LoggingEnabled = "true";
            //event_notification.RequireAcknowledgment ="true";
            //event_notification.UseSoapInterface= "false";
            //event_notification.IncludeCertificateWithSoap= "false";
            //event_notification.SignMessageWithX509Cert= "false";
            //event_notification.IncludeDocuments= "true";
            //event_notification.IncludeEnvelopeVoidReason= "true";
            //event_notification.IncludeTimeZone= "true";
            //event_notification.IncludeSenderAccountAsCustomField= "true";
            //event_notification.IncludeDocumentFields= "true";
            //event_notification.IncludeCertificateOfCompletion= "true";
            //event_notification.EnvelopeEvents = envelope_events;
            //event_notification.RecipientEvents = recipient_events;

            Document document1 = new Document();
            document1.DocumentId = "1";
            document1.Name = "NDA.pdf";
            Byte[] bytes1 = System.IO.File.ReadAllBytes(Server.MapPath("~/Documents/NDA.pdf"));
            document1.DocumentBase64 = Convert.ToBase64String(bytes1);
            document1.FileExtension = "pdf";

            Document document2 = new Document();
            document2.DocumentId = "2";
            document2.Name = "House.pdf";
            Byte[] bytes2 = System.IO.File.ReadAllBytes(Server.MapPath("~/Documents/House.pdf"));
            document2.DocumentBase64 = Convert.ToBase64String(bytes2);
            document2.FileExtension = "pdf";

            Document document3 = new Document();
            document3.DocumentId = "3";
            document3.Name = "contractor_agreement.docx";
            Byte[] bytes3 = System.IO.File.ReadAllBytes(Server.MapPath("~/Documents/contractor_agreement.docx"));
            document3.DocumentBase64 = Convert.ToBase64String(bytes3);
            document3.FileExtension = "docx";

            /*
             * The signing fields
             *
             * Invisible (white) Anchor field names for the NDA.pdf document:
             *   * signer1sig
             *   * signer1name
             *   * signer1company
             *   * signer1date
             *
             * Explicitly placed fields are used in the contractor_agreement
             * and on the house diagram
             *
             * Some anchor fields for document 3, the contractor_agreement.docx, use existing 
             * content from the document:
             *   * "Client Signature"
             *   * "Client Name"
             * 
             * NOTE: Anchor fields search ALL the documents in the envelope for
             * matches to the field's anchor text
             */

            //Anchored for doc 1
            SignHere sign_here_tab1 = new SignHere();
            sign_here_tab1.AnchorString = "signer1sig";
            sign_here_tab1.AnchorXOffset = "0";
            sign_here_tab1.AnchorYOffset = "0";
            sign_here_tab1.AnchorUnits = "mms";
            sign_here_tab1.RecipientId = "1";
            sign_here_tab1.Name = "Please sign here";
            sign_here_tab1.Optional = "false";
            sign_here_tab1.ScaleValue = 1;
            sign_here_tab1.TabLabel = "signer1sig";

            // Explicit position for doc 2
            SignHere sign_here_tab2 = new SignHere();
            sign_here_tab2.PageNumber = "1";
            sign_here_tab2.DocumentId = "2";
            sign_here_tab2.RecipientId = "2";
            sign_here_tab2.XPosition = "89";
            sign_here_tab2.YPosition = "40";
            sign_here_tab2.Name = "Please sign here";
            sign_here_tab2.Optional = "false";
            sign_here_tab2.ScaleValue = 1;
            sign_here_tab2.TabLabel = "signer1_doc2";

            // Anchored for doc 3
            SignHere sign_here_tab3 = new SignHere();
            sign_here_tab3.AnchorString = "Client Signature";
            sign_here_tab3.AnchorXOffset = "0";
            sign_here_tab3.AnchorYOffset = "-4";
            sign_here_tab3.AnchorUnits = "mms";
            sign_here_tab3.RecipientId = "1";
            sign_here_tab3.Name = "Please sign here";
            sign_here_tab3.Optional = "false";
            sign_here_tab3.ScaleValue = 1;
            sign_here_tab3.TabLabel = "doc3_client_sig";

            // Anchored for doc 1
            FullName full_name_tab = new FullName();
            full_name_tab.AnchorString = "signer1name";
            full_name_tab.AnchorYOffset = "-6";
            full_name_tab.FontSize = "Size12";
            full_name_tab.RecipientId = "1";
            full_name_tab.TabLabel = "Full Name";
            full_name_tab.Name = "Full Name";

            // Anchored for doc 1
            DocuSign.eSign.Model.Text text_tab1 = new DocuSign.eSign.Model.Text();
            text_tab1.AnchorString = "signer1company";
            text_tab1.AnchorYOffset = "-8";
            text_tab1.FontSize = "Size12";
            text_tab1.RecipientId = "1"; //Because the same tab label is 
            text_tab1.TabLabel = "Company"; //used, these fields will have duplicate data
            text_tab1.Name = "Company"; //Note that the account's "Data Population Scope"
            text_tab1.Required = "true"; //must be set to "Envelope" to enable this feature.

            // Anchored for doc 3
            DocuSign.eSign.Model.Text text_tab2 = new DocuSign.eSign.Model.Text();
            text_tab2.AnchorString = "Client Name";
            text_tab2.AnchorYOffset = "-38";
            text_tab2.FontSize = "Size12";
            text_tab2.RecipientId = "1";
            text_tab2.TabLabel = "Company";
            text_tab2.Name = "Company";
            text_tab2.Required = "true";

            // Anchored for doc 3
            DocuSign.eSign.Model.Text text_tab3 = new DocuSign.eSign.Model.Text();
            text_tab3.DocumentId = "3";
            text_tab3.PageNumber = "1";
            text_tab3.RecipientId = "1";
            text_tab3.XPosition = "145";
            text_tab3.YPosition = "195";
            text_tab3.FontSize = "Size10";
            text_tab3.TabLabel = "Company";
            text_tab3.Name = "Company";
            text_tab3.Required = "true";

            // Anchored for doc 1
            DateSigned date_signed_tab1 = new DateSigned();
            date_signed_tab1.AnchorString = "signer1date";
            date_signed_tab1.AnchorYOffset = "-6";
            date_signed_tab1.FontSize = "Size12";
            date_signed_tab1.RecipientId = "1";
            date_signed_tab1.Name = "Date Signed";
            date_signed_tab1.TabLabel = "date_signed";

            // Explicit position for doc 2
            DateSigned date_signed_tab2 = new DateSigned();
            date_signed_tab2.DocumentId = "2";
            date_signed_tab2.PageNumber = "1";
            date_signed_tab2.RecipientId = "1";
            date_signed_tab2.XPosition = "89";
            date_signed_tab2.YPosition = "100";
            date_signed_tab2.FontSize = "Size12";
            date_signed_tab2.RecipientId = "1";
            date_signed_tab2.Name = "Date Signed";
            date_signed_tab2.TabLabel = "doc3_date_signed";

            DocuSign.eSign.Model.Tabs tabs = new DocuSign.eSign.Model.Tabs();
            tabs.SignHereTabs = new List<SignHere>();
            tabs.SignHereTabs.Add(sign_here_tab1);
            tabs.SignHereTabs.Add(sign_here_tab2);
            tabs.SignHereTabs.Add(sign_here_tab3);

            tabs.FullNameTabs = new List<FullName>();
            tabs.FullNameTabs.Add(full_name_tab);

            tabs.TextTabs = new List<Text>();
            tabs.TextTabs.Add(text_tab1);
            tabs.TextTabs.Add(text_tab2);
            tabs.TextTabs.Add(text_tab3);

            tabs.DateSignedTabs = new List<DateSigned>();
            tabs.DateSignedTabs.Add(date_signed_tab1);
            tabs.DateSignedTabs.Add(date_signed_tab2);

            Signer signer = new Signer();
            signer.Email = ds_signer1_email;
            signer.Name = ds_signer1_name;
            signer.RecipientId = "1";
            signer.RoutingOrder = "1";
            signer.Tabs = tabs;

            CarbonCopy carbon_copy = new CarbonCopy();
            carbon_copy.Email = ds_cc1_email;
            carbon_copy.Name = ds_cc1_name;
            carbon_copy.RecipientId = "2";
            carbon_copy.RoutingOrder = "2";

            Recipients recipients = new Recipients();
            recipients.Signers = new List<Signer>();
            recipients.Signers.Add(signer);
            recipients.CarbonCopies = new List<CarbonCopy>();
            recipients.CarbonCopies.Add(carbon_copy);

            EnvelopeDefinition envelope_definition = new EnvelopeDefinition();
            envelope_definition.EmailSubject = "Please sign the house documentation package";
            envelope_definition.Documents = new List<Document>();
            envelope_definition.Documents.Add(document1);
            envelope_definition.Documents.Add(document2);
            envelope_definition.Documents.Add(document3);
            envelope_definition.Recipients = recipients;
            //envelope_definition.EventNotification = event_notification;
            envelope_definition.Status = "sent";

            EnvelopesApi envelopesApi = new EnvelopesApi(WebhookLibrary.Configuration);

            EnvelopeSummary envelope_summary = envelopesApi.CreateEnvelope(WebhookLibrary.AccountId, envelope_definition, null);
            if (envelope_summary == null || envelope_summary.EnvelopeId == null)
            {
                return Content("[\"ok\" => false, html => \"<h3>Problem</h3>\" \"<p>Error calling DocuSign</p>\"]");
            }

            string envelope_id = envelope_summary.EnvelopeId;

            // Create instructions for reading the email
            string html = "<h2>Signature request sent!</h2>" +
                "<p>Envelope ID: " + envelope_id + "</p>" +
                "<p>Signer: " + ds_signer1_name + "</p>" +
                "<p>CC: " + ds_cc1_name + "</p>" +
                "<h2>Next steps</h2>" +
                "<h3>Respond to the Signature Request</h3>";

            string ds_signer1_email_access = WebhookLibrary.GetFakeEmailAccess(ds_signer1_email);
            if (ds_signer1_email_access != null)
            {
                // A temp account was used for the email
                html += "<p>Respond to the request via your mobile phone by using the QR code: </p>" +
                    "<p>" + WebhookLibrary.GetFakeEmailAccessQRCode(ds_signer1_email_access) + "</p>" +
                    "<p> or via <a target='_blank' href='" + ds_signer1_email_access + "'>your web browser.</a></p>";
            }
            else
            {
                // A regular email account was used
                html += "<p>Respond to the request via your mobile phone or other mail tool.</p>" +
                    "<p>The email was sent to " + ds_signer1_name + " &lt;" + ds_signer1_email + "&gt;</p>";
            }

            //return Content("['ok'  => true,'envelope_id' => "+envelope_id+",'html' => "+ html+",'js' => [['disable_button' => 'sendbtn']]]");  // js is an array of items
            return Content(html);
        }
    }
}