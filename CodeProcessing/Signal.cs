using System;
using System.Collections.Generic;
using System.Linq;
using NationalInstruments.Analysis.SpectralMeasurements;

namespace Tools_SignalProcessing
{
    public static class SignalProcessing
    {
        #region GESTION_ERREUR

        ///----------------------------------------------------------------------------------------
        /// <signature> private static void ResetErrorParams(out Boolean bErrorOccured, out Int32
        /// iErrorCode, out String sErrorMessage)</signature>
        ///
        /// <summary>Réinitialisation des paramètres d'erreur.</summary>
        ///
        /// <param name="bErrorOccured" type="out Boolean"> [out] Flag de détection d'erreur.</param>
        /// <param name="iErrorCode" type="out Int32">      [out] Code erreur indiquant le type de l'erreur.</param>
        /// <param name="sErrorMessage" type="out String">  [out] Message d'erreur.</param>
        ///----------------------------------------------------------------------------------------
        private static void ResetErrorParams(out Boolean bErrorOccured, out Int32 iErrorCode, out String sErrorMessage)
        {
            bErrorOccured = false;
            iErrorCode = 0;
            sErrorMessage = string.Empty;
        }

        #endregion GESTION_ERREUR

        #region FRONTS

        ///----------------------------------------------------------------------------------------
        /// <signature> public static Int32 AnalyzeTwoStateSignal(Double dT0, Double dDeltaT, Int32
        /// iActualPoints, Double[] dTableauMesure, Double dLowTrigger, Double dHighTrigger, ref Edge[]
        /// eEdge, ref Boolean bErrorOccured, ref Int32 iErrorCode, ref String sErrorMessage)</signature>
        ///
        /// <summary>Génère un tableau de fronts pour un signal deux états.</summary>
        ///
        /// <param name="dT0" type="Double">               Temps initial à partir duquel faire l'analyse.</param>
        /// <param name="dDeltaT" type="Double">           Temps entre deux points.</param>
        /// <param name="iActualPoints" type="Int32">      Nombre de points total à analyser.</param>
        /// <param name="dTableauMesure" type="Double[]">  Tableau de mesures.</param>
        /// <param name="dLowTrigger" type="Double">       Valeur basse de détection de fronts.</param>
        /// <param name="dHighTrigger" type="Double">      Valeur haute de détection de fronts.</param>
        /// <param name="eEdge" type="ref Edge[]">         [out] Tableau des fronts détectés.</param>
        /// <param name="bErrorOccured" type="ref Boolean">[out] Flag de détection d'erreur.</param>
        /// <param name="iErrorCode" type="ref Int32">     [out] Code erreur indiquant le type de l'erreur.</param>
        /// <param name="sErrorMessage" type="ref String"> [out] Message d'erreur.</param>
        ///
        /// <returns>Code de retour de la fonction.</returns>
        ///----------------------------------------------------------------------------------------
        public static Int32 AnalyzeTwoStateSignal(Double dT0, Double dDeltaT, Int32 iActualPoints, Double[] dTableauMesure, Double dLowTrigger, Double dHighTrigger, ref Edge[] eEdge, ref Boolean bErrorOccured, ref Int32 iErrorCode, ref String sErrorMessage)
        {
            /// Initialisation des paramètres de gestion d'erreur Teststand
            ResetErrorParams(out bErrorOccured, out iErrorCode, out sErrorMessage);

            // Tests des paramètres d'entrée
            if (dTableauMesure.Length == 0) // Tableau de mesures
            {
                bErrorOccured = true;
                iErrorCode = 5200;
                sErrorMessage = "Tableau de mesures vide.";
            }
            else if (dDeltaT == 0) // Delta entre 2 points
            {
                bErrorOccured = true;
                iErrorCode = 5201;
                sErrorMessage = "Fréquence nulle.";
            }
            else
            {
                try
                {
                    // Test sur le premier point à partir duquel faire l'analyse
                    int numeroPoint;
                    double lastEdgeTime = 0;
                    if (dT0 == 0)
                    {
                        // Analyse à effectuer à partir du premier point
                        numeroPoint = 0;
                    }
                    else
                    {
                        // Analyse à effectuer à partir du point dépendant du deltaT
                        numeroPoint = Convert.ToInt32(dT0 / dDeltaT);
                        lastEdgeTime = dT0;
                    }

                    // Liste des fronts 
                    List<Edge> frontList = new List<Edge>();

                    // Initialisation pour effectuer les calculs de front
                    double timeLowTrigger = 0, timeHighTrigger = 0;
                    bool isObservingEdge = false;

                    // Détermination de l'état du signal
                    SignalLevel? twoStateSignalLevel = null;
                    EdgeType? observedEdge = null;

                    if (dTableauMesure[numeroPoint] > dLowTrigger && dTableauMesure[numeroPoint] < dHighTrigger)
                    {
                        // Etat "milieu" (entre les 2 valeurs limites)
                        twoStateSignalLevel = SignalLevel.MiddlePositive;
                    }
                    else if (dTableauMesure[numeroPoint] <= dLowTrigger)
                    {
                        // Etat bas
                        twoStateSignalLevel = SignalLevel.Low;
                    }
                    else if (dTableauMesure[numeroPoint] >= dHighTrigger)
                    {
                        // Etat haut
                        twoStateSignalLevel = SignalLevel.High;
                    }

                    // Analyse pour chaque point
                    for (int i = numeroPoint; i < dTableauMesure.Length; i++)
                    {
                        if (twoStateSignalLevel == SignalLevel.Low)
                        {
                            // Recherche front montant
                            if (dTableauMesure[i] >= dLowTrigger)
                            {
                                // Vérification de la non stabilité sur la valeur limite
                                if ((i + 1) < dTableauMesure.Length)
                                {
                                    if (dTableauMesure[i + 1] > dLowTrigger)
                                    {
                                        // Sauvegarde du temps
                                        timeLowTrigger = i * dDeltaT;

                                        // Passage en observation
                                        isObservingEdge = true;
                                        observedEdge = EdgeType.Rising;
                                        twoStateSignalLevel = SignalLevel.MiddlePositive;
                                    }
                                }
                            }
                        }
                        else if (twoStateSignalLevel == SignalLevel.High)
                        {
                            // Recherche front descendant
                            if (dTableauMesure[i] <= dHighTrigger)
                            {
                                // Vérification de la non stabilité sur la valeur limite
                                if ((i + 1) < dTableauMesure.Length)
                                {
                                    if (dTableauMesure[i + 1] < dHighTrigger)
                                    {
                                        // Sauvegarde du temps
                                        timeHighTrigger = i * dDeltaT;

                                        // Passage en observation
                                        isObservingEdge = true;
                                        observedEdge = EdgeType.Falling;
                                        twoStateSignalLevel = SignalLevel.MiddlePositive;
                                    }
                                }
                            }
                        }
                        else
                        {
                            // Recherche de front montant ou descendant
                            if (isObservingEdge)
                            {
                                if (observedEdge == EdgeType.Rising)
                                {
                                    // Recherche de la fin front montant
                                    if (dTableauMesure[i] >= dHighTrigger)
                                    {
                                        // Détection du front montant
                                        // Récupération du temps à 50% du front (T)
                                        timeHighTrigger = i * dDeltaT;
                                        double time = (timeLowTrigger + timeHighTrigger) / 2;

                                        // Calcul du temps de montée (DeltaRisingFallingTime)
                                        double risingTime = timeHighTrigger - timeLowTrigger;

                                        // Calcul de la durée du seuil entre la fin du front précédent 
                                        // et le début du front courant (ThresholdTime)
                                        double thresholdTime = timeLowTrigger - lastEdgeTime;

                                        // Calcul de la valeur moyenne mesurée du front entre la fin 
                                        // du front précédent et le début du front courant (AverageThresholdValue)
                                        int indexDebut = Convert.ToInt32(lastEdgeTime / dDeltaT);
                                        int indexFin = Convert.ToInt32(timeLowTrigger / dDeltaT);

                                        double averageThresholdValue = 0;
                                        for (int num = indexDebut; num < (indexFin + 1); num++)
                                        {
                                            averageThresholdValue += dTableauMesure[num];
                                        }
                                        averageThresholdValue /= (indexFin - indexDebut + 1);

                                        // Ajout du front dans la liste
                                        Edge detectedEdge = new Edge(time, EdgeType.Rising, risingTime, thresholdTime, averageThresholdValue);
                                        frontList.Add(detectedEdge);

                                        // Sauvegarde du temps du dernier front
                                        lastEdgeTime = timeHighTrigger;

                                        // Réinitialisation
                                        timeLowTrigger = 0;
                                        timeHighTrigger = 0;
                                        isObservingEdge = false;

                                        isObservingEdge = false;
                                        observedEdge = null;
                                        twoStateSignalLevel = SignalLevel.High;
                                    }
                                }
                                else if (observedEdge == EdgeType.Falling)
                                {
                                    // Recherche de la fin front descendant
                                    if (dTableauMesure[i] <= dLowTrigger)
                                    {
                                        // Détection du front descendant
                                        // Récupération du temps à 50% du front (T)
                                        timeLowTrigger = i * dDeltaT;
                                        double time = (timeHighTrigger + timeLowTrigger) / 2;

                                        // Calcul du temps de montée (DeltaRisingFallingTime)
                                        double fallingTime = timeLowTrigger - timeHighTrigger;

                                        // Calcul de la durée du seuil entre la fin du front précédent 
                                        // et le début du front courant (ThresholdTime)
                                        double thresholdTime = timeHighTrigger - lastEdgeTime;

                                        // Calcul de la valeur moyenne mesurée du front entre la fin 
                                        // du front précédent et le début du front courant (AverageThresholdValue)
                                        int indexDebut = Convert.ToInt32(lastEdgeTime / dDeltaT);
                                        int indexFin = Convert.ToInt32(timeHighTrigger / dDeltaT);

                                        double averageThresholdValue = 0;
                                        for (int num = indexDebut; num < (indexFin + 1); num++)
                                        {
                                            averageThresholdValue += dTableauMesure[num];
                                        }
                                        averageThresholdValue /= (indexFin - indexDebut + 1);

                                        // Ajout du front dans la liste
                                        Edge detectedEdge = new Edge(time, EdgeType.Falling, fallingTime, thresholdTime, averageThresholdValue);
                                        frontList.Add(detectedEdge);

                                        // Sauvegarde du temps du dernier front
                                        lastEdgeTime = timeLowTrigger;

                                        // Réinitialisation
                                        timeLowTrigger = 0;
                                        timeHighTrigger = 0;

                                        isObservingEdge = false;
                                        observedEdge = null;
                                        twoStateSignalLevel = SignalLevel.Low;
                                    }
                                }
                            }
                            else
                            {
                                // Démarrage de l'analyse en cours de front
                                if (dTableauMesure[i] >= dHighTrigger) // Recherche de la fin du front montant
                                {
                                    // Sauvegarde du temps du dernier front
                                    timeHighTrigger = i * dDeltaT;
                                    lastEdgeTime = timeHighTrigger;

                                    // Réinitialisation
                                    timeHighTrigger = 0;

                                    // Passage en niveau haut
                                    twoStateSignalLevel = SignalLevel.High;
                                }
                                else if (dTableauMesure[i] <= dLowTrigger) // Recherche de la fin du front descendant
                                {
                                    // Sauvegarde du temps du dernier front
                                    timeLowTrigger = i * dDeltaT;
                                    lastEdgeTime = timeLowTrigger;

                                    // Réinitialisation
                                    timeLowTrigger = 0;

                                    // Passage en niveau bas
                                    twoStateSignalLevel = SignalLevel.Low;
                                }
                            }
                        }
                    }

                    // Transformation de la liste en tableau
                    eEdge = frontList.ToArray();
                }
                catch (Exception Ex)
                {
                    bErrorOccured = true;
                    iErrorCode = Ex.HResult;
                    sErrorMessage = Ex.Message;
                }
            }

            return iErrorCode;
        }

