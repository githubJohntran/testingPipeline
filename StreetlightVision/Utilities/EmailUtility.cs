using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using S22.Imap;
using System.Net.Mail;

namespace StreetlightVision.Utilities
{
    /// <summary>
    /// Mail Type Enum
    /// </summary>
    public enum MailType
    {
        Gmail,
        Mailtrap,
        IMAP
    }

    public class EmailUtility
    {
        private const string SLV_EMAIL_SENDER = "alarming.asia@streetlightmonitoring.com";
        private const int EMAIL_RETRY_TIMEOUT_SECOND = 480;

        //IMAP config
        private const string IMAP_MAIL_HOST = "imap.gmail.com";
        private const string IMAP_MAIL_USERNAME = "slv-hcmcqa@qualitusvn.com";
        private const string IMAP_MAIL_PASSWORD = "slvtest123";
        private const int IMAP_MAIL_PORT = 993;
        private const bool IMAP_SSL = true;

        /// <summary>
        /// Get SLV Reset password link
        /// </summary>
        /// <returns>Reset password link</returns>
        public static string GetSLVResetPasswordLink(string subjectKeyword = "")
        {
            return WaitAndGetSLVResetPasswordLinkViaIMAP(subjectKeyword);
        }

        /// <summary>
        /// Get SLV New Password after reset
        /// </summary>
        /// <returns>New Password</returns>
        public static string GetSLVNewPassword(string subjectKeyword = "")
        {
            return WaitAndGetSLVNewPasswordViaIMAP(subjectKeyword);
        }

        /// <summary>
        /// Clean all messages in inbox
        /// </summary>
        /// <returns></returns>
        public static bool CleanInbox(string subjectKeyword = "")
        {
            return CleanInboxVia(MailType.IMAP, subjectKeyword);
        }

        private static bool CleanInboxVia(MailType type = MailType.IMAP, string subjectKeyword = "")
        {
            var isInboxCleaned = GenericOperation<bool>.Retry(() => CleanAllMailsInbox(type, subjectKeyword), (c) => c == true, EMAIL_RETRY_TIMEOUT_SECOND);

            return isInboxCleaned;
        }

        /// <summary>
        /// Wait and get new email from IMAP
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static MailMessage GetNewEmail(string subjectKeyword = "")
        {
            return WaitAndGetNewMailViaIMAP(subjectKeyword);
        }

        /// <summary>
        /// Wait and get new emails from IMAP
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<MailMessage> GetNewEmails(string subjectKeyword = "")
        {
            return WaitAndGetNewMailsViaIMAP(subjectKeyword);
        }

        private static string GetNewEmailBody(MailType type = MailType.IMAP, string subjectKeyword = "")
        {
            string result = string.Empty;

            switch (type)
            {
                case MailType.Gmail:
                    break;

                case MailType.Mailtrap:
                    break;

                case MailType.IMAP:
                    result = GetNewEmailIBodyIMAP(subjectKeyword);
                    break;

                default:
                    break;
            }

            return result;
        }

        private static bool CleanAllMailsInbox(MailType type = MailType.IMAP, string subjectKeyword = "")
        {
            bool result = false;

            switch (type)
            {
                case MailType.Gmail:
                    break;

                case MailType.Mailtrap:
                    break;

                case MailType.IMAP:
                    result = CleanInboxIMAP(subjectKeyword);
                    break;

                default:
                    break;
            }

            return result;
        }

        private static string ParseSLVNewPassword(string bodyMail)
        {
            var pattern = "Your new password is:";

            //html body mail
            if (!string.IsNullOrEmpty(bodyMail))
            {
                if (bodyMail.Contains("</body>"))
                {
                    var indexStart = bodyMail.IndexOf(pattern) + pattern.Length;
                    var indexEnd = bodyMail.IndexOf("</body>");

                    return bodyMail.Substring(indexStart, indexEnd - indexStart);
                }
                else
                {
                    var indexStart = bodyMail.IndexOf(pattern) + pattern.Length;

                    return bodyMail.Substring(indexStart);
                }
            }

            return string.Empty;
        }

        private static string ParseSLVLinkResetPassword(string bodyMail)
        {
            if (bodyMail.Contains("</a>"))
            {
                var link = XElement.Parse(bodyMail).Descendants("a").Select(x => x.Attribute("href").Value).ToList().FirstOrDefault();
                return link;
            }

            return string.Empty;
        }

        #region IMAP

        private static string WaitAndGetSLVResetPasswordLinkViaIMAP(string subjectKeyword = "")
        {
            var bodyMail = GenericOperation<string>.Retry(() => GetNewEmailBody(MailType.IMAP, subjectKeyword), (c) => !string.IsNullOrEmpty(c), EMAIL_RETRY_TIMEOUT_SECOND);

            return ParseSLVLinkResetPassword(bodyMail);
        }

