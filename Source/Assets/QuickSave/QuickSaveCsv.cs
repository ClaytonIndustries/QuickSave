////////////////////////////////////////////////////////////////////////////////
//  
// @module Quick Save for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using CI.QuickSave.Core.Storage;

namespace CI.QuickSave
{
    public class QuickSaveCsv
    {
        /// <summary>
        /// Returns the number of rows in the csv file
        /// </summary>
        public int RowCount => _values.Count;

        private readonly List<List<string>> _values;

        private QuickSaveCsv(List<List<string>> items)
        {
            _values = items;
        }

        public QuickSaveCsv()
        {
            _values = new List<List<string>>();
        }

        /// <summary>
        /// Loads a csv file from the specified path
        /// </summary>
        /// <param name="path">The path to the file</param>
        /// <returns>A QuickSaveCsv object that represents the loaded csv file</returns>
        /// <exception cref="QuickSaveException"></exception>
        public static QuickSaveCsv Load(string path)
        {
            var lines = FileAccess.LoadLines(path);

            if (lines == null)
            {
                throw new QuickSaveException("File does not exist");
            }

            List<List<string>> values = new List<List<string>>();

            try
            {
                foreach (var line in lines)
                {
                    values.Add(new List<string>());

                    var cells = line.Split(',');

                    foreach (var cell in cells)
                    {
                        values.Last().Add(cell);
                    }
                }
            }
            catch
            {
                throw new QuickSaveException("Failed to load file");
            }

            return new QuickSaveCsv(values);
        }

        /// <summary>
        /// Saves the QuickSaveCsv to a csv file
        /// </summary>
        /// <param name="path">The path to the file</param>
        /// <exception cref="QuickSaveException"></exception>
        public void Save(string path) 
        {
            var lines = _values.Select(x => string.Join(",", x)).ToList();

            if (!FileAccess.SaveLines(path, lines))
            {
                throw new QuickSaveException("Failed to save file");
            }
        }

        /// <summary>
        /// Gets the count of the cells in the specified row
        /// </summary>
        /// <param name="row">The zero based index of the row</param>
        /// <returns>Count of the cells in the row</returns>
        /// <exception cref="QuickSaveException"></exception>
        public int GetCellCount(int row)
        {
            if (_values.Count <= row)
            {
                throw new QuickSaveException("The specified row does not exist");
            }

            return _values[row].Count;
        }

        /// <summary>
        /// Gets value of the specified cell
        /// </summary>
        /// <param name="row">The zero based index of the row</param>
        /// <param name="column">The zero based index of the column</param>
        /// <returns>The value of the cell</returns>
        /// <exception cref="QuickSaveException"></exception>
        public string GetCell(int row, int column)
        {
            if (_values.Count <= row || _values[row].Count <= column)
            {
                throw new QuickSaveException("The specified cell does not exist");
            }

            return _values[row][column];
        }

        /// <summary>
        /// Gets the value of the specified cell
        /// </summary>
        /// <typeparam name="T">The type to return the value as</typeparam>
        /// <param name="row">The zero based index of the row</param>
        /// <param name="column">The zero based index of the column</param>
        /// <returns>The value of the cell</returns>
        /// <exception cref="QuickSaveException"></exception>
        public T GetCell<T>(int row, int column)
        {
            if (_values.Count <= row || _values[row].Count <= column)
            {
                throw new QuickSaveException("The specified cell does not exist");
            }

            return (T)Convert.ChangeType(_values[row][column], typeof(T));
        }

        /// <summary>
        /// Sets the value of the specified cell
        /// </summary>
        /// <param name="row">The zero based index of the row</param>
        /// <param name="column">The zero based index of the column</param>
        /// <param name="value">The value to set</param>
        public void SetCell(int row, int column, string value)
        {
            if (_values.Count <= row)
            {
                while (_values.Count <= row)
                {
                    _values.Add(new List<string>());
                }
            }

            if (_values[row].Count <= column)
            {
                while (_values[row].Count <= column)
                {
                    _values[row].Add(string.Empty);
                }
            }

            _values[row][column] = value;
        }

        /// <summary>
        /// Sets the value of the specified cell
        /// </summary>
        /// <typeparam name="T">The type of the value</typeparam>
        /// <param name="row">The zero based index of the row</param>
        /// <param name="column">The zero based index of the column</param>
        /// <param name="value">The value to set</param>
        public void SetCell<T>(int row, int column, T value) => SetCell(row, column, Convert.ToString(value));

        /// <summary>
        /// Deletes the specified row
        /// </summary>
        /// <param name="row">The zero based index of the row</param>
        public void DeleteRow(int row)
        {
            if (_values.Count > row)
            {
                _values.RemoveAt(row);
            }
        }
    }
}