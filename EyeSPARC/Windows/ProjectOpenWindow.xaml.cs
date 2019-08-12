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
using System.Collections.ObjectModel;
using System.IO;

using EyeSPARC.Scripting;

namespace EyeSPARC.Windows
{
    /// <summary>
    /// Interaction logic for ProjectOpen.xaml
    /// </summary>
    public partial class ProjectOpenWindow : Window
    {
        public string ChosenName { get; set; }
        public string ChosenPath { get; set; }
        public ProjectType ChosenType { get; set; }

        private ListViewViewModel _listViewViewModel;

        public ProjectOpenWindow()
        {
            InitializeComponent();

            _listViewViewModel = new ListViewViewModel();


            var _dirs = Directory.GetDirectories("./projects/");

            foreach (var f in _dirs)
            {
                var _files = Directory.GetFiles(f);

                foreach (var p in _files)
                {
                    if (Path.GetExtension(p) == ".eyeproj")
                    {
                        _listViewViewModel.Projects.Add(new ListViewItemModel() { Path = p, Name = Path.GetFileNameWithoutExtension(p).Replace("_", " ") });
                    }
                }
            }

            DataContext = _listViewViewModel;
        }

        private void _openButton_Click(object sender, RoutedEventArgs e)
        {
            if (ListView.SelectedItem != null)
            {
                ChosenName = ((ListViewItemModel) ListView.SelectedItem).Name;
                ChosenPath = ((ListViewItemModel) ListView.SelectedItem).Path;
                ChosenType = ((ListViewItemModel) ListView.SelectedItem).Type;


              /*  Console.WriteLine(ChosenName);
                Console.WriteLine(ChosenPath);
                Console.WriteLine(ChosenType);*/

                this.Close();
            }
            else
            {
                MessageBox.Show("Please select a project");
            }
        }
    }

    // --- ViewModel

    public sealed class ListViewViewModel
    {
        public ObservableCollection<ListViewItemModel> Projects { get; set; }

        public ListViewViewModel()
        {
            Projects = new ObservableCollection<ListViewItemModel>();
        }
            
    }
    public sealed class ListViewItemModel
    {
        public ProjectType Type { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
    }
}
