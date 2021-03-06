﻿using Scrap.Core;

namespace Scrap.Models
{
    public abstract class BaseWrapper<T> : BaseWrapper where T : PersistentObject
    {
        protected BaseWrapper(T dataForContainer = default (T))
            : base(dataForContainer)
        {
        }

        public new T Container
        {
            get { return (T)base.Container; }
            protected set { base.Container = value; }
        }

    }
}
