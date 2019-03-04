using Renci.SshNet;
using StreetlightVision.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;

namespace StreetlightVision.Utilities
{
    class FtpUtility
    {
        private const int FTP_RETRY_TIMEOUT_SECOND = 480;

        #region Variables

        private string _host;
        private string _username;
        private string _password;
        private string _directoryPath;
        private FtpWebRequest _ftpRequest;
        private string _ftpUrl;

        #endregion //Variables        

        #region Contructor

        public FtpUtility(string host, string username, string password, string directoryPath = "")
        {
            _host = host;
            _username = username;
            _password = password;
            _directoryPath = directoryPath;
            _ftpUrl = String.Format("ftp://{0}", _host);
            if (!string.IsNullOrEmpty(_directoryPath)) _ftpUrl = String.Format("ftp://{0}/{1}", _host, _directoryPath);
        }

        #endregion //Contructor

        #region Public methods

        /// <summary>
        /// Get all filenames of working directory. If we don't specify the directoryPath in the Construtor,
        /// it will get all file names at root.
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllFileNames(bool sftpMode = false)
        {
            try
            {
                if (sftpMode)
                {
                    using (var sftp = new SftpClient(_host, _username, _password))
                    {
                        try
                        {
                            sftp.Connect();
                            var files = sftp.ListDirectory(_directoryPath);
                            var filesName = files.Where(p => p.IsRegularFile == true).Select(p => p.Name).ToList();

                            return filesName;
                        }
                        catch
                        {
                            return new List<string>();
                        }
                    }
                }
                else
                {
                    _ftpRequest = (FtpWebRequest)WebRequest.Create(_ftpUrl);
                    _ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                    _ftpRequest.Credentials = new NetworkCredential(_username, _password);

                    var response = (FtpWebResponse)_ftpRequest.GetResponse();
                    Stream responseStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(responseStream);
                    string lines = reader.ReadToEnd();
                    reader.Close();
                    response.Close();
                    return lines.SplitEx(new string[] { "\r\n" });

                }
            }
            catch (WebException e)
            {
                Console.Error.WriteLine("Get error in {0} method . Detail error: {1}", GetCurrentMethod(), e.Message);
                return new List<string>();
            }
        }

        /// <summary>
        /// Wait and get a file's content based on its pattern
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public string WaitAndGetFileName(string pattern, bool sftpMode = false)
        {
            var filename = GenericOperation<string>.Retry(() => GetFileName(pattern, sftpMode), (c) => c != null, FTP_RETRY_TIMEOUT_SECOND);
            return filename;
        }
        
        #endregion //Public methods

        #region Private methods

        /// <summary>
        /// Get the first occurence filename based on name's pattern
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        private string GetFileName(string pattern, bool sftpMode = false)
        {
            var filenames = GetAllFileNames(sftpMode);
            return filenames.FirstOrDefault(f => f.IndexOf(pattern, StringComparison.InvariantCultureIgnoreCase) != -1);
        }

        /// <summary>
        /// Get filename based on its pattern and LastModified property. 
        /// If LastModified is equal expectedModifiedDate, return its name. Otherwise, return empty string.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="expectedModifiedDate"></param>
        /// <returns></returns>
        private string GetFileContent(string filename, DateTime expectedModifiedDate, bool sftpMode = false)
        {
            if (sftpMode)
            {
            }
            else
            {
                var filePath = String.Format("ftp://{0}{1}", _ftpUrl, filename);
                try
                {
                    _ftpRequest = (FtpWebRequest)WebRequest.Create(filePath);
                    _ftpRequest.Method = WebRequestMethods.Ftp.GetDateTimestamp;
                    _ftpRequest.Credentials = new NetworkCredential(_username, _password);

                    var response = (FtpWebResponse)_ftpRequest.GetResponse();
                    if (AreEquals(response.LastModified, expectedModifiedDate))
                    {
                        // Found the expected ftp file
                        WebClient clientRequest = new WebClient();
                        clientRequest.Credentials = new NetworkCredential(_username, _password);
                        try
                        {
                            byte[] newFileData = clientRequest.DownloadData(filePath);
                            string fileContent = System.Text.Encoding.UTF8.GetString(newFileData);
                            return fileContent;
                        }
                        catch (WebException e)
                        {
                            Console.Error.WriteLine("Error in {0} method . Detail error: {1}", GetCurrentMethod(), e.Message);
                            return String.Empty;
                        }
                    }
                    return String.Empty;
                }
                catch (WebException e)
                {
                    Console.Error.WriteLine("Error in {0} method . Detail error: {1}", GetCurrentMethod(), e.Message);
                    return String.Empty;
                }
            }

            return String.Empty;
        }
        
        /// <summary>
        /// Get current method's name
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private string GetCurrentMethod()
        {
            var st = new StackTrace();
            var sf = st.GetFrame(1);
            return sf.GetMethod().Name;
        }

        /// <summary>
        /// Check if 2 dates are equal
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        private bool AreEquals(DateTime date1, DateTime date2)
        {
            if (date1.Year != date2.Year ||
                date1.Month != date2.Month ||
                date1.Day != date2.Day ||
                date1.Hour != date2.Hour ||
                date1.Minute != date2.Minute)
                return false;
            return true;            
        }

        #endregion //Private methods


    }
}
