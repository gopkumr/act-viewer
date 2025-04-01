namespace ACRViewer.BlazorServer.ViewModel
{
    public class TreeViewItemViewModel(TreeViewType type, string name, string parentName, bool hasChildren=true)
    {
        public TreeViewType Type { get; set; } = type;
        public string Name { get; set; } = name;

        public string ParentName { get; set; } = parentName;

        public bool HasChildren { get; set; } = hasChildren;
        public HashSet<TreeViewItemViewModel> Children { get; set; } = [];
    }

    public enum TreeViewType
    {
        Repository,
        Tag,
        ACR,
    }
}
