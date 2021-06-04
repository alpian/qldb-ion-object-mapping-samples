using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Amazon.QLDB.Driver;
using Amazon.QLDB.Driver.Generic;
using Amazon.QLDB.Driver.Serialization;
using System.Threading.Tasks;

namespace Amazon.Ion.ObjectMappingSamplesTest
{
    class Car {
        public string Make { get; init; }
        public string Model { get; init; }
        public int Year { get; init; }

        public override string ToString()
        {
            return "<Car>{ Make: " + Make + ", Model: " + Model + ", Year: " + Year + " }";
        }
    }

    [TestClass]
    public class SamplesTest
    {
        [TestMethod]
        public void QueryReturnsUniformRows()
        {
            var driver = QldbDriver.Builder()
                .WithLedger("cars")
                .WithSerializer(new ObjectSerializer())
                .Build();

            driver.Execute(txn => 
            {
                txn.Execute(txn.Query<Document>("insert into Car ?", new Car { Make = "Opel", Model = "Kadett", Year = 1981 }));

                IResult<Car> cars = txn.Execute(txn.Query<Car>("select * from Car"));

                foreach (var c in cars)
                {
                    Console.WriteLine(c);
                }
            });
        }

        [TestMethod]
        public async Task AsyncQueryReturnsUniformRows()
        {
            var driver = AsyncQldbDriver.Builder()
                .WithLedger("cars")
                .WithSerializer(new ObjectSerializer())
                .Build();

            await driver.Execute(async txn => 
            {
                await txn.Execute(txn.Query<Document>("insert into Car ?", new Car { Make = "Opel", Model = "Kadett", Year = 1981 }));

                IAsyncResult<Car> cars = await txn.Execute(txn.Query<Car>("select * from Car"));

                await foreach (var c in cars)
                {
                    Console.WriteLine(c);
                }
            });
        }
    }
}
