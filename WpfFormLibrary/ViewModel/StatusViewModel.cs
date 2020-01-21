using System.Collections.Generic;
using ADR_Library.ViewModel;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace WpfFormLibrary.ViewModel
{
    public class StatusViewModel : ViewModelBase
    {
        private ImageSource icon;
        private bool isRunning ;

        private ObservableCollection<KeyValuePair<string, string>> statusFlags = new ObservableCollection<KeyValuePair<string, string>>();

        public StatusViewModel()
        {
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

        public bool IsRunning
        {
            get { return isRunning; }
            set
            {
                if (isRunning != value)
                {
                    isRunning = value;
                    OnPropertyChanged(nameof(IsRunning));
                }
            }
        }

        public ObservableCollection<KeyValuePair<string, string>> StatusFlags
        {
            get { return statusFlags; }
            set
            {
                if (statusFlags != value)
                {
                    statusFlags = value;
                    OnPropertyChanged(nameof(StatusFlags));
                }
            }
        }

        public void SetStatusFlags(List<KeyValuePair<string, string>> flags)
        {
            StatusFlags = new ObservableCollection<KeyValuePair<string, string>>(flags);
        }
    }
}
