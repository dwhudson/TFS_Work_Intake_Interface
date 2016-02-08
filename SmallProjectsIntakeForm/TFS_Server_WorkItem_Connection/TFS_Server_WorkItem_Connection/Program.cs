using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Common;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using ConnectionAndInformation;
using Microsoft.TeamFoundation.Common;
using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.TeamFoundation.Server;
using Microsoft.TeamFoundation.VersionControl.Controls;


namespace TfsApplication
{
    class Program
    {

        static void Main(String[] args)
        {
            // Connect to Team Foundation Server
            //     Server is the name of the server that is running the application tier for Team Foundation.
            //     Port is the port that Team Foundation uses. The default port is 8080.
            //     VDir is the virtual path to the Team Foundation application. The default path is tfs.

            string tfsServerAddress = "http://tfs13dev:8080/tfs/";
       
            //Create class object
            ConnectionAndInformation.ConnectionAndInformation cai = new ConnectionAndInformation.ConnectionAndInformation();
            //create URI
            cai.createUriFromStringAddress(tfsServerAddress);
            //TfsConfigurationServer
            cai.createConfigurationServer(cai.uriGetSet);
            //create catalog of team project collections
            cai.createCatalogTeamProjectCollections(cai.tfsConfigurationServerGetSet);

            cai.getCatalogTeamProjects(cai.catalogNodesGetSet);
         
            List<string> list = new List<string>();
            string info = String.Empty;
            var qText = String.Format(@"SELECT * FROM WorkItems WHERE [System.TeamProject] = 'TSDL' ORDER BY [System.Id]");
                //TfsTeamProjectCollection tpc = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(new Uri("http://tfs13dev:8080/tfs/"));
                var tpc = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(new Uri("http://tfs13dev:8080/tfs/"));
                var service = tpc.GetService<WorkItemStore>();
                //TfsTeamProjectCollection tpc = new TfsTeamProjectCollection(new Uri("http://tfs13dev:8080/tfs/"));
                WorkItemStore workItemStore = (WorkItemStore)tpc.GetService(typeof(WorkItemStore));
               // WorkItemCollection queryResults = workItemStore.Query("SELECT * FROM WorkItems WHERE [System.TeamProject] = LEAP");
               // WorkItemCollection queryResults = workItemStore.Query("SELECT [System.Id], [System.WorkItemType], [System.Title], [System.AssignedTo] FROM WorkItems WHERE [System.TeamProject] = 'ODS' ORDER by [System.Id] desc");
                //WorkItemStore workItemStore = new WorkItemStore(tpc);
                //Project teamProject = workItemStore.Projects["ODS"];
                //WorkItemType wit = teamProject.WorkItemTypes[""];
                Project teamProject = workItemStore.Projects["TSDL"];
                /*
                WorkItemType workItemType = teamProject.WorkItemTypes["Requirement"];

                WorkItem userStory = new WorkItem(workItemType)
                {
                    Title = "Recently ordered menu",
                    Description = "As a return customer, I want to see items that I've recently ordered."


                };
                userStory.Save();
            */
                Query query = new Query(service, qText);

                var workItems = query.RunQuery(); 

                foreach(WorkItem workItem in workItems)
                {
                    list.Add(workItem.Id + " - " + workItem.Title);
                }

                list.ForEach(i => Console.Write("{0}\n", i));

            /*
                WorkItemCollection queryResults = workItemStore.Query("SELECT * FROM WorkItems WHERE [System.TeamProject] = 'TSDL' ORDER BY [System.Id]");

                foreach(WorkItem wi in queryResults)
                {
                    Console.WriteLine("QueryResults: " + wi);
                
                }
            */
                
                


        }
    }
}
 

