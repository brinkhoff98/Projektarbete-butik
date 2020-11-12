using System;
using System.Collections.Generic;
using System.IO;
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
    public class Fan
    {
        public string Name;
        public string Description;
        public decimal Price;
        public string ImageName;
    }
    public partial class MainWindow : Window
    {
        //Global variables
        private List<Fan> itemsInCart = new List<Fan>(); //List for storing all the items in the cart
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

            //store products
            WrapPanel test = ProductList();
            grid.Children.Add(test);
            Grid.SetColumn(test, 0);
            Grid.SetRow(test, 1);

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

        //Displays all the products from Products.csv
        private WrapPanel ProductList()
        {
            Fan fan = new Fan();

            WrapPanel wrappanel = new WrapPanel 
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 200,
                Margin = new Thickness(20)
            };

            //loops through the products.csv file and displays all the items in it
            try
            {
                foreach (var item in File.ReadAllLines(@"products.csv").Select(a => a.Split(",")))
                {
                    fan.Name = item[0];
                    fan.Description = item[1];
                    fan.Price = decimal.Parse(item[2]);
                    fan.ImageName = item[3];

                    TextBlock productName = new TextBlock
                    {
                        Text = fan.Name,
                        FontFamily = new FontFamily("Comic Sans MS")
                    };
                    wrappanel.Children.Add(productName);

                    ImageSource imgSource = new BitmapImage(new Uri(@"Images\" + fan.ImageName, UriKind.Relative));
                    Image image = new Image
                    {
                        Source = imgSource,
                        Width = 100,
                        Height = 100,
                    };
                    wrappanel.Children.Add(image);

                    TextBlock productDescription = new TextBlock
                    {
                        Text = fan.Description,
                        FontFamily = new FontFamily("Comic Sans MS"),
                        Margin = new Thickness(5),
                    };
                    wrappanel.Children.Add(productDescription);

                    var buyButton = new Button
                    {
                        Content = "Buy",
                        Tag = fan.Name,
                        DataContext = fan.Price,
                        FontFamily = new FontFamily("Comic Sans MS"),
                        Width = 40,

                    };
                    wrappanel.Children.Add(buyButton);
                    buyButton.Click += buyFan;
                }
            }
            //If there are no products or it cant find the csv file it says "Sorry there are no products"
            catch (Exception)
            {

                TextBlock noProducts = new TextBlock
                {
                    Text = "Sorry there are no products",
                    FontFamily = new FontFamily("Comic Sans MS"),
                    FontSize = 15
                };
                wrappanel.Children.Add(noProducts);
            }
            

            return wrappanel;
        }

        //Adds the item to the shoppingcart
        private void buyFan(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Fan fan = new Fan
            {
                Name = button.Tag.ToString(),
                Price = decimal.Parse(button.DataContext.ToString()),
            };
            itemsInCart.Add(fan);
        }
    }
}
