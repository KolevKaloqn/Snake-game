using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    public class Possition
    {
        public int Row { get; }
        public int Column { get; }

        public Possition(int row, int col)
        {
            this.Row = row;
            this.Column = col;
        }

        //Returns the possition we get by moving one step in the given position
        public Possition Translate(Direction dir)
        {
            return new Possition(Row + dir.RowOffset, Column + dir.ColumnOffset);
        }

        public override bool Equals(object obj)
        {
            return obj is Possition possition &&
                   Row == possition.Row &&
                   Column == possition.Column;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Row, Column);
        }

        public static bool operator ==(Possition left, Possition right)
        {
            return EqualityComparer<Possition>.Default.Equals(left, right);
        }

        public static bool operator !=(Possition left, Possition right)
        {
            return !(left == right);
        }
    }
}
