using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace SimulIDE.src.simulator
{
    public class CircMatrix
    {

        protected static CircMatrix self = null;

        public CircMatrix()
        {
            self = this;
            numEnodes = 0;
        }

    

        public void CreateMatrix(List<eNode> eNodeList )
        {
            this.eNodeList = eNodeList;
            numEnodes = eNodeList.Count();
            //circMatrix.clear();
            //coefVect.clear();
            ResizeArray<double>(ref circMatrix, numEnodes, numEnodes);
            Array.Resize<double>(ref coefVect, numEnodes);
            

            circChanged = true;
            admitChanged = false;
            currChanged = false;
            Console.WriteLine("\n  Initializing Matrix: " + numEnodes.ToString() + " eNodes");
            for (int i = 0; i < numEnodes; i++) eNodeList[i].StampMatrix();
        }

        public void StampMatrix(int row, int col, double value)
        {
            admitChanged = true;
            circMatrix[row - 1,col - 1] = value;      // eNode numbers start at 1
        }

        public void StampCoef(int row, double value)
        {
            currChanged = true;
            coefVect[row - 1] = value;
        }

        public void AddConnections(int enodNum, ref List<int> nodeGroup, ref List<int> allNodes)
        {
            nodeGroup.Add(enodNum);
            allNodes.Remove(enodNum);

            eNode enod = eNodeList[enodNum - 1];
            enod.SetSingle(false);

            List<int> cons = enod.GetConnections();

            foreach (int nodeNum in cons)
            {
                if (nodeNum == 0) continue;
                if (!nodeGroup.Contains(nodeNum)) AddConnections(nodeNum, ref nodeGroup, ref allNodes);
            }
        }

        protected void ResizeArray<T>(ref T[,] original, int newCoNum, int newRoNum)
        {
            var newArray = new T[newCoNum, newRoNum];
            int columnCount = original.GetLength(1);
            int columnCount2 = newRoNum;
            int columns = original.GetUpperBound(0);
            for (int co = 0; co <= columns; co++)
                Array.Copy(original, co * columnCount, newArray, co * columnCount2, columnCount);
            original = newArray;
        }

        public bool SolveMatrix()
        {
            if (!admitChanged && !currChanged) return true;

            bool isOk = true;

            if (circChanged)          // Split Circuit into unconnected parts
            {
                //qDebug() <<"Spliting Circuit...";
                List<int> allNodes=new List<int>();

                for (int i = 0; i < numEnodes; i++) allNodes.Add(i + 1);

                aList.Clear();
                aFaList.Clear();
                bList.Clear();
                ipvtList.Clear();
                eNodeActList.Clear();
                int group = 0;

                while (allNodes.Count!=0) // Get a list of groups of nodes interconnected
                {
                    List<int> nodeGroup = new List<int>();
                    AddConnections(allNodes.First(), ref nodeGroup, ref allNodes); // Get a group of nodes interconnected
                                                                         //qDebug() <<"CircMatrix::solveMatrix split"<<nodeGroup<<allNodes;

                    //for( int num : nodeGroup ) allNodes.removeOne(num);

                    int numEnodes = nodeGroup.Count();
                    if (numEnodes == 1)           // Sigle nodes do by themselves
                    {
                        eNode enod = eNodeList[nodeGroup[0] - 1];
                        enod.SetSingle(true);
                        enod.SolveSingle();
                        //qDebug() <<"CircMatrix::solveMatrix solve single"<<enod->itemId();
                    }
                    else
                    {
                        double[,] a = new double[numEnodes, numEnodes];
                        double[] b = new double[numEnodes];
                        int[] ipvt = new int[numEnodes];
                        List<eNode> eNodeActive=new List<eNode>();
                        int ny = 0;
                        for (int y = 0; y < numEnodes; y++)    // Copy data to reduced Matrix
                        {
                            if (!nodeGroup.Contains(y + 1)) continue;
                            int nx = 0;
                            for (int x = 0; x < numEnodes; x++)
                            {
                                if (!nodeGroup.Contains(x + 1)) continue;
                                a[nx,ny] = circMatrix[x,y];
                                //qDebug() <<"CircMatrix::solveMatrix cell"<<nx<<ny<<*(a[nx][ny]);
                                nx++;
                            }
                            b[ny] = coefVect[y];
                            eNodeActive.Add(eNodeList[y]);
                            //eNode* enod = m_eNodeList->at(y);
                            //qDebug() <<"CircMatrix::solveMatrix node"<<enod->itemId();
                            ny++;
                        }
                        aList.Add(a);
                   //TYV     aFaList.Add(ap);
                        bList.Add(b);
                        ipvtList.Add(ipvt);
                        eNodeActList.Add(eNodeActive);
                    //TYV    eNodeActive = &eNodeActive;

                        FactorMatrix(ny, group);
                        isOk &= LuSolve(ny, group);
                            
                        group++;
                    }
                }
                circChanged = false;
                //qDebug() <<"CircMatrix::solveMatrix"<<group<<"Circuits";
            }
            else
            {
                for (int i = 0; i < bList.Count(); i++)
                {
                    eNodeActive = eNodeActList[i];
                    int n = eNodeActive.Count();

                    if (admitChanged) FactorMatrix(n, i);

                    isOk &= LuSolve(n, i);
                }
            }
            currChanged = false;
            admitChanged = false;
            return isOk;
        }

        protected void FactorMatrix(int n, int group)
        {
            // factors a matrix into upper and lower triangular matrices by
            // gaussian elimination.  On entry, a[0..n-1][0..n-1] is the
            // matrix to be factored.  ipvt[] returns an integer vector of pivot
            // indices, used in the solve routine.

            //dp_matrix_t & ap = m_aList[group];
            //i_vector_t & ipvt = m_ipvtList[group];

            //d_matrix_t & a = m_aFaList[group];
            //for (int ic = 0; ic < n; ic++)
            //{
            //    for (int jc = 0; jc < n; jc++)
            //    {
            //        a[ic,jc] = *(ap[ic,jc]);
            //        //qDebug() << m_circMatrix[i][j];
            //    }
            //}

            ///*std::cout << "\nAdmitance Matrix:\n"<< std::endl;
            //for( int i=0; i<n; i++ )
            //{
            //    for( int j=0; j<n; j++ )
            //    {
            //        std::cout << std::setw(10);
            //        std::cout << a[i][j];
            //    }
            //    std::cout << std::setw(10);
            //    std::cout << ipvt[i] << std::endl;
            //    //std::cout << std::endl;
            //}*/

            //int i, j, k;

            //for (j = 0; j < n; j++) // use Crout's method; loop through the columns
            //{
            //for (i = 0; i < j; i++) // calculate upper triangular elements for this column
            //    {
            //        double q = a[i,j];
            //        for (k = 0; k < i; k++) q -= a[i][k] * a[k][j];
                        
            //        a[i][j] = q;
            //    }
            //    // calculate lower triangular elements for this column
            //    double largest = 0;
            //    int largestRow = -1;
            //    for (i = j; i < n; i++)
            //    {
            //        double q = a[i][j];
            //        for (k = 0; k < j; k++) q -= a[i][k] * a[k][j];

            //        a[i][j] = q;
            //        double x = fabs(q);

            //        //qDebug() <<"is"<<x<<">="<<largest<<( x >= largest );
            //        if (x >= largest)
            //        {
            //            largest = x;
            //            largestRow = i;
            //        }
            //        //qDebug() <<"LTE"<<i<<j<<x<<q<<largest<<largestRow<<( !(x < largest) );
            //    }

            //    if (j != largestRow) // pivoting
            //    {
            //        double x;
            //        for (k = 0; k < n; k++)
            //        {
            //            x = a[largestRow][k];
            //            a[largestRow][k] = a[j][k];
            //            a[j][k] = x;
            //        }
            //    }
            //    ipvt[j] = largestRow;       // keep track of row interchanges
            //                                //qDebug() <<"IPVT"<< j<<largestRow;

            //    if (a[j][j] == 0.0) a[j][j] = 1e-18;           // avoid zeros

            //    if (j != n - 1)
            //    {
            //        double div = a[j][j];
            //        for (i = j + 1; i < n; i++) a[i][j] /= div;
            //    }   
            //}
            //m_aFaList.replace(group, a);

            ///*std::cout << "\nFactored Matrix:\n"<< std::endl;
            //for( int i=0; i<n; i++ )
            //{
            //    for( int j=0; j<n; j++ )
            //    {
            //        std::cout << std::setw(10);
            //        std::cout << a[i][j];
            //    }
            //    std::cout << std::setw(10);
            //    std::cout << ipvt[i] << std::endl;
            //    //std::cout << std::endl;
            //}*/
        }

    protected bool LuSolve(int n, int group)
    {
            //// Solves the set of n linear equations using a LU factorization
            //// previously performed by solveMatrix.  On input, b[0..n-1] is the right
            //// hand side of the equations, and on output, contains the solution.

            //const d_matrix_t&a = m_aFaList[group];
            //const dp_vector_t&bp = m_bList[group];
            //const i_vector_t&ipvt = m_ipvtList[group];

            //d_vector_t b;
            //b.resize(n, 0);
            //for (int i = 0; i < n; i++) b[i] = *(bp[i]);

            ///*std::cout << "\nAdmitance Matrix luSolve:\n"<< std::endl;
            //for( int i=0; i<n; i++ )
            //{
            //    for( int j=0; j<n; j++ )
            //    {
            //        std::cout << std::setw(10);
            //        std::cout << a[i][j]; // <<"\t";
            //    }
            //    std::cout << std::setw(10);
            //    std::cout << b[i]<<"\t"<< ipvt[i] << std::endl;
            //}*/

            //int i;
            //for (i = 0; i < n; i++)                 // find first nonzero b element
            //{
            //    int row = ipvt[i];

            //    double swap = b[row];
            //    b[row] = b[i];
            //    b[i] = swap;
            //    if (swap != 0) break;
            //}

            //int bi = i++;
            //for ( /*i = bi*/; i < n; i++)
            //{
            //    int row = ipvt[i];
            //    double tot = b[row];

            //    b[row] = b[i];

            //    for (int j = bi; j < i; j++) tot -= a[i][j] * b[j]; // forward substitution using the lower triangular matrix

            //    b[i] = tot;
            //}
            //bool isOk = true;

            //for (i = n - 1; i >= 0; i--)
            //{
            //    double tot = b[i];

            //    // back-substitution using the upper triangular matrix
            //    for (int j = i + 1; j < n; j++) tot -= a[i][j] * b[j];

            //    double volt = tot / a[i][i];
            //    b[i] = volt;

            //    if (std::isnan(volt))
            //    {
            //        isOk = false;
            //        volt = 0;
            //    }
            //    m_eNodeActive->at(i)->setVolt(volt);      // Set Node Voltages
            //}
            //return isOk;
            return false;
        }

        public void SetCircChanged()
        {
            circChanged = true;
            admitChanged = true;
        }

        public void PrintMatrix()
        {
            Console.WriteLine("\nAdmitance Matrix:\n");
            for (int i = 0; i < numEnodes; i++)
            {
                for (int j = 0; j < numEnodes; j++)
                {
                    Console.Write(circMatrix[i,j].ToString()+"\t");
                }
                Console.Write("\t");
                Console.WriteLine(coefVect[i].ToString());
                Console.WriteLine();
                Console.WriteLine();
            }

            //std::cout << "\nSantized Matrix:\n"<< std::endl;

        }

        public static CircMatrix Self() { return self; }

        public double[,] GetMatrix() { return circMatrix; }
        public double[] GetCoeffVect() { return coefVect; }

        private int numEnodes;
        private List<eNode> eNodeList;

        private List<double[,]> aList;
        private List<double[,]> aFaList;
        private List<double[]> bList;
        private List<int[]> ipvtList;

        private List<eNode> eNodeActive;
        private List<List<eNode>> eNodeActList;

        private double[,] circMatrix;
        private double[] coefVect;

        private bool admitChanged;
        private bool circChanged;
        private bool currChanged;

    }
}
