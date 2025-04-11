using Autodesk.DataExchange.BaseModels;
using Autodesk.DataExchange.Core;
using Autodesk.DataExchange.Core.Models;
using Autodesk.DataExchange.Interface;
using Autodesk.DataExchange.Schemas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataExchangeSDKRevitConnector
{
    public class DXSDKConnectorBaseModel : BaseReadOnlyExchangeModel
    {
        public DXSDKConnectorBaseModel(IClient client) : base(client)
        {
            GetLatestExchangeDetails += GetLatestExchangeDataAsync;
        }
        List<DataExchange> localStorage = new List<DataExchange>();
        public override async Task<List<DataExchange>> GetExchangesAsync(ExchangeSearchFilter exchangeSearchFilter)
        {
            localStorage = await GetValidExchangesAsync(exchangeSearchFilter, localStorage);
            return localStorage;
        }
        public override async Task<DataExchange> GetExchangeAsync(DataExchangeIdentifier exchangeIdentifier)
        {
            var response = await base.GetExchangeAsync(exchangeIdentifier);

            if (localStorage.Find(item => item.ExchangeID == response.ExchangeID) == null)
            {
                response.IsExchangeFromRead = true;
                localStorage.Add(response);
            }

            return response;
        }
        public override void SelectElements(List<string> exchangeIds)
        {
            //throw new NotImplementedException();
        }

        public List<DataExchange> GetLocalExchanges()
        {
            return localStorage?.ToList();
        }

        public void SetLocalExchanges(List<DataExchange> dataExchanges)
        {
            localStorage.AddRange(dataExchanges);
        }

        public override Task<IEnumerable<string>> UnloadExchangesAsync(List<ExchangeItem> exchanges)
        {
            return Task.Run(() =>
            {
                localStorage = localStorage.Where(x => !exchanges.Any(y => y.ExchangeID == x.ExchangeID)).ToList();
                _sDKOptions.Storage.Add("LocalExchanges", localStorage);
                _sDKOptions.Storage.Save();
                return exchanges.Select(n => n.ExchangeID);
            }
            );
        }

        public async Task GetLatestExchangeDataAsync(object sender, ExchangeItem exchangeItem)
        {
            var exchangeIdentifier = new DataExchangeIdentifier
            {
                CollectionId = exchangeItem.ContainerID,
                ExchangeId = exchangeItem.ExchangeID,
                HubId = exchangeItem.HubId,
                Region = exchangeItem.HubRegion
                //FileUrn = exchangeItem.FileURN,
                //FileVersionUrn = exchangeItem.FileVersion
            };
            var wholeGeometryPath = Client.DownloadCompleteExchangeAsSTEP(exchangeItem.ExchangeID);
        }
    }
}
