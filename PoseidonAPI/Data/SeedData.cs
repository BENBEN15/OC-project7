using Microsoft.Extensions.DependencyInjection;
using PoseidonAPI.Model;
using System;

namespace PoseidonAPI.Data
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new PoseidonDBContext(
                serviceProvider.GetRequiredService<Microsoft.EntityFrameworkCore.DbContextOptions<PoseidonDBContext>>()))
            {
                /*if (context.Cars.Any() || context.Makers.Any() || context.Models.Any() || context.Trims.Any())
                {
                    return;
                }*/

                if (context.Bids.Any())
                {
                    return;
                }

                /*if (context.Makers.Count() > 1)
                {
                    return;
                }*/

                /*if (context.Models.Count() > 1)
                {
                    return;
                }*/

                /*if (context.Trims.Count() > 1)
                {
                    return;
                }*/


                context.Bids.Add(
                    new Bid
                    {
                        Account = "account1",
                        Type = "type1",
                        BidQuantity = 1.0,
                        AskQuantity = 1.0,
                        BidValue = 1.0,
                        Ask = 1.0,
                        Benchmark = "benchmark1",
                        BidDate = new DateTime(2022, 01, 01),
                        Commentary = "commentary1",
                        Security = "securoty1",
                        Status = "status1",
                        Trader = "trader1",
                        Book = "book1",
                        CreationName = "creationName1",
                        CreationDate = new DateTime(2022, 01, 01),
                        RevisionName = "revisionName1",
                        RevisionDate = new DateTime(2022, 01, 01),
                        DealName = "dealName1",
                        DealType = "dealType1",
                        SourceListId = "SourceListId1",
                        Side = "side1",
                    }
                );

                /* context.Models.Add(
                      new Model
                      {
                          Name = "model2",
                          MakerId = 2,
                      }
                  );*/

                /*context.Trims.Add(
                    new Trim
                    {
                        Name = "trim2",
                        ModelId = 2,
                    }
                );*/

                /*context.Cars.Add(
                    new Car
                    {
                        MakerId = 2,
                        ModelId = 2,
                        TrimId = 2,
                        Vin = "vin2",
                        Year = 2004,
                        PurchaseDate = new DateTime(2021, 01, 17),
                        PurchasePrice = 5000.00,
                        Repairs = "repairs2",
                        RepairCost = 500.00,
                        LotDate = new DateTime(2021, 01, 17),
                        SellingPrice = 6000.0,
                        SaleDate = new DateTime(2021, 01, 17),
                        IsAvailable = true,
                        IsSold = true,
                        Description = "description2"
                    }
                );*/
                /*context.SaveChanges();*/
            }
        }
    }
}
