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
        private ListBox itemsInCartListBox = new ListBox {};
        private TextBlock cartPriceTextBlock;
        private TextBox couponTextBox;
        private decimal totalPriceWithoutCoupon;
        private decimal totalPrice;

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

            //Store products
            WrapPanel productPanel = ProductList();
            grid.Children.Add(productPanel);
            Grid.SetColumn(productPanel, 0);
            Grid.SetRow(productPanel, 1);

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
            WrapPanel cartPanel = CartList();
            grid.Children.Add(cartPanel);
            Grid.SetColumn(cartPanel, 1);
            Grid.SetRow(cartPanel, 1);
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
            itemsInCartListBox = new ListBox
            {
                VerticalContentAlignment = VerticalAlignment.Center,
                Margin = new Thickness(5),
                Width = 200 
            };
            cartWrapPanel.Children.Add(itemsInCartListBox);

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

            // Button for removing selected item in cart
            var removeSelectedItemFromCartButton = new Button
            {
                Content = "Remove selected item from Cart",
                FontFamily = new FontFamily("Comic Sans MS"),
                Width = 200              
            };
            removeSelectedItemFromCartButton.Click += RemoveSelectedItemFromCartOnClick;
            cartWrapPanel.Children.Add(removeSelectedItemFromCartButton);

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

            // Button for checkout items in cart
            var checkoutCartButton = new Button
            {
                Content = "Checkout",
                FontFamily = new FontFamily("Comic Sans MS"),
                Width = 140,
            };
            checkoutCartButton.Click += CheckoutCartOnClick;
            cartWrapPanel.Children.Add(checkoutCartButton);

            cartPriceTextBlock = new TextBlock
            {
                TextWrapping = TextWrapping.Wrap,
                FontFamily = new FontFamily("Comic Sans MS")
            };
            cartWrapPanel.Children.Add(cartPriceTextBlock);

            return cartWrapPanel;
        }

        private void RemoveSelectedItemFromCartOnClick(object sender, RoutedEventArgs e)
        {
            int itemIndex = itemsInCartListBox.SelectedIndex;
            itemsInCart.RemoveAt(itemIndex);
            itemsInCartListBox.Items.RemoveAt(itemIndex);
            UpdateTotalPrices();
        }

        private void ClearCartOnClick(object sender, RoutedEventArgs e)
        {
            itemsInCart.Clear();
            itemsInCartListBox.Items.Clear();
            UpdateTotalPrices();
        }

        private void SaveCartToCSVOnClick(object sender, RoutedEventArgs e)
        {
            itemsInCart.Clear();
        }
        //Check if the coupon code is valid and if so run the the Receipt method
        private void CheckoutCartOnClick(object sender, RoutedEventArgs e)
        {
            List<Tuple<string, int>> couponCodes = new List<Tuple<string, int>>();
            foreach (var item in File.ReadAllLines(@"CouponCodes.csv").Select(a => a.Split(",")))
            {
                Tuple<string, int> couponCode = new Tuple<string, int>(item[0], Int32.Parse(item[1]));
                couponCodes.Add(couponCode);
            }
            string userCouponCode = couponTextBox.Text;
            if (userCouponCode != "")
            {
                var findCouponCodeAndDiscount = couponCodes.FirstOrDefault(x => x.Item1 == userCouponCode);
            
                if (findCouponCodeAndDiscount != null)
                {
                    int discount = findCouponCodeAndDiscount.Item2;
                    MessageBox.Show(Receipt(discount));
                    itemsInCartListBox.Items.Clear();
                    itemsInCart.Clear();
                    cartPriceTextBlock.Text = "";
                    couponTextBox.Text = "";
                }
                else
                {
                    MessageBox.Show("The coupon code is invalid");
                    couponTextBox.Text = "";
                }
            }
            else
            {
                MessageBox.Show(Receipt());
                itemsInCartListBox.Items.Clear();
                itemsInCart.Clear();
                cartPriceTextBlock.Text = "";
            }
        }

        //Shows the receipt with what you have bought and what the discount was
        private string Receipt(int discount = 0)
        {
            decimal totalPrice = 0;
            string discountText = "";
            string receiptText = "Receipt \n \n \n";

            foreach (var item in itemsInCart)
            {
                receiptText += "\n " + item.Name + "     " + item.Price + " kr";

                totalPrice += item.Price;
            }

            if (discount != 0)
            {
                totalPrice = totalPrice * ((100 - discount) / (decimal)100);
                discountText = "\n Discount: " + discount + " %";
            }
            receiptText += "\n \n Total amout: " + totalPrice + " kr";
            receiptText += discountText;
            return receiptText;
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
                    buyButton.Click += AddFanToCart;
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
        private void AddFanToCart(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Fan fan = new Fan
            {
                Name = button.Tag.ToString(),
                Price = decimal.Parse(button.DataContext.ToString()),
            };
            itemsInCartListBox.Items.Add($"{fan.Name}: {fan.Price}kr");
            itemsInCart.Add(fan);
            UpdateTotalPrices();
        }

        private void UpdateTotalPrices()
        {
            if (itemsInCart.Count > 0)
            {
                totalPriceWithoutCoupon = itemsInCart.Sum(item => item.Price);
                totalPrice = totalPriceWithoutCoupon;
                cartPriceTextBlock.Text = totalPriceWithoutCoupon + "Kr without coupon " + totalPrice + "kr with your coupon";
            }
            else
            {
                cartPriceTextBlock.Text = "";
            }
        }
    }
}
