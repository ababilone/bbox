using System;
using Microsoft.Extensions.CommandLineUtils;

namespace BBox.Cli.Commands
{
    public interface IBBoxCommand
    {
        string Name { get; }
        Action<CommandLineApplication> Action { get; }
    }
}