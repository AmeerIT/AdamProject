using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Book
{
    public partial class Form1 : Form
    {
        private const string StartServer = "Start server";
        private const string StopServer = "Stop server";

        //Gets the connection string from the App.config
        private string ConnectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
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

            label3.Text = "127.0.0.1";
            label5.Text = "12345";

            var Connected = new SQL(ConnectionString).CheckConnection();
            //Toggle between the colors of the button
            btnConnect.BackColor = Connected ? Color.White : Color.Green;
            //Toggle between the colors of the buttons font
            btnConnect.ForeColor = Connected ? Color.Black : Color.White;
            //Toggle between the text of the button
            btnConnect.Text = Connected ? StopServer : StartServer;

            itemsBox.Items.Clear();

            //var dt = new SQL(ConnectionString).GetAllBooks();
            var dt = await Task.Factory.StartNew(() => { return new SQL(ConnectionString).GetAllBooks(); });

            for (var i = 0; i < dt.Tables["Table"]?.Rows?.Count; i++)
                AddBookToGUI(new Book()
                {
                    id = int.Parse($"{dt.Tables["Table"].Rows[i]["id"]}"),
                    Name = $"{dt.Tables["Table"].Rows[i]["Name"]}",
                    Author = $"{dt.Tables["Table"].Rows[i]["Author"]}",
                    Genre = $"{dt.Tables["Table"].Rows[i]["Genre"]}",
                    Available = bool.Parse($"{dt.Tables["Table"].Rows[i]["Available"]}")
                });
        }

        /// <summary>
        /// Scans the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteSelectedObjects_Click(object sender, EventArgs e)
        {
            var sql = new SQL(ConnectionString);
            //easiest practice, loop from the bottom of the list, remove the lowest
            //index then move up, this will eliminate the index confusion and errors
            for (var i = itemsBox.CheckedItems.Count; i > 0; i--)
            {
                sql.Delete(((Book)itemsBox.CheckedItems[i - 1]).id);
                itemsBox.Items.Remove(itemsBox.CheckedItems[i - 1]);
            }
        }

        /// <summary>
        /// Queries the selected DB for data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteAllObjects_Click(object sender, EventArgs e)
        {
            //the items is a list of objects, C# include a native function
            //to clear the list property of type objectcollection
            itemsBox.Items.Clear();
        }

        /// <summary>
        /// Uploads the selected file to the selected DB
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpload_Click(object sender, EventArgs e)
        {
            var sQL = new SQL(ConnectionString);
            UpdateStatus("Select a file");
            using (var openFile = new OpenFileDialog() { Multiselect = false })
            {
                UpdateStatus("Reading file");
                //if the user selected a file
                if (openFile.ShowDialog().Equals(DialogResult.OK))
                {
                    //read the file
                    var input = File.ReadAllText(openFile.FileName);
                    //split based on the return cartridge
                    var inputarray = input.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    //adds items to the list
                    for (var i = 0; i < inputarray.Length; i++)
                    {
                        var book = new Book(inputarray[i]);

                        if (string.IsNullOrWhiteSpace(book.Name))
                            book = new Book("Invalid###Invalid###Invalid###False");
                        else
                            if (sQL.Insert(book))
                            AddBookToGUI(book);

                    }
                }
                else
                    MessageBox.Show("User cancelled Action", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            UpdateStatus(idle);
        }

        private void AddBookToGUI(Book book) => itemsBox.Items.Add(book);
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
