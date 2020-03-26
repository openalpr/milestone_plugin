using ADR_Library.ViewModel;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace WpfFormLibrary.ViewModel
{
    public class AboutViewModel : ViewModelBase
    {
        private ImageSource icon;
        private ObservableCollection<ComponentVersionInfo> componentVersions;

        public AboutViewModel()
        {
            componentVersions = new ObservableCollection<ComponentVersionInfo>();
        }

        public ImageSource Icon
        {
            get { return icon; }
            set
            {
                if (icon != value)
                {
                    icon = value;
                    OnPropertyChanged(nameof(Icon));
                }
            }
        }

        public void AddVersionInfo(string name, string version)
        {
            foreach (ComponentVersionInfo item in ComponentVersions)
            {
                if (item.Name == name)
                {
                    item.Version = version;
                    OnPropertyChanged(nameof(ComponentVersions));
                    return;
                }
            }

            ComponentVersionInfo info = new ComponentVersionInfo
            {
                Name = name,
                Version = version
            };

            ComponentVersions.Add(info);
        }

        public ObservableCollection<ComponentVersionInfo> ComponentVersions
        {
            get { return componentVersions; }
            set
            {
                if (componentVersions != value)
                {
                    componentVersions = value;
                    OnPropertyChanged(nameof(ComponentVersions));
                }
            }
        }
    }
}
