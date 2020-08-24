using System;
using System.Data;
using System.Data.Sql;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdamProject
{

    public partial class Form1 : Form
    {
        static readonly string idle = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateStatus(idle);
        }

        /// <summary>
        /// Connects to a pre-configured server and db
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnConnect_Click(object sender, EventArgs e)
        {
            UpdateStatus("Getting servers list");

            btnConnect.Enabled = false;
            await Task.Factory.StartNew(() =>
            {
                //System class that lists the available servers/instances on the network.
                //Disclamer: 
                //this class Does not List on demand db such localdb located outside the
                //default SQLServer folder, i.e. localdb created per project.
                SqlDataSourceEnumerator instance = SqlDataSourceEnumerator.Instance;
                DataTable table = instance.GetDataSources();
                string ServerName = Environment.MachineName;
                //iterate through the available instances 
                foreach (DataRow row in table.Rows)
                {
                    //Add the instance to the treeview
                    Invoke((MethodInvoker)delegate { treeView.Nodes.Add(new TreeNode($"{row["Name"]}")); });
                    Console.WriteLine(ServerName + "\\" + row["InstanceName"].ToString());
                }

                Invoke((MethodInvoker)delegate
                {
                    //Enable the operations based on the servers amount
                    btnQuery.Enabled =
                    btnScan.Enabled =

                    //check if the tree view have more than one item
                    btnUpload.Enabled = treeView.Nodes?.Count >= 1;
                    btnConnect.Enabled = true;
                    UpdateStatus(idle);
                });
            });
        }

        /// <summary>
        /// Scans the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnScan_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Queries the selected DB for data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Uploads the selected file to the selected DB
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpload_Click(object sender, EventArgs e)
        {
            UpdateStatus("Select a file");
            using (OpenFileDialog openFile = new OpenFileDialog() { Multiselect = false })
            {
                //if the user selected a file
                if (openFile.ShowDialog().Equals(DialogResult.OK))
                {

                }
            }
            UpdateStatus(idle);
        }

        private async void UpdateStatus(string status)
        {
            //Thread Safe method to update the message on the status bar
            //Can add a timer to reduce the use of UpdateStatus(idle);
            Invoke((MethodInvoker)delegate
            {
                lblStatus.Text = status;
            });
        }
    }
}
