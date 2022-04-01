using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos.Table;
using Supplier_AzureTBLRModel.Entities;
using Supplier_AzureTBLRModel.ViewModels;

namespace GetSupplierFunc_AzureTBL
{
    public static class Function1
    {
        [FunctionName("GetSupplierList")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                CloudStorageAccount storageAcc = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=tablestorage1;AccountKey=mvdDaDEswE16qPIWefkIjcpiNjpvC8GbXEglDBjrMKItK0QsFXFxr0SwNSjIdzdKeDrShIZ6abHw+AStfRWs5A==;EndpointSuffix=core.windows.net");

                //// create table client
                CloudTableClient tblclient = storageAcc.CreateCloudTableClient(new TableClientConfiguration());

                // get customer table
                CloudTable cloudTable = tblclient.GetTableReference("Supplier");

                TableQuery<Supplier> suppliersListQuery = new TableQuery<Supplier>();
                var suppliers = cloudTable.ExecuteQuery(suppliersListQuery);
                // string customerListJson = JsonConvert.SerializeObject(customerList);


                return new OkObjectResult(new ResponseModel() { Data = suppliers, Success = true });
            }
            catch (Exception e)
            {
                return new OkObjectResult(new ResponseModel() { Data = e.Message, Success = true });
            }
          
        }
    }
}
