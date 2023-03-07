using Kyle.Infrastructure.Events.Bus;

namespace Kyle.Infrastructure.Events.Test
{
    [TestClass]
    public class UnitTest1 : TestBase
    {
        [TestMethod]
        public void TestMethod1()
        {
            EventBus.Trigger(new OrderEventData(Guid.NewGuid()));
            EventBus.Trigger(new UserEventData());
        }
    }
}