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
        private TextBox couponTextBox;


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

            //Cart items
            WrapPanel test2 = CartList();
            grid.Children.Add(test2);
            Grid.SetColumn(test2, 1);
            Grid.SetRow(test2, 1);
        }

        // Panel for showing items in cart and more
        private WrapPanel CartList()
        {
            WrapPanel cartWrapPanel = new WrapPanel
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 200,
                Margin = new Thickness(20)
            };

            // Yeet
            foreach (var item in itemsInCart)
            {
                TextBlock cartItemTextBlock = new TextBlock
                {
                    Text = item.Name + " " + item.Price + "kr",
                    FontFamily = new FontFamily("Comic Sans MS")
                };
                var removeIteamfromCartButton = new Button
                {
                    Content = "Remove item",
                    FontFamily = new FontFamily("Comic Sans MS"),
                    Width = 70,
                };
                removeIteamfromCartButton.Click += ReamoveItemFromCartOnClick;
                cartWrapPanel.Children.Add(removeIteamfromCartButton);
            }

            // Enter a coupon
            TextBlock couponTextBlock = new TextBlock
            {
                Text = "Enter coupon:",
                FontFamily = new FontFamily("Comic Sans MS")
            };
            cartWrapPanel.Children.Add(couponTextBlock);

            couponTextBox = new TextBox
            {
                VerticalContentAlignment = VerticalAlignment.Center,
                Margin = new Thickness(5),
                MaxLength = 10,
                Width = 80
            };
            cartWrapPanel.Children.Add(couponTextBox);

            // Button for saving items in cart
            var saveCartButton = new Button
            {
                Content = "Save Cart",
                FontFamily = new FontFamily("Comic Sans MS"),
                Width = 70,
            };
            saveCartButton.Click += SaveCartToCSVOnClick;
            cartWrapPanel.Children.Add(saveCartButton);

            // Button for removing all items in cart
            var clearCartButton = new Button
            {
                Content = "Clear Cart",
                FontFamily = new FontFamily("Comic Sans MS"),
                Width = 70,
            };
            clearCartButton.Click += ClearCartOnClick;
            cartWrapPanel.Children.Add(clearCartButton);

            // Button for saving items in cart
            var checkoutCartButton = new Button
            {
                Content = "Checkout",
                FontFamily = new FontFamily("Comic Sans MS"),
                Width = 140,
            };
            checkoutCartButton.Click += CheckoutCartOnClick;
            cartWrapPanel.Children.Add(checkoutCartButton);

            return cartWrapPanel;
        }

        private void ReamoveItemFromCartOnClick(object sender, RoutedEventArgs e)
        {
            itemsInCart = null;
        }

        private void ClearCartOnClick(object sender, RoutedEventArgs e)
        {
            itemsInCart = null;
        }

        private void SaveCartToCSVOnClick(object sender, RoutedEventArgs e)
        {
            itemsInCart = null;
        }
        private void CheckoutCartOnClick(object sender, RoutedEventArgs e)
        {
            itemsInCart = null;
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
