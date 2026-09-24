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

namespace CSharp_Calculator_GUI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{   
    string enteredNumber = "";
    string? stockedNumber = null;
    string? selectedOperator = null;
    bool reset = false;


    public MainWindow()
    {
        InitializeComponent();
    }

    private void GetNumber(object sender, RoutedEventArgs e)
    {
        Button clickedButton = sender as Button;
        if (!reset)
        {
            enteredNumber += clickedButton.Content.ToString();
        }
        else
        {
            enteredNumber = clickedButton.Content.ToString();
            reset = false;
        }
        DisplayText.Text = enteredNumber;
    }

    // methode opérateur (récupere le signe passe reset à true et stock les valeurs)
    private void GetOperator(object sender, RoutedEventArgs e)
    {
        Button clickedButton = sender as Button;
        if (!reset)
        {   
            if (selectedOperator == null)
            {
                selectedOperator = clickedButton.Content.ToString()
                StockedText.Text = enteredNumber + " " + selectedOperator + " ";
                reset = true;
            }
            else
            {
                // affiche le resultat du calcul complet dans le stock  et dans le display 
            }
        }
    }

    private double DoCalculation()
    {
        switch (selectedOperator)
        {
            case "+":
                // methode addition
                return Calculate.Addition(stockedNumber, enteredNumber);
                
            case "-":
                // methode soustraction 
                return Calculate.Subtraction(stockedNumber, enteredNumber);

            case "×":
                // methode multiplication 
                return Calculate.Multiplication(stockedNumber, enteredNumber);

            case "÷":
                // methode division 
                return Calculate.Division(stockedNumber, enteredNumber);
        }
        
    }

    // methode display result (effectue le calcul passe reset à true ecris le calcul complet dans stock et affiche le resultat)
}


class Calculate
{
    private static double TransformStringToDouble(string numberString)
    {
        return numberDouble
    }

    public static double Addition(string numberString1, string numberString2)
    {
        return result
    }

    public static double Subtraction(string numberString1, string numberString2)
    {
        return result
    }

    public static double Multiplication(string numberString1, string numberString2)
    {
        return result
    }

    public static double Division(string numberString1, string numberString2)
    {
        return result
    }

}