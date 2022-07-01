using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TestingWorkflowActivity
{
    public class MyWorkflowActivity : CodeActivity
    {

        //In your case you probably have an input parameter that is the timesheet / weekly timesheet using account as an example
        [Input("UserAccount")]  
        public InArgument<Entity> UserAccount
        {  
            get;  
            set;  
        }

        protected override void Execute(CodeActivityContext context)
        {
            // Obtain the Organization Service factory service from the service provider
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)context.GetExtension<IOrganizationServiceFactory>();
            IWorkflowContext worfklowContext = context.GetExtension<IWorkflowContext>();
            IOrganizationService service = factory.CreateOrganizationService(worfklowContext.UserId);
            ITracingService tracer = context.GetExtension<ITracingService>();

            this.MyWorkflowLogic(service, tracer, UserAccount.Get(context));
        }


        public void MyWorkflowLogic(IOrganizationService organizationService, ITracingService tracer, Entity account)
        {
            //I can call this method from a console application and just supply the parameters it expects and run it locally.
            //You'd move all of your logic out of the Execute above and put it here. Here I'm retrieving contacts associated with the account as an example 
            QueryExpression q = new QueryExpression("contact");
            q.ColumnSet = new ColumnSet(true);
            FilterExpression f = new FilterExpression();
            f.AddCondition("parentcustomerid", ConditionOperator.Equal, account["accountid"]);
            q.Criteria = f;

            int test = account.GetAttributeValue<int?>("numberofemployees") ?? 0;
            tracer.Trace("Account {0} has {1} employees", account.GetAttributeValue<string>("name"), test);

            var contactsForAccount = organizationService.RetrieveMultiple(q);

            tracer.Trace("Retrieved {0} contact(s) for account {1}", contactsForAccount.Entities.Count, account.GetAttributeValue<string>("name"));
            foreach (var contact in contactsForAccount.Entities)
            {
                tracer.Trace("Retrieved contact {0}", contact.GetAttributeValue<string>("fullname"));
            }
        }
    }
}
