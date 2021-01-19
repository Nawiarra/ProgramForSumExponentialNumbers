using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ValidationCore;

namespace ProgramForSumExponentialNumbers
{
    public class ExponentialNumber
    {
        private const byte _dot = 46;
        private const int _sizeOfMantissa = 39;

        private const string _mantissaPattern = @"\d+\.\d+";
        private const string _exponentPattern = @"e[+-]?\d+";
        public LinkedList<byte> Mantissa { get; set; }
        public short Order { get; set; }

        public ExponentialNumber()
        {
            Mantissa = new LinkedList<byte>();

            Mantissa.AddFirst(0);
            Mantissa.AddFirst(46);
            Mantissa.AddFirst(0);

            Order = 0;
        }
        public static ExponentialNumber ExponentialNumberCreate(string value)
        {
            ExponentialNumber result = new ExponentialNumber();

            result.Order = GetExponentialFromString(value);
            result.Mantissa.Clear();
            result.Mantissa = GetMantissaFromString(value, _mantissaPattern);

            return result;
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            foreach (byte item in Mantissa)
            {
                if (item != _dot)
                {
                    result.Append(item);
                }
                else
                {
                    result.Append('.');
                }

            }

            result.Append("e");

            if (Order > 0)
            {
                result.Append("+");
            }

            result.Append(Order.ToString());

            return result.ToString();
        }

        private static short GetExponentialFromString(string value)
        {
            string exponential = Regex.Match(value, _exponentPattern).Value;

            exponential = Regex.Replace(exponential, "e+?", "");

            return Validation.TryParseShortValueInLine(exponential);
        }

        private static LinkedList<byte> GetMantissaFromString(string value, string pattern)
        {
            List<char> mantissa = Regex.Match(value, pattern).Value.ToList();

            LinkedList<byte> result = new LinkedList<byte>();

            double temp;

            foreach (var item in mantissa)
            {
                if (item != _dot)
                {
                    temp = Char.GetNumericValue(item);
                    result.AddLast(Convert.ToByte(temp));
                }
                else
                {
                    result.AddLast(Convert.ToByte(item));
                }
            }
            return result;
        }

        public static ExponentialNumber operator +(ExponentialNumber firstTerm, ExponentialNumber secondTerm)
        {
            if (firstTerm.Mantissa.Count == 0)
            {
                return secondTerm;
            }

            ExponentialNumber result = new ExponentialNumber();

            firstTerm.CastToCanonicalFormBigNumbers();
            secondTerm.CastToCanonicalFormBigNumbers();

            short exponentialValue = secondTerm.Order > firstTerm.Order ? secondTerm.Order : firstTerm.Order;

            result.Order = exponentialValue;

            if (exponentialValue == firstTerm.Order)
            {
                secondTerm.СastExponentOfNumberToValue(exponentialValue);
            }
            else
            {
                firstTerm.СastExponentOfNumberToValue(exponentialValue);
            }

            AlignMantissas(firstTerm.Mantissa, secondTerm.Mantissa);

            if (firstTerm.Mantissa.First.Value > 0)
            {
                result.Mantissa = SumMantissasByteValue(firstTerm.Mantissa, secondTerm.Mantissa);
            }
            else
            {
                result.Mantissa = SumMantissasByteValue(secondTerm.Mantissa, firstTerm.Mantissa);
            }

            result.RoundingResult();

            if (result.Mantissa.First.Value != 0)
            {
                result.CastToCanonicalFormBigNumbers();
            }
            else
            {
                result.CastToCanonicalFormSmallNumbers();
            }


            return result;
        }

        private void СastExponentOfNumberToValue(short exponentValue)
        {

            if (Order == exponentValue)
                return;

            int exponentDifference = Order - exponentValue;

            Mantissa = ExpandMantissaFront(Math.Abs(exponentDifference), Mantissa);

            Order = exponentValue;
        }

        private static LinkedList<byte> ExpandMantissaBack(int quantity, LinkedList<byte> Mantissa)
        {
            LinkedListNode<byte> dotItem = Mantissa.Find(_dot);

            do
            {
                Mantissa.AddLast(0);
                quantity--;

            } while (quantity > 0);

            return Mantissa;
        }

        private static LinkedList<byte> ExpandMantissaFront(int quantity, LinkedList<byte> Mantissa)
        {
            var dotItem = Mantissa.Find(_dot);

            byte temp = 0;

            if (dotItem == null)
            {
                Mantissa.AddLast(_dot);
                dotItem = Mantissa.Last;
            }

            do
            {
                if (dotItem.Previous == null)
                {
                    Mantissa.AddFirst(0);
                }

                temp = dotItem.Previous.Value;
                dotItem.Previous.Value = dotItem.Value;
                dotItem.Value = temp;

                dotItem = dotItem.Previous;

                quantity--;

            } while (quantity > 0);

            if (Mantissa.First.Value == _dot)
            {
                Mantissa.AddFirst(0);
            }


            return Mantissa;
        }

