using System;
using System.Data;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Metadata;
using Avalonia.Input; // <-- Add this for KeyEventArgs

namespace Tabada_IntSys1_Calculator.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        SizeChanged += OnSizeChanged;
            
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

        // Keyboard mapping
        this.KeyDown += MainWindow_KeyDown;
    }
    
    // helper method to calculate the result of the expression in the input textbox
    private void Calculate()
    {
        try
        {
            var computeResult = new DataTable().Compute(TbInput.Text, "");
            TbResult.Text = ToEightDecimalPlaces(computeResult);
        }
        catch (Exception)
        {
            TbResult.Text = String.Empty;
        }
    }
    private string ToEightDecimalPlaces(object input)
    {
        if (double.TryParse(input.ToString(), out double num))
            return Math.Round(num, 8).ToString();
        return input.ToString();
    }
    // sets the width of the overlay to 50% of the panel size + 50 for the close panel of the window width
    private void OnSizeChanged(object? sender, SizeChangedEventArgs e)
    {
        if (Overlay.IsPaneOpen)
        {
            Overlay.OpenPaneLength = Bounds.Width * 0.5 + 50; 
        }
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
        
        // Calculate the result in real-time as numbers are entered
        Calculate();
    }
    private void OperatorButton_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button)
        {
            TbInput.Text += button.Content.ToString();
        }
        Calculate();
    }
    private void BtnEquals_Click(object? sender, RoutedEventArgs e)
    {
        object result;
        try
        {
            // Calculate the result
            result = new DataTable().Compute(TbInput.Text, "");
            string text = ToEightDecimalPlaces(result);
            TbResult.Text = text;

            // Avoid adding duplicate entries to history
            if (text!.Equals(TbInput.Text) || string.IsNullOrEmpty(text))
            {
                return;
            }

            // Add to history
            var historyItem = $"{TbInput.Text} \n= {text}";
            ListHistory.Items.Add(historyItem);
            ListHistory.IsVisible = true;
            
            // Update the input textbox with the result
            TbInput.Text = text;
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
        Calculate();
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
    // keybinds for calculator buttons
    private void MainWindow_KeyDown(object? sender, KeyEventArgs e)
    {
        bool handled = false;
        switch (e.Key)
        {
            case Key.D0:
                if (e.KeyModifiers == KeyModifiers.Shift)
                {
                    BtnClose.RaiseEvent(new RoutedEventArgs(Button.ClickEvent)); // Shift+0 = ')'
                    handled = true;
                }
                else
                {
                    Btn0.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    handled = true;
                }
                break;
            case Key.NumPad0:
                Btn0.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                handled = true;
                break;
            case Key.D1:
            case Key.NumPad1:
                Btn1.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                handled = true;
                break;
            case Key.D2:
            case Key.NumPad2:
                Btn2.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                handled = true;
                break;
            case Key.D3:
            case Key.NumPad3:
                Btn3.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                handled = true;
                break;
            case Key.D4:
            case Key.NumPad4:
                Btn4.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                handled = true;
                break;
            case Key.D5:
            case Key.NumPad5:
                Btn5.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                handled = true;
                break;
            case Key.D6:
            case Key.NumPad6:
                Btn6.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                handled = true;
                break;
            case Key.D7:
            case Key.NumPad7:
                Btn7.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                handled = true;
                break;
            case Key.D8:
                if (e.KeyModifiers == KeyModifiers.Shift)
                {
                    BtnMultiply.RaiseEvent(new RoutedEventArgs(Button.ClickEvent)); // Shift+8 = '*'
                    handled = true;
                }
                else
                {
                    Btn8.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    handled = true;
                }
                break;
            case Key.NumPad8:
                Btn8.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                handled = true;
                break;
            case Key.D9:
                if (e.KeyModifiers == KeyModifiers.Shift)
                {
                    BtnOpen.RaiseEvent(new RoutedEventArgs(Button.ClickEvent)); // Shift+9 = '('
                    handled = true;
                }
                else
                {
                    Btn9.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    handled = true;
                }
                break;
            case Key.NumPad9:
                Btn9.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                handled = true;
                break;
            case Key.Add:
            case Key.OemPlus:
                BtnAdd.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                handled = true;
                break;
            case Key.Subtract:
            case Key.OemMinus:
                BtnSubtract.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                handled = true;
                break;
            case Key.Multiply:
                BtnMultiply.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                handled = true;
                break;
            case Key.Divide:
                BtnDivide.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                handled = true;
                break;
            case Key.Oem2: // '/' on main keyboard
                BtnDivide.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                handled = true;
                break;
            case Key.Decimal:
            case Key.OemPeriod:
                BtnDecimal.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                handled = true;
                break;
            case Key.OemOpenBrackets:
                BtnOpen.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                handled = true;
                break;
            case Key.Oem6: // ')'
                BtnClose.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                handled = true;
                break;
            case Key.Return:
                BtnEquals.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                handled = true;
                break;
            case Key.Back:
                BtnBackSpace.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                handled = true;
                break;
            case Key.Oem5: // '\' key
                BtnHistory.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                handled = true;
                break;
        }
        if (handled)
            e.Handled = true;
    }
}