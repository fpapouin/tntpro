////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	cStringProcessing.cs
//
// summary:	Implements the string processing class
////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NationalInstruments.TestStand.Interop.API;
using System.IO;
using System.IO.Ports;
using System.Text.RegularExpressions;

namespace Tools_StringProcessing
{
    public static class cStringProcessing
    {
        #region Variables

        static int ERROR_EMPTY_STRING = -1;
        static int ERROR_REGEX_BADSTRING = -2;
        static int ERROR_REGEX_NOMATCH = -3;

        #endregion

        #region public Test

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Tools string processing extract strings with regular expression. </summary>
        ///
        ///
        /// <param name="sInputString" type="String">       The input string. </param>
        /// <param name="sRegExpression" type="String">     The register expression. </param>
        /// <param name="sOutputString1" type="out String"> [in,out] The first s output string. </param>
        /// <param name="bErrorOccured" type="out Boolean"> [in,out] The error occured. </param>
        /// <param name="iErrorCode" type="out Int32">      [in,out] Zero-based index of the error code. </param>
        /// <param name="sErrorMsg" type="out String">      [in,out] Message describing the error. </param>
        ///
        /// <returns>   An Int32. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static void Tools_StringProcessing_ExtractStringsWithRegex(String sInputString, String sRegExpression, ref String sOutputString1, ref Boolean bErrorOccured, ref Int32 iErrorCode, ref String sErrorMsg)
        {
            // Vérification de la chaine d'entrée
            if (String.IsNullOrEmpty(sInputString))
            {
                bErrorOccured = true;
                iErrorCode = ERROR_EMPTY_STRING;
                sErrorMsg = string.Format(Tools_StringProcessing.Properties.Resources.ERROR_FUNCTION, "Extract1SWithRegex", "La chaine d'entrée 'sInputString' est vide");
            }

            // Vérification de l'expression régulière
            if (!VerifyRegEx(sRegExpression))
            {
                bErrorOccured = true;
                iErrorCode = ERROR_REGEX_BADSTRING;
                sErrorMsg = string.Format(Tools_StringProcessing.Properties.Resources.ERROR_FUNCTION, "Extract1SWithRegex", "L'expression régulière d'entrée 'sRegExpression' est incorrecte");
            }

            // Création de l'expression régulière
            Regex regExpr = new Regex(sRegExpression, RegexOptions.None);
            // Création du match et matching de la chaine de caractère en entrée
            Match matchRegex = regExpr.Match(sInputString);

            // Vérification du bon matching
            if (!matchRegex.Success)
            {
                bErrorOccured = true;
                iErrorCode = ERROR_REGEX_NOMATCH;
                sErrorMsg = string.Format(Tools_StringProcessing.Properties.Resources.ERROR_FUNCTION, "ExtractProductInfos1S", "L'expression régulière ne match pas la chaine d'entrée");
            }

            // Récupération des données de matching
            sOutputString1 = matchRegex.Groups[1].Value;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Tools string processing extract strings with regular expression. </summary>
        ///
        ///
        /// <param name="sInputString" type="String">       The input string. </param>
        /// <param name="sRegExpression" type="String">     The register expression. </param>
        /// <param name="sOutputString1" type="out String"> [in,out] The first s output string. </param>
        /// <param name="sOutputString2" type="out String"> [in,out] The second s output string. </param>
        /// <param name="bErrorOccured" type="out Boolean"> [in,out] The error occured. </param>
        /// <param name="iErrorCode" type="out Int32">      [in,out] Zero-based index of the error code. </param>
        /// <param name="sErrorMsg" type="out String">      [in,out] Message describing the error. </param>
        ///
        /// <returns>   An Int32. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static void Tools_StringProcessing_ExtractStringsWithRegex(String sInputString, String sRegExpression, ref String sOutputString1, ref String sOutputString2, ref Boolean bErrorOccured, ref Int32 iErrorCode, ref String sErrorMsg)
        {
            // Vérification de la chaine d'entrée
            if (String.IsNullOrEmpty(sInputString))
            {
                bErrorOccured = true;
                iErrorCode = ERROR_EMPTY_STRING;
                sErrorMsg = string.Format(Tools_StringProcessing.Properties.Resources.ERROR_FUNCTION, "ExtractProductInfos2S", "La chaine d'entrée 'sInputString' est vide");
            }

            // Vérification de l'expression régulière
            if (!VerifyRegEx(sRegExpression))
            {
                bErrorOccured = true;
                iErrorCode = ERROR_REGEX_BADSTRING;
                sErrorMsg = string.Format(Tools_StringProcessing.Properties.Resources.ERROR_FUNCTION, "ExtractProductInfos2S", "L'expression régulière d'entrée 'sRegExpression' est incorrecte");
            }

            // Création de l'expression régulière
            Regex regExpr = new Regex(sRegExpression, RegexOptions.None);
            // Création du match et matching de la chaine de caractère en entrée
            Match matchRegex = regExpr.Match(sInputString);

            // Vérification du bon matching
            if (!matchRegex.Success)
            {
                bErrorOccured = true;
                iErrorCode = ERROR_REGEX_NOMATCH;
                sErrorMsg = string.Format(Tools_StringProcessing.Properties.Resources.ERROR_FUNCTION, "ExtractProductInfos2S", "L'expression régulière ne match pas la chaine d'entrée");
            }

            // Récupération des données de matching
            sOutputString1 = matchRegex.Groups[1].Value;
            sOutputString2 = matchRegex.Groups[2].Value;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Tools string processing extract strings with regular expression. </summary>
        ///
        ///
        /// <param name="sInputString" type="String">       The input string. </param>
        /// <param name="sRegExpression" type="String">     The register expression. </param>
        /// <param name="sOutputString1" type="out String"> [in,out] The first s output string. </param>
        /// <param name="sOutputString2" type="out String"> [in,out] The second s output string. </param>
        /// <param name="sOutputString3" type="out String"> [out] The third s output string. </param>
        /// <param name="bErrorOccured" type="out Boolean"> [in,out] The error occured. </param>
        /// <param name="iErrorCode" type="out Int32">      [in,out] Zero-based index of the error code. </param>
        /// <param name="sErrorMsg" type="out String">      [in,out] Message describing the error. </param>
        ///
        /// <returns>   An Int32. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static void Tools_StringProcessing_ExtractStringsWithRegex(String sInputString, String sRegExpression, ref String sOutputString1, ref String sOutputString2, ref String sOutputString3, ref Boolean bErrorOccured, ref Int32 iErrorCode, ref String sErrorMsg)
        {
            // Vérification de la chaine d'entrée
            if (String.IsNullOrEmpty(sInputString))
            {
                bErrorOccured = true;
                iErrorCode = ERROR_EMPTY_STRING;
                sErrorMsg = string.Format(Tools_StringProcessing.Properties.Resources.ERROR_FUNCTION, "ExtractProductInfos3S", "La chaine d'entrée 'sInputString' est vide");
            }

            // Vérification de l'expression régulière
            if (!VerifyRegEx(sRegExpression))
            {
                bErrorOccured = true;
                iErrorCode = ERROR_REGEX_BADSTRING;
                sErrorMsg = string.Format(Tools_StringProcessing.Properties.Resources.ERROR_FUNCTION, "ExtractProductInfos3S", "L'expression régulière d'entrée 'sRegExpression' est incorrecte");
            }

            // Création de l'expression régulière
            Regex regExpr = new Regex(sRegExpression, RegexOptions.None);
            // Création du match et matching de la chaine de caractère en entrée
            Match matchRegex = regExpr.Match(sInputString);

            // Vérification du bon matching
            if (!matchRegex.Success)
            {
                bErrorOccured = true;
                iErrorCode = ERROR_REGEX_NOMATCH;
                sErrorMsg = string.Format(Tools_StringProcessing.Properties.Resources.ERROR_FUNCTION, "ExtractProductInfos3S", "L'expression régulière ne match pas la chaine d'entrée");
            }

            // Récupération des données de matching
            sOutputString1 = matchRegex.Groups[1].Value;
            sOutputString2 = matchRegex.Groups[2].Value;
            sOutputString3 = matchRegex.Groups[3].Value;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Verify register ex. </summary>
        ///
        ///
        /// <param name="sTestPattern" type="String">   A pattern specifying the test. </param>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        private static bool VerifyRegEx(String sTestPattern)
        {
            bool bIsValid = true;

            if (!String.IsNullOrEmpty(sTestPattern) && sTestPattern.Trim().Length > 0)
            {
                try
                {
                    Regex.Match("", sTestPattern);
                }
                catch (ArgumentException)
                {
                    // Bad pattern : Syntax error
                    bIsValid = false;
                }
            }
            else
            {
                // Bad pattern : null or blank
                bIsValid = false;
            }

            return bIsValid;
        }

        #endregion
    }
}