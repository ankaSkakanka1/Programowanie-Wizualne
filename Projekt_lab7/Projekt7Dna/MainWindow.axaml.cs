using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;

namespace Projekt7Dna;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    public void PoliczDNA(object source, RoutedEventArgs args)
    {
        var tekst = InputDNA.Text ?? "";

        //int count = 0;

        if (string.IsNullOrEmpty(tekst) || tekst.Length < 4)
        {
            Wynik.Text = "Wprowadź conajmniej 4 znaki";
            return;
        }
        tekst = tekst.ToUpper();

        var slownik = new Dictionary<string, int>();

        for (int i = 0; i <= tekst.Length - 4; i++)
        {
            string fragment = tekst.Substring(i, 4);

            if (!CzyPoprawneDNA(fragment))
                continue;

            if (slownik.ContainsKey(fragment))
                slownik[fragment]++;
            else
                slownik[fragment] = 1;
        }

        string wynikTekst = "";

        foreach (var para in slownik)
        {
            wynikTekst += $"{para.Key}: {para.Value}\n";
        }

        if (wynikTekst == "")
            wynikTekst = "Brak poprawnych sekwencji ACGT!";

        Wynik.Text = wynikTekst;

    }

      private bool CzyPoprawneDNA(string fragment)
    {
        foreach (char c in fragment)
        {
            if (c != 'A' && c != 'C' && c != 'G' && c != 'T')
                return false;
        }
        return true;
    }
}