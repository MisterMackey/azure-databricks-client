﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models
{
    public enum ClusterMode
    {
        /// <summary>
        /// The standard cluster mode. Recommended for single-user clusters. Can run SQL, Python, R, and Scala workloads.
        /// </summary>
        Standard,

        /// <summary>
        /// High concurrency cluster mode. Optimized to run concurrent SQL, Python, and R workloads. Does not support Scala. Previously known as Serverless. <see href="https://docs.microsoft.com/en-us/azure/databricks/clusters/configure#high-concurrency"/>
        /// </summary>
        HighConcurrency,

        /// <summary>
        /// A Single Node cluster is a cluster consisting of a Spark driver and no Spark workers. <see href="https://docs.microsoft.com/en-us/azure/databricks/clusters/single-node"/>
        /// </summary>
        SingleNode
    }

    /// <summary>
    /// Describes all of the metadata about a single Spark cluster in Databricks.
    /// </summary>
    /// <seealso cref="T:Microsoft.Azure.Databricks.DatabricksClient.ClusterInstance" />
    public record ClusterInfo : ClusterAttributes
    {
        public static ClusterInfo GetNewClusterConfiguration(string clusterName = null)
        {
            return new ClusterInfo
            {
                ClusterName = clusterName
            };
        }

        /// <summary>
        /// The canonical identifier for the cluster used by a run. This field is always available for runs on existing clusters. For runs on new clusters, it becomes available once the cluster is created. This value can be used to view logs by browsing to /#setting/sparkui/$cluster_id/driver-logs. The logs will continue to be available after the run completes.
        /// If this identifier is not yet available, the response won’t include this field.
        /// </summary>
        [JsonPropertyName("cluster_id")]
        public string ClusterId { get; set; }

        /// <summary>
        /// The canonical identifier for the Spark context used by a run. This field will be filled in once the run begins execution. This value can be used to view the Spark UI by browsing to /#setting/sparkui/$cluster_id/$spark_context_id. The Spark UI will continue to be available after the run has completed.
        /// If this identifier is not yet available, the response won’t include this field.
        /// </summary>
        [JsonPropertyName("spark_context_id")]
        public long SparkContextId { get; set; }

        /// <summary>
        /// Number of worker nodes that this cluster should have. A cluster has one Spark Driver and num_workers Executors for a total of num_workers + 1 Spark nodes.
        /// </summary>
        /// <remarks>
        /// Note: When reading the properties of a cluster, this field reflects the desired number of workers rather than the actual current number of workers.
        /// For instance, if a cluster is resized from 5 to 10 workers, this field will immediately be updated to reflect the target size of 10 workers, whereas the workers listed in spark_info will gradually increase from 5 to 10 as the new nodes are provisioned.
        /// </remarks>
        [JsonPropertyName("num_workers")]
        public int? NumberOfWorkers { get; set; }

        /// <summary>
        /// Parameters needed in order to automatically scale clusters up and down based on load.Note: autoscaling works best with DB runtime versions 3.0 or later.
        /// </summary>
        [JsonPropertyName("autoscale")]
        public AutoScale AutoScale { get; set; }

        /// <summary>
        /// Creator user name. The field won’t be included in the response if the user has already been deleted.
        /// </summary>
        [JsonPropertyName("creator_user_name")]
        public string CreatorUserName { get; set; }

        /// <summary>
        /// Node on which the Spark driver resides. The driver node contains the Spark master and the Databricks application that manages the per-notebook Spark REPLs.
        /// </summary>
        [JsonPropertyName("driver")]
        public SparkNode Driver { get; set; }

        /// <summary>
        /// Nodes on which the Spark executors reside.
        /// </summary>
        [JsonPropertyName("executors")]
        public IEnumerable<SparkNode> Executors { get; set; }

        /// <summary>
        /// Port on which Spark JDBC server is listening, in the driver nod. No service will be listening on on this port in executor nodes.
        /// </summary>
        [JsonPropertyName("jdbc_port")]
        public int JdbcPort { get; set; }

        /// <summary>
        /// Current state of the cluster.
        /// </summary>
        [JsonPropertyName("state")]
        public ClusterState? State { get; set; }

        /// <summary>
        /// A message associated with the most recent state transition (e.g., the reason why the cluster entered a TERMINATED state).
        /// </summary>
        [JsonPropertyName("state_message")]
        public string StateMessage { get; set; }

        /// <summary>
        /// Time (in epoch milliseconds) when the cluster creation request was received (when the cluster entered a PENDING state).
        /// </summary>
        [JsonPropertyName("start_time")]
        public DateTimeOffset? StartTime { get; set; }

        /// <summary>
        /// Time (in epoch milliseconds) when the cluster was terminated, if applicable.
        /// </summary>
        [JsonPropertyName("terminated_time")]
        public DateTimeOffset? TerminatedTime { get; set; }

        /// <summary>
        /// Time when the cluster driver last lost its state (due to a restart or driver failure).
        /// </summary>
        [JsonPropertyName("last_state_loss_time")]
        public DateTimeOffset? LastStateLossTime { get; set; }

        /// <summary>
        /// Time (in epoch milliseconds) when the cluster was last active. A cluster is active if there is at least one command that has not finished on the cluster. This field is available after the cluster has reached a RUNNING state. Updates to this field are made as best-effort attempts. Certain versions of Spark do not support reporting of cluster activity. Refer to Automatic termination for details.
        /// </summary>
        [JsonPropertyName("last_activity_time")]
        public DateTimeOffset? LastActivityTime { get; set; }

        /// <summary>
        /// Total amount of cluster memory, in megabytes
        /// </summary>
        [JsonPropertyName("cluster_memory_mb")]
        public long ClusterMemoryMb { get; set; }

        /// <summary>
        /// Number of CPU cores available for this cluster. Note that this can be fractional, e.g. 7.5 cores, since certain node types are configured to share cores between Spark nodes on the same instance.
        /// </summary>
        [JsonPropertyName("cluster_cores")]
        public float ClusterCores { get; set; }

        /// <summary>
        /// Tags that are added by Databricks regardless of any custom_tags, including:
        ///     Vendor: Databricks
        ///     Creator: username_of_creator
        ///     ClusterName: name_of_cluster
        ///     ClusterId: id_of_cluster
        ///     Name: Databricks internal use
        /// </summary>
        [JsonPropertyName("default_tags")]
        public Dictionary<string, string> DefaultTags { get; set; }

        /// <summary>
        /// Cluster log delivery status.
        /// </summary>
        [JsonPropertyName("cluster_log_status")]
        public LogSyncStatus ClusterLogSyncStatus { get; set; }

        /// <summary>
        /// Information about why the cluster was terminated. This field only appears when the cluster is in a TERMINATING or TERMINATED state.
        /// </summary>
        [JsonPropertyName("termination_reason")]
        public TerminationReason TerminationReason { get; set; }

        [JsonPropertyName("pinned_by_user_name")]
        public string PinnedByUserName { get; set; }

        public ClusterInfo WithAutoScale(int minWorkers, int maxWorkers)
        {
            AutoScale = new AutoScale { MinWorkers = minWorkers, MaxWorkers = maxWorkers };
            NumberOfWorkers = null;
            return this;
        }

        public ClusterInfo WithNumberOfWorkers(int numWorkers)
        {
            NumberOfWorkers = numWorkers;
            AutoScale = null;
            return this;
        }

        /// <summary>
        /// When enabled:
        ///     Allows users to run SQL, Python, and PySpark commands. Users are restricted to the SparkSQL API and DataFrame API, and therefore cannot use Scala, R, RDD APIs, or clients that directly read the data from cloud storage, such as DBUtils.
        ///     Cannot acquire direct access to data in the cloud via DBFS or by reading credentials from the cloud provider’s metadata service.
        ///     Requires that clusters run Databricks Runtime 3.5 or above.
        ///     Must run their commands on cluster nodes as a low-privilege user forbidden from accessing sensitive parts of the filesystem or creating network connections to ports other than 80 and 443.
        /// </summary>
        private bool _enableTableAccessControl;
        public ClusterInfo WithTableAccessControl(bool enableTableAccessControl)
        {
            _enableTableAccessControl = enableTableAccessControl;

            if (SparkConfiguration == null)
            {
                SparkConfiguration = new Dictionary<string, string>();
            }

            if (enableTableAccessControl)
            {
                SparkConfiguration["spark.databricks.acl.dfAclsEnabled"] = "true";
            }
            else
            {
                SparkConfiguration.Remove("spark.databricks.acl.dfAclsEnabled");
            }

            var allowedReplLang = DatabricksAllowedReplLang(enableTableAccessControl, _clusterMode);

            if (string.IsNullOrEmpty(allowedReplLang))
            {
                SparkConfiguration.Remove("spark.databricks.repl.allowedLanguages");
            }
            else
            {
                SparkConfiguration["spark.databricks.repl.allowedLanguages"] = allowedReplLang;
            }

            return this;
        }

        private ClusterMode _clusterMode = ClusterMode.Standard;

        public ClusterInfo WithClusterMode(ClusterMode clusterMode)
        {
            _clusterMode = clusterMode;

            if (CustomTags == null)
            {
                CustomTags = new Dictionary<string, string>();
            }

            if (SparkConfiguration == null)
            {
                SparkConfiguration = new Dictionary<string, string>();
            }

            switch (clusterMode)
            {
                case ClusterMode.HighConcurrency:
                    CustomTags["ResourceClass"] = "Serverless";
                    SparkConfiguration["spark.databricks.cluster.profile"] = "serverless";
                    SparkConfiguration.Remove("spark.master");
                    break;
                case ClusterMode.SingleNode:
                    CustomTags["ResourceClass"] = "SingleNode";
                    SparkConfiguration["spark.databricks.cluster.profile"] = "singleNode";
                    SparkConfiguration["spark.master"] = "local[*]";
                    NumberOfWorkers = 0;
                    break;
                default: // Standard mode
                    CustomTags.Remove("ResourceClass");
                    SparkConfiguration.Remove("spark.databricks.cluster.profile");
                    SparkConfiguration.Remove("spark.master");
                    break;
            }

            var allowedReplLang = DatabricksAllowedReplLang(_enableTableAccessControl, clusterMode);

            if (string.IsNullOrEmpty(allowedReplLang))
            {
                SparkConfiguration.Remove("spark.databricks.repl.allowedLanguages");
            }
            else
            {
                SparkConfiguration["spark.databricks.repl.allowedLanguages"] = allowedReplLang;
            }

            return this;
        }

        private static string DatabricksAllowedReplLang(bool enableTableAccessControl, ClusterMode clusterMode) =>
            enableTableAccessControl ? "python,sql" : clusterMode == ClusterMode.HighConcurrency ? "sql,python,r" : null;

        public ClusterInfo WithAutoTermination(int? autoTerminationMinutes)
        {
            AutoTerminationMinutes = autoTerminationMinutes.GetValueOrDefault();
            return this;
        }

        public ClusterInfo WithRuntimeVersion(string runtimeVersion)
        {
            RuntimeVersion = runtimeVersion;
            return this;
        }

        /// <summary>
        /// This enables Photon engine on AWS Graviton-enabled clusters.
        /// For Azure Databricks, this setting has no effect. Specify Photon-specific runtimes instead.
        /// </summary>
        /// <see cref="https://docs.databricks.com/clusters/graviton.html#databricks-rest-api"/>
        public ClusterInfo WithRuntimeEngine(RuntimeEngine engine)
        {
            RuntimeEngine = engine;
            return this;
        }

        public ClusterInfo WithNodeType(string workerNodeType, string driverNodeType = null)
        {
            NodeTypeId = workerNodeType;
            DriverNodeTypeId = driverNodeType;
            return this;
        }

        public ClusterInfo WithClusterLogConf(string dbfsDestination)
        {
            ClusterLogConfiguration =
                new ClusterLogConf { Dbfs = new DbfsStorageInfo { Destination = dbfsDestination } };
            return this;
        }
    }
}