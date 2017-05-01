using System;
using System.Collections.Generic;


namespace Public.Common.Lib.Code
{
    public class ProjectReferenceCollection
    {
        private Dictionary<string, ProjectReference> zProjectsByPath { get; set; }
        private Dictionary<string, ProjectReference> zProjectsByName { get; set; }
        private Dictionary<Guid, ProjectReference> zProjectsByGUID { get; set; }



        public ProjectReferenceCollection()
        {
            this.zProjectsByName = new Dictionary<string, ProjectReference>();
            this.zProjectsByPath = new Dictionary<string, ProjectReference>();
            this.zProjectsByGUID = new Dictionary<Guid, ProjectReference>();
        }

        public bool ContainsByName(string name)
        {
            return this.zProjectsByName.ContainsKey(name);
        }

        public bool ContainsByPath(string path)
        {
            return this.zProjectsByPath.ContainsKey(path);
        }

        public bool ContainsByGUID(Guid guid)
        {
            return this.zProjectsByGUID.ContainsKey(guid);
        }

        public void Add(ProjectReference projectReference)
        {
            this.zProjectsByName.Add(projectReference.Name, projectReference);
            this.zProjectsByPath.Add(projectReference.Path, projectReference);
            this.zProjectsByGUID.Add(projectReference.GUID, projectReference);
        }

        public ProjectReference GetByName(string name)
        {
            return this.zProjectsByName[name];
        }

        public ProjectReference GetByPath(string path)
        {
            return this.zProjectsByPath[path];
        }

        public ProjectReference GetByGuid(Guid guid)
        {
            return this.zProjectsByGUID[guid];
        }

        public void Remove(ProjectReference projectReference)
        {
            this.zProjectsByName.Remove(projectReference.Name);
            this.zProjectsByPath.Remove(projectReference.Path);
            this.zProjectsByGUID.Remove(projectReference.GUID);
        }

        public void RemoveByName(string name)
        {
            ProjectReference projectReference = this.GetByName(name);
            this.Remove(projectReference);
        }

        public void RemoveByPath(string path)
        {
            ProjectReference projectReference = this.GetByPath(path);
            this.Remove(projectReference);
        }

        public void RemoveByGuid(Guid guid)
        {
            ProjectReference projectReference = this.GetByGuid(guid);
            this.Remove(projectReference);
        }
    }
}
