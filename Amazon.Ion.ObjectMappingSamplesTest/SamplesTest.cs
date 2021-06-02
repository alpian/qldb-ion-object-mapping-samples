using Microsoft.VisualStudio.TestTools.UnitTesting;
using Amazon.QLDB.Driver;
using System;
using Amazon.QLDB.Driver.Generic;
using Amazon.Ion.ObjectMapper;
using System.IO;

namespace Amazon.Ion.ObjectMappingSamplesTest
{
    class Car {
        string Make { get; init; }
        string Model { get; init; }
        int Year { get; init; }

        public override string ToString()
        {
            return "<Car>{ Make: " + Make + ", Model: " + Model + ", Year: " + Year + " }";
        }
    }

    class MappingSerialization : Serialization
    {
        private readonly IonSerializer serializer;

        public MappingSerialization()
        {
            serializer = new IonSerializer();
        }

        public T Deserialize<T>(Stream s)
        {
            return serializer.Deserialize<T>(s);
        }

        public Stream Serialize(object o)
        {
            return serializer.Serialize(o);
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
                .WithSerialization(new MappingSerialization())
                .Build();

            driver.Execute(txn => 
            {
                IResult<Car> cars = txn.Execute(txn.Query<Car>("select * from Car"));

                foreach (var c in cars)
                {
                    Console.WriteLine(c);
                }
            });
        }
    }
}