        ///----------------------------------------------------------------------------------------
        /// <signature> public static Int32 AnalyzeTriStateSignal(Double dT0, Double dDeltaT, Int32
        /// iActualPoints, Double[] dTableauMesure, Double dPositiveLowTrigger, Double
        /// dPositiveHighTrigger, Double dNegativeLowTrigger, Double dNegativeHighTrigger, ref Edge[]
        /// eEdge, ref Boolean bErrorOccured, ref Int32 iErrorCode, ref String sErrorMessage)</signature>
        ///
        /// <summary>Génère un tableau de fronts pour un signal trois états.</summary>
        ///
        /// <param name="dT0" type="Double">                 Temps initial à partir duquel faire l'analyse.</param>
        /// <param name="dDeltaT" type="Double">             Temps entre deux points.</param>
        /// <param name="iActualPoints" type="Int32">        Nombre de points total à analyser.</param>
        /// <param name="dTableauMesure" type="Double[]">    Tableau de mesures.</param>
        /// <param name="dPositiveLowTrigger" type="Double"> Valeur basse positive de détection de fronts.</param>
        /// <param name="dPositiveHighTrigger" type="Double">Valeur basse positive de fronts.</param>
        /// <param name="dNegativeLowTrigger" type="Double"> Valeur basse négative de détection de fronts.</param>
        /// <param name="dNegativeHighTrigger" type="Double">Valeur haute négative de détection de fronts.</param>
        /// <param name="eEdge" type="ref Edge[]">           [out] Tableau des fronts détectés.</param>
        /// <param name="bErrorOccured" type="ref Boolean">  [out] Flag de détection d'erreur.</param>
        /// <param name="iErrorCode" type="ref Int32">       [out] Code erreur indiquant le type de l'erreur.</param>
        /// <param name="sErrorMessage" type="ref String">   [out] Message d'erreur.</param>
        ///
        /// <returns>Code de retour de la fonction.</returns>
        ///----------------------------------------------------------------------------------------
        public static Int32 AnalyzeTriStateSignal(Double dT0, Double dDeltaT, Int32 iActualPoints, Double[] dTableauMesure, Double dPositiveLowTrigger, Double dPositiveHighTrigger, Double dNegativeLowTrigger, Double dNegativeHighTrigger, ref Edge[] eEdge, ref Boolean bErrorOccured, ref Int32 iErrorCode, ref String sErrorMessage)
        {
            /// Initialisation des paramètres de gestion d'erreur Teststand
            ResetErrorParams(out bErrorOccured, out iErrorCode, out sErrorMessage);

            // Tests des paramètres d'entrée
            if (dTableauMesure.Length == 0) // Tableau de mesures
            {
                bErrorOccured = true;
                iErrorCode = 5200;
                sErrorMessage = "Tableau de mesures vide.";
            }
            else if (dDeltaT == 0) // Delta entre 2 points
            {
                bErrorOccured = true;
                iErrorCode = 5201;
                sErrorMessage = "Fréquence nulle.";
            }
            else
            {
                try
                {
                    // Test sur le premier point à partir duquel faire l'analyse
                    int numeroPoint;
                    double lastEdgeTime = 0;
                    if (dT0 == 0)
                    {
                        // Analyse à effectuer à partir du premier point
                        numeroPoint = 0;
                    }
                    else
                    {
                        // Analyse à effectuer à partir du point dépendant du deltaT
                        numeroPoint = Convert.ToInt32(dT0 / dDeltaT);
                        lastEdgeTime = dT0;
                    }

                    // Liste des fronts 
                    List<Edge> frontList = new List<Edge>();

                    // Initialisation pour effectuer les calculs de front
                    double timePositiveLowTrigger = 0, timePositiveHighTrigger = 0, timeNegativeLowTrigger = 0, timeNegativeHighTrigger = 0;
                    bool isObservingEdge = false;

                    // Détermination de l'état du signal
                    SignalLevel? twoStateSignalLevel = null;
                    EdgeType? observedEdge = null;

                    if (dTableauMesure[numeroPoint] > dPositiveLowTrigger && dTableauMesure[numeroPoint] < dPositiveHighTrigger)
                    {
                        // Etat "milieu haut" (entre les 2 valeurs limites)
                        twoStateSignalLevel = SignalLevel.MiddlePositive;
                    }
                    else if (dTableauMesure[numeroPoint] < dNegativeLowTrigger && dTableauMesure[numeroPoint] > dNegativeHighTrigger)
                    {
                        // Etat "milieu bas" (entre les 2 valeurs limites)
                        twoStateSignalLevel = SignalLevel.MiddleNegative;
                    }
                    else if (dTableauMesure[numeroPoint] <= dNegativeLowTrigger)
                    {
                        // Etat bas
                        twoStateSignalLevel = SignalLevel.Low;
                    }
                    else if (dTableauMesure[numeroPoint] >= dPositiveHighTrigger)
                    {
                        // Etat haut
                        twoStateSignalLevel = SignalLevel.High;
                    }
                    else
                    {
                        twoStateSignalLevel = SignalLevel.Middle;
                    }

                    // Analyse pour chaque point
                    for (int i = numeroPoint; i < dTableauMesure.Length; i++)
                    {
                        if (twoStateSignalLevel == SignalLevel.Middle)
                        {
                            // Recherche front montant ou descendant
                            if (dTableauMesure[i] >= dPositiveLowTrigger)
                            {
                                // Vérification de la non stabilité sur la valeur limite
                                if ((i + 1) < dTableauMesure.Length)
                                {
                                    if (dTableauMesure[i + 1] > dPositiveLowTrigger)
                                    {
                                        // Détection front montant
                                        // Sauvegarde du temps
                                        timePositiveLowTrigger = i * dDeltaT;

                                        // Passage en observation
                                        isObservingEdge = true;
                                        observedEdge = EdgeType.Rising;
                                        twoStateSignalLevel = SignalLevel.MiddlePositive;
                                    }
                                    else
                                    {
                                        // Sauvegarde de la fin de front montant
                                        lastEdgeTime = i * dDeltaT;
                                    }
                                }
                            }
                            else if (dTableauMesure[i] <= dNegativeLowTrigger)
                            {
                                // Vérification de la non stabilité sur la valeur limite
                                if ((i + 1) < dTableauMesure.Length)
                                {
                                    if (dTableauMesure[i + 1] < dNegativeLowTrigger)
                                    {
                                        // Détection front descendant
                                        // Sauvegarde du temps
                                        timeNegativeLowTrigger = i * dDeltaT;

                                        // Passage en observation
                                        isObservingEdge = true;
                                        observedEdge = EdgeType.Falling;
                                        twoStateSignalLevel = SignalLevel.MiddleNegative;
                                    }
                                    else
                                    {
                                        // Sauvegarde de la fin de front descendant
                                        lastEdgeTime = i * dDeltaT;
                                    }
                                }
                            }
                        }
                        else if (twoStateSignalLevel == SignalLevel.Low)
                        {
                            // Recherche front montant
                            if (dTableauMesure[i] >= dNegativeHighTrigger)
                            {
                                // Vérification de la non stabilité sur la valeur limite
                                if ((i + 1) < dTableauMesure.Length)
                                {
                                    if (dTableauMesure[i + 1] > dNegativeHighTrigger)
                                    {
                                        // Sauvegarde du temps
                                        timeNegativeHighTrigger = i * dDeltaT;

                                        // Passage en observation
                                        isObservingEdge = true;
                                        observedEdge = EdgeType.Rising;
                                        twoStateSignalLevel = SignalLevel.MiddleNegative;
                                    }
                                }
                            }
                        }
                        else if (twoStateSignalLevel == SignalLevel.High)
                        {
                            // Recherche front descendant
                            if (dTableauMesure[i] <= dPositiveHighTrigger)
                            {
                                // Vérification de la non stabilité sur la valeur limite
                                if ((i + 1) < dTableauMesure.Length)
                                {
                                    if (dTableauMesure[i + 1] < dPositiveHighTrigger)
                                    {
                                        // Sauvegarde du temps
                                        timePositiveHighTrigger = i * dDeltaT;

                                        // Passage en observation
                                        isObservingEdge = true;
                                        observedEdge = EdgeType.Falling;
                                        twoStateSignalLevel = SignalLevel.MiddlePositive;
                                    }
                                }
                            }
                        }
                        else if (twoStateSignalLevel == SignalLevel.MiddlePositive)
                        {
                            // Recherche de front montant ou descendant
                            if (isObservingEdge)
                            {
                                if (observedEdge == EdgeType.Rising)
                                {
                                    // Recherche de la fin front montant
                                    if (dTableauMesure[i] >= dPositiveHighTrigger)
                                    {
                                        // Détection du front montant
                                        // Récupération du temps à 50% du front (T)
                                        timePositiveHighTrigger = i * dDeltaT;
                                        double time = (timePositiveLowTrigger + timePositiveHighTrigger) / 2;

                                        // Calcul du temps de montée (DeltaRisingFallingTime)
                                        double risingTime = timePositiveHighTrigger - timePositiveLowTrigger;

                                        // Calcul de la durée du seuil entre la fin du front précédent 
                                        // et le début du front courant (ThresholdTime)
                                        double thresholdTime = timePositiveLowTrigger - lastEdgeTime;

                                        // Calcul de la valeur moyenne mesurée du front entre la fin 
                                        // du front précédent et le début du front courant (AverageThresholdValue)
                                        int indexDebut = Convert.ToInt32(lastEdgeTime / dDeltaT);
                                        int indexFin = Convert.ToInt32(timePositiveLowTrigger / dDeltaT);

                                        double averageThresholdValue = 0;
                                        for (int num = indexDebut; num < (indexFin + 1); num++)
                                        {
                                            averageThresholdValue += dTableauMesure[num];
                                        }
                                        averageThresholdValue /= (indexFin - indexDebut + 1);

                                        // Ajout du front dans la liste
                                        Edge detectedEdge = new Edge(time, EdgeType.Rising, risingTime, thresholdTime, averageThresholdValue);
                                        frontList.Add(detectedEdge);

                                        // Sauvegarde du temps du dernier front
                                        lastEdgeTime = timePositiveHighTrigger;

                                        // Réinitialisation
                                        timePositiveLowTrigger = 0;
                                        timePositiveHighTrigger = 0;
                                        isObservingEdge = false;

                                        isObservingEdge = false;
                                        observedEdge = null;
                                        twoStateSignalLevel = SignalLevel.High;
                                    }
                                }
                                else if (observedEdge == EdgeType.Falling)
                                {
                                    // Recherche de la fin front descendant
                                    if (dTableauMesure[i] <= dPositiveLowTrigger)
                                    {
                                        // Détection du front descendant
                                        // Récupération du temps à 50% du front (T)
                                        timePositiveLowTrigger = i * dDeltaT;
                                        double time = (timePositiveHighTrigger + timePositiveLowTrigger) / 2;

                                        // Calcul du temps de montée (DeltaRisingFallingTime)
                                        double fallingTime = timePositiveLowTrigger - timePositiveHighTrigger;

                                        // Calcul de la durée du seuil entre la fin du front précédent 
                                        // et le début du front courant (ThresholdTime)
                                        double thresholdTime = timePositiveHighTrigger - lastEdgeTime;

                                        // Calcul de la valeur moyenne mesurée du front entre la fin 
                                        // du front précédent et le début du front courant (AverageThresholdValue)
                                        int indexDebut = Convert.ToInt32(lastEdgeTime / dDeltaT);
                                        int indexFin = Convert.ToInt32(timePositiveHighTrigger / dDeltaT);

                                        double averageThresholdValue = 0;
                                        for (int num = indexDebut; num < (indexFin + 1); num++)
                                        {
                                            averageThresholdValue += dTableauMesure[num];
                                        }
                                        averageThresholdValue /= (indexFin - indexDebut + 1);

                                        // Ajout du front dans la liste
                                        Edge detectedEdge = new Edge(time, EdgeType.Falling, fallingTime, thresholdTime, averageThresholdValue);
                                        frontList.Add(detectedEdge);

                                        // Sauvegarde du temps du dernier front
                                        lastEdgeTime = timePositiveLowTrigger;

                                        // Réinitialisation
                                        timePositiveLowTrigger = 0;
                                        timePositiveHighTrigger = 0;

                                        isObservingEdge = false;
                                        observedEdge = null;
                                        twoStateSignalLevel = SignalLevel.Middle;
                                    }
                                }
                            }
                            else
                            {
                                // Démarrage de l'analyse en cours de front
                                if (dTableauMesure[i] >= dPositiveHighTrigger) // Recherche de la fin du front montant
                                {
                                    // Sauvegarde du temps du dernier front
                                    timePositiveHighTrigger = i * dDeltaT;
                                    lastEdgeTime = timePositiveHighTrigger;

                                    // Réinitialisation
                                    timePositiveHighTrigger = 0;

                                    // Passage en niveau haut
                                    twoStateSignalLevel = SignalLevel.High;
                                }
                                else if (dTableauMesure[i] <= dPositiveLowTrigger) // Recherche de la fin du front descendant
                                {
                                    // Sauvegarde du temps du dernier front
                                    timePositiveLowTrigger = i * dDeltaT;
                                    lastEdgeTime = timePositiveLowTrigger;

                                    // Réinitialisation
                                    timePositiveLowTrigger = 0;

                                    // Passage en niveau bas
                                    twoStateSignalLevel = SignalLevel.Middle;
                                }
                            }
                        }
                        else if (twoStateSignalLevel == SignalLevel.MiddleNegative)
                        {
                            // Recherche de front montant ou descendant
                            if (isObservingEdge)
                            {
                                if (observedEdge == EdgeType.Rising)
                                {
                                    // Recherche de la fin front montant
                                    if (dTableauMesure[i] >= dNegativeLowTrigger)
                                    {
                                        // Détection du front montant
                                        // Récupération du temps à 50% du front (T)
                                        timeNegativeLowTrigger = i * dDeltaT;
                                        double time = (timeNegativeHighTrigger + timeNegativeLowTrigger) / 2;

                                        // Calcul du temps de montée (DeltaRisingFallingTime)
                                        double risingTime = timeNegativeLowTrigger - timeNegativeHighTrigger;

                                        // Calcul de la durée du seuil entre la fin du front précédent 
                                        // et le début du front courant (ThresholdTime)
                                        double thresholdTime = timeNegativeHighTrigger - lastEdgeTime;

                                        // Calcul de la valeur moyenne mesurée du front entre la fin 
                                        // du front précédent et le début du front courant (AverageThresholdValue)
                                        int indexDebut = Convert.ToInt32(lastEdgeTime / dDeltaT);
                                        int indexFin = Convert.ToInt32(timeNegativeHighTrigger / dDeltaT);

                                        double averageThresholdValue = 0;
                                        for (int num = indexDebut; num < (indexFin + 1); num++)
                                        {
                                            averageThresholdValue += dTableauMesure[num];
                                        }
                                        averageThresholdValue /= (indexFin - indexDebut + 1);

                                        // Ajout du front dans la liste
                                        Edge detectedEdge = new Edge(time, EdgeType.Rising, risingTime, thresholdTime, averageThresholdValue);
                                        frontList.Add(detectedEdge);

                                        // Sauvegarde du temps du dernier front
                                        lastEdgeTime = timeNegativeLowTrigger;

                                        // Réinitialisation
                                        timeNegativeLowTrigger = 0;
                                        timeNegativeHighTrigger = 0;
                                        isObservingEdge = false;

                                        isObservingEdge = false;
                                        observedEdge = null;
                                        twoStateSignalLevel = SignalLevel.Middle;
                                    }
                                }
                                else if (observedEdge == EdgeType.Falling)
                                {
                                    // Recherche de la fin front descendant
                                    if (dTableauMesure[i] <= dNegativeHighTrigger)
                                    {
                                        // Détection du front descendant
                                        // Récupération du temps à 50% du front (T)
                                        timeNegativeHighTrigger = i * dDeltaT;
                                        double time = (timeNegativeLowTrigger + timeNegativeHighTrigger) / 2;

                                        // Calcul du temps de montée (DeltaRisingFallingTime)
                                        double fallingTime = timeNegativeHighTrigger - timeNegativeLowTrigger;

                                        // Calcul de la durée du seuil entre la fin du front précédent 
                                        // et le début du front courant (ThresholdTime)
                                        double thresholdTime = timeNegativeLowTrigger - lastEdgeTime;

                                        // Calcul de la valeur moyenne mesurée du front entre la fin 
                                        // du front précédent et le début du front courant (AverageThresholdValue)
                                        int indexDebut = Convert.ToInt32(lastEdgeTime / dDeltaT);
                                        int indexFin = Convert.ToInt32(timeNegativeLowTrigger / dDeltaT);

                                        double averageThresholdValue = 0;
                                        for (int num = indexDebut; num < (indexFin + 1); num++)
                                        {
                                            averageThresholdValue += dTableauMesure[num];
                                        }
                                        averageThresholdValue /= (indexFin - indexDebut + 1);

                                        // Ajout du front dans la liste
                                        Edge detectedEdge = new Edge(time, EdgeType.Falling, fallingTime, thresholdTime, averageThresholdValue);
                                        frontList.Add(detectedEdge);

                                        // Sauvegarde du temps du dernier front
                                        lastEdgeTime = timeNegativeHighTrigger;

                                        // Réinitialisation
                                        timeNegativeLowTrigger = 0;
                                        timeNegativeHighTrigger = 0;

                                        isObservingEdge = false;
                                        observedEdge = null;
                                        twoStateSignalLevel = SignalLevel.Low;
                                    }
                                }
                            }
                            else
                            {
                                // Démarrage de l'analyse en cours de front
                                if (dTableauMesure[i] >= dNegativeLowTrigger) // Recherche de la fin du front montant
                                {
                                    // Sauvegarde du temps du dernier front
                                    timeNegativeLowTrigger = i * dDeltaT;
                                    lastEdgeTime = timeNegativeLowTrigger;

                                    // Réinitialisation
                                    timeNegativeLowTrigger = 0;

                                    // Passage en niveau haut
                                    twoStateSignalLevel = SignalLevel.Middle;
                                }
                                else if (dTableauMesure[i] <= dNegativeHighTrigger) // Recherche de la fin du front descendant
                                {
                                    // Sauvegarde du temps du dernier front
                                    timeNegativeHighTrigger = i * dDeltaT;
                                    lastEdgeTime = timeNegativeHighTrigger;

                                    // Réinitialisation
                                    timeNegativeHighTrigger = 0;

                                    // Passage en niveau bas
                                    twoStateSignalLevel = SignalLevel.Low;
                                }
                            }
                        }
                    }

                    // Transformation de la liste en tableau
                    eEdge = frontList.ToArray();
                }
                catch (Exception Ex)
                {
                    bErrorOccured = true;
                    iErrorCode = Ex.HResult;
                    sErrorMessage = Ex.Message;
                }
            }

            return iErrorCode;
        }

