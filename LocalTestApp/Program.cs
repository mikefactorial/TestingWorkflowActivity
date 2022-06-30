using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingWorkflowActivity;

namespace LocalTestApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // e.g. https://yourorg.crm.dynamics.com
            string url = "https://yourorg.crm.dynamics.com";

            // e.g. you@yourorg.onmicrosoft.com
            string userName = "you@yourorg.onmicrosoft.com";

            // e.g. y0urp455w0rd 
            string password = "y0urp455w0rd";

            string conn = $@"
                Url = {url};
                AuthType = OAuth;
                UserName = {userName};
                Password = {password};
                AppId = 51f81489-12ee-4a9e-aaae-a2591f45987d;
                RedirectUri = app://58145B91-0C36-4500-8554-080854F2AC97;
                LoginPrompt=Auto;
                RequireNewInstance = True";

            MyWorkflowActivity activity = new MyWorkflowActivity();
            DummyTracingService trace = new DummyTracingService();
            using (var svc = new CrmServiceClient(conn))
            {
                QueryExpression q = new QueryExpression("account");
                var allAccounts = svc.RetrieveMultiple(q);
                foreach (var account in allAccounts.Entities)
                {
                    activity.MyWorkflowLogic(svc, trace, account);
                }
                Console.WriteLine("Press any key to exit.");
                Console.ReadLine();
            }
        }
    }
}
