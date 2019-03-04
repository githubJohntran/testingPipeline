using Fiddler;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using StreetlightVision.Extensions;
using System.Security.Cryptography.X509Certificates;

namespace StreetlightVision.Utilities
{
    public class FiddlerUtility
    {
        private Proxy _oSecureEndpoint;
        private string _sSecureEndpointHostname = "localhost";
        private int _iSecureEndpointPort = 7777;
        private CancellationTokenSource _source;

        public IList<CustomizedSession> Sessions { get; private set; }

        public FiddlerUtility()
        {
            Sessions = new List<CustomizedSession>();
        }

        /// <summary>
        /// Start fiddler
        /// </summary>
        public void StartCapture()
        {
            _source = new CancellationTokenSource();
            Sessions.Clear();
            Task.Factory.StartNew(() => Capturing(this), _source.Token);
            Wait.ForSeconds(4);
        }        

        /// <summary>
        /// Start and capture requests, responses
        /// </summary>
        /// <param name="listenner"></param>
        private void Capturing(FiddlerUtility listenner)
        {
            CONFIG.bCaptureCONNECT = true;
            var cert = InstallCertificate();// getting true

            FiddlerApplication.SetAppDisplayName("StreetlightVisionCMSTests");
            FiddlerApplication.Prefs.SetBoolPref("fiddler.network.streaming.abortifclientaborts", true);
            FiddlerCoreStartupFlags oFCSF = FiddlerCoreStartupFlags.Default;
            //oFCSF = (oFCSF & ~FiddlerCoreStartupFlags.DecryptSSL);      

            int iPort = 0;
            FiddlerApplication.Startup(iPort, oFCSF);
            _oSecureEndpoint = FiddlerApplication.CreateProxyEndpoint(_iSecureEndpointPort, true, _sSecureEndpointHostname);

            if (_oSecureEndpoint == null)
            {
                throw new Exception("Cannot start Fiddler !!!");
            }

            Wait.ForSeconds(2);
            
            FiddlerApplication.BeforeRequest += delegate(Session oS)
            {
                if (listenner._source.IsCancellationRequested) return;
                if (oS.hostname.Contains(Settings.HostName))
                {
                    Monitor.Enter(Sessions);
                    Sessions.Add(new CustomizedSession(oS));
                    Monitor.Exit(Sessions);
                    oS.bBufferResponse = true;
                }
            };

            FiddlerApplication.AfterSessionComplete += delegate(Session oS)
            {
                if (listenner._source.IsCancellationRequested) return;

                if (oS.hostname.Contains(Settings.HostName))
                {
                    Monitor.Enter(Sessions);
                    var oldSession = Sessions.FirstOrDefault(s => s.Session.id == oS.id);
                    if (oldSession != null)
                        Sessions.Remove(oldSession);
                    Sessions.Add(new CustomizedSession(oS));
                    Monitor.Exit(Sessions);
                }
            };

            WaitingForFiddlerStarted();
        }        

        /// <summary>
        /// Stop fiddler
        /// </summary>
        public void StopCapture()
        {
            if (FiddlerApplication.IsStarted())
            {
                Wait.ForSeconds(4);
                if (null != _oSecureEndpoint) _oSecureEndpoint.Dispose();
                FiddlerApplication.Shutdown();
                WaitingForFiddlerStopped();
                _source.Dispose();
            }            
        }

        /// <summary>
        /// Install Certificate for HTTPS
        /// </summary>
        /// <returns></returns>
        public static bool InstallCertificate()
        {
            CertMaker.createRootCert();
            X509Certificate2 oRootCert = CertMaker.GetRootCertificate();
            SetMachineTrust(oRootCert);

            CONFIG.IgnoreServerCertErrors = true;

            if (!CertMaker.rootCertExists())
            {
                if (!CertMaker.createRootCert())
                {
                    throw new Exception("Unable to create cert for FiddlerCore.");
                }
            }

            if (!CertMaker.rootCertIsTrusted())
            {
                if (!CertMaker.trustRootCert())
                {
                    throw new Exception("Unable to install FiddlerCore's cert.");
                }
            }           

            return true;
        }

        private static bool SetMachineTrust(X509Certificate2 oRootCert)
        {
            try
            {
                X509Store certStore = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
                certStore.Open(OpenFlags.ReadWrite);
                try
                {
                    certStore.Add(oRootCert);
                }
                finally
                {
                    certStore.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Waiting for fiddler started completely
        /// </summary>
        /// <param name="msWaitTimeout"></param>
        /// <returns></returns>
        public bool WaitingForFiddlerStarted(double msWaitTimeout = 5000)
        {
            DateTime endingTime = DateTime.Now.AddMilliseconds(msWaitTimeout);
            while (DateTime.Now < endingTime)
            {
                try
                {
                    if (FiddlerApplication.IsStarted())
                    {
                        return true;
                    }
                }
                catch
                {
                }
                Wait.ForMilliseconds(200);
            }
            throw new TimeoutException("While waiting for Fiddler started.");
        }

        /// <summary>
        /// Waiting for fiddler stopped completely
        /// </summary>
        /// <param name="msWaitTimeout"></param>
        /// <returns></returns>
        public bool WaitingForFiddlerStopped(double msWaitTimeout = 5000)
        {
            DateTime endingTime = DateTime.Now.AddMilliseconds(msWaitTimeout);
            while (DateTime.Now < endingTime)
            {
                try
                {
                    if (!FiddlerApplication.IsStarted())
                    {
                        return true;
                    }
                }
                catch
                {
                }
                Wait.ForMilliseconds(200);
            }
            throw new TimeoutException("While waiting for Fiddler stopped.");
        }
    }
    
    /// <summary>
    /// Customized Session
    /// </summary>
    public class CustomizedSession
    {
        public CustomizedSession(Session session)
        {
            Session = session;
            URL = session.fullUrl;
            if (session.bHasResponse)
                Response = session.GetResponseBodyAsString();
            else
                Response = string.Empty;
            Request = session.GetRequestBodyAsString();
            
        }

        public Session Session { get; private set; }
        public string Response { get; private set; }
        public string Request { get; private set; }
        public string URL { get; private set; }

    }
}