        #endregion FRONTS

        #region PHASE

        ///----------------------------------------------------------------------------------------
        /// <signature> public static Int32 MeasureAnalogPhaseShift(Double dT0, Double dDeltaT, Int32
        /// iActualPoints, Double[] dTableauMesure1, Double[] dTableauMesure2, ref double phaseShiftValue,
        /// ref Boolean bErrorOccured, ref Int32 iErrorCode, ref String sErrorMessage)</signature>
        ///
        /// <summary>Mesure le déphasage entre deux signaux.</summary>
        ///
        /// <param name="dT0" type="Double">                Temps initial à partir duquel faire l'analyse.</param>
        /// <param name="dDeltaT" type="Double">            Temps entre deux points.</param>
        /// <param name="iActualPoints" type="Int32">       Nombre de points total à analyser.</param>
        /// <param name="dTableauMesure1" type="Double[]">  Tableau de mesures (signal 1).</param>
        /// <param name="dTableauMesure2" type="Double[]">  Tableau de mesures (signal 2).</param>
        /// <param name="phaseShiftValue" type="ref double">[out] Valeur du déphasage en degré.</param>
        /// <param name="bErrorOccured" type="ref Boolean"> [out] Flag de détection d'erreur.</param>
        /// <param name="iErrorCode" type="ref Int32">      [out] Code erreur indiquant le type de l'erreur.</param>
        /// <param name="sErrorMessage" type="ref String">  [out] Message d'erreur.</param>
        ///
        /// <returns>Code de retour de la fonction.</returns>
        ///----------------------------------------------------------------------------------------
        public static Int32 MeasureAnalogPhaseShift(Double dT0, Double dDeltaT, Int32 iActualPoints, Double[] dTableauMesure1, Double[] dTableauMesure2, ref double phaseShiftValue, ref Boolean bErrorOccured, ref Int32 iErrorCode, ref String sErrorMessage)
        {
            /// Initialisation des paramètres de gestion d'erreur Teststand
            ResetErrorParams(out bErrorOccured, out iErrorCode, out sErrorMessage);

            // Tests des paramètres d'entrée
            if (dTableauMesure1.Length == 0) // Tableau de mesures 1
            {
                bErrorOccured = true;
                iErrorCode = 5200;
                sErrorMessage = "Tableau de mesures vide.";
            }
            else if (dDeltaT == 0) // Delta entre 2 points
            {
                bErrorOccured = true;
                iErrorCode = 5201;
                sErrorMessage = "Fréquence nulle.";
            }
            else if (dTableauMesure2.Length == 0) // Tableau de mesures 2
            {
                bErrorOccured = true;
                iErrorCode = 5202;
                sErrorMessage = "Tableau de mesures n°2 vide.";
            }
            else if (dTableauMesure1.Length != dTableauMesure2.Length)
            {
                bErrorOccured = true;
                iErrorCode = 5203;
                sErrorMessage = "Les tableaux de mesures n'ont pas la même taille.";
            }
            else
            {
                try
                {
                    // Supression des points inutiles
                    if (dT0 != 0)
                    {
                        dTableauMesure1 = dTableauMesure1.Skip(Convert.ToInt32(dT0 / dDeltaT)).ToArray();
                        dTableauMesure2 = dTableauMesure2.Skip(Convert.ToInt32(dT0 / dDeltaT)).ToArray();
                    }

                    //Get PeakList
                    List<Peak> PeakList1 = CalcPeakList(dTableauMesure1);
                    List<Peak> PeakList2 = CalcPeakList(dTableauMesure2);

                    //Remove minimums, keep maximums
                    PeakList1 = PeakList1.Where(p => p.isAnMax == true).ToList();
                    PeakList2 = PeakList2.Where(p => p.isAnMax == true).ToList();

                    if (PeakList1.Count() > PeakList2.Count())
                        PeakList1.Remove(PeakList1[0]);

                    if (PeakList2.Count() > PeakList1.Count())
                        PeakList2.Remove(PeakList2[0]);

                    //Period equal time diff between two max
                    int sinusPeriod = 0;
                    int timeDifference = 0;
                    if (PeakList1.Count() > 1)
                        sinusPeriod = PeakList1[1].indexOfPeak - PeakList1[0].indexOfPeak;
                    if (PeakList2.Count() > 0)
                        timeDifference = PeakList1[0].indexOfPeak - PeakList2[0].indexOfPeak;

                    if (sinusPeriod == 0)
                    {
                        bErrorOccured = true;
                        iErrorCode = 5201;
                        sErrorMessage = "Fréquence nulle.";
                    }
                    else
                    {
                        // Calcul du déphasage
                        phaseShiftValue = 360 * timeDifference / sinusPeriod;
                    }


                }
                catch (Exception Ex)
                {
                    bErrorOccured = true;
                    iErrorCode = Ex.HResult;
                    sErrorMessage = Ex.Message;
                }
            }

            return iErrorCode;
        }

        private static List<Peak> CalcPeakList(double[] tab)
        {
            bool raising = (tab[0] < tab[1]);

            List<Peak> PeakList = new List<Peak>();
            for (int i = 0; i < tab.Length - 1; i++)
            {
                if (raising)
                {
                    if (tab[i] > tab[i + 1])
                    {
                        raising = false;
                        //Save peak
                        PeakList.Add(new Peak { indexOfPeak = i, isAnMax = true });
                    }
                }
                else
                {
                    if (tab[i] < tab[i + 1])
                    {
                        raising = true;
                        //Save peak
                        PeakList.Add(new Peak { indexOfPeak = i, isAnMax = false });
                    }
                }
            }

            return PeakList;
        }

        public static void ttt(double[] d1)
        {
            double[] asp = new double[100];
            double[] psp = new double[100];
            double df = 1;

            //          NationalInstruments.Analysis.SpectralMeasurements.Measurements.AmplitudePhaseSpectrum( = new 
            Measurements.AmplitudePhaseSpectrum(d1, false, 1, out asp, out psp, out df);


        }

        #endregion PHASE
    }

    class Peak
    {
        public int indexOfPeak;
        public bool isAnMax;
    }
}
