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
using System.IO;

using EyeSPARC.Scripting;

namespace EyeSPARC
{
    /// <summary>
    /// Interaction logic for ProjectCreationWindow.xaml
    /// </summary>
    public partial class ProjectCreationWindow : Window
    {
        public ProjectType SelectedProjectType { get; set; }

        public string SelectedName { get; set; }

        public ProjectCreationWindow()
        {
            InitializeComponent();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string _name = projectNameTextBox.Text;

            if (EyeProject.Exists(_name))
            {
                MessageBox.Show($"A project wiht the selected name already exists");

                return;
            }
            else if (_name == String.Empty || projectTypeBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please enter a name and type for the project");

                return;
            }

            SelectedName = _name;

            this.Close();
        }

        private void ProjectTypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems[0].ToString() == "C#")
            {
                SelectedProjectType = ProjectType.CSharp;
            }
            else if (e.AddedItems[0].ToString() == "IronPython")
            {
                SelectedProjectType = ProjectType.IronPython;
            }
        }
    }
}
