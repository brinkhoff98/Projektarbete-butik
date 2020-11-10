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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OnlyFans
{
    public partial class MainWindow : Window
    {
        //Global variables
        private List<string> itemsInCart = new List<string>(); //List for storing all the items in the cart
        private string discountCode = "";

        public MainWindow()
        {
            InitializeComponent();
            Start();
        }

        private void Start()
        {
            // Window options
            Title = "OnlyFans";
            Width = 1000;
            Height = 800;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            // Scrolling
            ScrollViewer root = new ScrollViewer();
            root.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            Content = root;

            // Main grid
            Grid grid = new Grid();
            root.Content = grid;
            grid.Margin = new Thickness(5);
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition());

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            //Store header text
            TextBlock titleText = new TextBlock
            {
                Text = "OnlyFans",
                Margin = new Thickness(20),
                FontFamily = new FontFamily("Comic Sans MS"),
                FontSize = 25,
            };
            grid.Children.Add(titleText);
            Grid.SetColumn(titleText, 0);
            Grid.SetRow(titleText, 0);

            //Cart header Text
            TextBlock cartHeaderText = new TextBlock
            {
                Text = "Cart",
                Margin = new Thickness(20),
                FontFamily = new FontFamily("Comic Sans MS"),
                FontSize = 25,
            };
            grid.Children.Add(cartHeaderText);
            Grid.SetColumn(cartHeaderText, 1);
            Grid.SetRow(cartHeaderText, 0);
        }
    }
}
