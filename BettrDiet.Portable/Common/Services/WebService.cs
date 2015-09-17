using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WebAccess.ServiceReference;



namespace BettrDiet.Core.Common
{
    public class WebService
    {
        WebServiceSoapClient ws = new WebServiceSoapClient();
        public AuthData Auth { get; set; }

        private static WebService instance;

        public static WebService Instance
        {
            get { 
                if (instance==null)
                    instance=new WebService();
                return instance; }
            set { instance = value; }
        }


        public WebServiceSoapClient WS
        {
            get
            {
                // [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
                var b = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
                                
                b.MaxReceivedMessageSize = 1024 * 1024 * 50;
                var addr = new EndpointAddress(Server + "/WebService.asmx");
                
                return ws = new WebServiceSoapClient(b, addr);
            }
            set { ws = value; }
        }

        public WebService()
        {
            Server = "https://www.bettrfit.com";
            
            Auth = new AuthData();
        }

        private string _server;

        public string Server
        {
            get { return _server; 
            }
            set { _server = value; }
        }
        

        
    }
}