        private static LinkedList<byte> SumMantissasByteValue(LinkedList<byte> firstMantissa, LinkedList<byte> secondMantissa)
        {
            LinkedListNode<byte> sum;
            byte remainder = 0;

            LinkedList<byte> result = new LinkedList<byte>();

            for (int i = 0; i < firstMantissa.Count; i++)
            {
                if (firstMantissa.ElementAt(i) != _dot)
                {
                    sum = new LinkedListNode<byte>(Convert.ToByte(firstMantissa.ElementAt(i) + secondMantissa.ElementAt(i)));
                }
                else
                {
                    sum = new LinkedListNode<byte>(_dot);

                }

                if (sum.Value > 10 && sum.Value != _dot)
                {
                    remainder = (byte)(sum.Value - 10);
                    sum.Value = 1;
                }

                result.AddLast(sum);

                if (remainder != 0)
                {
                    result.AddLast(remainder);
                    remainder = 0;
                }
            }

            return result;
        }

        private static void AlignMantissas(LinkedList<byte> firstMantissa, LinkedList<byte> secondMantissa)
        {

            int differenceBetweenPartialPartOfMantissaCount = firstMantissa.Count - secondMantissa.Count;

            if (differenceBetweenPartialPartOfMantissaCount == 0)
                return;

            if (differenceBetweenPartialPartOfMantissaCount > 0)
            {
                ExpandMantissaBack(differenceBetweenPartialPartOfMantissaCount, secondMantissa);
            }
            else
            {
                ExpandMantissaBack(Math.Abs(differenceBetweenPartialPartOfMantissaCount), firstMantissa);
            }
        }

        private static void RemoveItemsFromMantissa(int count, LinkedList<byte> list)
        {
            LinkedListNode<byte> temp;

            int listSize = list.Count;

            for (int i = 0; i < listSize - 1; i++)
            {
                temp = list.Last;

                if ((count <= 0) && (temp.Value != 0) && (temp.Value != _dot))
                {
                    return;
                }

                if (temp.Value > 5 && temp.Value != _dot)
                {
                    temp.Previous.Value += 1;
                }

                list.RemoveLast();

                count--;
            }

        }

        private static byte RoundingValue(LinkedListNode<byte> lastPartialItem)
        {
            LinkedListNode<byte> roundPartialItem = lastPartialItem;

            byte lastWholeItemIncrease = 0;

            while (roundPartialItem.Previous != null)
            {
                if ((roundPartialItem.Value >= 10) && (roundPartialItem.Value != _dot))
                {
                    roundPartialItem.Value -= 10;
                    if (roundPartialItem.Previous.Value != _dot)
                    {
                        roundPartialItem.Previous.Value += 1;
                    }
                    else
                    {
                        roundPartialItem.Previous.Previous.Value += 1;
                    }

                }

                roundPartialItem = roundPartialItem.Previous;

            }

            if ((roundPartialItem.Value >= 10) && (roundPartialItem.Value != _dot))
            {
                lastWholeItemIncrease = 1;
                roundPartialItem.Value -= 10;
            }

            return lastWholeItemIncrease;
        }

        private void RoundingResult()
        {
            int countItemsToRemove = 0;
            byte firstWholeItemIncrease = 0;

            var TempNode = Mantissa.First;

            countItemsToRemove = (Mantissa.Count - 1) - _sizeOfMantissa;

            RemoveItemsFromMantissa(countItemsToRemove, Mantissa);

            firstWholeItemIncrease = RoundingValue(Mantissa.Last);

            if (firstWholeItemIncrease == 1)
            {
                Mantissa.AddFirst(firstWholeItemIncrease);
            }

            byte roundingValue = RoundingValue(Mantissa.Last);
            Mantissa.Last.Value += roundingValue;

        }
        private void CastToCanonicalFormBigNumbers()
        {
            int countItemsToRemove = 0;

            var TempNode = Mantissa.First;

            while (TempNode.Next != null)
            {
                if (TempNode.Next.Value == _dot)
                {
                    break;
                }

                countItemsToRemove++;

                TempNode = TempNode.Next;
            }

            if (countItemsToRemove != 0)
            {
                ExpandMantissaFront(countItemsToRemove, Mantissa);
                Order += (short)countItemsToRemove;
            }
        }

        private void CastToCanonicalFormSmallNumbers()
        {
            var DotNode = Mantissa.Find(_dot);
            var TempNode = Mantissa.First;

            if (DotNode == null)
                return;

            if (DotNode.Previous.Value != 0)
                return;

            while (TempNode.Next != null)
            {
                if ((TempNode.Previous != null) && (TempNode.Previous.Value == _dot) && (TempNode.Value == 0))
                {
                    Mantissa.Remove(TempNode);

                    TempNode = DotNode;

                    Order--;
                }

                TempNode = TempNode.Next;
            }

            TempNode = DotNode.Next;
            Mantissa.Remove(DotNode.Next);
            Mantissa.Remove(DotNode.Previous);
            Mantissa.AddFirst(TempNode);
            Order--;

        }
    }
}
