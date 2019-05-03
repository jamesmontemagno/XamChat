using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace XamChat.Model
{
    public class ChatMessage : ObservableObject
    {
        static Random Random = new Random();
        string user;
        public string User
        {
            get => user;
            set => SetProperty(ref user, value);
        }

        string message;
        public string Message
        {
            get => message;
            set => SetProperty(ref message, value);
        }

        string firstLetter;
        public string FirstLetter
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(firstLetter))
                    return firstLetter;

                firstLetter = User?.Length > 0 ? User[0].ToString() : "?";
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

        Color backgroundColor;
        public Color BackgroundColor
        {
            get
            {
                if (backgroundColor != null && backgroundColor.A != 0)
                    return backgroundColor;

                backgroundColor = Color.MultiplyAlpha(.6);
                return backgroundColor;
            }
            set => backgroundColor = value;
        }
    }
}
