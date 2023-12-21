using System;
using System.Collections.Generic;

namespace RicKit.Pool
{
    public interface IPool<T>
    {
        T Allocate();

        bool Recycle(T obj);
    }

    public interface ICountObserveAble
    {
        int CurCount { get; }
    }

    public interface IObjectFactory<out T>
    {
        T Create();
    }

    public abstract class Pool<T> : IPool<T>, ICountObserveAble
    {
        protected readonly Stack<T> mCacheStack = new Stack<T>();

        protected IObjectFactory<T> mFactory;

        /// <summary>
        ///     default is 5
        /// </summary>
        protected int mMaxCount = 12;

        #region ICountObserverable

        /// <summary>
        ///     Gets the current count.
        /// </summary>
        /// <value>The current count.</value>
        public int CurCount => mCacheStack.Count;

        #endregion

        public virtual T Allocate()
        {
            return mCacheStack.Count == 0
                ? mFactory.Create()
                : mCacheStack.Pop();
        }

        public abstract bool Recycle(T obj);
    }

    public class CustomObjectFactory<T> : IObjectFactory<T>
    {
        private readonly Func<T> mFactoryMethod;

        public CustomObjectFactory(Func<T> factoryMethod)
        {
            mFactoryMethod = factoryMethod;
        } // ReSharper disable Unity.PerformanceAnalysis
        public T Create()
        {
            return mFactoryMethod();
        }
    }
}