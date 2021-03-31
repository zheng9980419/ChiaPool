﻿using ChiaMiningManager.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ChiaMiningManager.Api
{
    public class ClientApiAccessor : ApiAccessor
    {
        private const string ApiUrl = "http://localhost:8888/";

        public ClientApiAccessor(HttpClient client)
            : base(client)
        {
        }

        public Task<ClientStatus> GetStatusAsync()
            => Client.GetFromJsonAsync<ClientStatus>(ClientRoutes.Status(ApiUrl));

        public Task<PlotInfo[]> GetPlotsAsync()
            => Client.GetFromJsonAsync<PlotInfo[]>(ClientRoutes.ListPlots(ApiUrl));

        //public Task<bool> DeletePlotByIdAsync(Guid id)
        //    => PostAsync<bool>(ClientRoutes.DeletePlotById(ApiUrl), new Dictionary<string, string>()
        //    {
        //        ["id"] = id.ToString(),
        //    });

        //public Task<bool> DeletePlotByNameAsync(string name)
        //    => PostAsync<bool>(ClientRoutes.DeletePlotById(ApiUrl), new Dictionary<string, string>()
        //    {
        //        ["name"] = name,
        //    });

    }
}
