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
    /// <summary>
    /// Interaction logic for ScriptEditorWindow.xaml
    /// </summary>
    public partial class ScriptEditorWindow : Window
    {

        EyeProject _currentProject;

        public ScriptEditorWindow(EyeProject _proj)
        {
            InitializeComponent();

            _currentProject = _proj;


            foreach (var v in _currentProject.Files)
            {
                _mainTabcontrol.Items.Add(new TabItem() { Header = v.Name, Content = new TextBox() { Text = v.Content } });
            }

            projectTreeView.ItemsSource = new ObservableCollection<EyeProject> { _proj };
        }
    }
}
