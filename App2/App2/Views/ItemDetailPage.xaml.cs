using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using App2.Models;
using App2.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using App2.Services;
using System.IO;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
//main snakes and ladders page for each saved game
namespace App2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemDetailPage : ContentPage
    {
        public Item Item { get; set; }
        private List<BoardObject> _snakes;
        private List<BoardObject> _ladders;
        private bool _turn = true;
        ItemDetailViewModel viewModel;
        public ItemDetailPage(ItemDetailViewModel viewModel)
        {

            BindingContext = this.viewModel = viewModel;
            InitializeComponent();
            AddSnakesLadders();
            Image player;
            string coords = Convert.ToString(viewModel.Title);
            player = PageContentGrid.FindByName<Image>("ImagePlayer1");
            // the -48 is because it gives ascii value
            player.SetValue(Grid.RowProperty, Convert.ToInt32(coords[0]) - 48);
            player.SetValue(Grid.ColumnProperty, Convert.ToInt32(coords[2]) - 48);
            BoardGameGrid.Children.Add(player);
            player = PageContentGrid.FindByName<Image>("ImagePlayer2");
            // the -48 is because it gives ascii value
            player.SetValue(Grid.RowProperty, Convert.ToInt32(coords[4]) - 48);
            player.SetValue(Grid.ColumnProperty, Convert.ToInt32(coords[6]) - 48);
            BoardGameGrid.Children.Add(player);
        }
       
    

    private void AddSnakesLadders()
    {
        if (_snakes == null) _snakes = new List<BoardObject>();
        if (_ladders == null) _ladders = new List<BoardObject>();

        BoardObject snake, ladder;
        #region snakesandLadders
        snake = new BoardObject()
        {
            TopX = 1,
            TopY = 0,
            BottomX = 2,
            BottomY = 2
        };
        _snakes.Add(snake);

        snake = new BoardObject()
        {
            TopX = 5,
            TopY = 0,
            BottomX = 5,
            BottomY = 2
        };
        _snakes.Add(snake);

        snake = new BoardObject()
        {
            TopX = 7,
            TopY = 0,
            BottomX = 7,
            BottomY = 2
        };
        _snakes.Add(snake);
        snake = new BoardObject()
        {
            TopX = 6,
            TopY = 1,
            BottomX = 3,
            BottomY = 7
        };
        _snakes.Add(snake);

        snake = new BoardObject()
        {
            TopX = 3,
            TopY = 3,
            BottomX = 0,
            BottomY = 4
        };
        _snakes.Add(snake);

        snake = new BoardObject()
        {
            TopX = 1,
            TopY = 3,
            BottomX = 1,
            BottomY = 8
        };
        _snakes.Add(snake);

        snake = new BoardObject()
        {
            TopX = 6,
            TopY = 4,
            BottomX = 6,
            BottomY = 6
        };
        _snakes.Add(snake);

        snake = new BoardObject()
        {
            TopX = 3,
            TopY = 8,
            BottomX = 6,
            BottomY = 9
        };
        _snakes.Add(snake);

        ladder = new BoardObject()
        {
            TopX = 9,
            TopY = 2,
            BottomX = 9,
            BottomY = 0
        };
        _ladders.Add(ladder);

        ladder = new BoardObject()
        {
            TopX = 7,
            TopY = 7,
            BottomX = 3,
            BottomY = 1
        };
        _ladders.Add(ladder);

        ladder = new BoardObject()
        {
            TopX = 2,
            TopY = 3,
            BottomX = 0,
            BottomY = 1
        };
        _ladders.Add(ladder);

        ladder = new BoardObject()
        {
            TopX = 9,
            TopY = 4,
            BottomX = 6,
            BottomY = 3
        };
        _ladders.Add(ladder);

        ladder = new BoardObject()
        {
            TopX = 0,
            TopY = 8,
            BottomX = 2,
            BottomY = 6
        };
        _ladders.Add(ladder);

        ladder = new BoardObject()
        {
            TopX = 0,
            TopY = 6,
            BottomX = 1,
            BottomY = 4
        };
        _ladders.Add(ladder);

        ladder = new BoardObject()
        {
            TopX = 8,
            TopY = 9,
            BottomX = 9,
            BottomY = 6
        };
        _ladders.Add(ladder);
        ladder = new BoardObject()
        {
            TopX = 3,
            TopY = 9,
            BottomX = 6,
            BottomY = 8
        };
        _ladders.Add(ladder);
        #endregion
    }

    #region Move By Translation
    private void TranslationButton_Clicked(object sender, EventArgs e)
    {
        Random rnd = new Random();
        Image player;
        int spaces = 0;
        if (_turn)
        {
            player = PageContentGrid.FindByName<Image>("ImagePlayer1");
            _turn = false;
            TranslationButton.Text = "Player 2's turn";
        }
        else
        {
            _turn = true;
            player = PageContentGrid.FindByName<Image>("ImagePlayer2");
            TranslationButton.Text = "Player 1's turn";
        }
        //spaces = Math.Abs(Convert.ToInt32(DiceEntry.Text));
        spaces = rnd.Next(1, 6);

     
        HandleDiceRollTranslation(spaces, player);
    }

    private async void HandleDiceRollTranslation(int diceRoll, Image piece)
    {
        // figure out how far to move.
        // based on the diceroll and the width of the board
        int currentRow, currentCol;
        double xStep = BoardGameGrid.Width / 10;
        currentCol = (int)piece.GetValue(Grid.ColumnProperty);
        currentRow = (int)piece.GetValue(Grid.RowProperty);

        // move based on direction
        if (currentRow % 2 == 0) // R to Left
        {
            // check that diceroll is less than the current column
            xStep *= -1;
            if (diceRoll <= currentCol)
            {
                await MoveHorizontalTranslate(xStep * diceRoll,
                                        currentCol - diceRoll,
                                        piece);
            }
            else
            {
                await MoveHorizontalTranslate(xStep * currentCol, 0, piece);
                await MoveVerticalTranslate(currentRow, piece);
                await MoveHorizontalTranslate(xStep * -1 * (diceRoll - (currentCol + 1)),
                                        diceRoll - (currentCol + 1),
                                        piece);
            }
        }
        else // move L to Right
        {
            // check that the diceroll is less than 9 - currentcol
            if (diceRoll <= 9 - currentCol)
            {
                await MoveHorizontalTranslate(xStep * diceRoll,
                                        currentCol + diceRoll,
                                        piece);
            }
            else // go around a corner
            {
                int diff = 9 - currentCol;
                await MoveHorizontalTranslate(xStep * diff, 9, piece);
                await MoveVerticalTranslate(currentRow, piece);
                await MoveHorizontalTranslate(xStep * -1 * (diceRoll - (diff + 1)),
                                        9 - (diceRoll - (diff + 1)),
                                        piece);
            }
        }
        CheckForSnakesTranslation(piece);
    }

    private async Task MoveHorizontalTranslate(double xTranslate,
                                        int nCol, Image piece)
    {
        await piece.TranslateTo(xTranslate, 0, 500);
        piece.SetValue(Grid.ColumnProperty, nCol);
        piece.TranslationX = 0;
    }

    private async Task MoveVerticalTranslate(int currentRow, Image piece)
    {
        if (currentRow == 0) return;
        double yStep = (BoardGameGrid.Height / 10) * -1;
        await piece.TranslateTo(0, yStep, 250);
        piece.SetValue(Grid.RowProperty, currentRow - 1);
        piece.TranslationY = 0;
    }

    #endregion

    private async void CheckForSnakesTranslation(Image p)
    {
        // check if the player is on a row and column that contains
        // the bottom of a ladder
        int pX, pY, xTrans, yTrans;
        double xStep, yStep;
        pX = (int)p.GetValue(Grid.ColumnProperty);
        pY = (int)p.GetValue(Grid.RowProperty);

        foreach (var s in _snakes)
        {
            if ((s.TopX == pX) && (s.TopY == pY))
            {
                // set the translation
                xTrans = s.BottomX - s.TopX;
                yTrans = s.BottomY - s.TopY;
                xStep = (BoardGameGrid.Width / 10) * xTrans;
                yStep = (BoardGameGrid.Height / 10) * yTrans;
                await p.TranslateTo(xStep, yStep, 500);
                p.SetValue(Grid.RowProperty, s.BottomY);
                p.SetValue(Grid.ColumnProperty, s.BottomX);
                p.TranslationX = 0;
                p.TranslationY = 0;
                break;
            }
        }
        foreach (var s in _ladders)
        {
            if ((s.TopX == pX) && (s.TopY == pY))
            {
                // set the translation
                xTrans = s.BottomX - s.TopX;
                yTrans = s.BottomY - s.TopY;
                xStep = (BoardGameGrid.Width / 10) * xTrans;
                yStep = (BoardGameGrid.Height / 10) * yTrans;
                await p.TranslateTo(xStep, yStep, 500);
                p.SetValue(Grid.RowProperty, s.BottomY);
                p.SetValue(Grid.ColumnProperty, s.BottomX);
                p.TranslationX = 0;
                p.TranslationY = 0;
                break;
            }
        }

    }

    private void CheckForSnakes(Image p)
    {
        // check if the player is on a row and column that contains
        // the bottom of a ladder
        int pX, pY;
        pX = (int)p.GetValue(Grid.ColumnProperty);
        pY = (int)p.GetValue(Grid.RowProperty);

        foreach (var s in _snakes)
        {
            if ((s.TopX == pX) && (s.TopY == pY))
            {
                // move the player
                p.SetValue(Grid.RowProperty, s.BottomY);
                p.SetValue(Grid.ColumnProperty, s.BottomX);
                break;
            }
        }
    }

    private void CheckForLadders(Image p)
    {
        // check if the player is on a row and column that contains
        // the bottom of a ladder
        int pX, pY;
        pX = (int)p.GetValue(Grid.ColumnProperty);
        pY = (int)p.GetValue(Grid.RowProperty);

        foreach (var l in _ladders)
        {
            if ((l.BottomX == pX) && (l.BottomY == pY))
            {

                // move the player
                p.SetValue(Grid.RowProperty, l.TopY);
                p.SetValue(Grid.ColumnProperty, l.TopX);
                break;
            }
        }
    }

    async void SaveGame_Clicked(object sender, EventArgs e)
    {
        BindingContext = this;
        Image player1 = PageContentGrid.FindByName<Image>("ImagePlayer1");
        int pX1 = (int)player1.GetValue(Grid.ColumnProperty);
        int pY1 = (int)player1.GetValue(Grid.RowProperty);
        Image player2 = PageContentGrid.FindByName<Image>("ImagePlayer2");
        int pX2 = (int)player2.GetValue(Grid.ColumnProperty);
        int pY2 = (int)player2.GetValue(Grid.RowProperty);
        string savecoords = pY1 + "," + pX1 + "," + pY2 + "," + pX2;
                    Item = new Item
                    {
                        Title = savecoords,
                        Description = "Player 1 coordinates are " + pX1+","+pY1+" Player 2 coordinates are "+pX2+","+pY2
                    };
                
                        MessagingCenter.Send(this, "AddItem", Item);
                        await Navigation.PopAsync();
            
        }

        async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            BindingContext = this;
            Image player1 = PageContentGrid.FindByName<Image>("ImagePlayer1");
            int pX1 = (int)player1.GetValue(Grid.ColumnProperty);
            int pY1 = (int)player1.GetValue(Grid.RowProperty);
            Image player2 = PageContentGrid.FindByName<Image>("ImagePlayer2");
            int pX2 = (int)player2.GetValue(Grid.ColumnProperty);
            int pY2 = (int)player2.GetValue(Grid.RowProperty);
            string savecoords = pY1 + "," + pX1 + "," + pY2 + "," + pX2;
            Item = new Item
            {
                Title = savecoords,
                Description = "This is an item description."
            };
            MessagingCenter.Send(this, "DeleteItem", Item);
            await Navigation.PopAsync();
        }
        /*      public static void SaveListData()
     {
         // need the path to the file
         string path = Environment.GetFolderPath(
             Environment.SpecialFolder.LocalApplicationData);
         string filename = Path.Combine(path, "TextFile1.txt");
         // use a stream writer to write the list
         using (var writer = new StreamWriter(filename, false))
         {
             // stringify equivalent
             string jsonText = JsonConvert.SerializeObject("wellllllll");
             writer.WriteLine(jsonText);
             ObservableCollection<Item> items;
             string jsonText = JsonConvert.SerializeObject(items);
             writer.WriteLine(jsonText);
         }
     }*/
    }
}