﻿using ChiaPool.Api;
using CliFx.Attributes;
using CliFx.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace ChiaPool.Commands
{
    [Command("Miner List", Description = "Lists all miners of a specific user. Defaults to your miners")]
    public class MinerListCommand : ChiaCommand
    {
        private readonly ClientApiAccessor ClientAccessor;
        private readonly ServerApiAccessor ServerAccessor;

        public MinerListCommand(ClientApiAccessor clientAccessor, ServerApiAccessor serverAccessor)
        {
            ClientAccessor = clientAccessor;
            ServerAccessor = serverAccessor;
        }

        [CommandOption("name", 'n', Description = "The name of the user")]
        public string Name { get; set; }

        [CommandOption("id", 'i', Description = "The id of the user")]
        public long Id { get; set; }

        protected override async Task ExecuteAsync(IConsole console)
        {
            if (Name != default && Id != default)
            {
                await ErrorAsync("Please specify either name or id, not both");
                return;
            }

            var miners = Name != default
                ? await ServerAccessor.ListMinersByNameAsync(Name)
                : Id != default
                ? await ServerAccessor.ListMinersByIdAsync(Id)
                : await ClientAccessor.ListOwnedMinersAsync();

            if (miners.Count == 0)
            {
                await WarnLineAsync("No miners found");
                return;
            }

            int idLength = miners.Max(x => x.Id.ToString().Length) + 2;
            int nameLength = miners.Max(x => x.Name.Length);

            long totalPM = miners.Sum(x => x.PlotMinutes);
            await InfoLineAsync($"Total PM mined by this user: {totalPM}");
            await WriteLineAsync();
            await InfoLineAsync($"Id{Space(idLength)}Name{Space(nameLength)}PM");

            foreach (var miner in miners)
            {
                await WriteLineAsync($"{miner.Id}    {miner.Name}    {miner.PlotMinutes}");
            }
        }
    }
}
