using System.Media;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tetris.Blocks;
using Block = Tetris.Blocks.Block;
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
        private readonly int maxDelay = 1000;
        private readonly int minDelay = 150;
        private readonly int delayDecrease = 25;

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
                    
                    // The +10 will let us see just a tiny bit of the hidden row, letting us see what block ended our game
                    Canvas.SetTop(imageControl, (row - 2) * cellSize + 10); // We'll have two rows of padding for block spawning
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
                    _imageControls[row, column].Opacity = 1; // undo ghost block
                }
            }
        }

        private void DrawBlock(Tetris.Blocks.Block block)
        {
            foreach (Position p in block.TilePositions())
            {
                _imageControls[p.Row, p.Column].Source = _tileImages[block.Id]; // Set the tile's background to the id's associated image
                _imageControls[p.Row, p.Column].Opacity = 1; // undo ghost block
            }
        }

        private void DrawNextBlock(BlockQueue blockQueue)
        {
            Tetris.Blocks.Block next = blockQueue.NextBlock;
            NextImage.Source = _blockImages[next.Id];
        }

        private void DrawHeldBlock(Block heldBlock)
        {
            if (heldBlock == null)
            {
                HoldImage.Source = _blockImages[0];
            }
            else
            {
                HoldImage.Source = _blockImages[heldBlock.Id];
            }
        }

        private void DrawGhostBlock(Block block) // This will let us see where we're dropping our block
        {
            int dropDistance = _gameState.BlockDropDistance();

            foreach(Position p in block.TilePositions())
            {
                _imageControls[p.Row + dropDistance, p.Column].Opacity = 0.25; // We reset the opacity within the draw grid and block functions
                _imageControls[p.Row + dropDistance, p.Column].Source = _tileImages[block.Id];
            }
        }

        private void Draw(GameState gameState) // Draw the game
        {
            DrawGrid(gameState.GameGrid);
            DrawGhostBlock(gameState.CurrentBlock);
            DrawBlock(gameState.CurrentBlock);
            DrawNextBlock(gameState.BlockQueue);
            DrawHeldBlock(gameState.HeldBlock);
            ScoreText.Text = $"Score: {gameState.Score}";
        }

        private async Task GameLoop()
        {
            Draw(_gameState); // Draw the game

            

            while (!_gameState.GameOver)
            {
                int delay = Math.Max(minDelay, maxDelay - (_gameState.Score * delayDecrease));
                await Task.Delay(delay); // This is the delay between the block going down automatically
                _gameState.MoveBlockDown();
                Draw(_gameState);
            }

            GameOverMenu.Visibility = Visibility.Visible;
            FinalScoreText.Text = $"Score: {_gameState.Score}";
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

                // Hard drop
                case Key.Up:
                case Key.W:
                    _gameState.HardDropBlock();
                    break;

                // Clockwise rotation
                case Key.E:
                    _gameState.RotateBlockClockwise();
                    break;

                // Counterclockwise rotation
                case Key.Z:
                case Key.Q:
                    _gameState.RotateBlockCounterClockwise();
                    break;

                // Hold block
                case Key.F:
                    _gameState.HoldBlock();
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