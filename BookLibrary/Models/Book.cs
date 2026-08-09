using System;
using System.Collections.Generic;
using System.Text;

namespace BookLibrary.Models
{
    public class Book
    {
        public string Title { get; private set; }
        public string Author { get; private set; }
        public int Year { get; private set; }
        public int Pages { get; private set; }
        public string Isbn { get; private set; }

        public string Category { get; private set; }

        public Book(string title, string author, int year, int pages, string isbn, string category      )
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Title is required.", nameof(title));
            }
            if (string.IsNullOrWhiteSpace(author))
            {
                throw new ArgumentException("Author is required.", nameof(author));
            }
            if (string.IsNullOrWhiteSpace(isbn))
            {
                throw new ArgumentException("ISBN is required.", nameof(isbn));
            }

            if (year < 1450 || year > DateTime.Now.Year + 1)
            {
                throw new ArgumentOutOfRangeException(nameof(year), "Year must be between 1450 and next year.");
            }

            if (pages <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pages), "Number of pages must be a positive number.");
            }

            if(string.IsNullOrWhiteSpace(category))
            {
                throw new ArgumentException("Category is required.", nameof(category));
            }

            this.Title = title;
            this.Author = author;
            this.Year = year;
            this.Pages = pages;
            this.Isbn = isbn;
            this.Category = category;
        }

        public override string ToString()
        {
            return $"Title: {Title}\n" +
                   $"Author: {Author}\n" +
                   $"Year: {Year}\n" +
                   $"Pages: {Pages}\n" +
                   $"ISBN: {Isbn}";
        }
    }
}
