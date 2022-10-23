using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace TTCalc;

/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window
{
    private readonly Calculator _calculator = new();

    public MainWindow()
    {
        InitializeComponent();
    }

    private void myButton_Click(object sender, RoutedEventArgs e)
    {
        myButton.Content = "Clicked";
    }

    private void Expression_TextChanged(object sender, TextChangedEventArgs e)
    {
        _calculator.Expression = Expression.Text;
        ExpQueue.Text = _calculator.ReversePolish;
        var answers = _calculator.Answers;
        if (answers.Length > 0)
        {
            AnswerStack.Children.Clear();
            var panel = new StackPanel() { Orientation = Orientation.Horizontal };
            foreach (var symbol in _calculator.Symbols)
            {
                panel.Children.Add(new TextBlock { Text = symbol + ", " });
            }
            AnswerStack.Children.Add(panel);
            for (int i = 0; i < answers.Length; i++)
            {
                panel = new StackPanel() { Orientation = Orientation.Horizontal };
                panel.Children.Add(new TextBlock { Text = Convert.ToString(i, 2).PadLeft(10, '0') });
                panel.Children.Add(new TextBlock { Text = "---" });
                panel.Children.Add(new TextBlock { Text = answers[i].ToString() });
                AnswerStack.Children.Add(panel);
            }
        }
    }
}

