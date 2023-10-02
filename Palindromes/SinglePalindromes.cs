﻿using System.Collections.Generic;
using System.Linq;

using Serilog;
using Serilog.Core;

namespace Palindromes;

/// <summary>
/// This class contain methods to check if a given number (in base 10) is a palindrome or not
/// </summary>
public class SinglePalindromes
{
    private readonly ILogger logger;

    public SinglePalindromes(ILogger logger)
    {
        this.logger = logger;
    }

    public bool IsPalindrome(uint number)
    {
        if(number == 0)
        {
            return false;
        }

        var normalOrderDigits = GetDigits(number);
        var numberOfDigits = normalOrderDigits.Count();

        var reverseOrderDigits = new short[normalOrderDigits.Length];
        normalOrderDigits.CopyTo(reverseOrderDigits, 0);

        reverseOrderDigits = reverseOrderDigits.Reverse().ToArray();

        var i = 0;
        var foundDifferentDigit = false;

        while(i < numberOfDigits && !foundDifferentDigit)
        {
            var nextDigit = normalOrderDigits[i];
            var reverseDigit = reverseOrderDigits[i];

            if(nextDigit != reverseDigit)
            {
                foundDifferentDigit = true;
            }

            i++;
        }

        return !foundDifferentDigit;
    }

    public uint GetLowestNextPalindrome(uint number)
    {
        var isNextPalindrome = false;
        var i = number;

        while(!isNextPalindrome)
        {
            i++;
            isNextPalindrome = IsPalindrome(i);
        }

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
        } while(i < maxNumber);

        palindromes.Add(i);

        var numberOfItems = palindromes.Count;

        if(palindromes[numberOfItems - 1] > maxNumber)
        {
            palindromes.RemoveAt(numberOfItems - 1);
        }

        return palindromes;
    }

    public short[] GetDigits(uint number)
    {
        var digits = new List<short>();
        var remainder = number;
        uint currentDigit;

        while(remainder > 0)
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

        var nthPalindrome = 1u;
        var order = 1;

        while (order != position) 
        {
            order++;
            nthPalindrome = GetLowestNextPalindrome(nthPalindrome);
        }

        logger.Information("Finished GetNthPalindrome for {position} - Palindrome is: {palindrome}", position, nthPalindrome);
        return nthPalindrome;
    }
}
