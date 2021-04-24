using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Roaster.Test
{
    [TestClass]
    public class RoasterTest
    {
        private static ThreadClockImpl clock = new ThreadClockImpl(10);

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            // for faster test, just wait for 10ms each interval (10ms as 1s)
            DI.Resolver.Config<Clock.IClock>(clock);
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            clock.Dispose();
        }

        [TestMethod]
        public void TestGoodCookingByTimer()
        {
            Roaster roaster = new Roaster(2, 2);
            Assert.IsTrue(roaster.Receive(new Bread(), 0, 0));
            Assert.IsTrue(roaster.Receive(new Bread(), 0, 1));
            roaster.Settimer(0, 5);
            roaster.CloseLever(0);
            System.Threading.Thread.Sleep(100);
            // simulate timeout --> lever auto open

            ItemAbstract item = roaster.Release(0, 0);
            Assert.AreEqual(CookingStatus.Cooked, item.CookingStatus);

            item = roaster.Release(0, 1);
            Assert.AreEqual(CookingStatus.Cooked, item.CookingStatus);
        }

        [TestMethod]
        public void TestUnderCookingByForceEnded()
        {
            Roaster roaster = new Roaster(2, 2);
            Assert.IsTrue(roaster.Receive(new Bread(), 1, 0));
            Assert.IsTrue(roaster.Receive(new Bagel(), 1, 1));
            roaster.Settimer(1, 10); // 10 ticks
            roaster.CloseLever(1);
            System.Threading.Thread.Sleep(95);
            roaster.OpenLever(1); // simulate force close

            ItemAbstract item = roaster.Release(1, 0);
            Assert.AreEqual(CookingStatus.Over, item.CookingStatus);

            item = roaster.Release(1, 1);
            Assert.AreEqual(CookingStatus.Under, item.CookingStatus);
        }

        [TestMethod]
        public void TestUnderCookingByOffRoaster()
        {
            Roaster roaster = new Roaster(2, 2);
            Assert.IsTrue(roaster.Receive(new Bread(), 1, 0));
            Assert.IsTrue(roaster.Receive(new Bagel(), 1, 1));
            roaster.Settimer(1, 10);
            roaster.CloseLever(1);
            System.Threading.Thread.Sleep(30);
            roaster.ToggleStatus();
            System.Threading.Thread.Sleep(100);
            roaster.OpenLever(1); // simulate force close

            ItemAbstract item = roaster.Release(1, 0);
            Assert.AreEqual(CookingStatus.Under, item.CookingStatus);

            item = roaster.Release(1, 1);
            Assert.AreEqual(CookingStatus.Under, item.CookingStatus);
        }

        [TestMethod]
        public void TestCookingFromOffToOnRoaster()
        {
            Roaster roaster = new Roaster(2, 2);
            roaster.ToggleStatus();
            Assert.IsTrue(roaster.Receive(new Bread(), 1, 0));
            Assert.IsTrue(roaster.Receive(new Bagel(), 1, 1));
            roaster.Settimer(1, 10);
            roaster.CloseLever(1);
            System.Threading.Thread.Sleep(100);
            roaster.ToggleStatus();
            System.Threading.Thread.Sleep(80);
            roaster.OpenLever(1); // simulate force close

            ItemAbstract item = roaster.Release(1, 0);
            Assert.AreEqual(CookingStatus.Over, item.CookingStatus);

            item = roaster.Release(1, 1);
            Assert.AreEqual(CookingStatus.Under, item.CookingStatus);
        }
    }
}