        private static string WaitAndGetSLVNewPasswordViaIMAP(string subjectKeyword = "")
        {
            var bodyMail = GenericOperation<string>.Retry(() => GetNewEmailBody(MailType.IMAP, subjectKeyword), (c) => !string.IsNullOrEmpty(c), EMAIL_RETRY_TIMEOUT_SECOND);

            return ParseSLVNewPassword(bodyMail);
        }

        private static MailMessage WaitAndGetNewMailViaIMAP(string subjectKeyword = "")
        {
            var mail = GenericOperation<MailMessage>.Retry(() => GetNewEmailIMAP(subjectKeyword), (c) => c != null, EMAIL_RETRY_TIMEOUT_SECOND);

            return mail;
        }

        private static List<MailMessage> WaitAndGetNewMailsViaIMAP(string subjectKeyword = "")
        {
            var mails = GenericOperation<List<MailMessage>>.Retry(() => GetNewEmailsIMAP(subjectKeyword), (c) => c != null, EMAIL_RETRY_TIMEOUT_SECOND);

            return mails;
        }

        private static string GetNewEmailIBodyIMAP(string subjectKeyword = "")
        {
            try
            {
                using (ImapClient client = new ImapClient(IMAP_MAIL_HOST, IMAP_MAIL_PORT, IMAP_MAIL_USERNAME, IMAP_MAIL_PASSWORD, AuthMethod.Login, IMAP_SSL))
                {
                    MailMessage latestMsg = null;
                    var uids = client.Search(SearchCondition.Unseen());

                    if (uids == null || !uids.Any())
                    {
                        return string.Empty;
                    }

                    var newMessages = client.GetMessages(uids, false);
                    if (!string.IsNullOrEmpty(subjectKeyword))
                    {
                        latestMsg = newMessages.FirstOrDefault(p => p.Subject.Contains(subjectKeyword));
                    }

                    if (latestMsg != null && !string.IsNullOrEmpty(latestMsg.Body))
                    {
                        return latestMsg.Body;
                    }

                    return string.Empty;
                }

            }
            catch
            {
                return string.Empty;
            }
        }

        private static MailMessage GetNewEmailIMAP(string subjectKeyword = "")
        {
            try
            {
                using (ImapClient client = new ImapClient(IMAP_MAIL_HOST, IMAP_MAIL_PORT, IMAP_MAIL_USERNAME, IMAP_MAIL_PASSWORD, AuthMethod.Login, IMAP_SSL))
                {
                    MailMessage latestMsg = null;
                    var uids = client.Search(SearchCondition.Unseen());

                    if (uids == null || !uids.Any())
                    {
                        return null;
                    }

                    var newMessages = client.GetMessages(uids, false);
                    if (!string.IsNullOrEmpty(subjectKeyword))
                    {
                        latestMsg = newMessages.FirstOrDefault(p => p.Subject.Contains(subjectKeyword));
                    }

                    return latestMsg;
                }
            }
            catch
            {
                return null;
            }
        }

        private static List<MailMessage> GetNewEmailsIMAP(string subjectKeyword = "")
        {
            try
            {
                using (ImapClient client = new ImapClient(IMAP_MAIL_HOST, IMAP_MAIL_PORT, IMAP_MAIL_USERNAME, IMAP_MAIL_PASSWORD, AuthMethod.Login, IMAP_SSL))
                {
                    var uids = client.Search(SearchCondition.Unseen());

                    if (uids == null || !uids.Any())
                    {
                        return null;
                    }

                    var newMessages = client.GetMessages(uids, false);
                    if (!string.IsNullOrEmpty(subjectKeyword))
                    {
                        if(newMessages.Any(p => p.Subject.Contains(subjectKeyword)))
                            return newMessages.Where(p => p.Subject.Contains(subjectKeyword)).ToList();                       
                    }

                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        private static bool CleanInboxIMAP(string subjectKeyword = "")
        {
            try
            {
                using (ImapClient client = new ImapClient(IMAP_MAIL_HOST, IMAP_MAIL_PORT, IMAP_MAIL_USERNAME, IMAP_MAIL_PASSWORD, AuthMethod.Login, IMAP_SSL))
                {
                    IEnumerable<uint> uids;

                    if (string.IsNullOrEmpty(subjectKeyword))
                    {
                        uids = client.Search(SearchCondition.All());
                    }
                    else
                    {
                        uids = client.Search(SearchCondition.All().And(SearchCondition.Subject(subjectKeyword)));
                    }

                    if (uids != null && uids.Any())
                    {
                        client.DeleteMessages(uids);
                    }

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}
