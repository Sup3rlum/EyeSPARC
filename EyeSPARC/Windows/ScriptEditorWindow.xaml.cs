using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

using EyeSPARC.Scripting;

namespace EyeSPARC.Windows
{
    public partial class ScriptEditorWindow : Window
    {



        public EyeProject CurrentProject { get; private set; }
        TabControlViewModel _vm;

        public ScriptEditorWindow()
        {
            InitializeComponent();


        }

        public void SetProject(EyeProject _proj)
        {
            CurrentProject = _proj;

            _vm = new TabControlViewModel();


            foreach (var v in _proj.Files)
            {
                _vm.Tabs.Add(new FileTabItem()
                {
                    FileName = v.Name + v.Extension,
                    Content = v.Content
                });
            }

            DataContext = _vm;
            projectTreeView.ItemsSource = new ObservableCollection<EyeProject> { _proj };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
        }
    }

    // -- ViewModel


    public sealed class FileTabItem
    {
        public string FileName  { get; set; }
        public string Content   { get; set; }
    }
    public sealed class TabControlViewModel
    {
        public ObservableCollection<FileTabItem> Tabs { get; set; }
        public FileTabItem SelectedTab { get; set; }

        public TabControlViewModel()
        {
            Tabs = new ObservableCollection<FileTabItem>();
        }
    }
}
