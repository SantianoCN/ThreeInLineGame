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

namespace ThreeInLine
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isActiveChanging = false;
        private Stack<Vector2> selectedPoisitons = new Stack<Vector2>();
        private Stack<Button> selectedButtons = new Stack<Button>();
        public MainWindow()
        {
            InitializeComponent();

            var i = 0;
            while(i < 12)
            {
                var j = 0;
                field.RowDefinitions.Add(new RowDefinition());
                field.ColumnDefinitions.Add(new ColumnDefinition());

                i++;
            }

            SpawnButton();
        }

        public async Task SpawnButton()
        {
            var arr = new[]
            {
                Brushes.Red,
                Brushes.Green,
                Brushes.Blue,
                Brushes.Orange
            };

            int i = 0;
            while (i < 12)
            {
                bool isBusy = false;
                foreach (var ui in field.Children)
                {
                    if (Grid.GetRow((UIElement)ui) == 0 && Grid.GetColumn((UIElement)ui) == i)
                    {
                        isBusy = true; break;
                    }
                }
                
                if (!isBusy)
                {
                    var btn = new Button
                    {
                        Background = arr[new Random().Next(0, 4)],
                    };

                    btn.Click += BtnClickHandler;
                    
                    field.Children.Add(btn);

                    Grid.SetRow(btn, 0);
                    Grid.SetColumn(btn, i);

                    await Collision(btn);
                } else
                {
                    i++;

                    continue;
                }
            }

            await CheckHorizontalLine();
        }

        public async Task Collision(Button btn)
        {
            //await Task.Delay(10);
            isActiveChanging = true;

            var y = Grid.GetRow(btn);
            var x = Grid.GetColumn(btn);

            bool isBusy = false;
            foreach (var ui in field.Children)
            {
                if (Grid.GetColumn((UIElement)ui) == x && Grid.GetRow((UIElement)ui) == y + 1)
                {
                    isBusy = true;
                    break;
                }
            }
            if (!isBusy && y + 1 <= 12 - 1) // size - 1
            {
                Grid.SetRow(btn, y + 1);
                await Collision(btn);
            }
            isActiveChanging = false;
        }


        private async void BtnClickHandler(object sender, RoutedEventArgs e)
        {
            if (isActiveChanging)
            {
                return;
            }
            

            var position = new Vector2();
            position.X = Grid.GetColumn((UIElement)sender);
            position.Y = Grid.GetRow((UIElement)sender);


            selectedPoisitons.Push(position);
            selectedButtons.Push((Button)sender);

            if (selectedButtons.Count == 2)
            {
                await Swap();
            }
        }

        private async Task Swap()
        {
            if (selectedPoisitons.Last().Y == selectedPoisitons.First().Y)
            {
                if (selectedPoisitons.Last().X == selectedPoisitons.First().X + 1
                    || selectedPoisitons.Last().X == selectedPoisitons.First().X - 1)
                {
                    Grid.SetRow(selectedButtons.First(), (int)selectedPoisitons.Last().Y);
                    Grid.SetColumn(selectedButtons.First(), (int)selectedPoisitons.Last().X);

                    Grid.SetRow(selectedButtons.Last(), (int)selectedPoisitons.First().Y);
                    Grid.SetColumn(selectedButtons.Last(), (int)selectedPoisitons.First().X);
                }
            } else if (selectedPoisitons.Last().X == selectedPoisitons.First().X)
            {
                if (selectedPoisitons.Last().Y == selectedPoisitons.First().Y + 1
                    || selectedPoisitons.Last().Y == selectedPoisitons.First().Y - 1)
                {
                    Grid.SetRow(selectedButtons.First(), (int)selectedPoisitons.Last().Y);
                    Grid.SetColumn(selectedButtons.First(), (int)selectedPoisitons.Last().X);

                    Grid.SetRow(selectedButtons.Last(), (int)selectedPoisitons.First().Y);
                    Grid.SetColumn(selectedButtons.Last(), (int)selectedPoisitons.First().X);
                }
            }

            await CheckHorizontalLine();

            selectedButtons.Clear();
            selectedPoisitons.Clear();
        }

        private async Task CheckHorizontalLine()
        {
            var colors = new List<List<SolidColorBrush>>();
            
            for (int i = 0; i < 12; i++) // size
            {
                for (int j = 0; j < 12; j++) // size
                {
                    foreach (var ui in field.Children)
                    {
                        if (Grid.GetRow((UIElement)ui) == i && Grid.GetRow((UIElement)ui) == j)
                        {
                            colors.Add(new List<SolidColorBrush>());
                            colors.Last().Add((SolidColorBrush)(ui as Button).Background);
                        }
                    }
                }
            }
            
            foreach (var row in colors)
            {
                for (int i = 0; i < 12 - 2; i++) // size
                {
                    if (row[i] == row[i + 1] && row[i] == row[i + 2])
                    {
                        
                    }
                }
            }
        }

        private async Task Delete(Stack<Button> buttons)
        {
            foreach (var btn in buttons)
            {
                field.Children.Remove((UIElement)btn);
            }

            foreach (var btn in field.Children)
            {
                await Collision((Button)btn);
            }
            await SpawnButton();
        }
    }
}
