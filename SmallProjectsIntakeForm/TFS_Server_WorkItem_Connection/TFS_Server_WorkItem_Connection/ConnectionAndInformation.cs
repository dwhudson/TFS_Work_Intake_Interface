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
using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.TeamFoundation.VersionControl.Controls;

namespace ConnectionAndInformation
{
    class ConnectionAndInformation
    {
        private WorkItemCollection _workItemCollection;
        //private WorkItemStore _workItemStore;
        //private String TfsCollectionUri = "http://tfs13dev:8080/tfs/";
        private TfsTeamProjectCollection _tpcConnection;
        private Uri _uri;
        private TfsConfigurationServer _configurationServer;
        private ReadOnlyCollection<CatalogNode> _collectionNodes;
        private ReadOnlyCollection<CatalogNode> _projectNodes;

        private static Uri _collectionUri;
        private static TfsTeamProjectCollection _projectCollection;
        private static WorkItemStore _workItemStore;
        
     

        /*
        private void createWorkItemCollection(WorkItemStore wis, string query)
        {
            WorkItemCollection workItemCollection = wis.Query(query);
            _workItemCollection = workItemCollection;
        }
        */
        /*
        public WorkItemCollection workItemCollectionGetSet
        {
            get
            {
                return _workItemCollection;
            }
            set
            {
                _workItemCollection = value;
            }
        }

        public void createWorkItemStore(TfsTeamProjectCollection tpc) 
        {
            WorkItemStore workItemStore = (WorkItemStore)tpc.GetService(typeof(WorkItemStore));
            _workItemStore = workItemStore;
        }

        public WorkItemStore wisGetSet
        {
            get 
            {
                return _workItemStore;
            }
            set 
            {
                _workItemStore = value;
            }
        }

        public void createTeamProjectCollection(Uri uri)
        {
            TfsTeamProjectCollection tpc = new TfsTeamProjectCollection(uri);
            _tpcConnection = tpc;
        }

        public TfsTeamProjectCollection tfsTeamProjectCollection
        {
            get
            {
                return _tpcConnection;
            }
            set
            {
                _tpcConnection = value;
            }
        }
        */
        private static Uri CreateCollectionUri() 
        { 
            if(_collectionUri == null)
            {
                //Create a new team Project Collection using the TFS_COLLECTION_URI
                _collectionUri = new Uri("http://tfs13dev:8080/tfs/");
            }
            return _collectionUri;
        
        }

        private static TfsTeamProjectCollection CreateProjectCollection(Uri collectionUri)
        {
            _projectCollection = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(collectionUri);
            return _projectCollection;
        }

        private static WorkItemStore CreateWorkItemStore(TfsTeamProjectCollection projColl) 
        {
            return new WorkItemStore(projColl);
        }

        public static WorkItemStore GetWorkItemStore 
        {
            get 
            {
                return _workItemStore ?? (_workItemStore = CreateWorkItemStore(CreateProjectCollection(CreateCollectionUri())));
            }
        }

        public static List<WorkItem> GetWorkItemListForAreaPath(string areaPath)
        {
            _workItemStore = GetWorkItemStore;
            WorkItemCollection workItemCollection = _workItemStore.Query(
                "SELECT * FROM WorkItems WHERE [System.TeamProject] = 'LEAP'");

            var workItemList = new List<WorkItem>();

            foreach(WorkItem wi in workItemCollection) 
            {
                workItemList.Add(wi);
            }
            return workItemList;
        }

        public static List<WorkItem> GetWorkItemListById(string idList)
        {

            _workItemStore = GetWorkItemStore;
            WorkItemCollection workItemCollection = _workItemStore.Query(
                "SELECT * FROM WorkItems WHERE [System.TeamProject] = 'LEAP'");

            var _workItemList = new List<WorkItem>();

            foreach(WorkItem wi in workItemCollection)
            {
                _workItemList.Add(wi);
            }
            return _workItemList;
        }


//Correct methods
//***********************************************************


       
        public void createUriFromStringAddress(string uri) 
        { 
            Uri tfsUri = new Uri(uri);
            _uri = tfsUri; 
        }

        public Uri uriGetSet
        {
            get
            {
                return _uri;
            }
            set
            {
                _uri = value;
            }
        }

        public void createConfigurationServer(Uri uri)
        {
            TfsConfigurationServer configurationServer =
                TfsConfigurationServerFactory.GetConfigurationServer(uri);

            _configurationServer = configurationServer;
        }

        public TfsConfigurationServer tfsConfigurationServerGetSet
        {
            get
            {
                return _configurationServer;
            }
            set
            {
                _configurationServer = value;
            }
        }

        public void createCatalogTeamProjectCollections(TfsConfigurationServer cs)
        {
            // Get the catalog of team project collections
            ReadOnlyCollection<CatalogNode> collectionNodes = cs.CatalogNode.QueryChildren(
                new[] { CatalogResourceTypes.ProjectCollection },
                false, CatalogQueryOptions.None);

            _collectionNodes = collectionNodes;
        }

        public ReadOnlyCollection<CatalogNode> catalogNodesGetSet
        {
            get 
            {
                return _collectionNodes;
            }
            set 
            {
                _collectionNodes = value;
            }
        }

        public void createCatalogOfProjects(CatalogNode collectionNode)
        {
            ReadOnlyCollection<CatalogNode> projectNodes = collectionNode.QueryChildren(
                          new[] { CatalogResourceTypes.TeamProject },
                          false, CatalogQueryOptions.None);

            _projectNodes = projectNodes;
        }

        public ReadOnlyCollection<CatalogNode> projectCatalogGetSet 
        {
            get 
            {
                return _projectNodes;
            }
            set 
            {
                _projectNodes = value;
            }
        }

        public Guid guidCollectionId(CatalogNode collectionNode)
        {
            Guid collectionId = new Guid(collectionNode.Resource.Properties["InstanceId"]);
            return collectionId;
        }

        public TfsTeamProjectCollection tfsTeamProjectCollectionConfigServer(TfsConfigurationServer configurationServer, Guid collectionId)
        {
            TfsTeamProjectCollection teamProjectCollection = configurationServer.GetTeamProjectCollection(collectionId);
            return teamProjectCollection;
        }

        public void writeCollectionName(TfsTeamProjectCollection tpc)
        {
            Console.WriteLine("Collection: " + tpc.Name);
        }

        public void listTeamProjectsInCollection(ReadOnlyCollection<CatalogNode> projectNodes)
        {
            foreach (CatalogNode projectNode in projectNodes)
            {
                Console.WriteLine(" Team Project: " + projectNode.Resource.DisplayName);
            }
        }

        public void getCatalogTeamProjects(ReadOnlyCollection<CatalogNode> collectionNodes)
        {
            foreach(CatalogNode collectionNode in collectionNodes)
            {
                TfsTeamProjectCollection tpc = tfsTeamProjectCollectionConfigServer(tfsConfigurationServerGetSet, guidCollectionId(collectionNode));
                //write collection - call method
                writeCollectionName(tpc);
                // Get a catalog of team projects for the collection
                createCatalogOfProjects(collectionNode);
                //list team projects in collection foreach
                listTeamProjectsInCollection(projectCatalogGetSet);
            }
        }

        
    }
}
