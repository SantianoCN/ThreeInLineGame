using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

using ThreeInLine.Lib;

namespace ThreeInLine
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            InitializeGameField();

            StartGame();
        }

        public void InitializeGameField()
        {
            var i = 0;
            while (i < 12)
            {
                field.RowDefinitions.Add(new RowDefinition());
                field.ColumnDefinitions.Add(new ColumnDefinition());

                i++;
            }
        }

        public void StartGame()
        {
            var game = new GameFacade(field, scoreField);

            game.StartGame();
        }
    }
}
