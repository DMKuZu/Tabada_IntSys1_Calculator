using System;
using System.Data;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Tabada_IntSys1_Calculator.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        // History and clear buttons
        BtnHistory.Click += BtnHistory_Click;
        BtnClearHistory.Click += BtnClearHistory_Click;
    
        // Clear buttons
        BtnClear.Click += BtnClear_Click;
        BtnClearEntry.Click += BtnClearEntry_Click;
        BtnBackSpace.Click += BtnBackSpace_Click;
    
        // Numbers
        Btn0.Click += NumberButton_Click;
        Btn1.Click += NumberButton_Click;
        Btn2.Click += NumberButton_Click;
        Btn3.Click += NumberButton_Click;
        Btn4.Click += NumberButton_Click;
        Btn5.Click += NumberButton_Click;
        Btn6.Click += NumberButton_Click;
        Btn7.Click += NumberButton_Click;
        Btn8.Click += NumberButton_Click;
        Btn9.Click += NumberButton_Click;
    
        // Operators
        BtnAdd.Click += OperatorButton_Click;
        BtnSubtract.Click += OperatorButton_Click;
        BtnDivide.Click += OperatorButton_Click;
        BtnMultiply.Click += OperatorButton_Click;
        
        //decimal and parentheses
        BtnDecimal.Click += DecimalButton_Click;
        BtnOpen.Click += ParenthesisButton_Click;
        BtnClose.Click += ParenthesisButton_Click;
        
        // Equals Button
        BtnEquals.Click += BtnEquals_Click;
    }
    private void BtnHistory_Click(object? sender, RoutedEventArgs e)
    {
        Overlay.IsPaneOpen = !Overlay.IsPaneOpen;
    }
    private void BtnClearHistory_Click(object? sender, RoutedEventArgs e)
    {
        ListHistory.Items.Clear();
    }
    private void NumberButton_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button)
        {
            TbInput.Text += button.Content.ToString();
        }
    }
    private void OperatorButton_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button)
        {
            TbInput.Text += button.Content.ToString();
        }
    }
    private void BtnEquals_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            // Calculate the result
            var result = new DataTable().Compute(TbInput.Text, "").ToString();
            TbResult.Text = result;
            
            // Add to history
            var historyItem = $"{TbInput.Text} = {result}";
            ListHistory.Items.Add(historyItem);
            ListHistory.IsVisible = true;
        }
        catch (Exception ex)
        {
            TbResult.Text = "Error";
        }
    }
    private void BtnClear_Click(object? sender, RoutedEventArgs e)
    {
        TbInput.Text = string.Empty;
        TbResult.Text = string.Empty;
    }
    private void BtnClearEntry_Click(object? sender, RoutedEventArgs e)
    {
        TbInput.Text = string.Empty;
    }
    private void BtnBackSpace_Click(object? sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrEmpty(TbInput.Text))
        {
            TbInput.Text = TbInput.Text.Remove(TbInput.Text.Length - 1);
        }
    }
    private void DecimalButton_Click(object? sender, RoutedEventArgs e)
    {
        // Check if the current number already contains a decimal point
        if (string.IsNullOrEmpty(TbInput.Text))
        {
            // If input is empty, start with "0."
            TbInput.Text = "0.";
            return;
        }
    
        // Find the last operator in the expression
        string currentText = TbInput.Text;
        int lastOperatorIndex = -1;
        
        foreach (char op in new[] { '+', '-', '*', '/', '(', ')' })
        {
            int lastIndex = currentText.LastIndexOf(op);
            if (lastIndex > lastOperatorIndex)
                lastOperatorIndex = lastIndex;
        }
        
        // Get the last number (substring after the last operator)
        string lastNumber = currentText.Substring(lastOperatorIndex + 1);
        
        // Only add decimal point if it doesn't already exist in the current number
        if (!lastNumber.Contains("."))
        {
            TbInput.Text += ".";
        }
    }
    private void ParenthesisButton_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button)
        {
            // Add the parenthesis character to the expression
            TbInput.Text += button.Content.ToString();
        }
    }
}