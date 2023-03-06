using Kyle.Infrastructure.Events.Bus;

namespace Kyle.Infrastructure.Events.Test
{
    [TestClass]
    public class UnitTest1 : TestBase
    {
        [TestMethod]
        public void TestMethod1()
        {
            EventBus.Default.Trigger<OrderEventData>(new OrderEventData(Guid.NewGuid()));
        }
    }
}