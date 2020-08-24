namespace AdamProject
{
    public class Book
    {
        string Name;
        string Author;
        string Genre;
        bool Available;

        public Book(string input)
        {
            var inputarray = input.Replace("###", "\n").Split('\n');
            Name = inputarray[0];
            Author = inputarray[1];
            Genre = inputarray[2];
            bool.TryParse(inputarray[3], out Available);
        }

        public override string ToString() => $"Book: {Name} - by {Author}, Genre {Genre} - {(Available ? "Available" : "Out of stock")}";
    }
}
