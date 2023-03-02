using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.DIDemo
{
    public interface ITest<TKey, TValue>
    {
        void Foo();
    }

    public abstract class TestBase<TKey, TValue> : ITest<TKey, TValue>
    {
        public abstract void Foo();
    }

    public interface ICache : ITest<string, object>
    {
        void Test();
    }

    public class Cache : TestBase<string, object>, ICache
    {
        public override void Foo()
        {
            Console.WriteLine("this is Cache class foo");
        }

        public void Test()
        {
            Console.WriteLine("this is Cache class Test");
        }
    }

    public class RedisCache : TestBase<int, string>, ICache
    {
        public override void Foo()
        {
            Console.WriteLine("this is RedisCache class foo");
        }

        public void Test()
        {
            Console.WriteLine("this is RedisCache class Test");
        }
    }
}
