namespace GW2NET.Connectivity
{
    using Common;

    /// <summary>Represents a result given back by a <see cref="Connector"/> query.</summary>
    /// <typeparam name="TData">The query's data type.</typeparam>
    public sealed class Result<TData>
    {
        /// <summary>Initializes a new instance of the <see cref="Result{TData}"/> class.</summary>
        /// <param name="data">The data to pass on.</param>
        /// <param name="state">The optional state the data was in.</param>
        public Result(ISlice<TData> data, object state = null)
        {
            this.Data = data;
            this.State = state;
        }

        /// <summary>Gets the data the query returned.</summary>
        public ISlice<TData> Data { get; }

        /// <summary>Gets the optional state the data was in when returned.</summary>
        public object State { get; }
    }
}