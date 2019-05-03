using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

// kymphillpotts cheered 10 May 3, 2019

namespace XamChat.Model
{
    public class User : ObservableObject
    {
        static Random Random = new Random();
        string name;
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        string firstLetter;
        public string FirstLetter
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(firstLetter))
                    return firstLetter;

                firstLetter = Name?.Length > 0 ? Name[0].ToString() : "?";
                return firstLetter;
            }
            set => firstLetter = value;
        }

        Color color;
        public Color Color
        {
            get
            {
                if (color != null && color.A != 0)
                    return color;

                color = Color.FromRgb(Random.Next(0, 255), Random.Next(0, 255), Random.Next(0, 255)).MultiplyAlpha(.9);
                return color;
            }
            set => color = value;
        }
    }
}
