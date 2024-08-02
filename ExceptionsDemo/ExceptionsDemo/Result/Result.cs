using System;

namespace ExceptionsDemo
{
    public abstract class Result<T, TError>
    {
        public abstract T OkValue { get; }
        public abstract TError ErrorValue { get; }

        /// <summary>
        /// Checks if a <see cref="Result{T, TError}"/> is of type <see cref="Ok{T, TError}"/>
        /// </summary>
        public abstract bool IsOk { get; }

        /// <summary>
        /// Checks if a <see cref="Result{T, TError}"/> is of type <see cref="Error{T, TError}"/>
        /// </summary>
        public abstract bool IsError { get; }

        /// <summary>
        /// Runs the one of the passed functions to process values inside the Result type
        /// </summary>
        /// <typeparam name="TReturn">Type returned by both passed functions</typeparam>
        /// <param name="ok">Function to run if the Result is of type <see cref="Ok{T, TError}"/></param>
        /// <param name="error">Function to run if the Result is of type <see cref="Error{T, TError}"/></param>
        /// <returns>The return value of the chosen function</returns>
        public abstract TReturn Match<TReturn>(Func<T, TReturn> ok, Func<TError, TReturn> error);
    }

    public sealed class Ok<T, TError> : Result<T, TError>
    {
        public override T OkValue { get => _value; }
        public override TError ErrorValue { get => throw new NotImplementedException(); }
        public override bool IsOk => true;
        public override bool IsError => false;

        private readonly T _value;

        public Ok(T value)
        {
            _value = value;
        }

        public override TReturn Match<TReturn>(Func<T, TReturn> ok, Func<TError, TReturn> _)
        {
            return ok(_value);
        }
    }

    public sealed class Error<T, TError> : Result<T, TError>
    {
        public override T OkValue { get => throw new NotImplementedException(); }
        public override TError ErrorValue { get => _value; }
        public override bool IsOk => false;
        public override bool IsError => true;

        private readonly TError _value;

        public Error(TError value)
        {
            _value = value;
        }

        public override TReturn Match<TReturn>(Func<T, TReturn> _, Func<TError, TReturn> error)
        {
            return error(_value);
        }
    }
}