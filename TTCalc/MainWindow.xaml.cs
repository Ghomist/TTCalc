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

    private void Expression_TextChanged(object sender, TextChangedEventArgs e)
    {
        _calculator.Expression = Expression.Text;
        ExpQueue.Text = _calculator.ReversePolish;

        bool[] answers = _calculator.Answers;
        if (answers.Length > 0)
        {
            TextBlock text;
            AnswerStack.Children.Clear();
            AnswerStack.ColumnDefinitions.Clear();
            AnswerStack.RowDefinitions.Clear();
            var symbols = _calculator.Symbols;

            // row for header
            AnswerStack.RowDefinitions.Add(new() { Height = new(1, GridUnitType.Star) });
            for (int i = 0; i < symbols.Count; ++i)
            {
                text = new() { Text = symbols[i] };
                AnswerStack.ColumnDefinitions.Add(new() { Width = new(1, GridUnitType.Star) });
                AnswerStack.Children.Add(text);
                Grid.SetColumn(text, i);
            }

            // column for value
            AnswerStack.ColumnDefinitions.Add(new() { Width = new(1, GridUnitType.Star) });

            for (int i = 0; i < answers.Length; i++)
            {
                AnswerStack.RowDefinitions.Add(new() { Height = new(1, GridUnitType.Star) });
                for (int j = 0; j < symbols.Count; ++j)
                {
                    text = new() { Text = (i & (1 << j)) > 0 ? "1" : "0" };
                    AnswerStack.Children.Add(text);
                    Grid.SetRow(text, i + 1);
                    Grid.SetColumn(text, symbols.Count - j - 1);
                }
                text = new() { Text = answers[i] ? "1" : "0" };
                AnswerStack.Children.Add(text);
                Grid.SetRow(text, i + 1);
                Grid.SetColumn(text, symbols.Count);
            }
        }
    }
}

