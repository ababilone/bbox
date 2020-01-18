using System;
using System.Linq;
using System.Text;
using BBox.Client;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BBox.Cli.Commands
{
    public class BBoxWirelessCommand : IBBoxCommand
    {
        public BBoxWirelessCommand(IBBoxClient bboxClient, ILogger<BBoxWirelessCommand> logger)
        {
            Action = command =>
            {
                command.Description = "Configure wireless";
                command.HelpOption(CommandLineApplicationBuilder.Help);

                command.Command("list", listCommand =>
                {
                    listCommand.Description = "Listing the wireless interfaces";
                    var rawCommandOption = listCommand.Option("-r|--raw", "Display raw json data", CommandOptionType.NoValue);
                    var detailedCommandOption = listCommand.Option("-d|--detailed", "Display detailed information", CommandOptionType.NoValue);
                    
                    listCommand.OnExecute(() =>
                    {
                        
                        var bBoxResult = bboxClient.GetSSIDAsync().Result;
                        if (bBoxResult.Succeed)
                        {
                            if (rawCommandOption.HasValue())
                            {
                                Console.WriteLine(JsonConvert.SerializeObject(bBoxResult.Result, Formatting.Indented));
                            }
                            else
                            {
                                foreach (var ssidInfo in bBoxResult.Result.OrderBy(info => info.Metadata.SSID).ThenByDescending(info => info.Metadata.Enable).ThenByDescending(info => info.Metadata.Status))
                                {
                                    var enabled = ssidInfo.Metadata.Enable ? "enabled" : "disabled";
                                    var stringBuilder = new StringBuilder($"{ssidInfo.Metadata.SSID} ({enabled}, {ssidInfo.Metadata.Status})");

                                    if (detailedCommandOption.HasValue())
                                    {
                                        if (!string.IsNullOrEmpty(ssidInfo.Metadata.BSSID))
                                            stringBuilder.Append($", BSSID {ssidInfo.Metadata.BSSID}");

                                        var lowerLayers = ssidInfo.Metadata.LowerLayers == "Device/WiFi/Radios/Radio[RADIO2G4]" ? "2.4 Ghz" : "5.0 Ghz";
                                        stringBuilder.Append($", {lowerLayers}");
                                    }
                                    
                                    Console.WriteLine(stringBuilder.ToString());
                                }                                
                            }
                        }
                        else
                            logger.LogError($"Error while listing wireless: {bBoxResult}");
                    
                        return bBoxResult.Succeed ? 0 : 1;
                    });
                });
                
                command.Command("enable", enableCommand =>
                {
                    enableCommand.Description = "Enable the wireless interface";
                    enableCommand.OnExecute(() => {
                        var bBoxResult = bboxClient.EnableWirelessAsync(WirelessType.Private).Result;
                        if (bBoxResult.Succeed)
                            logger.LogInformation($"Successfully enabled wireless");
                        else
                            logger.LogError($"Error while enabling wireless: {bBoxResult}");
                    
                        return bBoxResult.Succeed ? 0 : 1;
                    });
                });
            
                command.Command("disable", disableCommand =>
                {
                    disableCommand.Description = "Disable the wireless interface";
                    disableCommand.OnExecute(() => { 
                        var bBoxResult = bboxClient.DisableWirelessAsync(WirelessType.Private).Result;
                        if (bBoxResult.Succeed)
                            logger.LogInformation($"Successfully enabled wireless");
                        else
                            logger.LogError($"Error while enabling wireless: {bBoxResult}");
                    
                        return bBoxResult.Succeed ? 0 : 1;
                    });
                });
            };
        }
        
        public string Name { get; } = "wireless";
        public Action<CommandLineApplication> Action { get; }
    }
}