using System;
using System.Collections.Generic;


namespace Public.Common.Lib.Code.Serialization
{
    public class ItemGroups
    {
        #region Static

        private static void FillItemGroups(ItemGroups groups, Dictionary<string, ProjectItem> projectItemsByRelativePath)
        {
            Dictionary<string, Action<ProjectItem>> distributors = new Dictionary<string, Action<ProjectItem>>();
            distributors.Add(typeof(ReferenceProjectItem).FullName, (x) => groups.References.Add((ReferenceProjectItem)x));
            distributors.Add(typeof(CompileProjectItem).FullName, (x) => groups.Compiles.Add((CompileProjectItem)x));
            distributors.Add(typeof(ProjectReferenceProjectItem).FullName, (x) => groups.ProjectReferences.Add((ProjectReferenceProjectItem)x));
            distributors.Add(typeof(ContentProjectItem).FullName, (x) => groups.Contents.Add((ContentProjectItem)x));
            distributors.Add(typeof(FolderProjectItem).FullName, (x) => groups.Folders.Add((FolderProjectItem)x));
            distributors.Add(typeof(NoneProjectItem).FullName, (x) => groups.Nones.Add((NoneProjectItem)x));

            foreach(ProjectItem item in projectItemsByRelativePath.Values)
            {
                distributors[item.GetType().FullName](item);
            }
        }

        #endregion


        public List<ReferenceProjectItem> References { get; set; }
        public List<CompileProjectItem> Compiles { get; set; }
        public List<ProjectReferenceProjectItem> ProjectReferences { get; set; }
        public List<ContentProjectItem> Contents { get; set; }
        public List<FolderProjectItem> Folders { get; set; }
        public List<NoneProjectItem> Nones { get; set; }


        public ItemGroups()
        {
            this.References = new List<ReferenceProjectItem>();
            this.Compiles = new List<CompileProjectItem>();
            this.ProjectReferences = new List<ProjectReferenceProjectItem>();
            this.Contents = new List<ContentProjectItem>();
            this.Folders = new List<FolderProjectItem>();
            this.Nones = new List<NoneProjectItem>();
        }

        public ItemGroups(Dictionary<string, ProjectItem> projectItemsByRelativePath)
            : this()
        {
            ItemGroups.FillItemGroups(this, projectItemsByRelativePath);
        }
    }
}
