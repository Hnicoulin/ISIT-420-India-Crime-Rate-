using EF500Orders.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EF500Orders.Controllers
{
    // https://stackoverflow.com/questions/9499794/single-controller-with-multiple-get-methods-in-asp-net-web-api
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrdersDBController : ControllerBase
    {
        // GET: api/<OrdersDBController>
        [HttpGet]
        [ActionName("Get")]
        public List<OrderData> Get()
        {
            System.Diagnostics.Debug.WriteLine($"getting data...");

            var context = new OrdersDBContext();
            // https://stackoverflow.com/questions/7325278/group-by-in-linq
            // https://stackoverflow.com/questions/5344805/linq-orderby-descending-query
            var totalQuery = (from eachOrder in context.OrdersTables
                              where eachOrder.PricePaid > 13
                              group eachOrder by eachOrder.StoreID into eachOrderStore
                              select new OrderData
                              {
                                  StoreID = eachOrderStore.Key,
                                  Count = eachOrderStore.Count()
                              }).OrderByDescending(item => item.Count).ToList();
            // checking query result in Output window
            foreach (var item in totalQuery)
            {
                // https://stackoverflow.com/questions/9466838/writing-to-output-window-of-visual-studio
                System.Diagnostics.Debug.WriteLine($"StoreID = {item.StoreID} | Count = {item.Count}");
            }
            return totalQuery;
        }

        // GET api/<OrdersDBController>/5
        [HttpGet("{id}")]
        [ActionName("GetTotalSale")]
        public int GetTotalSale(int id)
        {
            System.Diagnostics.Debug.WriteLine($"storeid => ${id}"); // prints to output window

            var context = new OrdersDBContext();
            var totalQuery = (from eachOrder in context.OrdersTables
                              where eachOrder.StoreID == id
                              select eachOrder.PricePaid).Sum();

            return totalQuery;
        }

        // POST api/<OrdersDBController>
        [HttpPost]
        [ActionName("PostOrder")]
        public void PostOrder([FromBody] NewOrder oneEvent)
        {
            System.Diagnostics.Debug.WriteLine($"this is oneEvend: cdid-{oneEvent.CdID} | storeid-{oneEvent.StoreID} | salespersonenid-{oneEvent.SalesPersonID} | pricepaid-{oneEvent.PricePaid}");
            var context = new Models.OrdersDBContext();

            CdTable pointedToCd = new CdTable();
            var findCd = (from oneCd in context.CdTables // nned to be plural "CdTable => CdTables
                            where oneCd.CdId == oneEvent.CdID
                            select oneCd).First();

            SalesPersonTable pointedToSalesPerson = new SalesPersonTable();
            var findSalesPerson = (from oneSalesPerson in context.SalesPersonTables
                                   where oneSalesPerson.SalesPersonId == oneEvent.SalesPersonID
                              select oneSalesPerson).FirstOrDefault();

            StoreTable pointedToStore = new StoreTable();
            var findStore = (from oneStore in context.StoreTables
                              where oneStore.StoreId == oneEvent.StoreID
                              select oneStore).First();

            OrdersTable newEvent = new OrdersTable
            {
                StoreID = oneEvent.StoreID,
                SalesPersonID = oneEvent.SalesPersonID,
                PricePaid = oneEvent.PricePaid,
                Date = DateTime.Now.ToString(),
                CdID = oneEvent.CdID,
                Cd = findCd,
                SalesPerson = findSalesPerson,
                Store = findStore
            };

            try
            {
                context.OrdersTables.Add(newEvent);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                // writes to output window
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        [HttpGet("{storeid}")]
        [ActionName("GetStoreEmployees")]
        public List<SalesPersonTable> GetStoreEmployees(string storeid)
        {
            var context = new OrdersDBContext();
            var queryResult = (from eachStore in context.SalesPersonTables
                               where eachStore.StoreId == int.Parse(storeid)
                               select eachStore).ToList();
            foreach (var item in queryResult)
            {
                System.Diagnostics.Debug.WriteLine($"{item.SalesPersonId}");
            }
            return queryResult;
        }



        // PUT api/<OrdersDBController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<OrdersDBController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }

    public class OrderData // json or somebody forces prop names to start lower case!
    {
        public int StoreID { get; set; }
        public int Count { get; set; }
    }

    public class NewOrder
    {
        public int StoreID { get; set; }
        public int SalesPersonID { get; set; }
        public int CdID { get; set; }
        public int PricePaid { get; set; }
    }
}
