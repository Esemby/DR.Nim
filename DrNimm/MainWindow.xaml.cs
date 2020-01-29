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
using System.Windows.Threading;

namespace DrNimm
{
    class Match : Grid
    {
        Ellipse phosphor = new Ellipse();
        Rectangle wood = new Rectangle();
        public string name;

        public Match()
        {
            phosphor.Height = 20;
            phosphor.Width = 20;
            phosphor.Fill = Brushes.IndianRed;
            phosphor.HorizontalAlignment = HorizontalAlignment.Center;
            phosphor.VerticalAlignment = VerticalAlignment.Top;

            wood.Height = 100;
            wood.Width = 10;
            wood.Fill = Brushes.Beige;
            wood.HorizontalAlignment = HorizontalAlignment.Center;
            wood.VerticalAlignment = VerticalAlignment.Bottom;

            Height = 100;
            Width = 20;
            Opacity = 1;
            Children.Add(wood);
            Children.Add(phosphor);
        }

        public void remove(string toRemove)
        {
            if(name == toRemove)
            {
                Children.Clear();
            }
        }
    }
    class Rulewindow : Window
    {
        StackPanel mainPanel = new StackPanel();

        public CheckBox iStart = new CheckBox();
        public ComboBox matchBox = new ComboBox();
        public Button gameStart = new Button();
        Label rules = new Label();

        public Rulewindow()
        {
            Height = 200;
            Width = 200;
            AddChild(mainPanel);

            matchBox.FontSize = 15;
            matchBox.Width = Width;
            matchBox.SelectedIndex = 0;
            for(int x = 24;x<=240;x++)
            {
                matchBox.Items.Add(x); 
            }

            iStart.Content = "Player Starts";

            gameStart.Content = "Start Game!";

            rules.Content = "Rules:\nWho ever takes the \nlast match wins";

            mainPanel.Height = Height;
            mainPanel.Width = Width;

            mainPanel.Children.Add(matchBox);
            mainPanel.Children.Add(iStart);
            mainPanel.Children.Add(gameStart);
            mainPanel.Children.Add(rules);
        }        
    }

    public partial class MainWindow : Window
    {
        DispatcherTimer AI = new DispatcherTimer();
        Rulewindow preGameWindow = new Rulewindow();
        Match[] match = new Match[240];

        bool playerTurn;
        int index = 0;
        long thinktime;

        public MainWindow()
        {
            InitializeComponent();
            
            AI.Tick += AI_Tick;
            AI.Interval = TimeSpan.FromTicks(1);

            Visibility = Visibility.Hidden;

            preGameWindow.gameStart.Click += GameStart_Click;
            preGameWindow.Closing += PreGameWindow_Closing;
            preGameWindow.Show();
        }

        private void PreGameWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Visibility = Visibility.Visible;
        }

        private void AI_Tick(object sender, EventArgs e)
        {
            if(index == 0 && playerTurn)
            {
                MessageBox.Show("You lose!");
                Close();
            }
            else if (index == 0 && !playerTurn)
            {
                MessageBox.Show("You win!");
                Close();
            }
            if (!playerTurn) { AIProgress.Value = DateTime.Now.Millisecond / 10; }
            if (DateTime.Now.Ticks >= thinktime && !playerTurn)
            {
                if(index % 4 == 0)
                {
                    int RNG = new Random().Next(1, 3);
                    for (int x = 0; x < RNG;x++)
                    {
                        index--;
                        match[index].remove("match" + index.ToString());
                    }
                }
                else
                {
                    int RAM = index % 4;
                    for (int x = 0; x < RAM; x++)
                    {
                        index--;
                        match[index].remove("match" + index.ToString());
                    }
                }
                playerTurn = true;
                AIProgress.Value = 100;
            }
        }

        private void GameStart_Click(object sender, RoutedEventArgs e)
        {
            for (int x = 0; x < (int)preGameWindow.matchBox.SelectedItem; x++)
            {
                match[x] = new Match();
                match[x].name = "match" + x.ToString();
                MatchContainer.Children.Add(match[x]);
            }
            index = (int)preGameWindow.matchBox.SelectedItem;
            playerTurn = (bool)preGameWindow.iStart.IsChecked;
            preGameWindow.Close();
            AI.Start();
            if (!playerTurn)
            {
                thinktime = DateTime.Now.Ticks + 30000000;
            }
        }

        private void Button_Nimm1(object sender, RoutedEventArgs e)
        {
            if(index > 0 && playerTurn)
            {
                index--;
                match[index].remove("match" + index.ToString());
                playerTurn = false;
                thinktime = DateTime.Now.Ticks + 10000000;
                AI.Start();
            }
        }
        private void Button_Nimm2(object sender, RoutedEventArgs e)
        {
            if (index > 1 && playerTurn)
            {
                index--;
                match[index].remove("match" + index.ToString());
                index--;
                match[index].remove("match" + index.ToString());
                playerTurn = false;
                thinktime = DateTime.Now.Ticks + 10000000;
                AI.Start();
            }
        }
        private void Button_Nimm3(object sender, RoutedEventArgs e)
        {
            if (index > 2 && playerTurn)
            {
                index--;
                match[index].remove("match" + index.ToString());
                index--;
                match[index].remove("match" + index.ToString());
                index--;
                match[index].remove("match" + index.ToString());

                playerTurn = false;
                thinktime = DateTime.Now.Ticks + 10000000;
                AI.Start();
            }
        }

    }
}
