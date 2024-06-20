using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tetris.Blocks;
namespace Tetris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ImageSource[] _tileImages = new ImageSource[]
        { // This is ordered by the IDs of the blocks
            new BitmapImage(new Uri("Assets/TileEmpty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileCyan.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileBlue.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileOrange.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileYellow.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileGreen.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TilePurple.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileRed.png", UriKind.Relative)),
        };

        private readonly ImageSource[] _blockImages = new ImageSource[]
        { // This is also ordered by the IDs of the blocks
            new BitmapImage(new Uri("Assets/Block-Empty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-I.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-J.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-L.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-O.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-S.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-T.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-Z.png", UriKind.Relative)),
        };

        private readonly Image[,] _imageControls;

        private GameState _gameState = new GameState();


        public MainWindow()
        {
            InitializeComponent();
            _imageControls = SetupGameCanvas(_gameState.GameGrid);
        }

        private Image[,] SetupGameCanvas(GameGrid grid) // This will create a 2d array for every cell in the game
        {
            Image[,] imageControls = new Image[grid.Rows, grid.Columns];
            int cellSize = 25; // 25 pixels for each visible cell, as determined by our 250x500 game panel

            for (int row = 0; row < grid.Rows; row++)
            {
                for (int column = 0; column < grid.Columns; column++)
                {
                    Image imageControl = new Image
                    {
                        Width = cellSize,
                        Height = cellSize,
                    };

                    Canvas.SetTop(imageControl, (row - 2) * cellSize); // We'll have two rows of padding for block spawning
                    Canvas.SetLeft(imageControl, column  * cellSize); 

                    GameCanvas.Children.Add(imageControl);
                    imageControls[row, column] = imageControl;
                }
            }
            return imageControls;
        }

        private void DrawGrid(GameGrid grid)
        {
            for (int row = 0; row < grid.Rows; row++)
            {
                for (int column = 0; column < grid.Columns; column++)
                {
                    int id = grid[row, column]; // Get the id of each tile
                    _imageControls[row, column].Source = _tileImages[id]; // Set the tile's background to the id's associated image
                }
            }
        }

        private void DrawBlock(Tetris.Blocks.Block block)
        {
            foreach(Position p in block.TilePositions())
            {
                _imageControls[p.Row, p.Column].Source = _tileImages[block.Id]; // Set the tile's background to the id's associated image
            }
        }

        private void Draw(GameState gameState) // Draw the game
        {
            DrawGrid(gameState.GameGrid);
            DrawBlock(gameState.CurrentBlock);
        }

        private async Task GameLoop()
        {
            Draw(_gameState); // Draw the game

            while (!_gameState.GameOver)
            {
                await Task.Delay(500); // This is the delay between the block going down automatically
                _gameState.MoveBlockDown();
                Draw(_gameState);
            }

            GameOverMenu.Visibility = Visibility.Visible;
        }


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(_gameState.GameOver)
            {
                return; // We don't want anything to happen if the game is over
            }

            switch (e.Key)
            {
                // Left movement
                case Key.Left:
                case Key.A:
                    _gameState.MoveBlockLeft();
                    break;
                
                // Right movement
                case Key.Right:
                case Key.D:
                    _gameState.MoveBlockRight();
                    break;

                // Down movement
                case Key.Down:
                case Key.S:
                    _gameState.MoveBlockDown();
                    break;

                // Clockwise rotation
                case Key.Up:
                case Key.W:
                case Key.E:
                    _gameState.RotateBlockClockwise();
                    break;

                // Counterclockwise rotation
                case Key.Z:
                case Key.Q:
                    _gameState.RotateBlockCounterClockwise();
                    break;

                default:
                    return;
           
            }

            Draw(_gameState);
        }

        private async void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            await GameLoop();
        }

        private async void PlayAgain_Click(object sender, RoutedEventArgs e)
        {
            _gameState  = new GameState();
            GameOverMenu.Visibility = Visibility.Hidden;
            await GameLoop();
        }
    }
}