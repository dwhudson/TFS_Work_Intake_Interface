using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using System.Collections.ObjectModel;
using Microsoft.TeamFoundation.Framework.Common;

namespace TfsApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            // Let the user choose a TFS Server.
            Console.Write("Please enter a valid TFS Server or URI: ");
            String tfsServer = Console.ReadLine();
            tfsServer = tfsServer.Trim();

            // Connect to the TeamFoundationServer.
            Console.WriteLine();
            Console.Write("Connecting to Team Foundation Server {0}...", tfsServer);
            TeamFoundationServer tfs =
                TeamFoundationServerFactory.GetServer(tfsServer);

            // Connect to the WorkItemStore.
            Console.WriteLine();
            Console.Write("Reading from the Work Item Store...");
            WorkItemStore workItemStore = (WorkItemStore)tfs.GetService(typeof(WorkItemStore));

            // Display the details about the TeamFoundationServer.
            Console.WriteLine("\n");
            Console.WriteLine("Team Foundation Server details");
            Console.WriteLine("Server Name: " + tfs.Name);
            Console.WriteLine("Uri: " + tfs.Uri);
            Console.WriteLine("AuthenticatedUserDisplayName: " + tfs.AuthenticatedUserDisplayName);
            Console.WriteLine("AuthenticatedUserName: " + tfs.AuthenticatedUserName);
            Console.WriteLine("WorkItemStore:");

            //  List the Projects in the WorkItemStore.
            Console.WriteLine("  Projects.Count: " + workItemStore.Projects.Count);
            foreach (Project pr in workItemStore.Projects)
            {
                Console.WriteLine("    " + pr.Name);
            }
        }
    }
}
