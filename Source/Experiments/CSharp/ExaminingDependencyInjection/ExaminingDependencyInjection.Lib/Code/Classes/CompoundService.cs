using System;
using System.Collections.Generic;


namespace ExaminingDependencyInjection.Lib
{
    public class CompoundService : IService
    {
        public IEnumerable<IService> Services { get; }


        public CompoundService(IEnumerable<IService> services)
        {
            this.Services = services;
        }
    }
}
