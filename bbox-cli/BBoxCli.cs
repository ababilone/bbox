using System;
using BBox.Cli.Configuration;
using Microsoft.Extensions.CommandLineUtils;

namespace BBox.Cli
{
    public class BBoxCli
    {
        private readonly CommandLineApplication _commandLineApplication;
        private readonly ArgsContainer _argsContainer;

        public BBoxCli(CommandLineApplication commandLineApplication, ArgsContainer argsContainer)
        {
            _commandLineApplication = commandLineApplication;
            _argsContainer = argsContainer;
        }
        
        public int Execute()
        {
            try
            {
                if (_argsContainer.Args.Length == 0)
                    _commandLineApplication.ShowHelp();
                else
                    return _commandLineApplication.Execute(_argsContainer.Args);
            }
            catch (CommandParsingException ex)
            {
                Console.WriteLine(ex.Message);
                _commandLineApplication.ShowHelp();
            }

            return 1;
        }
    }
}