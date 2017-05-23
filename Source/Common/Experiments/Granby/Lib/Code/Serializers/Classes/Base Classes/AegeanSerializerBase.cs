using System;
using System.Collections.Generic;
using System.IO;

using LogLog = Public.Common.Granby.Lib.Log;


namespace Public.Common.Granby.Lib
{
    public abstract class AegeanSerializerBase<TOutput>
    {
        public IAegeanFactory<TOutput> Factory { get; protected set; }
        public ILog Log { get; set; }


        protected AegeanSerializerBase(IAegeanFactory<TOutput> factory, ILog log)
        {
            this.Factory = factory;
            this.Log = log;
        }

        protected AegeanSerializerBase(IAegeanFactory<TOutput> factory)
            : this(factory, LogLog.StringListLog())
        {
        }

        public List<TOutput> Deserialize(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);

            List<TOutput> output = new List<TOutput>();
            foreach (string line in lines)
            {
                try
                {
                    TOutput schedule = this.Factory[line];
                    output.Add(schedule);
                }
                catch (Exception ex)
                {
                    Log.WriteLine(ex.ToString());
                }
            }

            return output;
        }
    }
}
