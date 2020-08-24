using System;
using System.Data;
using System.Data.Sql;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdamProject
{

    public partial class Form1 : Form
    {
        private const string StartServer = "Start server";
        private const string StopServer = "Stop server";
        static readonly string idle = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Connects to a pre-configured server and db
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnConnect_Click(object sender, EventArgs e)
        {
            btnConnect.BackColor = btnConnect.BackColor == Color.Green ? Color.White : Color.Green;
            btnConnect.ForeColor = btnConnect.ForeColor == Color.White ? Color.Black : Color.White;
            btnConnect.Text = btnConnect.Text == StartServer ? StopServer : StartServer;
        }

        /// <summary>
        /// Scans the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteSelectedObjects_Click(object sender, EventArgs e)
        {
            for (int i = itemsBox.CheckedItems.Count; i > 0; i--)
                itemsBox.Items.Remove(itemsBox.CheckedItems[i- 1]);
        }

        /// <summary>
        /// Queries the selected DB for data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteAllObjects_Click(object sender, EventArgs e)
        {
            itemsBox.Items.Clear();
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
                UpdateStatus("Reading file");
                //if the user selected a file
                if (openFile.ShowDialog().Equals(DialogResult.OK))
                {
                    var input = File.ReadAllText(openFile.FileName);
                    var inputarray = input.Split('\n');
                    for (int i = 0; i < inputarray.Length; i++)
                        itemsBox.Items.Add(new Book(inputarray[i]));
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
