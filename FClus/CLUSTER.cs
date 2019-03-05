using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FClus
{
    class CLUSTER
    {
        private double[][] dataSet; // every col a diminsion ==> number of lines = N (nb of patterns)
                                    //                           number of cols = n (Diminsions).
        private int clusterNB;   // how many clusters do we need.
        private double exponent; 
        private double ebsilon;
        private double[][] partitionMatrix; // i*k , i represents the cluster(i) , k represents the pattern(k)
        private double[][] V;  // n*j , n is the number of clusters , j is number of diminsion (same like patterns).
        private double[] P;
        private double[][,] F;

        public CLUSTER(double[][] dataSet_Z, int ClustersNb_C, double exponent_m, double E)
        {
            this.dataSet = dataSet_Z;
            this.ebsilon = E;
            this.clusterNB = ClustersNb_C;
            this.exponent = exponent_m;
            partitionMatrix = initPartitionMatrix();
/*            partitionMatrix = new double[clusterNB][];
            double myInitVal = 1.0d / clusterNB;
            for (int i = 0; i < partitionMatrix.Length; i++)
            {
                partitionMatrix[i] = new double[dataSet.Length];
                for (int j = 0; j < partitionMatrix[i].Length; j++)
                    partitionMatrix[i][j] = myInitVal;
            }*/
            V = new double[clusterNB][];
/*            for (int i = 0; i < V.Length; i++)
                V[i] = new double[dataSet[0].Length];*/
        }

        private double[][] initPartitionMatrix()
        {
            ////// ADD ZERO STATE TO BE ACCURATE.
            double[][] tempV = new double[clusterNB][];
            double[][] res = new double[clusterNB][];
            double[][] Distances = new double[clusterNB][];
            Random ra = new Random();
            for (int v = 0; v < clusterNB; v++)
            {
                tempV[v] = new double[dataSet[0].Length];
                for (int k = 0; k < tempV[v].Length; k++)
                    tempV[v][k] = ra.NextDouble() * ra.Next(15);
            }
            double acuu = 0;
            for (int c = 0; c < clusterNB; c++)
            {
                Distances[c] = new double[dataSet.Length];
                for (int k = 0; k < Distances[c].Length; k++)
                {
                    Distances[c][k] = euclideanDistance(dataSet[k], tempV[c]);
                }
            }
            for (int p = 0; p < clusterNB; p++)
                res[p] = new double[dataSet.Length];
            for (int k = 0; k < dataSet.Length; k++)
            {
                for (int i = 0; i < clusterNB; i++)
                {
                    acuu = 0;
                    for (int j = 0; j < clusterNB; j++)
                        acuu += Math.Pow((Distances[i][k] / Distances[j][k]), (2 / (exponent - 1)));
                    res[i][k] = 1.0d / acuu;
                }
            }
            return res;
        }

        private double euclideanDistance(double[] zk, double[] vi)
        {
            if (zk.Length != vi.Length)
                throw new Exception("Vi and Zk dont have the same diminsions");
            double res=0;
            for (int i = 0; i < vi.Length; i++)
                res += Math.Pow((zk[i] - vi[i]), 2);
            return res;
        }

        public double[][] ClusterMyDataByC_Mean()
        {
            //double[][] tempV=new double[clusterNB][];
            double  denominator = 0;
            VECTOR holdData, nominator = new VECTOR(new double[dataSet[0].Length]);
            double[][] Distances = new double[clusterNB][];
            double[][] tempPartMat;
            while (true)
            {
                tempPartMat = new double[clusterNB][];
                /////////////   STEP 1
                for (int i = 0; i < clusterNB; i++)
                {
                    denominator = 0;
                    nominator = new VECTOR(new double[dataSet[0].Length]);
                    for (int k = 0; k < dataSet.Length; k++)
                    {
                        holdData = new VECTOR(dataSet[k]);
                        nominator = nominator + (Math.Pow(partitionMatrix[i][k], exponent) * holdData);
                        denominator += Math.Pow(partitionMatrix[i][k], exponent);
                    }
                    holdData = nominator / denominator;
                    V[i] = holdData.Val;
                }
                //////////////     END OF STEP 1
                
                /////////////      STEP 2
                for (int c = 0; c < clusterNB; c++)
                {
                    Distances[c] = new double[dataSet.Length];
                    for (int k = 0; k < Distances[c].Length; k++)
                    {
                        Distances[c][k] = euclideanDistance(dataSet[k], V[c]);
                    }
                }
                /////////////       END OF STEP 2
                
                /////////////       STEP 3
                for (int u = 0; u < clusterNB; u++)
                    tempPartMat[u] = new double[dataSet.Length];
                for (int k = 0; k < dataSet.Length; k++)
                {
                    if (allClusterValuesPositive(Distances, k))
                    {
                        for (int c = 0; c < clusterNB; c++)
                        {
                            denominator = 0;
                            for (int j = 0; j < clusterNB; j++)
                                denominator += Math.Pow((Distances[c][k] / Distances[j][k]), (2 / (exponent - 1)));
                            tempPartMat[c][k] = 1.0d / denominator;
                        }
                    }
                    else
                    {
                        int count = 0;
                        for (int c = 0; c < clusterNB; c++)
                        {
                            if (Distances[c][k] > 0)
                                tempPartMat[c][k] = 0;
                            else
                                count++;
                        }
                        denominator = 1.0d / count;
                        for (int h = 0; h < clusterNB; h++)
                            if (Distances[h][k] <= 0)
                                tempPartMat[h][k] = denominator;
                    }
                }
                if (!diferenceGreaterThanEbsilon(tempPartMat))
                    break;
                partitionMatrix = tempPartMat;
            }
            partitionMatrix = tempPartMat;
            return partitionMatrix;
        }

        private bool diferenceGreaterThanEbsilon(double[][] tempP)
        {
            for (int x = 0; x < tempP.Length; x++)
                for (int y = 0; y < tempP[x].Length; y++)
                    if (Math.Abs(partitionMatrix[x][y] - tempP[x][y]) > ebsilon)
                        return true;
            return false;
        }

        private bool allClusterValuesPositive(double[][] disMat, int patternID)
        {
            for (int c = 0; c < clusterNB; c++)
            {
                if (disMat[c][patternID] <= 0)
                    return false;
            }
            return true;
        }

        public double[][] clusterByGK(double[] clusterV)
        {
            double denominator = 0;
            VECTOR holdData, nominator = new VECTOR(new double[dataSet[0].Length]);
            MATRIX2 nominator2,hold1,hold2,temp1,temp2;
            double[][] Distances = new double[clusterNB][];
            double[][] tempPartMat;
            double[,] Zk, Vi;
            this.P = clusterV;
            F = new double[clusterNB][,];
            while (true)
            {
                tempPartMat = new double[clusterNB][];
                /////////////   STEP 1 + 2
                for (int i = 0; i < clusterNB; i++)
                {
                    denominator = 0;
                    nominator = new VECTOR(new double[dataSet[0].Length]);
                    nominator2 = new MATRIX2(dataSet[0].Length, dataSet[0].Length);
                    //nominator2.initMatrix2();
                    for (int k = 0; k < dataSet.Length; k++)
                    {
                        holdData = new VECTOR(dataSet[k]);
                        nominator = nominator + (Math.Pow(partitionMatrix[i][k], exponent) * holdData);
                        denominator += Math.Pow(partitionMatrix[i][k], exponent);
                    }
                    holdData = nominator / denominator;
                    V[i] = holdData.Val;
                    //// Step 2
                    Vi = convertToTable(V[i],false);
                    hold2 = new MATRIX2(Vi);
                    for (int k = 0; k < dataSet.Length; k++)
                    {
                        //holdData = new VECTOR(dataSet[k]);
                        //hold2 = new VECTOR(V[i]);
                        //hold2 = holdData - hold2;
                        Zk = convertToTable(dataSet[k],false);
                        hold1 = new MATRIX2(Zk);
                        hold1 = hold1 - hold2;
                        temp1 = hold1 * hold1.Transpose();
                        temp1 = Math.Pow(partitionMatrix[i][k], exponent) * temp1;
                        nominator2 = nominator2 + temp1;
                        //nominator2 = nominator2 + (Math.Pow(partitionMatrix[i][k], exponent) * (hold1 * hold1.Transpose()));
                    }
                    nominator2 = nominator2 / denominator;
                    F[i] = nominator2.Elements;
                    //// Step 3
                    for (int k = 0; k < dataSet.Length; k++)
                        Distances[i] = new double[dataSet.Length];
                    for (int k = 0; k < dataSet.Length; k++)
                    {
                        Zk = convertToTable(dataSet[k],false);
                        hold1 = new MATRIX2(Zk);
                        hold1 = hold1 - hold2;
                        temp1 = (P[i] * Math.Pow(nominator2.Determinent(),1.0d/dataSet[0].Length)) * nominator2.Inverse();
                        temp2 = temp1 * hold1;
                        Distances[i][k] = (hold1.Transpose() * temp2).Determinent();
                        //Distances[i][k] = hold1.Transpose() * (((P[i] * Math.Pow(nominator2.Determinent().ConvertToDouble(), 1.0d / dataSet[0].Length)) * nominator2.Inverse()) * hold1);
                    }
                }
                //////////////     END OF STEP 1 + 2 + 3

                //////////////     STEP 4
                for (int u = 0; u < clusterNB; u++)
                    tempPartMat[u] = new double[dataSet.Length];
                for (int k = 0; k < dataSet.Length; k++)
                {
                    if (allClusterValuesPositive(Distances, k))
                    {
                        for (int c = 0; c < clusterNB; c++)
                        {
                            denominator = 0;
                            for (int j = 0; j < clusterNB; j++)
                                denominator += Math.Pow((Distances[c][k] / Distances[j][k]), (2 / (exponent - 1)));
                            tempPartMat[c][k] = 1.0d / denominator;
                        }
                    }
                    else
                    {
                        int count = 0;
                        for (int c = 0; c < clusterNB; c++)
                        {
                            if (Distances[c][k] > 0)
                                tempPartMat[c][k] = 0;
                            else
                                count++;
                        }
                        denominator = 1.0d / count;
                        for (int h = 0; h < clusterNB; h++)
                            if (Distances[h][k] <= 0)
                                tempPartMat[h][k] = denominator;
                    }
                }
                if (!diferenceGreaterThanEbsilon(tempPartMat))
                    break;
                partitionMatrix = tempPartMat;
            }
            partitionMatrix = tempPartMat;
            return partitionMatrix;
        }

        private double[,] convertToTable(double[] vector, bool is_T)
        {
            double[,] temp;
            if (is_T)
            {
                temp = new double[1, vector.Length];
                for (int k = 0; k < vector.Length; k++)
                    temp[0, k] = vector[k];
            }
            else
            {
                temp = new double[vector.Length, 1];
                for (int k = 0; k < vector.Length; k++)
                    temp[k, 0] = vector[k];
            }
            return temp;
        }

        public bool[][] cluster_BY_data()
        {
            bool[][] temp = new bool[clusterNB][];
            for (int v = 0; v < clusterNB; v++)
            {
                temp[v] = new bool[dataSet.Length];
                for (int b = 0; b < dataSet.Length; b++)
                    temp[v][b] = false;
            }
            double max = double.MinValue;
            int cl = -1;
            for (int k = 0; k < dataSet.Length; k++)
            {
                max = double.MinValue;
                for (int c = 0; c < clusterNB; c++)
                {
                    if (partitionMatrix[c][k] > max)
                    {
                        max = partitionMatrix[c][k];
                        cl = c;
                    }
                }
                temp[cl][k] = true;
            }
            return temp;
        }
    }
}
