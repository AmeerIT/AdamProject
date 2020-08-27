namespace Book
{
    public class Book
    {
        public int id;
        public string Name;
        public string Author;
        public string Genre;
        public bool Available;
            
        public Book(string input)
        {
            GenereateBook(input.Replace("###", "\n").Split('\n'));
        }

        public Book()
        {

        }

        private void GenereateBook(string[] inputarray)
        {
            if (inputarray.Length != 4)
                return;

            Name = inputarray[0];
            Author = inputarray[1];
            Genre = inputarray[2];
            bool.TryParse(inputarray[3], out Available);
        }

        public override string ToString() => $"{Name}- by {Author}, Genre {Genre} - {(Available ? "Available" : "Out of stock")}";
    }
}
