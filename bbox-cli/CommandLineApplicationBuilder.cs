using System.Collections.Generic;
using BBox.Cli.Commands;
using Microsoft.Extensions.CommandLineUtils;

namespace BBox.Cli
{
    public class CommandLineApplicationBuilder
    {
        public const string Help = "-h|--help";
        private readonly IEnumerable<IBBoxCommand> _bboxCommands;

        public CommandLineApplicationBuilder(IEnumerable<IBBoxCommand> bboxCommands)
        {
            _bboxCommands = bboxCommands;
        }
        
        public CommandLineApplication Build()
        {
            var commandLineApplication = new CommandLineApplication()
            {
                FullName = "Proximus bbox 3 cli"
            };

            commandLineApplication.HelpOption(Help);
            commandLineApplication.VersionOption("--version", "1.0.0");

            foreach (var bboxCommand in _bboxCommands)
                commandLineApplication.Command(bboxCommand.Name, bboxCommand.Action);

            return commandLineApplication;
        }
    }
}