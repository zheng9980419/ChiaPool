﻿using ChiaMiningManager.Api;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using System;
using System.Threading.Tasks;

namespace ChiaMiningManager.Commands
{
    [Command("Plots DeleteName", Description = "Deletes a plot by specifying the plot name")]
    public class PlotDeleteByNameCommand : ICommand
    {
        private readonly ClientApiAccessor ApiClient;

        public PlotDeleteByNameCommand(ClientApiAccessor apiClient)
        {
            ApiClient = apiClient;
        }

        [CommandParameter(0, Description = "Name of the plot", Name = "Name")]
        public string Name { get; set; }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            bool success = await ApiClient.DeletePlotByNameAsync(Name);

            if (success)
            {
                await console.Output.WriteLineAsync("Ok");
                return;
            }

            await console.Output.WriteLineAsync("Plot not found!");
        }
    }
}