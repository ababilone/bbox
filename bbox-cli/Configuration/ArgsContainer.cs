namespace BBox.Cli.Configuration
{
    public class ArgsContainer
    {
        public string[] Args { get; }

        public ArgsContainer(string[] args)
        {
            Args = args;
        }
    }
}