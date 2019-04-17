using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        private bool _turn;

        int col1 = 6;
        int col2 = 6;
        int col3 = 6;
        int col4 = 6;
        int col5 = 6;
        int col6 = 6;
        int col7 = 6;
        public AboutPage()
        {
            _turn = true;
            InitializeComponent();
        }

        private void ChangeColour_Clicked(object sender, EventArgs e)
        {
            //turn swaps from false to true to swap turns.
            if (_turn)
            {
                //Image player = PageContentGrid.FindByName<Image>("ImagePlayerRed");
                Image player = new Image();
                player.Source = ImageSource.FromFile("yellowpiece.png");
                HandleMovement(player);
                player = PageContentGrid.FindByName<Image>("ImagePlayerYellow");
                player.Source = ImageSource.FromFile("redpiece.png");

                _turn = false;
            }
            else
            {
                // Image player = PageContentGrid.FindByName<Image>("ImagePlayerRed");
                Image player = new Image();
                //Image player.Source = ImageSource.FromFile("redpiece.png");
                player.Source = ImageSource.FromFile("redpiece.png");
                HandleMovement(player);
                player = PageContentGrid.FindByName<Image>("ImagePlayerYellow");
                player.Source = ImageSource.FromFile("yellowpiece.png");
                _turn = true;
            }


        }
        //keeps track of each column
        private void HandleMovement(Image p)
        {
            int columnNo = 0;
            columnNo = Convert.ToInt32(ColumnEntry.Text);

            if (columnNo == 1)
            {
                col1--;
                p.SetValue(Grid.RowProperty, col1);
            }
            else if (columnNo == 2)
            {
                col2--;
                p.SetValue(Grid.RowProperty, col2);
            }
            else if (columnNo == 3)
            {
                col3--;
                p.SetValue(Grid.RowProperty, col3);
            }
            else if (columnNo == 4)
            {
                col4--;
                p.SetValue(Grid.RowProperty, col4);
            }
            else if (columnNo == 5)
            {
                col5--;
                p.SetValue(Grid.RowProperty, col5);
            }
            else if (columnNo == 6)
            {
                col6--;
                p.SetValue(Grid.RowProperty, col6);
            }
            else if (columnNo == 7)
            {
                col7--;
                p.SetValue(Grid.RowProperty, col7);
            }
            
            p.SetValue(Grid.ColumnProperty, columnNo);
            BoardGameGrid.Children.Add(p);
        }


    }
}
