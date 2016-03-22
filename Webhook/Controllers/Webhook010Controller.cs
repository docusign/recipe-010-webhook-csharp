using DocuSign.eSign.Api;
using DocuSign.eSign.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using System.Xml;
using Webhook.Helpers;

namespace Webhook.Controllers
{
    public class Webhook010Controller : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Home";

            return View();
        }

        public ActionResult SendSignRequest()
        {
           return View();
        }

        public ActionResult Status()
        {
            EnvelopesApi envelopesApi = new EnvelopesApi(WebhookLibrary.Configuration);
            ViewBag.Envelope = envelopesApi.GetEnvelope(WebhookLibrary.AccountId, Request.QueryString["envelope_id"], null);

            return View();
        }

        public ActionResult SendSignatureRequest()
        {
            string ds_signer1_name = WebhookLibrary.GetFakeName();
            string ds_signer1_email = WebhookLibrary.GetFakeEmail(ds_signer1_name);
            string ds_cc1_name = WebhookLibrary.GetFakeName();
            string ds_cc1_email = WebhookLibrary.GetFakeEmail(ds_cc1_name);
            string webhook_url = Request.Url.GetLeftPart(UriPartial.Authority) + "/api/Webhook";

            if (WebhookLibrary.AccountId == null)
            {
                return Content("[\"ok\" => false, \"html\" => \"<h3>Problem</h3><p>Couldn't login to DocuSign: \"]");
            }

		    // The envelope request includes a signer-recipient and their tabs object,
		    // and an eventNotification object which sets the parameters for
		    // webhook notifications to us from the DocuSign platform
		    List<EnvelopeEvent> envelope_events = new List<EnvelopeEvent>();

		    EnvelopeEvent envelope_event1 = new EnvelopeEvent();
		    envelope_event1.EnvelopeEventStatusCode = "sent";
		    envelope_events.Add(envelope_event1);
		    EnvelopeEvent envelope_event2 = new EnvelopeEvent();
		    envelope_event2.EnvelopeEventStatusCode = "delivered";
		    envelope_events.Add(envelope_event2);
		    EnvelopeEvent envelope_event3 = new EnvelopeEvent();
		    envelope_event3.EnvelopeEventStatusCode = "completed";
		    envelope_events.Add(envelope_event3);
		    EnvelopeEvent envelope_event4 = new EnvelopeEvent();
		    envelope_event4.EnvelopeEventStatusCode = "declined";
		    envelope_events.Add(envelope_event4);
		    EnvelopeEvent envelope_event5 = new EnvelopeEvent();
		    envelope_event5.EnvelopeEventStatusCode = "voided";
		    envelope_events.Add(envelope_event5);

		    List<RecipientEvent> recipient_events = new List<RecipientEvent>();
		    RecipientEvent recipient_event1 = new RecipientEvent();
		    recipient_event1.RecipientEventStatusCode = "Sent";
		    recipient_events.Add(recipient_event1);
		    RecipientEvent recipient_event2 = new RecipientEvent();
		    recipient_event2.RecipientEventStatusCode = "Delivered";
		    recipient_events.Add(recipient_event2);
		    RecipientEvent recipient_event3 = new RecipientEvent();
		    recipient_event3.RecipientEventStatusCode = "Completed";
		    recipient_events.Add(recipient_event3);
		    RecipientEvent recipient_event4 = new RecipientEvent();
		    recipient_event4.RecipientEventStatusCode = "Declined";
		    recipient_events.Add(recipient_event4);
		    RecipientEvent recipient_event5 = new RecipientEvent();
		    recipient_event5.RecipientEventStatusCode = "AuthenticationFailed";
		    recipient_events.Add(recipient_event5);
		    RecipientEvent recipient_event6 = new RecipientEvent();
		    recipient_event6.RecipientEventStatusCode = "AutoResponded";
		    recipient_events.Add(recipient_event6);

		    EventNotification event_notification = new EventNotification();
		    event_notification.Url = webhook_url;
		    event_notification.LoggingEnabled = "true";
		    event_notification.RequireAcknowledgment ="true";
		    event_notification.UseSoapInterface= "false";
		    event_notification.IncludeCertificateWithSoap= "false";
		    event_notification.SignMessageWithX509Cert= "false";
		    event_notification.IncludeDocuments= "true";
		    event_notification.IncludeEnvelopeVoidReason= "true";
		    event_notification.IncludeTimeZone= "true";
		    event_notification.IncludeSenderAccountAsCustomField= "true";
		    event_notification.IncludeDocumentFields= "true";
		    event_notification.IncludeCertificateOfCompletion= "true";
		    event_notification.EnvelopeEvents = envelope_events;
		    event_notification.RecipientEvents = recipient_events;

		    Document document = new Document();
		    document.DocumentId= "1";
		    document.Name = "NDA.pdf";

            //string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Documents\NDA.pdf");
            Byte[] bytes = System.IO.File.ReadAllBytes(Server.MapPath("~/Documents/NDA.pdf"));
		    document.DocumentBase64 = Convert.ToBase64String(bytes);

		    SignHere sign_here_tab = new SignHere();
		    sign_here_tab.AnchorString= "signer1sig";
		    sign_here_tab.AnchorXOffset= "0";
		    sign_here_tab.AnchorYOffset= "0";
		    sign_here_tab.AnchorUnits= "mms";
		    sign_here_tab.RecipientId= "1";
		    sign_here_tab.Name= "Please sign here";
		    sign_here_tab.Optional= "false";
		    sign_here_tab.ScaleValue = 1;
		    sign_here_tab.TabLabel= "signer1sig";

		    FullName full_name_tab = new FullName();
		    full_name_tab.AnchorString= "signer1name";
		    full_name_tab.AnchorYOffset= "-6";
		    full_name_tab.FontSize= "Size12";
		    full_name_tab.RecipientId= "1";
		    full_name_tab.TabLabel= "Full Name";
		    full_name_tab.Name= "Full Name";

		    DocuSign.eSign.Model.Text text_tab = new DocuSign.eSign.Model.Text();
		    text_tab.AnchorString= "signer1company";
		    text_tab.AnchorYOffset= "-8";
		    text_tab.FontSize= "Size12";
		    text_tab.RecipientId= "1";
		    text_tab.TabLabel= "Company";
		    text_tab.Name= "Company";
		    text_tab.Required= "false";

		    DateSigned date_signed_tab = new DateSigned();
		    date_signed_tab.AnchorString= "signer1date";
		    date_signed_tab.AnchorYOffset= "-6";
		    date_signed_tab.FontSize= "Size12";
		    date_signed_tab.RecipientId= "1";
		    date_signed_tab.Name= "Date Signed";
		    date_signed_tab.TabLabel= "Company";

		    DocuSign.eSign.Model.Tabs tabs = new DocuSign.eSign.Model.Tabs();
		    tabs.SignHereTabs = new List<SignHere>();
            tabs.SignHereTabs.Add(sign_here_tab);
		    tabs.FullNameTabs = new List<FullName>();
            tabs.FullNameTabs.Add(full_name_tab);
		    tabs.TextTabs = new List<Text>();
            tabs.TextTabs.Add(text_tab);
		    tabs.DateSignedTabs = new List<DateSigned>();
            tabs.DateSignedTabs.Add(date_signed_tab);

		    Signer signer = new Signer();
            signer.Email = ds_signer1_email;
		    signer.Name = ds_signer1_name;
		    signer.RecipientId= "1";
		    signer.RoutingOrder= "1";
		    signer.Tabs = tabs;

		    CarbonCopy carbon_copy = new CarbonCopy();
            carbon_copy.Email = ds_cc1_email;
		    carbon_copy.Name = ds_cc1_name;
		    carbon_copy.RecipientId= "2";
		    carbon_copy.RoutingOrder= "2";

		    Recipients recipients = new Recipients();
		    recipients.Signers = new List<Signer>();
            recipients.Signers.Add(signer);
		    recipients.CarbonCopies = new List<CarbonCopy>();
            recipients.CarbonCopies.Add(carbon_copy);

		    EnvelopeDefinition envelope_definition = new EnvelopeDefinition();
		    envelope_definition.EmailSubject= "Please sign the " + "NDA.pdf" + " document";
		    envelope_definition.Documents = new List<Document>();
            envelope_definition.Documents.Add(document);
		    envelope_definition.Recipients = recipients;
		    envelope_definition.EventNotification = event_notification;
		    envelope_definition.Status= "sent";

            EnvelopesApi envelopesApi = new EnvelopesApi(WebhookLibrary.Configuration);

            EnvelopeSummary envelope_summary = envelopesApi.CreateEnvelope(WebhookLibrary.AccountId, envelope_definition, null);
		    if ( envelope_summary == null || envelope_summary.EnvelopeId == null ) {
			    return Content("[\"ok\" => false, html => \"<h3>Problem</h3>\" \"<p>Error calling DocuSign</p>\"]");
		    }

		    string envelope_id = envelope_summary.EnvelopeId;

            // Create instructions for reading the email
		    string html = "<h2>Signature request sent!</h2>" +
			    "<p>Envelope ID: " + envelope_id + "</p>" +
			    "<h2>Next steps</h2>" +
			    "<h3>1. Open the Webhook Event Viewer</h3>" +
                "<p><a href='" + Request.Url.GetLeftPart(UriPartial.Authority) + "/Webhook010/status?envelope_id=" + envelope_id + "'" +
				    "  class='btn btn-primary' role='button' target='_blank' style='margin-right:1.5em;'>" +
				    "View Events</a> (A new tab/window will be used.)</p>" +
			    "<h3>2. Respond to the Signature Request</h3>";

		    string email_access = WebhookLibrary.GetFakeEmailAccess(ds_signer1_email);
		    if (email_access != null) {
			    // A temp account was used for the email
			    html += "<p>Respond to the request via your mobile phone by using the QR code: </p>" +
                    "<p>" + WebhookLibrary.GetFakeEmailAccessQRCode(email_access) + "</p>" +
				    "<p> or via <a target='_blank' href='" + email_access + "'>your web browser.</a></p>";
		    } else {
			    // A regular email account was used
			    html += "<p>Respond to the request via your mobile phone or other mail tool.</p>" +
				    "<p>The email was sent to " + ds_signer1_name + " &lt;" + ds_signer1_email + "&gt;</p>";
		    }

		    //return Content("['ok'  => true,'envelope_id' => "+envelope_id+",'html' => "+ html+",'js' => [['disable_button' => 'sendbtn']]]");  // js is an array of items
            return Content(html);
        }

        public ActionResult StatusUpdate(string envelopeid)
        {
            List<Dictionary<string, string>> json = new List<Dictionary<string, string>>();

            DirectoryInfo taskDirectory = new DirectoryInfo(Server.MapPath("~/Documents/"));
            FileInfo[] taskFiles = taskDirectory.GetFiles(envelopeid + "*.xml");
            if (taskFiles.Length > 0)
            {
                foreach (FileInfo file in taskFiles)
                {
                    XmlDocument xml = new XmlDocument();
                    xml.Load(file.FullName);

                    var mgr = new XmlNamespaceManager(xml.NameTable);
                    mgr.AddNamespace("a", "http://www.docusign.net/API/3.0");

                    XmlNode envelopeStatus = xml.SelectSingleNode("//a:EnvelopeStatus", mgr);

                    string envelope_id = envelopeStatus.SelectSingleNode("//a:EnvelopeID", mgr).InnerText;
                    string envelope_status = envelopeStatus.SelectSingleNode("//a:Status", mgr).InnerText;

                    string signer_name = "";
                    string signer_status = "";
                    string cc_name = "";
                    string cc_status = "";

                    XmlNodeList recipientStatuses = envelopeStatus.SelectNodes("//a:RecipientStatus", mgr);
                    if (recipientStatuses != null && recipientStatuses.Count > 0)
                    {
                        foreach (XmlNode recipientStatus in recipientStatuses)
                        {
                            switch (recipientStatus.FirstChild.InnerText)
                            {
                                case "Signer":
                                    signer_name = recipientStatus.SelectSingleNode("//a:UserName", mgr).InnerText;
                                    signer_status = recipientStatus.SelectSingleNode("//a:Status", mgr).InnerText;
                                    break;
                                case "CarbonCopy":
                                    cc_name = envelopeStatus.SelectSingleNode("//a:UserName", mgr).InnerText;
                                    cc_status = envelopeStatus.SelectSingleNode("//a:Status", mgr).InnerText;
                                    break;
                            }
                        }
                    }

                    var item = new Dictionary<string, string> {
                        { "envelope_id", envelope_id},
                        {"envelope_status", envelope_status},
                        {"signer_name", signer_name},
                        {"signer_status",signer_status},
                        {"cc_name", cc_name},
                        {"cc_status", cc_status},
                        {"xml_file_path", file.FullName},
                        {"xml_file_url", Request.Url.GetLeftPart(UriPartial.Authority) + "/Documents/" + file.Name}
                    };
                    json.Add(item);
                }
            }

            return PartialView("_EnvelopeStatus", json);
            //return RenderViewToString("EnvelopeStatus", json, ControllerContext);
        }
    }
}