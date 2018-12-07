using System;

namespace Utils
{
    public struct Optional<T>
    {
        private bool HasValue { get; set; }
        private readonly T _value;

        public T Value
        {
            get
            {
                if (HasValue)
                    return _value;
                throw new InvalidOperationException();
            }
        }

        private Optional(T value) : this()
        {
            this._value = value;
            HasValue = value != null;
        }

        public void IfPresent(Action<T> consumer)
        {
            if (HasValue)
                consumer.Invoke(_value);
        }

        public void IfAbsent(Action consumer)
        {
            if (!HasValue)
                consumer.Invoke();
        }

        public Optional<T> Filter(Predicate<T> predicate)
        {
            if (!HasValue)
                return this;
            return predicate(_value) ? this : Empty<T>();
        }

        public Optional<TM> Map<TM>(Func<T, TM> mapper)
        {
            return !HasValue ? Empty<TM>() : mapper(_value).Optional();
        }

        public T OrElse(T other)
        {
            return HasValue ? _value : other;
        }

        public static Optional<TV> Of<TV>(TV value)
        {
            return new Optional<TV>(value);
        }

        public static Optional<TV> Empty<TV>()
        {
            return new Optional<TV>();
        }
    }
    
    public static class OptionalUtil
    {
        public static Optional<T> Optional<T>(this T value)
        {
            return Utils.Optional<T>.Of(value);
        }
    }
}