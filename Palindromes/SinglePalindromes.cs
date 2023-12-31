﻿using System;
using System.Collections.Generic;
using System.Linq;

using Serilog;

namespace Palindromes;

/// <summary>
/// This class contain methods to check if a given number (in base 10) is a palindrome or not
/// </summary>
public class SinglePalindromes
{
    private readonly ILogger logger;

    public SinglePalindromes(ILogger logger)
    {
        this.logger = logger.ForContext<SinglePalindromes>();
    }

    public bool IsPalindrome(uint number)
    {
        if (number == 0)
        {
            logger.Verbose("Zero is not a valid palindrome number");
            return false;
        }

        var normalOrderDigits = GetDigits(number);
        var numberOfDigits = normalOrderDigits.Count();

        var reverseOrderDigits = new short[normalOrderDigits.Length];
        normalOrderDigits.CopyTo(reverseOrderDigits, 0);

        reverseOrderDigits = reverseOrderDigits.Reverse().ToArray();

        var i = 0;
        var foundDifferentDigit = false;

        while (i < numberOfDigits && !foundDifferentDigit)
        {
            var nextDigit = normalOrderDigits[i];
            var reverseDigit = reverseOrderDigits[i];

            if (nextDigit != reverseDigit)
            {
                foundDifferentDigit = true;
                logger.Verbose("Found a different digit in position {position}", i);
            }

            i++;
        }

        return !foundDifferentDigit;
    }

    public uint GetLowestNextPalindrome(uint number)
    {
        var isNextPalindrome = false;
        var i = number;

        while (!isNextPalindrome)
        {
            if (i == uint.MaxValue)
            {
                logger.Error("Search for lowest next palindrome caused an overflow exception.");
                throw new System.OverflowException();
            }

            i++;
            isNextPalindrome = IsPalindrome(i);
        }

        logger.Verbose("After {number}, the next palindrome is {i}", number, i);
        return i;
    }

    public List<uint> GetAllPalindromesInARange(uint maxNumber)
    {
        var palindromes = new List<uint>();

        var i = 1u;

        do
        {
            palindromes.Add(i);
            i = GetLowestNextPalindrome(i);
        } while (i < maxNumber);

        palindromes.Add(i);

        var numberOfItems = palindromes.Count;

        if (palindromes[numberOfItems - 1] > maxNumber)
        {
            palindromes.RemoveAt(numberOfItems - 1);
        }

        logger.Verbose("There are {numberOfPalindromes} palindromes up to {MaximumNumber}", palindromes.Count, maxNumber);
        return palindromes;
    }

    public short[] GetDigits(uint number)
    {
        var digits = new List<short>();
        var remainder = number;
        uint currentDigit;

        while (remainder > 0)
        {
            currentDigit = remainder % 10;
            remainder = remainder / 10;
            digits.Add((short)currentDigit);
        }

        digits.Reverse();

        return digits.ToArray();
    }

    public uint GetNthPalindrome(int position)
    {
        logger.Information("Starting GetNthPalindrome for {position}", position);

        if (position > 142946)
        {
            var message = $"The maximum palindrome that this program is able to calculate is 4294884924 at 142946th position";
            logger.Error("The maximum palindrome that this program is able to calculate is {nthPalindrome} at {order}th position", 4294884924, 142946);
            throw new ArgumentOutOfRangeException(message);
        }

        var nthPalindrome = 1u;
        var order = 1;
        var quarter = position / 4;
        var quarterDisplayMessage = true;

        var half = position / 2;
        var halfDisplayMessage = true;

        var thirdquarter = 3 * position / 4;
        var thirdquarterDisplayMessage = true;

        while (order != position)
        {
            var remainder = order % 10000;
            if (remainder == 1)
            {
                logger.Error("CurrentPalindrome: {currentPalindrome} - Maximum value for uint: {maxInt}", nthPalindrome, uint.MaxValue);
                logger.Information("{currentOrder}th Palindrome is: {currentPalindrome}", order, nthPalindrome);
            }

            if (quarterDisplayMessage && order >= quarter - 1 && order <= quarter + 1)
            {
                logger.Information("A quarter of processing for {currentOrder} has been completed - Current palindrome: {palindrome}...", order, nthPalindrome);
                quarterDisplayMessage = false;
            }
            else if (halfDisplayMessage && order >= half - 1 && order <= half + 1)
            {
                logger.Information("Half of processing for {currentOrder} has been completed - Current palindrome: {palindrome}...", order, nthPalindrome);
                halfDisplayMessage = false;
            }
            else if (thirdquarterDisplayMessage && order >= thirdquarter - 1 && order <= thirdquarter + 1)
            {
                logger.Information("Three quarters of processing for {currentOrder} have been completed - Current palindrome: {palindrome}...", order, nthPalindrome);
                thirdquarterDisplayMessage = false;
            }

            order++;
            nthPalindrome = GetLowestNextPalindrome(nthPalindrome);
        }

        logger.Information("Finished GetNthPalindrome for {position} - Palindrome is: {palindrome}", position, nthPalindrome);
        return nthPalindrome;
    }
}
