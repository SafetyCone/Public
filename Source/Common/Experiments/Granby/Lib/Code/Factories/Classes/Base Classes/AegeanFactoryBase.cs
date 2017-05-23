using System;
using System.Collections.Generic;


namespace Public.Common.Granby.Lib
{
    /// <summary>
    /// A very simple factory.
    /// </summary>
    /// <remarks>
    /// A key-constructor dictionary is the dispatch mechanism. Key type is string.
    /// Abstract base class, inheritors must add key-constructor pairs to the internal dictionary.
    /// Since the specification is a string, specification tokenization is done via a simple separator character. The specification token separator character can be customized.
    /// Specification -> Key is non-virtual, and simply uses the first token of the specification.
    /// No virtual methods for error message customization.
    /// No logging of errors. Errors are thrown.
    /// </remarks>
    public abstract class AegeanFactoryBase<TOutput> : IAegeanFactory<TOutput>
    {
        protected const char DefaultSpecificationTokenSeparator = ' ';


        #region IAegeanFactory<TOutput> Members

        public TOutput this[string scheduleSpecification]
        {
            get
            {
                string[] scheduleTokens = scheduleSpecification.Split(this.zSpecifcationTokenSeparator);

                string scheduleKeyToken = scheduleTokens[0];

                Func<string[], TOutput> constructor;
                if (this.zConstructors.ContainsKey(scheduleKeyToken))
                {
                    constructor = this.zConstructors[scheduleKeyToken];
                }
                else
                {
                    string message = String.Format(@"Unrecognized key: {0}.", scheduleKeyToken);
                    throw new InvalidOperationException(message);
                }

                TOutput output;
                try
                {
                    output = this.zConstructors[scheduleKeyToken](scheduleTokens);
                }
                catch (Exception ex)
                {
                    string message = String.Format(@"Failed to construct object for specification: {0}. Key: {1}.", scheduleSpecification, scheduleKeyToken);
                    throw new InvalidOperationException(message, ex);
                }

                return output;
            }
        }

        #endregion


        protected Dictionary<string, Func<string[], TOutput>> zConstructors;
        protected char zSpecifcationTokenSeparator;


        protected AegeanFactoryBase(char specificationTokensSeparator)
        {
            this.zConstructors = new Dictionary<string, Func<string[], TOutput>>();

            this.zSpecifcationTokenSeparator = specificationTokensSeparator;
        }

        protected AegeanFactoryBase()
            : this(AegeanFactoryBase<TOutput>.DefaultSpecificationTokenSeparator)
        {
        }
    }
}
