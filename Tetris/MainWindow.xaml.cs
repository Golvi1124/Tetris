using System.Diagnostics;
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
using System.Windows.Threading;
using Tetris.Core;
using Tetris.Models;

namespace Tetris;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly ImageSource[] tileImages = new ImageSource[]
    {
        new BitmapImage(new Uri("Assets/TileEmpty.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/TileCyan.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/TileBlue.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/TileOrange.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/TileYellow.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/TileGreen.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/TilePurple.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/TileRed.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/TileGray.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/TilePink.png", UriKind.Relative))
    };

    private readonly ImageSource[] blockImages = new ImageSource[]
    {
    new BitmapImage(new Uri("Assets/Block-Empty.png", UriKind.Relative)),
    new BitmapImage(new Uri("Assets/Block-I.png", UriKind.Relative)),
    new BitmapImage(new Uri("Assets/Block-J.png", UriKind.Relative)),
    new BitmapImage(new Uri("Assets/Block-L.png", UriKind.Relative)),
    new BitmapImage(new Uri("Assets/Block-O.png", UriKind.Relative)),
    new BitmapImage(new Uri("Assets/Block-S.png", UriKind.Relative)),
    new BitmapImage(new Uri("Assets/Block-T.png", UriKind.Relative)),
    new BitmapImage(new Uri("Assets/Block-Z.png", UriKind.Relative)),
    new BitmapImage(new Uri("Assets/Block-ii.png", UriKind.Relative)),
    new BitmapImage(new Uri("Assets/Block-oo.png", UriKind.Relative)),
    };


    private readonly Image[,] imageControls;
    private readonly int maxDelay = 1000;
    private readonly int minDelay = 75;
    private readonly int delayDecrese = 25; // decrease delay by 25 milliseconds each time

    private bool isPaused = false;
    private Stopwatch gameTimer = new Stopwatch();
    private DispatcherTimer uiTimer;

    private CancellationTokenSource? gameLoopCts;


    private GameState gameState = new GameState();

    public MainWindow()
    {
        InitializeComponent();

        gameTimer.Start();

        uiTimer = new DispatcherTimer();
        uiTimer.Interval = TimeSpan.FromMilliseconds(50); // update UI every 50ms
        uiTimer.Tick += (s, e) =>
        {
            TimeText.Text = $"Time: {gameTimer.Elapsed:mm\\:ss}";
        };
        uiTimer.Start();

        imageControls = SetupGameCanvas(gameState.GameGrid);
    }

    private Image[,] SetupGameCanvas(GameGrid grid)
    {
        Image[,] imageControls = new Image[grid.Rows, grid.Columns];
        int cellSize = 25; // size of each cell in pixels

        for (int r = 0; r < grid.Rows; r++)
        {
            for (int c = 0; c < grid.Columns; c++)
            {
                Image imageControl = new Image
                {
                    Width = cellSize,
                    Height = cellSize
                };

                Canvas.SetTop(imageControl, (r - 2) * cellSize + 10); // push 2 invisible cells up, so they are not visible
                Canvas.SetLeft(imageControl, c * cellSize);
                GameCanvas.Children.Add(imageControl); // add the image control to the canvas
                imageControls[r, c] = imageControl; // store the image control in the array
            }
        }
        return imageControls;
    }

    private void DrawGrid(GameGrid grid)
    {
        for (int r = 0; r < grid.Rows; r++)
        {
            for (int c = 0; c < grid.Columns; c++)
            {
                int id = grid[r, c];
                imageControls[r, c].Opacity = 1; // reset opacity to full
                imageControls[r, c].Source = tileImages[id];
            }
        }
    }

    private void DrawBlock(Block block)  //draw the current block on the game canvas
    {
        foreach (Position p in block.TilePositions())
        {
            imageControls[p.Row, p.Column].Opacity = 1; // set opacity to full
            imageControls[p.Row, p.Column].Source = tileImages[block.Id];
        }
    }

    private void DrawNextBlock(BlockQueue blockQueue)
    {
        Block next = blockQueue.NextBlock;
        NextImage.Source = blockImages[next.Id];
    }

    private void DrawHeldBlock(Block heldBlock)
    {
        if (heldBlock == null)
        {
            HoldImage.Source = blockImages[0];
        }
        else
        {
            HoldImage.Source = blockImages[heldBlock.Id];
        }
    }

    private void DrawGhostBlock(Block block)
    {
        int dropDistance = gameState.BlockDropDistance();

        foreach (Position p in block.TilePositions())
        {
            imageControls[p.Row + dropDistance, p.Column].Opacity = 0.25; // make the ghost block semi-transparent
            imageControls[p.Row + dropDistance, p.Column].Source = tileImages[block.Id];
        }
    }

    private void Draw(GameState gameState)
    {
        DrawGrid(gameState.GameGrid);
        DrawGhostBlock(gameState.CurrentBlock); // draw the ghost block before the current block
        DrawBlock(gameState.CurrentBlock);
        DrawNextBlock(gameState.BlockQueue);
        DrawHeldBlock(gameState.HeldBlock); // draw the held block if any
        ScoreText.Text = $"Score: {gameState.Score}"; // update the score text
    }

    private async Task GameLoop(CancellationToken token)
    {
        Draw(gameState);// initial draw of the game state

        while (!gameState.GameOver)
        {
            while (isPaused) // pause loop without exiting game loop
            {
                await Task.Delay(10); // lightweight wait
                if (token.IsCancellationRequested) return; // check for cancellation
            }

            int delay = Math.Max(minDelay, maxDelay - (gameState.Score * delayDecrese));// calculate the delay based on the score
            await Task.Delay(delay);

            if (token.IsCancellationRequested) return;

            gameState.MoveBlockDown();
            Draw(gameState);
        }

        gameTimer.Stop();
        FinalScoreText.Text = $"Your score: {gameState.Score}";
        FinalTimeText.Text = $"Your time: {gameTimer.Elapsed:mm\\:ss}";
        GameOverMenu.Visibility = Visibility.Visible;
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (gameState.GameOver) // if the game is over, ignore key presses
            return;

        switch (e.Key)
        {
            case Key.Left:
                gameState.MoveBlockLeft();
                break;
            case Key.Right:
                gameState.MoveBlockRight();
                break;
            case Key.Down:
                gameState.MoveBlockDown();
                break;
            case Key.Up:
                gameState.RotateBlockCW();
                break;
            case Key.Z:
                gameState.RotateBlockCCW();
                break;
            case Key.C:
                gameState.HoldBlock();
                break;
            case Key.Space:
                gameState.DropBlock();
                break;
            case Key.P:
                TogglePause();
                break;
            default:
                return; // ignore other keys
        }
        if (!isPaused)
        {
            Draw(gameState); // only draw if not paused
        } // redraw the game state after the move
    }
    private async void GameCanvas_Loaded(object sender, RoutedEventArgs e)
    {
        gameLoopCts = new CancellationTokenSource(); // create a new cancellation token source for the game loop
        await GameLoop(gameLoopCts.Token);
    }

    private async void PlayAgain_Click(object sender, RoutedEventArgs e)
    {
        // Cancel the previous loop (if any)
        gameLoopCts?.Cancel();

        gameState = new GameState();// reset the game state
        isPaused = false;
        PausedMenu.Visibility = Visibility.Hidden;// hide the menus
        GameOverMenu.Visibility = Visibility.Hidden;

        gameTimer.Reset();
        gameTimer.Start();
        uiTimer.Start();

        // Start a fresh cancellation token
        gameLoopCts = new CancellationTokenSource();
        await GameLoop(gameLoopCts.Token);
    }

    private void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            gameTimer.Stop();
            uiTimer.Stop();
            PausedMenu.Visibility = Visibility.Visible;
        }
        else
        {
            gameTimer.Start();
            uiTimer.Start();
            PausedMenu.Visibility = Visibility.Hidden;
        }
    }

    private void ResumeGame_Click(object sender, RoutedEventArgs e)
    {
        TogglePause();
    }

    private void ExitGame_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown(); // exit the application
    }
}