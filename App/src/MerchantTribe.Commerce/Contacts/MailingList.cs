using System;
using System.Data;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace MerchantTribe.Commerce.Contacts
{

	public class MailingList
	{
        public long Id { get; set; }
        public DateTime LastUpdatedUtc { get; set; }		        
		public string Name {get;set;}
		public bool IsPrivate {get;set;}
		public List<Contacts.MailingListMember> Members {get; set;}
        public long StoreId { get; set; }

		public MailingList()
		{
            this.Id = 0;
            this.LastUpdatedUtc = DateTime.UtcNow;
            this.Name = string.Empty;
            this.IsPrivate = false;
            this.Members = new List<MailingListMember>();
            this.StoreId = 0;
		}
       
        // Mailing List Send Functions
		public void SendToList(Content.HtmlTemplate t, bool sendAsync, MerchantTribeApplication app)
		{			
			if (t != null) {				
					foreach (MailingListMember m in this.Members) {
						try {
                            Content.HtmlTemplate copy = t.ReplaceTagsInTemplate(app, m);
							System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
							msg = copy.ConvertToMailMessage(m.EmailAddress);
							if (msg != null) {
								Utilities.MailServices.SendMail(msg, app.CurrentStore);
							}
						}
						catch (Exception ex) {
							EventLog.LogEvent(ex);
						}
					}				
			}
		}
		public System.Net.Mail.MailMessage PreviewMessage(Content.HtmlTemplate t, MerchantTribeApplication app)
		{
			System.Net.Mail.MailMessage result = new System.Net.Mail.MailMessage();

			if (this.Members.Count > 0) {
				if (t != null) {
                    Content.HtmlTemplate copy = t.ReplaceTagsInTemplate(app, this.Members[0]);
                    result = copy.ConvertToMailMessage(this.Members[0].EmailAddress);
				}
			}

			return result;
		}

        // Member Functions
        public MailingListMember FindMemberByEmail(string email)
        {
            MailingListMember m = (from mem in this.Members
                                   where mem.EmailAddress == email
                                   select mem).SingleOrDefault();
            return m;
        }
		public bool CheckMembership(string email)
		{
            MailingListMember m = FindMemberByEmail(email);
            if (m != null)
            {
                return true;
            }
            return false;
		}
        public void UpdateMemberEmail(string oldEmail, string newEmail)
        {
            MailingListMember m = FindMemberByEmail(oldEmail);
            if (m != null)
            {
                m.EmailAddress = newEmail;
            }
        }
        public void RemoveMemberByEmail(string email)
        {
            MailingListMember m = FindMemberByEmail(email);
            if (m != null)
            {
                this.Members.Remove(m);
            }
        }
        public void RemoveMemberById(long id)
        {
            MailingListMember m = (from mem in this.Members
                                   where mem.Id == id
                                   select mem).SingleOrDefault();
            if (m != null)
            {
                this.Members.Remove(m);
            }
        }

        // Import Export Functions
		public string ExportToCommaDelimited(bool onlyExportEmail)
		{
			string result = string.Empty;

			StringBuilder sb = new StringBuilder();
			for (int i = 0; i <= Members.Count - 1; i++) {
				sb.Append(Members[i].EmailAddress);
				if (onlyExportEmail == false) {
					sb.Append(", ");
					sb.Append(Members[i].LastName);
					sb.Append(", ");
					sb.Append(Members[i].FirstName);
				}
				sb.Append("\n");
			}
			result = sb.ToString();

			return result;
		}
		public void ImportFromCommaDelimited(string inputText)
		{
			StringReader sw = new StringReader(inputText);
			string splitCharacter = ",";
			string lineToProcess = string.Empty;
			lineToProcess = sw.ReadLine();

			while (lineToProcess != null) {
				string[] lineValues = lineToProcess.Split(splitCharacter.ToCharArray());
				if (lineValues.Length > 0) {
					MailingListMember mm = new MailingListMember();
					mm.EmailAddress = lineValues[0];
					mm.ListId = this.Id;
					if (lineValues.Length > 1) {
						mm.LastName = lineValues[1];
					}
					if (lineValues.Length > 2) {
						mm.FirstName = lineValues[2];
					}
                    this.Members.Add(new MailingListMember() { LastName = mm.LastName, FirstName = mm.FirstName, EmailAddress = mm.EmailAddress });					
				}
				lineToProcess = sw.ReadLine();
			}
			sw.Dispose();
		}

	}
}

