using System;


namespace Public.Common.Lib.Organizational
{
    /// <summary>
    /// Lists all repositories.
    /// </summary>
    public static class Repositories
    {
        public static readonly ExplorationRepository Exploration = new ExplorationRepository();
        public static readonly MinexRepository Minex = new MinexRepository();
        public static readonly PublicRepository Public = new PublicRepository();
    }
}
